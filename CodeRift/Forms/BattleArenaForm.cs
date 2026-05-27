using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeRift.Core;
using CodeRift.Entities;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    internal sealed class ScreenTintOverlay : Control
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TintColor { get; set; }

        public ScreenTintOverlay()
        {
            TintColor = Color.Transparent;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            Enabled = false;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Prevent default background fill to keep overlay smooth.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush brush = new SolidBrush(TintColor))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }
    }

    public partial class BattleArenaForm : Form
    {
        private const int AttackFrameCount = 8;
        private const int IdleFrameCount = 4;
        private const int AnimationTimerIntervalMs = 80;
        private const int ImpactFramesRemaining = 3;
        private const int GroundBottomPadding = 20;
        private const int PlayerScalePercent = 105;
        private const string DefaultEnemyAssetFolder = "enemy1";
        private const string DefaultEnemyPortraitFileName = "enemy_level_1.jpeg";
        private static readonly Dictionary<string, Image> BattleAssetCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
        private static readonly object BattleAssetCacheLock = new object();
        private static readonly int[] NeighborOffsetX = { 1, -1, 0, 0 };
        private static readonly int[] NeighborOffsetY = { 0, 0, 1, -1 };

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_COMPOSITED = 0x02000000;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_COMPOSITED;
                return cp;
            }
        }

        // Animation states used by the arena scene.
        private enum BattleState { IntroRunning, IdleLoop, PlayerAttacking, EnemyHurting, EnemyAttacking, PlayerHurting, PlayerReturning, EnemyReturning }
        private BattleState _currentState = BattleState.IntroRunning;

        private readonly PlayerActorController _playerActor = new PlayerActorController(AttackFrameCount, IdleFrameCount);
        private readonly EnemyActorController _enemyActor = new EnemyActorController(AttackFrameCount, IdleFrameCount);

        private int _currentFrame = 0;
        private int _animFrameIdx = 0;
        private int _stateTickCounter = 0;
        private double _idleTimeElapsedSeconds = 0.0;
        private int _idleTimeoutFlashFrames = 0;
        private readonly System.Windows.Forms.Timer _animTimer = new System.Windows.Forms.Timer();
        private bool _isAnimTimerWired;

        private bool _checkBattleAfterAnimation;
        private bool _battleEnded;
        private int _remainingEnemyRetaliationHits;
        private bool _pendingIncorrectAnswerPopup;
        private bool _enemyNeedsReturnAfterPlayerAttack;
        private readonly PictureBox _spriteCanvas = new PictureBox();
        private readonly ScreenTintOverlay _backgroundTintLayer = new ScreenTintOverlay();
        private Bitmap _spriteBuffer;
        private Graphics _spriteBufferGraphics;
        private Color _lastBackgroundTint = Color.Transparent;
        private readonly Random _vfxRandom = new Random();
        private readonly Task _animationLoadTask;

        // Tracks in-flight prewarm tasks keyed by level so hover-triggered loads
        // are never duplicated when the user hovers the same button twice.
        private static readonly Dictionary<int, Task> PrewarmTasks = new Dictionary<int, Task>();
        private static readonly object PrewarmLock = new object();

        // Card mapping for turn selection and lock validation.
        private readonly Dictionary<PictureBox, int> _cardIdByPicture = new Dictionary<PictureBox, int>();
        private readonly Dictionary<int, PictureBox> _pictureByCardId = new Dictionary<int, PictureBox>();
        private readonly HashSet<int> _usedPlayerCards = new HashSet<int>();
        private readonly HashSet<PictureBox> _wiredPlayerCardBoxes = new HashSet<PictureBox>();

        // Core battle logic engine.
        private readonly QuizBattleEngine _battleEngine;
        private readonly LevelConfig _levelConfig;

        public int Level { get; private set; }

        public BattleArenaForm(int level = 1)
        {
            InitializeComponent();
            Level = level;
            _levelConfig = LevelConfig.ForLevel(level);
            _battleEngine = new QuizBattleEngine(level);

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            EnableDoubleBuffer(this);

            picPlayerPortrait.Visible = false;
            picEnemyPortrait.Visible = false;
            SetupSpriteCanvas();
            SetupBackgroundTintLayer();
            ConfigureActorRenderSizes();

            // Pre-load the background synchronously so the form is never shown as a plain black rectangle.
            // Backgrounds are already decoded by the splash/asset bootstrapper, so this clone is near-instant.
            LoadBattleBackground();

            // Always run LoadAnimationAssets to populate _playerActor / _enemyActor frame arrays.
            // If PrewarmAsync already ran for this level, BattleAssetCache is warm, so GetCachedImage
            // returns cached clones instantly — making this task complete in near-zero time.
            _animationLoadTask = Task.Run(new Action(LoadAnimationAssets));

            PrepareBattleScreen();
        }

        private void EnableDoubleBuffer(Control control)
        {
            var property = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null) property.SetValue(control, true, null);

            foreach (Control child in control.Controls)
            {
                EnableDoubleBuffer(child);
            }
        }

        /// <summary>
        /// Begins decoding and caching all sprite frames for <paramref name="level"/> on a
        /// background thread.  Safe to call multiple times — duplicate calls for the same
        /// level are collapsed into one shared task so no work is repeated.
        /// Call this from a hover event so assets are ready before the user clicks.
        /// </summary>
        public static void PrewarmAsync(int level)
        {
            lock (PrewarmLock)
            {
                if (PrewarmTasks.ContainsKey(level))
                {
                    return; // Already warming or fully cached.
                }

                Task prewarmTask = Task.Run(() => PrewarmLevel(level));
                PrewarmTasks[level] = prewarmTask;
            }
        }

        /// <summary>
        /// Awaits the pre-loading and caching of ALL battle assets for the entire game.
        /// Call this during the Splash Screen to guarantee completely instant transition
        /// performance with zero CPU/Disk I/O spikes later on.
        /// </summary>
        public static async Task PrewarmAllLevelsAsync()
        {
            var tasks = new List<Task>();
            for (int i = 1; i <= 5; i++)
            {
                lock (PrewarmLock)
                {
                    if (!PrewarmTasks.ContainsKey(i))
                    {
                        Task t = Task.Run(() => PrewarmLevel(i));
                        PrewarmTasks[i] = t;
                        tasks.Add(t);
                    }
                    else
                    {
                        tasks.Add(PrewarmTasks[i]);
                    }
                }
            }
            await Task.WhenAll(tasks);
        }

        public static async Task PrewarmLevelWithProgressAsync(int level, Action<string, double> progressReport)
        {
            Task loadTask;
            bool isExisting = false;

            lock (PrewarmLock)
            {
                Task existingTask;
                if (PrewarmTasks.TryGetValue(level, out existingTask))
                {
                    loadTask = existingTask;
                    isExisting = true;
                }
                else
                {
                    loadTask = Task.Run(() =>
                    {
                        try
                        {
                            LevelConfig config = LevelConfig.ForLevel(level);
                            string playerPath = ResolveAssetPath("Assets", "Images", "player");
                            string enemyRoot   = ResolveAssetPath("Assets", "Images", "enemies");
                            string enemyFolder = Directory.Exists(Path.Combine(enemyRoot, config.Enemy.AssetFolder))
                                ? config.Enemy.AssetFolder
                                : DefaultEnemyAssetFolder;
                            string enemyPath   = Path.Combine(enemyRoot, enemyFolder);

                            double step = 100.0 / 8.0;
                            double currentProgress = 0;

                            if (progressReport != null) progressReport.Invoke("DECRYPTING PLAYER RUN SEQUENCES...", currentProgress);
                            PrewarmFrameSequence(Path.Combine(playerPath, "run"), "player_run", AttackFrameCount, applyTransparency: true);
                            currentProgress += step;

                            if (progressReport != null) progressReport.Invoke("OPTIMIZING PLAYER ATTACK VECTORS...", currentProgress);
                            PrewarmFrameSequence(Path.Combine(playerPath, "attack"), "player_attack", AttackFrameCount, applyTransparency: true);
                            currentProgress += step;

                            if (progressReport != null) progressReport.Invoke("CACHING PLAYER REACTION FRAMES...", currentProgress);
                            PrewarmFrameSequence(Path.Combine(playerPath, "hurt"), "player_hurt", AttackFrameCount, applyTransparency: false);
                            currentProgress += step;

                            if (progressReport != null) progressReport.Invoke("STABILIZING PLAYER COGNITIVE STATE...", currentProgress);
                            PrewarmFrameSequence(Path.Combine(playerPath, "ide"), "player_ide", IdleFrameCount, applyTransparency: false);
                            currentProgress += step;

                            if (progressReport != null) progressReport.Invoke(string.Format("INITIALIZING {0} INTERFACES...", config.EnemyName.ToUpperInvariant()), currentProgress);
                            PrewarmFrameSequence(Path.Combine(enemyPath, "run"), string.Format("{0}_run", enemyFolder), AttackFrameCount, applyTransparency: false);
                            currentProgress += step;

                            if (progressReport != null) progressReport.Invoke(string.Format("DECODING {0} ATTACK LOGIC...", config.EnemyName.ToUpperInvariant()), currentProgress);
                            PrewarmFrameSequence(Path.Combine(enemyPath, "attack"), string.Format("{0}_attack", enemyFolder), AttackFrameCount, applyTransparency: true);
                            currentProgress += step;

                            if (progressReport != null) progressReport.Invoke(string.Format("CACHING {0} REACTION ARRAYS...", config.EnemyName.ToUpperInvariant()), currentProgress);
                            PrewarmFrameSequence(Path.Combine(enemyPath, "hurt"), string.Format("{0}_hurt", enemyFolder), AttackFrameCount, applyTransparency: false);
                            currentProgress += step;

                            if (progressReport != null) progressReport.Invoke(string.Format("STABILIZING {0} COGNITIVE STATE...", config.EnemyName.ToUpperInvariant()), currentProgress);
                            PrewarmFrameSequence(Path.Combine(enemyPath, "ide"), string.Format("{0}_ide", enemyFolder), IdleFrameCount, applyTransparency: false);
                            currentProgress += step;

                            if (progressReport != null) progressReport.Invoke("DECRYPTION SYNCHRONIZED. READY FOR COMBAT.", 100.0);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Prewarm Error: " + ex.Message);
                            if (progressReport != null) progressReport.Invoke("ERROR: DECRYPTION SCHEMATIC TAMPERED.", 100.0);
                        }
                    });

                    PrewarmTasks[level] = loadTask;
                }
            }

            if (isExisting)
            {
                if (progressReport != null) progressReport.Invoke("DECRYPTION SYNCHRONIZED. READY FOR COMBAT.", 100.0);
            }

            await loadTask;
        }

        private static void PrewarmLevel(int level)
        {
            try
            {
                LevelConfig config = LevelConfig.ForLevel(level);

                // Player frames — cache keys are path-based so they are shared with
                // any future BattleArenaForm instance for this level.
                string playerPath = ResolveAssetPath("Assets", "Images", "player");
                PrewarmFrameSequence(Path.Combine(playerPath, "run"),    "player_run",    AttackFrameCount, applyTransparency: true);
                PrewarmFrameSequence(Path.Combine(playerPath, "attack"), "player_attack", AttackFrameCount, applyTransparency: true);
                PrewarmFrameSequence(Path.Combine(playerPath, "hurt"),   "player_hurt",   AttackFrameCount, applyTransparency: false);
                PrewarmFrameSequence(Path.Combine(playerPath, "ide"),    "player_ide",    IdleFrameCount,   applyTransparency: false);

                // Enemy frames.
                string enemyRoot   = ResolveAssetPath("Assets", "Images", "enemies");
                string enemyFolder = Directory.Exists(Path.Combine(enemyRoot, config.Enemy.AssetFolder))
                    ? config.Enemy.AssetFolder
                    : DefaultEnemyAssetFolder;
                string enemyPath   = Path.Combine(enemyRoot, enemyFolder);

                PrewarmFrameSequence(Path.Combine(enemyPath, "run"),    string.Format("{0}_run", enemyFolder),    AttackFrameCount, applyTransparency: false);
                PrewarmFrameSequence(Path.Combine(enemyPath, "attack"), string.Format("{0}_attack", enemyFolder), AttackFrameCount, applyTransparency: true);
                PrewarmFrameSequence(Path.Combine(enemyPath, "hurt"),   string.Format("{0}_hurt", enemyFolder),   AttackFrameCount, applyTransparency: false);
                PrewarmFrameSequence(Path.Combine(enemyPath, "ide"),    string.Format("{0}_ide", enemyFolder),    IdleFrameCount,   applyTransparency: false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Prewarm Error: " + ex.Message);
            }
        }

        private static void PrewarmFrameSequence(string folderPath, string prefix, int frameCount, bool applyTransparency)
        {
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            for (int i = 0; i < frameCount; i++)
            {
                string path = Path.Combine(folderPath, string.Format("{0}_{1:D2}.png", prefix, i + 1));
                if (File.Exists(path))
                {
                    // LoadFrame populates BattleAssetCache; the returned clone is discarded here.
                    using (Image frame = LoadFrame(path, applyTransparency))
                    {
                    }
                }
            }
        }

        private void SetupSpriteCanvas()
        {
            _spriteCanvas.Dock = DockStyle.Fill;
            _spriteCanvas.BackColor = Color.Transparent;
            _spriteCanvas.SizeMode = PictureBoxSizeMode.Normal;
            _spriteCanvas.Enabled = false;
            _spriteCanvas.TabStop = false;
            pnlBattleZone.Controls.Add(_spriteCanvas);
            _spriteCanvas.BringToFront();
        }

        private void ConfigureActorRenderSizes()
        {
            Size baseSize = picPlayerPortrait.Size;
            float playerScale = PlayerScalePercent / 100f;
            _playerActor.RenderSize = new Size(
                (int)Math.Round(baseSize.Width * playerScale),
                (int)Math.Round(baseSize.Height * playerScale));

            _enemyActor.RenderSize = new Size(
                (int)Math.Round(baseSize.Width * _levelConfig.EnemyRenderScale),
                (int)Math.Round(baseSize.Height * _levelConfig.EnemyRenderScale));
        }

        private void SetupBackgroundTintLayer()
        {
            // Background tint sits above form background and below all UI controls.
            _backgroundTintLayer.Dock = DockStyle.None;
            _backgroundTintLayer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            UpdateBackgroundTintBounds();
            _backgroundTintLayer.Visible = false;
            _backgroundTintLayer.TintColor = Color.Transparent;
            _backgroundTintLayer.Enabled = false;
            Controls.Add(_backgroundTintLayer);
            _backgroundTintLayer.SendToBack();
        }

        private void UpdateBackgroundTintBounds()
        {
            Rectangle desiredBounds = new Rectangle(Point.Empty, ClientSize);
            if (_backgroundTintLayer.Bounds == desiredBounds)
            {
                return;
            }

            _backgroundTintLayer.Bounds = desiredBounds;
        }

        private void LoadAnimationAssets()
        {
            try
            {
                LoadPlayerAnimationAssets();
                LoadEnemyAnimationAssets();

                // Normalize frame canvas sizes to prevent size/position snapping between sequences.
                NormalizeActorFrames(_playerActor.RunFrames, _playerActor.IdleFrames, _playerActor.AttackFrames, _playerActor.HurtFrames);
                NormalizeActorFrames(_enemyActor.RunFrames, _enemyActor.IdleFrames, _enemyActor.AttackFrames, _enemyActor.HurtFrames);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Asset Load Error: " + ex.Message);
            }
        }

        private void LoadPlayerAnimationAssets()
        {
            string playerPath = ResolveAssetPath("Assets", "Images", "player");

            LoadFrameSequence(Path.Combine(playerPath, "run"), "player_run", _playerActor.RunFrames, true, "player run");
            LoadFrameSequence(Path.Combine(playerPath, "attack"), "player_attack", _playerActor.AttackFrames, true, "player attack");
            LoadFrameSequence(Path.Combine(playerPath, "hurt"), "player_hurt", _playerActor.HurtFrames, false, "player hurt");
            LoadFrameSequence(Path.Combine(playerPath, "ide"), "player_ide", _playerActor.IdleFrames, false, "player idle");

            FillMissingFrames(_playerActor.AttackFrames, _playerActor.RunFrames);
            FillMissingFrames(_playerActor.HurtFrames, _playerActor.RunFrames);
            FillMissingFrames(_playerActor.IdleFrames, _playerActor.RunFrames);
        }

        private void LoadEnemyAnimationAssets()
        {
            string enemyRoot = ResolveAssetPath("Assets", "Images", "enemies");
            string enemyPath = Path.Combine(enemyRoot, _levelConfig.Enemy.AssetFolder);
            string enemyFolder = Directory.Exists(enemyPath) ? _levelConfig.Enemy.AssetFolder : DefaultEnemyAssetFolder;

            if (!StringComparer.OrdinalIgnoreCase.Equals(enemyFolder, _levelConfig.Enemy.AssetFolder))
            {
                LogAssetWarning(string.Format("Enemy asset folder '{0}' was not found. Falling back to '{1}'.", _levelConfig.Enemy.AssetFolder, DefaultEnemyAssetFolder));
            }

            enemyPath = Path.Combine(enemyRoot, enemyFolder);

            LoadFrameSequence(Path.Combine(enemyPath, "run"), string.Format("{0}_run", enemyFolder), _enemyActor.RunFrames, false, string.Format("{0} run", enemyFolder));
            LoadFrameSequence(Path.Combine(enemyPath, "attack"), string.Format("{0}_attack", enemyFolder), _enemyActor.AttackFrames, true, string.Format("{0} attack", enemyFolder));
            LoadFrameSequence(Path.Combine(enemyPath, "hurt"), string.Format("{0}_hurt", enemyFolder), _enemyActor.HurtFrames, false, string.Format("{0} hurt", enemyFolder));
            LoadFrameSequence(Path.Combine(enemyPath, "ide"), string.Format("{0}_ide", enemyFolder), _enemyActor.IdleFrames, false, string.Format("{0} idle", enemyFolder));

            FillMissingFrames(_enemyActor.HurtFrames, _enemyActor.RunFrames);
            FillMissingFrames(_enemyActor.AttackFrames, _enemyActor.RunFrames);
            FillMissingFrames(_enemyActor.IdleFrames, _enemyActor.RunFrames);
        }

        private static void LoadFrameSequence(string folderPath, string preferredPrefix, Image[] targetFrames, bool applyTransparency, string sequenceName)
        {
            if (!Directory.Exists(folderPath))
            {
                LogAssetWarning(string.Format("Animation folder missing for {0}: {1}", sequenceName, folderPath));
                return;
            }

            bool foundPreferredFrames = false;
            for (int i = 0; i < targetFrames.Length; i++)
            {
                string preferredPath = Path.Combine(folderPath, string.Format("{0}_{1:D2}.png", preferredPrefix, i + 1));
                if (!File.Exists(preferredPath))
                {
                    continue;
                }

                targetFrames[i] = LoadFrame(preferredPath, applyTransparency);
                foundPreferredFrames = true;
            }

            if (foundPreferredFrames)
            {
                return;
            }

            string[] framePaths = Directory.GetFiles(folderPath, "*.png")
                .OrderBy(GetFrameSortNumber)
                .ThenBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
                .Take(targetFrames.Length)
                .ToArray();

            for (int i = 0; i < targetFrames.Length && i < framePaths.Length; i++)
            {
                targetFrames[i] = LoadFrame(framePaths[i], applyTransparency);
            }

            if (targetFrames.All(frame => frame == null))
            {
                LogAssetWarning(string.Format("No animation frames loaded for {0}: {1}", sequenceName, folderPath));
            }
        }

        private static string ResolveAssetPath(params string[] relativeSegments)
        {
            return AssetPathHelper.ResolveAssetPath(relativeSegments);
        }

        private static void LogAssetWarning(string message)
        {
            Debug.WriteLine("Asset Warning: " + message);
            Console.WriteLine("Asset Warning: " + message);
        }

        private static Image LoadFrame(string path, bool applyTransparency)
        {
            string cacheKey = applyTransparency ? "FRAME_T|" + path : "FRAME|" + path;
            return GetCachedImage(cacheKey, () =>
            {
                Image source = LoadImageFromDisk(path);
                if (!applyTransparency)
                {
                    return source;
                }

                Image transparentFrame = MakeNearBlackBackgroundTransparent(source);
                source.Dispose();
                return transparentFrame;
            });
        }

        private static Image LoadImageCopy(string path)
        {
            return GetCachedImage("RAW|" + path, () => LoadImageFromDisk(path));
        }

        private static Image LoadImageFromDisk(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            using (MemoryStream stream = new MemoryStream(bytes, writable: false))
            {
                using (Image loadedImage = Image.FromStream(stream))
                {
                    return new Bitmap(loadedImage);
                }
            }
        }

        private static Image GetCachedImage(string cacheKey, Func<Image> createImage)
        {
            lock (BattleAssetCacheLock)
            {
                Image cachedImage;
                if (!BattleAssetCache.TryGetValue(cacheKey, out cachedImage))
                {
                    cachedImage = createImage();
                    BattleAssetCache[cacheKey] = cachedImage;
                }

                return (Image)cachedImage.Clone();
            }
        }

        private static int GetFrameSortNumber(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            string digits = new string(name.Where(char.IsDigit).ToArray());
            int value;
            if (int.TryParse(digits, out value))
                return value;
            return int.MaxValue;
        }

        private static void FillMissingFrames(Image[] targetFrames, Image[] fallbackFrames)
        {
            for (int i = 0; i < targetFrames.Length; i++)
            {
                if (targetFrames[i] != null)
                {
                    continue;
                }

                Image fallback = i < fallbackFrames.Length && fallbackFrames[i] != null
                    ? fallbackFrames[i]
                    : targetFrames.FirstOrDefault(frame => frame != null) ?? fallbackFrames.FirstOrDefault(frame => frame != null);

                if (fallback != null)
                {
                    targetFrames[i] = (Image)fallback.Clone();
                }
            }
        }

        /// <summary>
        /// Pads all frames to one shared canvas (max width/height in the set), bottom-aligned and centered.
        /// This keeps animation scale/footing stable even when source image sizes differ.
        /// </summary>
        private static void NormalizeActorFrames(params Image[][] frameSets)
        {
            int maxWidth = 0;
            int maxHeight = 0;

            foreach (var frameSet in frameSets)
            {
                foreach (var frame in frameSet)
                {
                    if (frame == null) continue;
                    maxWidth = Math.Max(maxWidth, frame.Width);
                    maxHeight = Math.Max(maxHeight, frame.Height);
                }
            }

            if (maxWidth <= 0 || maxHeight <= 0)
            {
                return;
            }

            foreach (var frameSet in frameSets)
            {
                for (int i = 0; i < frameSet.Length; i++)
                {
                    if (frameSet[i] == null) continue;
                    frameSet[i] = NormalizeFrameCanvas(frameSet[i], maxWidth, maxHeight);
                }
            }
        }

        private static Image NormalizeFrameCanvas(Image source, int canvasWidth, int canvasHeight)
        {
            if (source.Width == canvasWidth && source.Height == canvasHeight)
            {
                return source;
            }

            Bitmap normalized = new Bitmap(canvasWidth, canvasHeight);
            using (Graphics g = Graphics.FromImage(normalized))
            {
                g.Clear(Color.Transparent);

                int x = (canvasWidth - source.Width) / 2;
                int y = canvasHeight - source.Height; // bottom align
                g.DrawImage(source, x, y, source.Width, source.Height);
            }

            source.Dispose();
            return normalized;
        }

        private static Image MakeNearBlackBackgroundTransparent(Image source, byte threshold = 18)
        {
            Bitmap output = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (Graphics g = Graphics.FromImage(output))
            {
                g.DrawImage(source, 0, 0, source.Width, source.Height);
            }

            int width = output.Width;
            int height = output.Height;
            bool[] visited = new bool[width * height];
            Queue<int> queue = new Queue<int>();

            Rectangle bounds = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = null;

            try
            {
                bitmapData = output.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
                int stride = bitmapData.Stride;
                int totalBytes = stride * height;
                byte[] pixels = new byte[totalBytes];
                Marshal.Copy(bitmapData.Scan0, pixels, 0, totalBytes);

                // Seed flood-fill from image borders only. This removes background black
                // while preserving dark details inside the character.
                for (int x = 0; x < width; x++)
                {
                    FloodFillTryEnqueue(x, 0, width, height, visited, pixels, stride, threshold, queue);
                    FloodFillTryEnqueue(x, height - 1, width, height, visited, pixels, stride, threshold, queue);
                }

                for (int y = 0; y < height; y++)
                {
                    FloodFillTryEnqueue(0, y, width, height, visited, pixels, stride, threshold, queue);
                    FloodFillTryEnqueue(width - 1, y, width, height, visited, pixels, stride, threshold, queue);
                }

                while (queue.Count > 0)
                {
                    int visitedIndex = queue.Dequeue();
                    int y = visitedIndex / width;
                    int x = visitedIndex - (y * width);
                    int currentPixelOffset = FloodFillToPixelOffset(x, y, stride);
                    FloodFillSetTransparent(currentPixelOffset, pixels);

                    for (int i = 0; i < 4; i++)
                    {
                        int nextX = x + NeighborOffsetX[i];
                        int nextY = y + NeighborOffsetY[i];
                        if (nextX < 0 || nextY < 0 || nextX >= width || nextY >= height)
                        {
                            continue;
                        }

                        int nextVisitedIndex = FloodFillToVisitedIndex(nextX, nextY, width);
                        if (visited[nextVisitedIndex])
                        {
                            continue;
                        }

                        visited[nextVisitedIndex] = true;
                        int nextPixelOffset = FloodFillToPixelOffset(nextX, nextY, stride);
                        if (FloodFillIsNearBlack(nextPixelOffset, pixels, threshold))
                        {
                            queue.Enqueue(nextVisitedIndex);
                        }
                    }
                }

                Marshal.Copy(pixels, 0, bitmapData.Scan0, totalBytes);
            }
            finally
            {
                if (bitmapData != null)
                {
                    output.UnlockBits(bitmapData);
                }
            }

            return output;
        }

        private static int FloodFillToVisitedIndex(int x, int y, int width)
        {
            return (y * width) + x;
        }

        private static int FloodFillToPixelOffset(int x, int y, int stride)
        {
            return (y * stride) + (x * 4);
        }

        private static bool FloodFillIsNearBlack(int offset, byte[] pixels, byte threshold)
        {
            byte b = pixels[offset];
            byte gCh = pixels[offset + 1];
            byte r = pixels[offset + 2];
            byte a = pixels[offset + 3];
            return a > 0 && r <= threshold && gCh <= threshold && b <= threshold;
        }

        private static void FloodFillSetTransparent(int offset, byte[] pixels)
        {
            pixels[offset] = 0;
            pixels[offset + 1] = 0;
            pixels[offset + 2] = 0;
            pixels[offset + 3] = 0;
        }

        private static void FloodFillTryEnqueue(int x, int y, int width, int height, bool[] visited, byte[] pixels, int stride, byte threshold, Queue<int> queue)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
            {
                return;
            }

            int visitedIndex = FloodFillToVisitedIndex(x, y, width);
            if (visited[visitedIndex])
            {
                return;
            }

            visited[visitedIndex] = true;
            int pixelOffset = FloodFillToPixelOffset(x, y, stride);
            if (!FloodFillIsNearBlack(pixelOffset, pixels, threshold))
            {
                return;
            }

            queue.Enqueue(visitedIndex);
        }

        private void StartAnimations()
        {
            if (_isAnimTimerWired)
            {
                return;
            }

            PrepareActorIntroPositions();
            ConfigureAnimationTimer();
            RenderActorsToPictureBoxes();
        }

        private void PrepareActorIntroPositions()
        {
            // Align both actors by feet on the same baseline, while preserving animation states.
            int groundBaseline = CalculateGroundBaseline();

            _playerActor.IdleY = groundBaseline - _playerActor.RenderSize.Height;
            _enemyActor.IdleY = groundBaseline - _enemyActor.RenderSize.Height + _levelConfig.EnemyGroundYOffset;

            UpdateActorLayout();
            SetActorIntroRunPositions();
            SetInitialRunFrames();
        }

        private int CalculateGroundBaseline()
        {
            return pnlBattleZone.Height - GroundBottomPadding;
        }

        private void SetActorIntroRunPositions()
        {
            const int runDistance = 600;
            _playerActor.SetPosition(_playerActor.IdleX - runDistance, _playerActor.IdleY);
            _enemyActor.SetPosition(_enemyActor.IdleX + runDistance, _enemyActor.IdleY);
        }

        private void SetInitialRunFrames()
        {
            if (_playerActor.RunFrames[0] != null)
            {
                _playerActor.SetCurrentImage(_playerActor.RunFrames[0]);
            }

            if (_enemyActor.RunFrames[0] != null)
            {
                _enemyActor.SetCurrentImage(_enemyActor.RunFrames[0]);
            }
        }

        private void ConfigureAnimationTimer()
        {
            _animTimer.Interval = AnimationTimerIntervalMs;
            _animTimer.Tick += AnimTimer_Tick;
            _isAnimTimerWired = true;
            _animTimer.Start();
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            if (_battleEnded || IsDisposed || Disposing)
            {
                return;
            }

            _currentFrame++;

            UpdateActorLayout();

            switch (_currentState)
            {
                case BattleState.IntroRunning:
                    HandleIntroAnimation(_playerActor.IdleX, _enemyActor.IdleX);
                    break;

                case BattleState.IdleLoop:
                    HandleIdleAnimation();
                    UpdateIdleTimer();
                    break;

                case BattleState.PlayerAttacking:
                    HandlePlayerAttack();
                    break;

                case BattleState.EnemyHurting:
                    HandleEnemyHurt();
                    break;

                case BattleState.EnemyAttacking:
                    HandleEnemyAttack(_enemyActor.ContactX);
                    break;

                case BattleState.PlayerHurting:
                    HandlePlayerHurt();
                    break;

                case BattleState.PlayerReturning:
                    HandlePlayerReturn();
                    break;

                case BattleState.EnemyReturning:
                    HandleEnemyReturn();
                    break;
            }

            if (_battleEnded || IsDisposed || Disposing)
            {
                return;
            }

            RenderActorsToPictureBoxes();
        }

        private void HandleIntroAnimation(int pTarget, int eTarget)
        {
            int frameIdx = (_currentFrame / 2) % AttackFrameCount;
            _playerActor.SetRunFrame(frameIdx);
            _enemyActor.SetRunFrame(frameIdx);

            bool pArrived = false;
            bool eArrived = false;

            if (_playerActor.Position.X < pTarget)
            {
                _playerActor.MoveXTowards(pTarget, 25);
            }
            else
            {
                pArrived = true;
            }

            if (_enemyActor.Position.X > eTarget)
            {
                _enemyActor.MoveXTowards(eTarget, 25);
            }
            else
            {
                eArrived = true;
            }

            if (pArrived && eArrived)
            {
                SetState(BattleState.IdleLoop);
                _currentFrame = 0;
            }
        }

        private void UpdateActorLayout()
        {
            int centerX = pnlBattleZone.Width / 2;
            int playerW = _playerActor.RenderSize.Width;
            int enemyW = _enemyActor.RenderSize.Width;

            if (_levelConfig.CenterActorsByWidth)
            {
                int totalWidth = playerW + _levelConfig.ActorIdleGap + enemyW;
                _playerActor.IdleX = centerX - totalWidth / 2;
                _enemyActor.IdleX = _playerActor.IdleX + playerW + _levelConfig.ActorIdleGap;
            }
            else
            {
                // Place player on the left quarter, enemy on the right quarter.
                _playerActor.IdleX = centerX / 2 - playerW / 2;
                _enemyActor.IdleX = centerX + centerX / 2 - enemyW / 2;
            }

            // Contact X: where the attacking character's leading edge reaches the target.
            _playerActor.ContactX = _enemyActor.IdleX - playerW + _levelConfig.PlayerAttackContactOverlap;
            _enemyActor.ContactX = _playerActor.IdleX + playerW - _levelConfig.EnemyAttackContactOverlap;
        }

        private void HandleIdleAnimation()
        {
            int ticksPerIdleFrame = 6;
            int frameIdx = (_currentFrame / ticksPerIdleFrame) % IdleFrameCount;
            _playerActor.SetIdleFrame(frameIdx);
            _enemyActor.SetIdleFrame(frameIdx);
        }

        private void HandlePlayerAttack()
        {
            int frameIdx = _animFrameIdx;
            _playerActor.SetAttackFrame(frameIdx);
            if (_playerActor.Position.X < _playerActor.ContactX)
            {
                _playerActor.MoveXTowards(_playerActor.ContactX, 34);
            }

            if (!ShouldAdvanceStateFrame(2))
            {
                return;
            }

            _animFrameIdx++;
            int impactFrameStart = Math.Max(0, _playerActor.AttackFrames.Length - ImpactFramesRemaining);
            if (_animFrameIdx >= impactFrameStart)
            {
                _animFrameIdx = 0;
                _enemyNeedsReturnAfterPlayerAttack = false;
                SetState(BattleState.EnemyHurting);
                UpdateEnemyHudFromEngine();
            }
        }

        private void HandleEnemyHurt()
        {
            int frameIdx = _animFrameIdx;
            _enemyActor.SetHurtFrame(frameIdx);

            if (!ShouldAdvanceStateFrame(2))
            {
                return;
            }

            _animFrameIdx++;
            if (_animFrameIdx >= AttackFrameCount)
            {
                _animFrameIdx = 0;
                if (_enemyNeedsReturnAfterPlayerAttack)
                {
                    _enemyNeedsReturnAfterPlayerAttack = false;
                    SetState(BattleState.EnemyReturning);
                    return;
                }

                SetState(BattleState.PlayerReturning);
            }
        }

        private void HandlePlayerReturn()
        {
            int frameIdx = (_currentFrame / 4) % IdleFrameCount;
            _playerActor.SetIdleFrame(frameIdx);

            if (_playerActor.Position.X > _playerActor.IdleX)
            {
                _playerActor.MoveXTowards(_playerActor.IdleX, 30);
                return;
            }

            SetState(BattleState.IdleLoop);
            if (_checkBattleAfterAnimation)
            {
                _checkBattleAfterAnimation = false;
                EvaluateBattleResult();
            }
        }

        private void HandleEnemyAttack(int enemyContactX)
        {
            int frameIdx = _animFrameIdx;
            _enemyActor.SetAttackFrame(frameIdx);
            if (_enemyActor.Position.X > enemyContactX)
            {
                _enemyActor.MoveXTowards(enemyContactX, 38);
            }

            if (!ShouldAdvanceStateFrame(2))
            {
                return;
            }

            _animFrameIdx++;
            int impactFrameStart = Math.Max(0, _enemyActor.AttackFrames.Length - ImpactFramesRemaining);
            if (_animFrameIdx >= impactFrameStart)
            {
                _animFrameIdx = 0;
                SetState(BattleState.PlayerHurting);
            }
        }

        private void HandlePlayerHurt()
        {
            int frameIdx = _animFrameIdx;
            _playerActor.SetHurtFrame(frameIdx);

            if (!ShouldAdvanceStateFrame(2))
            {
                return;
            }

            _animFrameIdx++;
            if (_animFrameIdx >= AttackFrameCount)
            {
                _animFrameIdx = 0;
                _remainingEnemyRetaliationHits--;

                if (_remainingEnemyRetaliationHits > 0)
                {
                    SetState(BattleState.EnemyAttacking);
                    return;
                }

                SetState(BattleState.EnemyReturning);
                UpdatePlayerHudFromEngine();
            }
        }

        private void HandleEnemyReturn()
        {
            int frameIdx = (_currentFrame / 4) % IdleFrameCount;
            _enemyActor.SetIdleFrame(frameIdx);

            if (_enemyActor.Position.X < _enemyActor.IdleX)
            {
                _enemyActor.MoveXTowards(_enemyActor.IdleX, 30);
                return;
            }

            SetState(BattleState.IdleLoop);
            if (_checkBattleAfterAnimation)
            {
                _checkBattleAfterAnimation = false;
                EvaluateBattleResult();
            }

            if (_pendingIncorrectAnswerPopup)
            {
                _pendingIncorrectAnswerPopup = false;
                if (_battleEngine.CheckBattleResult() != BattleResult.PlayerDefeat && _battleEngine.PlayerHP > 0)
                {
                    ShowIncorrectAnswerMessage();
                }
                EvaluateBattleResult();
            }
        }

        private void PrepareBattleScreen()
        {
            lblLevelTitle.Text = string.Format("// LEVEL {0} : {1} //", Level, _levelConfig.EnemyName);
            lblEnemyName.Text = _levelConfig.EnemyName;
        }

        private async void BattleArenaForm_Load(object sender, EventArgs e)
        {
            try
            {
                await _animationLoadTask;
                if (ShouldAbortBattleLoad())
                {
                    return;
                }

                // Background was already set in the constructor; load the remaining UI assets.
                LoadPortraitAssets();
                LoadCardAssets();
                InitializeBattleRound();
                StartBattleAudioAndDebugTools();
            }
            catch (Exception ex)
            {
                ShowBattleLoadError(ex);
            }
        }

        private bool ShouldAbortBattleLoad()
        {
            return _battleEnded || IsDisposed || Disposing;
        }

        private void LoadBattleAssets()
        {
            LoadBattleBackground();
            LoadPortraitAssets();
            LoadCardAssets();
        }

        // Note: LoadBattleBackground is also called from the constructor to pre-paint
        // the background before the form becomes visible (prevents the black-screen flash).

        private void LoadBattleBackground()
        {
            string backgroundPath = ResolveAssetPath("Assets", "Images", "backgrounds", "level_background", string.Format("level_{0}.png", Level));
            if (File.Exists(backgroundPath))
            {
                BackgroundImage = LoadImageCopy(backgroundPath);
                BackgroundImageLayout = ImageLayout.Stretch;
                return;
            }

            LogAssetWarning(string.Format("Background image missing for level {0}: {1}", Level, backgroundPath));
        }

        private void LoadPortraitAssets()
        {
            string playerPortraitPath = ResolveAssetPath("Assets", "Images", "portraits", "player.jpeg");
            string enemyPortraitPath = ResolveAssetPath("Assets", "Images", "portraits", _levelConfig.Enemy.PortraitFileName);
            string fallbackEnemyPortraitPath = ResolveAssetPath("Assets", "Images", "portraits", DefaultEnemyPortraitFileName);

            LoadPictureBoxImage(picPlayerThumb, playerPortraitPath, "player portrait");
            LoadPictureBoxImage(picEnemyThumb, enemyPortraitPath, string.Format("{0} portrait", _levelConfig.Enemy.Name), fallbackEnemyPortraitPath);
        }

        private void LoadCardAssets()
        {
            LoadCards("player", "player_card", picPlayerCard1, picPlayerCard2, picPlayerCard3, picPlayerCard4, picPlayerCard5);
            LoadCards("enemies", "enemy_card", picEnemyCard1, picEnemyCard2, picEnemyCard3, picEnemyCard4, picEnemyCard5);
        }

        private void InitializeBattleRound()
        {
            SyncAllHudFromEngine();
            RefreshPlayerCardLockVisuals();
            StartAnimations();
        }

        private static void StartBattleAudioAndDebugTools()
        {
            AudioManager.Instance.PlayMusic(Constants.MUSIC_LEVELS);
#if DEBUG
            foreach (var line in QuizBattleEngine.RunSimpleTestSimulation())
            {
                Debug.WriteLine(line);
            }
#endif
        }

        private void ShowBattleLoadError(Exception ex)
        {
            TerminalMessageBox.Show(this, "Load Error: " + ex.Message, "Load Error", TerminalMessageType.Error);
        }

        private static void LoadPictureBoxImage(PictureBox pictureBox, string path, string description, string fallbackPath = null)
        {
            string selectedPath = path;
            if (!File.Exists(selectedPath))
            {
                LogAssetWarning(string.Format("{0} missing: {1}", description, selectedPath));

                if (!string.IsNullOrWhiteSpace(fallbackPath) && File.Exists(fallbackPath))
                {
                    selectedPath = fallbackPath;
                    LogAssetWarning(string.Format("Using fallback for {0}: {1}", description, selectedPath));
                }
                else
                {
                    return;
                }
            }

            pictureBox.Image = LoadImageCopy(selectedPath);
        }

        private void LoadCards(string folder, string prefix, params PictureBox[] boxes)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                string path = ResolveAssetPath("Assets", "Images", folder, "cards", string.Format("{0}_{1}.jpeg", prefix, i + 1));
                LoadPictureBoxImage(boxes[i], path, string.Format("{0} card {1}", folder, i + 1));

                if (folder == "player")
                {
                    RegisterPlayerCard(boxes[i], i + 1);
                }
            }
        }

        private void RegisterPlayerCard(PictureBox cardPictureBox, int cardId)
        {
            _cardIdByPicture[cardPictureBox] = cardId;
            _pictureByCardId[cardId] = cardPictureBox;
            cardPictureBox.Cursor = Cursors.Hand;

            if (!_wiredPlayerCardBoxes.Add(cardPictureBox))
            {
                return;
            }

            cardPictureBox.MouseEnter += PlayerCard_MouseEnter;
            cardPictureBox.Click += PlayerCard_Click;
        }

        private void PlayerCard_MouseEnter(object sender, EventArgs e)
        {
            PictureBox cardPictureBox = sender as PictureBox;
            if (cardPictureBox == null)
            {
                return;
            }

            int cardId;
            if (!_cardIdByPicture.TryGetValue(cardPictureBox, out cardId))
            {
                return;
            }

            if (_usedPlayerCards.Contains(cardId) || !_battleEngine.CanSelectCard(cardId))
            {
                return;
            }

            AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
        }

        /// <summary>
        /// Player turn:
        /// 1) select card
        /// 2) answer question
        /// 3) if correct -> player attack
        /// 4) if wrong -> card lock + enemy attack + forced retry on same card
        /// </summary>
        private void PlayerCard_Click(object sender, EventArgs e)
        {
            int selectedCardId;
            if (!TryGetPlayableCardId(sender, out selectedCardId))
            {
                return;
            }

            BattleArenaQuestionForm questionForm = OpenQuestionForm();
            if (questionForm == null)
            {
                return;
            }

            PlayerTurnResult turnResult = RunPlayerTurn(selectedCardId, questionForm);
            RefreshPlayerCardLockVisuals();
            ApplyPlayerTurnResult(turnResult);
        }

        private bool TryGetPlayableCardId(object sender, out int selectedCardId)
        {
            selectedCardId = 0;

            PictureBox card = sender as PictureBox;
            if (_battleEnded || _currentState != BattleState.IdleLoop || card == null)
            {
                return false;
            }

            if (!_cardIdByPicture.TryGetValue(card, out selectedCardId))
            {
                return false;
            }

            if (_usedPlayerCards.Contains(selectedCardId))
            {
                return false;
            }

            if (_battleEngine.CanSelectCard(selectedCardId))
            {
                return true;
            }

            ShowLockedCardWarning();
            return false;
        }

        private void ShowLockedCardWarning()
        {
            TerminalMessageBox.Show(
                this,
                string.Format("Card {0} is locked. Retry that card first.", _battleEngine.LockedCardId),
                "Locked Card",
                TerminalMessageType.Warning);
        }

        private BattleArenaQuestionForm OpenQuestionForm()
        {
            Question challenge = QuestionManager.Instance.GetRandomQuestion(Level);
            BattleArenaQuestionForm questionForm = new BattleArenaQuestionForm();

            // Wire up the OnTimerTick callback to keep BattleArenaForm's timer synchronized in real-time!
            questionForm.OnTimerTick = (timeText, timeColor) =>
            {
                lblTimer.Text = timeText;
                lblTimer.ForeColor = timeColor;
                lblTimer.Update(); // Force visual redraw instantly
            };

            questionForm.Populate(challenge, 1, 5);

            // Pause the idle threat timer while the question is open so it
            // does not expire silently in the background.
            double savedIdleElapsed = _idleTimeElapsedSeconds;

            DialogResult questionResult = questionForm.ShowDialog();

            // Restore the idle timer so the player keeps their remaining
            // threat time from before the question opened.
            _idleTimeElapsedSeconds = savedIdleElapsed;
            UpdateIdleTimerDisplay();

            if (questionResult == DialogResult.Cancel)
            {
                questionForm.Dispose();
                return null;
            }

            return questionForm;
        }

        private void ApplyPlayerTurnResult(PlayerTurnResult turnResult)
        {
            if (turnResult.PlayerAttacked)
            {
                StartPlayerAttack(turnResult.SelectedCardId);
                return;
            }

            StartEnemyAttack(turnResult.EnemyAttacks.Count);
        }

        private void StartPlayerAttack(int selectedCardId)
        {
            MarkCardAsUsed(selectedCardId);
            _playerActor.SetPositionX(_playerActor.IdleX);
            _animFrameIdx = 0;
            _checkBattleAfterAnimation = true;
            SetState(BattleState.PlayerAttacking);
        }

        private void StartEnemyAttack(int attackCount)
        {
            StartEnemyRetaliation(attackCount);
        }

        private PlayerTurnResult RunPlayerTurn(int selectedCardId, BattleArenaQuestionForm qForm)
        {
            if (qForm.SkipCommand == QuestionSkipCommandType.SkipAllQuestions)
            {
                return _battleEngine.SkipAllRemainingQuestions(selectedCardId);
            }

            if (qForm.SkipCommand == QuestionSkipCommandType.SkipCurrentQuestion)
            {
                return _battleEngine.SkipCurrentQuestion(selectedCardId);
            }

            return _battleEngine.PlayerTurn(selectedCardId, qForm.WasAnswerCorrect);
        }

        private void StartEnemyRetaliation(int attackCount)
        {
            if (attackCount <= 0)
            {
                HandleImmediateEnemyRetaliation();
                return;
            }

            _remainingEnemyRetaliationHits = attackCount;
            _pendingIncorrectAnswerPopup = true;
            _animFrameIdx = 0;
            _enemyActor.SetCurrentImage(_enemyActor.AttackFrames[0] ?? _enemyActor.CurrentImage);
            _enemyNeedsReturnAfterPlayerAttack = true;
            SetState(BattleState.EnemyAttacking);
        }

        private void HandleImmediateEnemyRetaliation()
        {
            UpdatePlayerHudFromEngine();
            if (_battleEngine.CheckBattleResult() != BattleResult.PlayerDefeat && _battleEngine.PlayerHP > 0)
            {
                ShowIncorrectAnswerMessage();
            }
            EvaluateBattleResult();
        }

        private bool ShouldAdvanceStateFrame(int ticksPerFrame)
        {
            _stateTickCounter++;
            if (_stateTickCounter < ticksPerFrame)
            {
                return false;
            }

            _stateTickCounter = 0;
            return true;
        }

        private void SetState(BattleState newState)
        {
            _currentState = newState;
            _stateTickCounter = 0;
            if (newState == BattleState.IdleLoop)
            {
                _idleTimeElapsedSeconds = 0.0;
            }
        }

        private void RenderActorsToPictureBoxes()
        {
            if (_battleEnded || IsDisposed || Disposing)
            {
                return;
            }

            if (_spriteCanvas.Width <= 0 || _spriteCanvas.Height <= 0)
            {
                return;
            }

            PrepareSpriteBuffer(_spriteCanvas.Size);
            if (_spriteBuffer == null || _spriteBufferGraphics == null)
            {
                return;
            }

            Point shake = GetShakeOffset();
            Graphics g = _spriteBufferGraphics;
            g.Clear(Color.Transparent);

            RenderBattleActor(g, _playerActor, shake);
            RenderBattleActor(g, _enemyActor, shake);

            _spriteCanvas.Invalidate();
            ApplyBackgroundTintOnly();
        }

        private static void RenderBattleActor(Graphics graphics, BattleActorController actor, Point shakeOffset)
        {
            if (actor.CurrentImage == null)
            {
                return;
            }

            Point drawPoint = new Point(actor.Position.X + shakeOffset.X, actor.Position.Y + shakeOffset.Y);
            graphics.DrawImage(actor.CurrentImage, new Rectangle(drawPoint, actor.RenderSize));
        }

        private void UpdateIdleTimer()
        {
            if (_battleEnded || IsDisposed || Disposing)
            {
                return;
            }

            _idleTimeElapsedSeconds += (AnimationTimerIntervalMs / 1000.0);

            double timeLeft = Math.Max(0.0, 15.0 - _idleTimeElapsedSeconds);
            UpdateIdleTimerDisplay();

            if (timeLeft <= 0.0)
            {
                _idleTimeElapsedSeconds = 0.0;

                // Play hurt audio-visual feedback to make it completely fair and clear
                AudioManager.Instance.PlaySFX(Constants.SFX_HIT);

                // Chip damage
                _battleEngine.ApplyChipDamageToPlayer(5);
                UpdatePlayerHudFromEngine();

                // Trigger a momentary background danger overlay flash
                _idleTimeoutFlashFrames = 15;

                EvaluateBattleResult();
            }
        }

        private void UpdateIdleTimerDisplay()
        {
            double timeLeft = Math.Max(0.0, 15.0 - _idleTimeElapsedSeconds);
            lblTimer.Text = string.Format("[THREAT: {0:00}s]", Math.Ceiling(timeLeft));

            if (timeLeft <= 5.0)
            {
                lblTimer.ForeColor = Color.FromArgb(255, 65, 65); // Cyber Red
            }
            else if (timeLeft <= 8.0)
            {
                lblTimer.ForeColor = Color.Yellow;
            }
            else
            {
                lblTimer.ForeColor = Color.FromArgb(0, 255, 65); // Matrix Green
            }
        }

        private Color GetBackgroundShadeColor()
        {
            if (_idleTimeoutFlashFrames > 0)
            {
                _idleTimeoutFlashFrames--;
                return Color.FromArgb(120, 200, 0, 0); // Danger Cyber Red
            }

            // Player attack phase: pure black from player attack start through enemy hurt end.
            if (_currentState == BattleState.PlayerAttacking || _currentState == BattleState.EnemyHurting)
            {
                return Color.FromArgb(255, 0, 0, 0);
            }

            // Enemy attack phase: shaded red danger overlay.
            if (_currentState == BattleState.EnemyAttacking || _currentState == BattleState.PlayerHurting || _currentState == BattleState.EnemyReturning)
            {
                return Color.FromArgb(120, 200, 0, 0);
            }

            return Color.Transparent;
        }

        private Point GetShakeOffset()
        {
            bool isCombatImpactState =
                _currentState == BattleState.PlayerAttacking ||
                _currentState == BattleState.EnemyHurting ||
                _currentState == BattleState.EnemyAttacking ||
                _currentState == BattleState.PlayerHurting;

            if (!isCombatImpactState)
            {
                return Point.Empty;
            }

            int intensity = _currentState == BattleState.PlayerAttacking || _currentState == BattleState.EnemyAttacking ? 8 : 5;
            int offsetX = _vfxRandom.Next(-intensity, intensity + 1);
            int offsetY = _vfxRandom.Next(-intensity, intensity + 1);
            return new Point(offsetX, offsetY);
        }

        private void ApplyBackgroundTintOnly()
        {
            UpdateBackgroundTintBounds();
            Color tint = GetBackgroundShadeColor();
            if (tint == _lastBackgroundTint)
            {
                return;
            }

            if (tint.A <= 0)
            {
                _backgroundTintLayer.Visible = false;
                _backgroundTintLayer.TintColor = Color.Transparent;
                _lastBackgroundTint = Color.Transparent;
                return;
            }

            bool wasVisible = _backgroundTintLayer.Visible;
            _backgroundTintLayer.TintColor = tint;
            _backgroundTintLayer.Visible = true;
            if (!wasVisible)
            {
                _backgroundTintLayer.SendToBack();
            }

            _lastBackgroundTint = tint;
        }

        private void PrepareSpriteBuffer(Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
            {
                DisposeSpriteBufferResources();
                return;
            }

            if (_spriteBuffer != null)
            {
                try
                {
                    if (_spriteBuffer.Size == size)
                    {
                        return;
                    }
                }
                catch (ObjectDisposedException)
                {
                    // Recreate disposed buffer below.
                }

                DisposeSpriteBufferResources();
            }

            _spriteBuffer = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            _spriteBufferGraphics = Graphics.FromImage(_spriteBuffer);
            _spriteBufferGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            _spriteBufferGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            _spriteBufferGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            _spriteCanvas.Image = _spriteBuffer;
        }

        private void DisposeSpriteBufferResources()
        {
            if (ReferenceEquals(_spriteCanvas.Image, _spriteBuffer))
            {
                _spriteCanvas.Image = null;
            }

            if (_spriteBufferGraphics != null) _spriteBufferGraphics.Dispose();
            _spriteBufferGraphics = null;

            if (_spriteBuffer != null) _spriteBuffer.Dispose();
            _spriteBuffer = null;
        }

        private void ShowIncorrectAnswerMessage()
        {
            const string incorrectAnswerMessage =
                "Incorrect answer. You cannot use any other card until you answer this card correctly. " +
                "The question for this card will be shuffled.";

            TerminalMessageBox.Show(this, incorrectAnswerMessage, "Incorrect Answer", TerminalMessageType.Warning);
        }

        private void EvaluateBattleResult()
        {
            if (_battleEnded)
            {
                return;
            }

            var result = _battleEngine.CheckBattleResult();
            if (result == BattleResult.Ongoing)
            {
                return;
            }

            _battleEnded = true;
            _animTimer.Stop();

            if (result == BattleResult.PlayerDefeat)
            {
                HandleBattleLose();
                return;
            }

            if (result == BattleResult.EnemyDefeat)
            {
                HandleBattleWin();
                return;
            }

            Close();
        }

        private void HandleBattleWin()
        {
            ProgressManager.Instance.CompleteLevel(Level);
            ShowFinalVent(playerWon: true);
        }

        private void HandleBattleLose()
        {
            ShowFinalVent(playerWon: false);
        }

        private void ShowFinalVent(bool playerWon)
        {
            FinalVentForm finalVent = new FinalVentForm(Level, playerWon);
            if (!FormTransitionManager.ShowChild(this, finalVent, () =>
            {
                if (playerWon && _levelConfig.OpensEpilogueOnWin)
                {
                    ShowEpilogue();
                    return false;
                }

                Close();
                return false;
            }))
            {
                finalVent.Dispose();
            }
        }

        private void ShowEpilogue()
        {
            AudioManager.Instance.StopMusic();
            var epilogue = new StoryForm(StoryScripts.CreateEpilogue());
            if (!FormTransitionManager.ShowChild(this, epilogue, () =>
            {
                Close();
                return false;
            }))
            {
                epilogue.Dispose();
                Close();
            }
        }

        private void SyncAllHudFromEngine()
        {
            UpdatePlayerHudFromEngine();
            UpdateEnemyHudFromEngine();
        }

        private void UpdatePlayerHudFromEngine()
        {
            lblPlayerHP.Text = _battleEngine.PlayerHP.ToString();
            int fullWidth = pnlPlayerHealthBg.Width;
            int newWidth = (int)((_battleEngine.PlayerHP / 100.0) * fullWidth);
            pnlPlayerHealthFill.Width = Math.Max(0, newWidth);

            if (_currentState != BattleState.IdleLoop)
            {
                lblTimer.Text = "[RIFT_SECURE]";
                lblTimer.ForeColor = Color.FromArgb(0, 255, 65); // Matrix Green
            }
        }

        private void UpdateEnemyHudFromEngine()
        {
            lblEnemyHP.Text = _battleEngine.EnemyHP.ToString();
            int fullWidth = pnlEnemyHealthBg.Width;
            int newWidth = (int)((_battleEngine.EnemyHP / 100.0) * fullWidth);
            pnlEnemyHealthFill.Width = Math.Max(0, newWidth);
        }

        private void RefreshPlayerCardLockVisuals()
        {
            foreach (var pair in _pictureByCardId)
            {
                int cardId = pair.Key;
                PictureBox card = pair.Value;

                if (_usedPlayerCards.Contains(cardId))
                {
                    card.BorderStyle = BorderStyle.Fixed3D;
                    card.Cursor = Cursors.No;
                    continue;
                }

                bool isLockedCard = _battleEngine.LockedCardId == cardId;
                bool isBlockedByLock = _battleEngine.LockedCardId.HasValue && !isLockedCard;

                card.BorderStyle = isLockedCard ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
                card.Cursor = isBlockedByLock ? Cursors.No : Cursors.Hand;
            }
        }

        private void MarkCardAsUsed(int cardId)
        {
            if (_usedPlayerCards.Contains(cardId))
            {
                return;
            }

            _usedPlayerCards.Add(cardId);
            PictureBox card;
            if (_pictureByCardId.TryGetValue(cardId, out card))
            {
                Image originalImage = card.Image;
                card.Image = CreateDarkenedImage(originalImage);
                if (originalImage != null) originalImage.Dispose();
                card.BorderStyle = BorderStyle.Fixed3D;
                card.Cursor = Cursors.No;
            }
        }

        private Image CreateDarkenedImage(Image source)
        {
            if (source == null)
            {
                return null;
            }

            Bitmap darkened = new Bitmap(source.Width, source.Height);
            using (Graphics g = Graphics.FromImage(darkened))
            using (ImageAttributes imageAttributes = new ImageAttributes())
            {
                // Reduce brightness heavily to make the card look "used".
                ColorMatrix matrix = new ColorMatrix(new float[][]
                {
                    new float[] { 0.20f, 0, 0, 0, 0 },
                    new float[] { 0, 0.20f, 0, 0, 0 },
                    new float[] { 0, 0, 0.20f, 0, 0 },
                    new float[] { 0, 0, 0, 1f, 0 },
                    new float[] { 0, 0, 0, 0, 1f }
                });

                imageAttributes.SetColorMatrix(matrix);
                g.DrawImage(
                    source,
                    new Rectangle(0, 0, darkened.Width, darkened.Height),
                    0,
                    0,
                    source.Width,
                    source.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);
            }

            return darkened;
        }

        private void btnBack_MouseEnter(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            CloseBattleArena();
        }

        private void CloseBattleArena()
        {
            _animTimer.Stop();
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _animTimer.Stop();
            _animTimer.Dispose();
            AudioManager.Instance.StopMusic();
            DisposeSpriteBufferResources();
            DisposeFrameSet(_playerActor.RunFrames);
            DisposeFrameSet(_enemyActor.RunFrames);
            DisposeFrameSet(_playerActor.IdleFrames);
            DisposeFrameSet(_enemyActor.IdleFrames);
            DisposeFrameSet(_playerActor.AttackFrames);
            DisposeFrameSet(_enemyActor.AttackFrames);
            DisposeFrameSet(_playerActor.HurtFrames);
            DisposeFrameSet(_enemyActor.HurtFrames);
            DisposeControlImages(
                picPlayerThumb,
                picEnemyThumb,
                picPlayerCard1,
                picPlayerCard2,
                picPlayerCard3,
                picPlayerCard4,
                picPlayerCard5,
                picEnemyCard1,
                picEnemyCard2,
                picEnemyCard3,
                picEnemyCard4,
                picEnemyCard5);
            if (BackgroundImage != null) BackgroundImage.Dispose();
            BackgroundImage = null;
            base.OnFormClosing(e);
        }

        private static void DisposeFrameSet(Image[] frames)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null) frames[i].Dispose();
                frames[i] = null;
            }
        }

        private static void DisposeControlImages(params PictureBox[] pictureBoxes)
        {
            foreach (PictureBox pictureBox in pictureBoxes)
            {
                Image image = pictureBox.Image;
                pictureBox.Image = null;
                if (image != null) image.Dispose();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateBackgroundTintBounds();
        }

        private void lblEnemyHP_Click(object sender, EventArgs e) { }
    }
}

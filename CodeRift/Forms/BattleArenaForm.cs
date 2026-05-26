using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CodeRift.Core;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    internal sealed class ScreenTintOverlay : Control
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TintColor { get; set; } = Color.Transparent;

        public ScreenTintOverlay()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            
            Enabled = false;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Prevent default background fill to keep overlay smooth.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using Brush brush = new SolidBrush(TintColor);
            e.Graphics.FillRectangle(brush, ClientRectangle);
        }
    }

    public partial class BattleArenaForm : Form
    {
        private const int AttackFrameCount = 8;
        private const int IdleFrameCount = 4;
        private const int AnimationTimerIntervalMs = 80;
        private const int ImpactFramesRemaining = 3;
        private const int GroundDropOffset = 64;
        private const int GroundVisiblePadding = 0;
        private const string DefaultEnemyAssetFolder = "enemy1";
        private const string DefaultEnemyPortraitFileName = "enemy_level_1.jpeg";
        private static readonly Dictionary<string, Image> BattleAssetCache = new(StringComparer.OrdinalIgnoreCase);
        private static readonly object BattleAssetCacheLock = new();

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

        // Frames
        private readonly Image?[] _playerRunFrames = new Image[AttackFrameCount];
        private readonly Image?[] _enemyRunFrames = new Image[AttackFrameCount];
        private readonly Image?[] _playerIdleFrames = new Image[IdleFrameCount];
        private readonly Image?[] _enemyIdleFrames = new Image[IdleFrameCount];
        private readonly Image?[] _playerAtkFrames = new Image[AttackFrameCount];
        private readonly Image?[] _enemyAtkFrames = new Image[AttackFrameCount];
        private readonly Image?[] _playerHurtFrames = new Image[AttackFrameCount];
        private readonly Image?[] _enemyHurtFrames = new Image[AttackFrameCount];

        private int _currentFrame = 0;
        private int _animFrameIdx = 0;
        private int _stateTickCounter = 0;
        private readonly System.Windows.Forms.Timer _animTimer = new();
        private bool _isAnimTimerWired;

        private Point _playerPos;
        private Point _enemyPos;
        private int _playerIdleX;
        private int _enemyIdleX;
        private int _playerIdleY;
        private int _enemyIdleY;
        private int _playerContactX;
        private int _enemyContactX;
        private Size _playerRenderSize;
        private Size _enemyRenderSize;
        private Image? _playerCurrentImg;
        private Image? _enemyCurrentImg;
        private bool _checkBattleAfterAnimation;
        private bool _battleEnded;
        private int _remainingEnemyRetaliationHits;
        private bool _pendingIncorrectAnswerPopup;
        private bool _enemyNeedsReturnAfterPlayerAttack;
        private readonly PictureBox _spriteCanvas = new();
        private readonly ScreenTintOverlay _backgroundTintLayer = new();
        private Bitmap? _spriteBuffer;
        private readonly Random _vfxRandom = new();

        // Card mapping for turn selection and lock validation.
        private readonly Dictionary<PictureBox, int> _cardIdByPicture = new();
        private readonly Dictionary<int, PictureBox> _pictureByCardId = new();
        private readonly HashSet<int> _usedPlayerCards = new();
        private readonly HashSet<PictureBox> _wiredPlayerCardBoxes = new();

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
            EnableDoubleBuffer(pnlBattleZone);

            picPlayerPortrait.Visible = false;
            picEnemyPortrait.Visible = false;
            SetupSpriteCanvas();
            SetupBackgroundTintLayer();
            ConfigureActorRenderSizes();

            SetupLevel();
            LoadAnimationAssets();
        }

        private void EnableDoubleBuffer(Control control)
        {
            var property = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            property?.SetValue(control, true, null);
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
            _playerRenderSize = picPlayerPortrait.Size;
            _enemyRenderSize = picEnemyPortrait.Size;

            if (_levelConfig.EnemyRenderScale != 1.0f)
            {
                _enemyRenderSize = new Size(
                    (int)Math.Round(_enemyRenderSize.Width * _levelConfig.EnemyRenderScale),
                    (int)Math.Round(_enemyRenderSize.Height * _levelConfig.EnemyRenderScale));
            }
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
            _backgroundTintLayer.Bounds = new Rectangle(Point.Empty, ClientSize);
        }

        private void LoadAnimationAssets()
        {
            try
            {
                LoadPlayerAnimationAssets();
                LoadEnemyAnimationAssets();

                // Normalize frame canvas sizes to prevent size/position snapping between sequences.
                NormalizeActorFrames(_playerRunFrames, _playerIdleFrames, _playerAtkFrames, _playerHurtFrames);
                NormalizeActorFrames(_enemyRunFrames, _enemyIdleFrames, _enemyAtkFrames, _enemyHurtFrames);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Asset Load Error: " + ex.Message);
            }
        }

        private void LoadPlayerAnimationAssets()
        {
            string playerPath = ResolveAssetPath("Assets", "Images", "player");

            LoadFrameSequence(Path.Combine(playerPath, "run"), "player_run", _playerRunFrames, applyTransparency: true, "player run");
            LoadFrameSequence(Path.Combine(playerPath, "attack"), "player_attack", _playerAtkFrames, applyTransparency: true, "player attack");
            LoadFrameSequence(Path.Combine(playerPath, "hurt"), "player_hurt", _playerHurtFrames, applyTransparency: false, "player hurt");
            LoadFrameSequence(Path.Combine(playerPath, "ide"), "player_ide", _playerIdleFrames, applyTransparency: false, "player idle");

            FillMissingFrames(_playerAtkFrames, _playerRunFrames);
            FillMissingFrames(_playerHurtFrames, _playerRunFrames);
            FillMissingFrames(_playerIdleFrames, _playerRunFrames);
        }

        private void LoadEnemyAnimationAssets()
        {
            string enemyRoot = ResolveAssetPath("Assets", "Images", "enemies");
            string enemyPath = Path.Combine(enemyRoot, _levelConfig.Enemy.AssetFolder);
            string enemyFolder = Directory.Exists(enemyPath) ? _levelConfig.Enemy.AssetFolder : DefaultEnemyAssetFolder;

            if (!StringComparer.OrdinalIgnoreCase.Equals(enemyFolder, _levelConfig.Enemy.AssetFolder))
            {
                LogAssetWarning($"Enemy asset folder '{_levelConfig.Enemy.AssetFolder}' was not found. Falling back to '{DefaultEnemyAssetFolder}'.");
            }

            enemyPath = Path.Combine(enemyRoot, enemyFolder);

            LoadFrameSequence(Path.Combine(enemyPath, "run"), $"{enemyFolder}_run", _enemyRunFrames, applyTransparency: false, $"{enemyFolder} run");
            LoadFrameSequence(Path.Combine(enemyPath, "attack"), $"{enemyFolder}_attack", _enemyAtkFrames, applyTransparency: true, $"{enemyFolder} attack");
            LoadFrameSequence(Path.Combine(enemyPath, "hurt"), $"{enemyFolder}_hurt", _enemyHurtFrames, applyTransparency: false, $"{enemyFolder} hurt");
            LoadFrameSequence(Path.Combine(enemyPath, "ide"), $"{enemyFolder}_ide", _enemyIdleFrames, applyTransparency: false, $"{enemyFolder} idle");

            FillMissingFrames(_enemyHurtFrames, _enemyRunFrames);
            FillMissingFrames(_enemyAtkFrames, _enemyRunFrames);
            FillMissingFrames(_enemyIdleFrames, _enemyRunFrames);
        }

        private static void LoadFrameSequence(string folderPath, string preferredPrefix, Image?[] targetFrames, bool applyTransparency, string sequenceName)
        {
            if (!Directory.Exists(folderPath))
            {
                LogAssetWarning($"Animation folder missing for {sequenceName}: {folderPath}");
                return;
            }

            bool foundPreferredFrames = false;
            for (int i = 0; i < targetFrames.Length; i++)
            {
                string preferredPath = Path.Combine(folderPath, $"{preferredPrefix}_{i + 1:D2}.png");
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
                LogAssetWarning($"No animation frames loaded for {sequenceName}: {folderPath}");
            }
        }

        private static string ResolveAssetPath(params string[] relativeSegments)
        {
            string relativePath = Path.Combine(relativeSegments);
            string outputPath = Path.Combine(Application.StartupPath, relativePath);
            if (File.Exists(outputPath) || Directory.Exists(outputPath))
            {
                return outputPath;
            }

            return Path.GetFullPath(Path.Combine(Application.StartupPath, "..", "..", "..", relativePath));
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
            using MemoryStream stream = new MemoryStream(bytes, writable: false);
            using Image loadedImage = Image.FromStream(stream);
            return new Bitmap(loadedImage);
        }

        private static Image GetCachedImage(string cacheKey, Func<Image> createImage)
        {
            lock (BattleAssetCacheLock)
            {
                if (!BattleAssetCache.TryGetValue(cacheKey, out Image? cachedImage))
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
            return int.TryParse(digits, out int value) ? value : int.MaxValue;
        }

        private static void FillMissingFrames(Image?[] targetFrames, Image?[] fallbackFrames)
        {
            for (int i = 0; i < targetFrames.Length; i++)
            {
                if (targetFrames[i] != null)
                {
                    continue;
                }

                Image? fallback = i < fallbackFrames.Length && fallbackFrames[i] != null
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
        private static void NormalizeActorFrames(params Image?[][] frameSets)
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
                    frameSet[i] = NormalizeFrameCanvas(frameSet[i]!, maxWidth, maxHeight);
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
            bool[,] visited = new bool[width, height];
            Queue<Point> queue = new Queue<Point>();

            // Seed flood-fill from image borders only. This removes background black
            // while preserving dark details inside the character.
            void TryEnqueue(int x, int y)
            {
                if (x < 0 || y < 0 || x >= width || y >= height || visited[x, y])
                {
                    return;
                }

                visited[x, y] = true;
                Color c = output.GetPixel(x, y);
                if (!IsNearBlack(c, threshold))
                {
                    return;
                }

                queue.Enqueue(new Point(x, y));
            }

            for (int x = 0; x < width; x++)
            {
                TryEnqueue(x, 0);
                TryEnqueue(x, height - 1);
            }
            for (int y = 0; y < height; y++)
            {
                TryEnqueue(0, y);
                TryEnqueue(width - 1, y);
            }

            int[] dx = new[] { 1, -1, 0, 0 };
            int[] dy = new[] { 0, 0, 1, -1 };

            while (queue.Count > 0)
            {
                Point p = queue.Dequeue();
                output.SetPixel(p.X, p.Y, Color.Transparent);

                for (int i = 0; i < 4; i++)
                {
                    int nx = p.X + dx[i];
                    int ny = p.Y + dy[i];
                    if (nx < 0 || ny < 0 || nx >= width || ny >= height || visited[nx, ny])
                    {
                        continue;
                    }

                    visited[nx, ny] = true;
                    Color nc = output.GetPixel(nx, ny);
                    if (IsNearBlack(nc, threshold))
                    {
                        queue.Enqueue(new Point(nx, ny));
                    }
                }
            }

            return output;
        }

        private static bool IsNearBlack(Color c, byte threshold)
        {
            return c.A > 0 && c.R <= threshold && c.G <= threshold && c.B <= threshold;
        }

        private void StartAnimations()
        {
            if (_isAnimTimerWired)
            {
                return;
            }

            // Align both actors by feet on the same baseline, while preserving animation states.
            int desiredGroundBaseline = Math.Max(picPlayerPortrait.Bottom, picEnemyPortrait.Bottom) + GroundDropOffset;
            int maxVisibleGroundBaseline = pnlBattleZone.Height - GroundVisiblePadding;
            int groundBaseline = Math.Min(desiredGroundBaseline, maxVisibleGroundBaseline);

            _playerIdleY = groundBaseline - _playerRenderSize.Height;
            _enemyIdleY = groundBaseline - _enemyRenderSize.Height;

            UpdateActorLayout();
            int playerTargetX = _playerIdleX;
            int enemyTargetX = _enemyIdleX;
            int runDistance = 600;

            _playerPos = new Point(playerTargetX - runDistance, _playerIdleY);
            _enemyPos = new Point(enemyTargetX + runDistance, _enemyIdleY);

            if (_playerRunFrames[0] != null) _playerCurrentImg = _playerRunFrames[0];
            if (_enemyRunFrames[0] != null) _enemyCurrentImg = _enemyRunFrames[0];

            _animTimer.Interval = AnimationTimerIntervalMs;
            _animTimer.Tick += AnimTimer_Tick;
            _isAnimTimerWired = true;
            _animTimer.Start();
            RenderActorsToPictureBoxes();
        }

        private void AnimTimer_Tick(object? sender, EventArgs e)
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
                    HandleIntroAnimation(_playerIdleX, _enemyIdleX);
                    break;

                case BattleState.IdleLoop:
                    HandleIdleAnimation();
                    break;

                case BattleState.PlayerAttacking:
                    HandlePlayerAttack();
                    break;

                case BattleState.EnemyHurting:
                    HandleEnemyHurt();
                    break;

                case BattleState.EnemyAttacking:
                    HandleEnemyAttack(_enemyContactX);
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
            _playerCurrentImg = _playerRunFrames[frameIdx];
            _enemyCurrentImg = _enemyRunFrames[frameIdx];

            bool pArrived = false;
            bool eArrived = false;

            if (_playerPos.X < pTarget)
            {
                _playerPos.X = Math.Min(pTarget, _playerPos.X + 25);
            }
            else
            {
                pArrived = true;
            }

            if (_enemyPos.X > eTarget)
            {
                _enemyPos.X = Math.Max(eTarget, _enemyPos.X - 25);
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
            if (_levelConfig.CenterActorsByWidth)
            {
                int totalWidth = _playerRenderSize.Width + _levelConfig.ActorIdleGap + _enemyRenderSize.Width;
                _playerIdleX = centerX - totalWidth / 2;
                _enemyIdleX = _playerIdleX + _playerRenderSize.Width + _levelConfig.ActorIdleGap;
            }
            else
            {
                _playerIdleX = centerX - 460;
                _enemyIdleX = centerX + 10;
            }

            _playerContactX = _enemyIdleX - _playerRenderSize.Width + _levelConfig.PlayerAttackContactOverlap;
            _enemyContactX = _playerIdleX + _playerRenderSize.Width - _levelConfig.EnemyAttackContactOverlap;
        }

        private void HandleIdleAnimation()
        {
            int ticksPerIdleFrame = 6;
            int frameIdx = (_currentFrame / ticksPerIdleFrame) % IdleFrameCount;
            _playerCurrentImg = _playerIdleFrames[frameIdx];
            _enemyCurrentImg = _enemyIdleFrames[frameIdx];
        }

        private void HandlePlayerAttack()
        {
            int frameIdx = _animFrameIdx;
            _playerCurrentImg = _playerAtkFrames[frameIdx];
            if (_playerPos.X < _playerContactX)
            {
                _playerPos.X = Math.Min(_playerContactX, _playerPos.X + 34);
            }

            if (!ShouldAdvanceStateFrame(2))
            {
                return;
            }

            _animFrameIdx++;
            int impactFrameStart = Math.Max(0, _playerAtkFrames.Length - ImpactFramesRemaining);
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
            _enemyCurrentImg = _enemyHurtFrames[frameIdx];

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
            _playerCurrentImg = _playerIdleFrames[frameIdx];

            if (_playerPos.X > _playerIdleX)
            {
                _playerPos.X = Math.Max(_playerIdleX, _playerPos.X - 30);
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
            _enemyCurrentImg = _enemyAtkFrames[frameIdx];
            if (_enemyPos.X > enemyContactX)
            {
                _enemyPos.X = Math.Max(enemyContactX, _enemyPos.X - 38);
            }

            if (!ShouldAdvanceStateFrame(2))
            {
                return;
            }

            _animFrameIdx++;
            int impactFrameStart = Math.Max(0, _enemyAtkFrames.Length - ImpactFramesRemaining);
            if (_animFrameIdx >= impactFrameStart)
            {
                _animFrameIdx = 0;
                SetState(BattleState.PlayerHurting);
            }
        }

        private void HandlePlayerHurt()
        {
            int frameIdx = _animFrameIdx;
            _playerCurrentImg = _playerHurtFrames[frameIdx];

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
            _enemyCurrentImg = _enemyIdleFrames[frameIdx];

            if (_enemyPos.X < _enemyIdleX)
            {
                _enemyPos.X = Math.Min(_enemyIdleX, _enemyPos.X + 30);
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
                ShowIncorrectAnswerMessage();
                EvaluateBattleResult();
            }
        }

        private void SetupLevel()
        {
            lblLevelTitle.Text = $"// LEVEL {Level} : {_levelConfig.EnemyName} //";
            lblEnemyName.Text = _levelConfig.EnemyName;
        }

        private void BattleArenaForm_Load(object sender, EventArgs e)
        {
            try
            {
                string bgPath = ResolveAssetPath("Assets", "Images", "backgrounds", "level_background", $"level_{Level}.png");
                if (File.Exists(bgPath))
                {
                    BackgroundImage = LoadImageCopy(bgPath);
                    BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    LogAssetWarning($"Background image missing for level {Level}: {bgPath}");
                }

                string playerPath = ResolveAssetPath("Assets", "Images", "portraits", "player.jpeg");
                string enemyPath = ResolveAssetPath("Assets", "Images", "portraits", _levelConfig.Enemy.PortraitFileName);
                string fallbackEnemyPath = ResolveAssetPath("Assets", "Images", "portraits", DefaultEnemyPortraitFileName);
                LoadPictureBoxImage(picPlayerThumb, playerPath, "player portrait");
                LoadPictureBoxImage(picEnemyThumb, enemyPath, $"{_levelConfig.Enemy.Name} portrait", fallbackEnemyPath);

                LoadCards("player", "player_card", picPlayerCard1, picPlayerCard2, picPlayerCard3, picPlayerCard4, picPlayerCard5);
                LoadCards("enemies", "enemy_card", picEnemyCard1, picEnemyCard2, picEnemyCard3, picEnemyCard4, picEnemyCard5);
                SyncAllHudFromEngine();
                RefreshPlayerCardLockVisuals();
                StartAnimations();

                AudioManager.Instance.PlayMusic(Constants.MUSIC_LEVELS);
#if DEBUG
                foreach (var line in QuizBattleEngine.RunSimpleTestSimulation())
                {
                    Debug.WriteLine(line);
                }
#endif
            }
            catch (Exception ex)
            {
                TerminalMessageBox.Show(this, "Load Error: " + ex.Message, "Load Error", TerminalMessageType.Error);
            }
        }

        private static void LoadPictureBoxImage(PictureBox pictureBox, string path, string description, string? fallbackPath = null)
        {
            string selectedPath = path;
            if (!File.Exists(selectedPath))
            {
                LogAssetWarning($"{description} missing: {selectedPath}");

                if (!string.IsNullOrWhiteSpace(fallbackPath) && File.Exists(fallbackPath))
                {
                    selectedPath = fallbackPath;
                    LogAssetWarning($"Using fallback for {description}: {selectedPath}");
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
                string path = ResolveAssetPath("Assets", "Images", folder, "cards", $"{prefix}_{i + 1}.jpeg");
                LoadPictureBoxImage(boxes[i], path, $"{folder} card {i + 1}");

                if (folder == "player")
                {
                    int cardId = i + 1;
                    _cardIdByPicture[boxes[i]] = cardId;
                    _pictureByCardId[cardId] = boxes[i];
                    boxes[i].Cursor = Cursors.Hand;

                    if (_wiredPlayerCardBoxes.Add(boxes[i]))
                    {
                        boxes[i].MouseEnter += (s, e) => {
                            if (s is PictureBox pb && _cardIdByPicture.TryGetValue(pb, out int id)) {
                                if (!_usedPlayerCards.Contains(id) && _battleEngine.CanSelectCard(id)) {
                                    AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
                                }
                            }
                        };
                        boxes[i].Click += PlayerCard_Click;
                    }
                }
            }
        }

        /// <summary>
        /// Player turn:
        /// 1) select card
        /// 2) answer question
        /// 3) if correct -> player attack
        /// 4) if wrong -> card lock + enemy attack + forced retry on same card
        /// </summary>
        private void PlayerCard_Click(object? sender, EventArgs e)
        {
            if (_battleEnded || sender is not PictureBox card || _currentState != BattleState.IdleLoop)
            {
                return;
            }

            if (!_cardIdByPicture.TryGetValue(card, out int selectedCardId))
            {
                return;
            }

            if (_usedPlayerCards.Contains(selectedCardId))
            {
                return;
            }

            if (!_battleEngine.CanSelectCard(selectedCardId))
            {
                TerminalMessageBox.Show(
                    this,
                    $"Card {_battleEngine.LockedCardId} is locked. Retry that card first.",
                    "Locked Card",
                    TerminalMessageType.Warning);
                return;
            }

            var challenge = QuestionManager.Instance.GetRandomQuestion(Level);
            using BattleArenaQuestionForm qForm = new BattleArenaQuestionForm();
            qForm.Populate(challenge, 1, 5);
            var questionResult = qForm.ShowDialog();
            if (questionResult == DialogResult.Cancel)
            {
                return;
            }

            PlayerTurnResult turnResult = RunPlayerTurn(selectedCardId, qForm);
            RefreshPlayerCardLockVisuals();

            if (turnResult.PlayerAttacked)
            {
                MarkCardAsUsed(turnResult.SelectedCardId);
                _playerPos.X = _playerIdleX;
                _animFrameIdx = 0;
                _checkBattleAfterAnimation = true;
                SetState(BattleState.PlayerAttacking);
            }
            else
            {
                StartEnemyRetaliation(turnResult.EnemyAttacks.Count);
            }
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
                UpdatePlayerHudFromEngine();
                ShowIncorrectAnswerMessage();
                EvaluateBattleResult();
                return;
            }

            _remainingEnemyRetaliationHits = attackCount;
            _pendingIncorrectAnswerPopup = true;
            _animFrameIdx = 0;
            _enemyCurrentImg = _enemyAtkFrames[0] ?? _enemyCurrentImg;
            _enemyNeedsReturnAfterPlayerAttack = true;
            SetState(BattleState.EnemyAttacking);
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

            EnsureSpriteBufferSize(_spriteCanvas.Size);
            if (_spriteBuffer == null)
            {
                return;
            }

            Point shake = GetShakeOffset();
            using (Graphics g = Graphics.FromImage(_spriteBuffer))
            {
                g.Clear(Color.Transparent);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                if (_playerCurrentImg != null)
                {
                    Point playerDraw = new Point(_playerPos.X + shake.X, _playerPos.Y + shake.Y);
                    g.DrawImage(_playerCurrentImg, new Rectangle(playerDraw, _playerRenderSize));
                }

                if (_enemyCurrentImg != null)
                {
                    Point enemyDraw = new Point(_enemyPos.X + shake.X, _enemyPos.Y + shake.Y);
                    g.DrawImage(_enemyCurrentImg, new Rectangle(enemyDraw, _enemyRenderSize));
                }
            }

            _spriteCanvas.Image = _spriteBuffer;
            _spriteCanvas.Invalidate();
            ApplyBackgroundTintOnly();
        }

        private Color GetBackgroundShadeColor()
        {
            // Player attack phase: 80% black from player attack start through enemy hurt end.
            if (_currentState == BattleState.PlayerAttacking || _currentState == BattleState.EnemyHurting)
            {
                return Color.FromArgb(204, 0, 0, 0);
            }

            // Enemy attack phase: red danger shade.
            if (_currentState == BattleState.EnemyAttacking || _currentState == BattleState.PlayerHurting || _currentState == BattleState.EnemyReturning)
            {
                return Color.FromArgb(105, 150, 0, 0);
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
            if (tint.A <= 0)
            {
                _backgroundTintLayer.Visible = false;
                _backgroundTintLayer.TintColor = Color.Transparent;
                return;
            }

            _backgroundTintLayer.TintColor = tint;
            _backgroundTintLayer.Visible = true;
            _backgroundTintLayer.SendToBack();
        }

        private void EnsureSpriteBufferSize(Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
            {
                if (_spriteBuffer != null)
                {
                    if (ReferenceEquals(_spriteCanvas.Image, _spriteBuffer))
                    {
                        _spriteCanvas.Image = null;
                    }

                    _spriteBuffer.Dispose();
                    _spriteBuffer = null;
                }

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

                if (ReferenceEquals(_spriteCanvas.Image, _spriteBuffer))
                {
                    _spriteCanvas.Image = null;
                }

                _spriteBuffer.Dispose();
                _spriteBuffer = null;
            }

            _spriteBuffer = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
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
                ShowFinalVent(playerWon: false);
                return;
            }
            else if (result == BattleResult.EnemyDefeat)
            {
                ProgressManager.Instance.CompleteLevel(Level);
                ShowFinalVent(playerWon: true);
                return;
            }

            Close();
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
            if (_pictureByCardId.TryGetValue(cardId, out PictureBox? card))
            {
                Image? originalImage = card.Image;
                card.Image = CreateDarkenedImage(originalImage);
                originalImage?.Dispose();
                card.BorderStyle = BorderStyle.Fixed3D;
                card.Cursor = Cursors.No;
            }
        }

        private Image? CreateDarkenedImage(Image? source)
        {
            if (source == null)
            {
                return null;
            }

            Bitmap darkened = new Bitmap(source.Width, source.Height);
            using Graphics g = Graphics.FromImage(darkened);
            using ImageAttributes imageAttributes = new ImageAttributes();

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

            return darkened;
        }

        private void btnBack_MouseEnter(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            _animTimer.Stop();
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _animTimer.Stop();
            _animTimer.Dispose();
            AudioManager.Instance.StopMusic();
            if (ReferenceEquals(_spriteCanvas.Image, _spriteBuffer))
            {
                _spriteCanvas.Image = null;
            }
            _spriteBuffer?.Dispose();
            DisposeFrameSet(_playerRunFrames);
            DisposeFrameSet(_enemyRunFrames);
            DisposeFrameSet(_playerIdleFrames);
            DisposeFrameSet(_enemyIdleFrames);
            DisposeFrameSet(_playerAtkFrames);
            DisposeFrameSet(_enemyAtkFrames);
            DisposeFrameSet(_playerHurtFrames);
            DisposeFrameSet(_enemyHurtFrames);
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
            BackgroundImage?.Dispose();
            BackgroundImage = null;
            base.OnFormClosing(e);
        }

        private static void DisposeFrameSet(Image?[] frames)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i]?.Dispose();
                frames[i] = null;
            }
        }

        private static void DisposeControlImages(params PictureBox[] pictureBoxes)
        {
            foreach (PictureBox pictureBox in pictureBoxes)
            {
                Image? image = pictureBox.Image;
                pictureBox.Image = null;
                image?.Dispose();
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

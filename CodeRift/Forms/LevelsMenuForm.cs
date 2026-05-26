using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using CodeRift.Core;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    // Level selection gate: unlock state comes from ProgressManager.
    public partial class LevelsMenuForm : Form
    {
        private const int BackgroundFadeIntervalMs = 16;
        private const float BackgroundFadeStep = 0.08f;
        private const int LevelBackgroundShadeAlpha = 150;

        private static readonly Color MatrixGreen = Color.FromArgb(0, 255, 65);

        private readonly System.Windows.Forms.Timer _backgroundFadeTimer = new System.Windows.Forms.Timer();
        private readonly HashSet<Button> _wiredButtons = new HashSet<Button>();
        private readonly Dictionary<Button, LevelButtonInfo> _buttonInfo = new Dictionary<Button, LevelButtonInfo>();

        private Image? _currentBackground;
        private readonly Dictionary<string, Image> _preScaledBackgrounds = new Dictionary<string, Image>();
        private int _cachedScaleWidth;
        private int _cachedScaleHeight;
        private Image? _nextBackground;
        private Bitmap? _transitionBaseBackground;
        private string _currentBackgroundKey = Constants.IMG_BG_MENU;
        private string _targetBackgroundKey = Constants.IMG_BG_MENU;
        private float _backgroundFade;
        private float _currentShadeAlpha;
        private float _startShadeAlpha;
        private float _targetShadeAlpha;

        private sealed class LevelButtonInfo
        {
            public LevelButtonInfo(int level, string text, string backgroundKey)
            {
                Level = level;
                Text = text;
                BackgroundKey = backgroundKey;
            }

            public int Level { get; }

            public string Text { get; }

            public string BackgroundKey { get; }
        }

        public LevelsMenuForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            ConfigureWindow();
            PreScaleBackgrounds();
            BackgroundImage = null;
            ConfigureBackgroundFadeTimer();
            ConfigureTitleLabel();
            UpdateLevelButtons();
            StyleBackButton();

            // Enable double buffering recursively on all components to prevent flickering.
            EnableDoubleBuffer(this);
        }

        private void PreScaleBackgrounds()
        {
            int targetWidth = ClientSize.Width;
            int targetHeight = ClientSize.Height;

            if (targetWidth <= 0 || targetHeight <= 0)
            {
                Size screenSize = Screen.PrimaryScreen?.Bounds.Size ?? new Size(1920, 1080);
                targetWidth = screenSize.Width;
                targetHeight = screenSize.Height;
            }

            if (_cachedScaleWidth == targetWidth && _cachedScaleHeight == targetHeight && _preScaledBackgrounds.Count > 0)
            {
                return;
            }

            _cachedScaleWidth = targetWidth;
            _cachedScaleHeight = targetHeight;

            foreach (var kvp in _preScaledBackgrounds.Values)
            {
                kvp.Dispose();
            }
            _preScaledBackgrounds.Clear();

            string[] keys = {
                Constants.IMG_BG_MENU,
                Constants.IMG_BG_LEVEL1,
                Constants.IMG_BG_LEVEL2,
                Constants.IMG_BG_LEVEL3,
                Constants.IMG_BG_LEVEL4,
                Constants.IMG_BG_LEVEL5
            };

            foreach (string key in keys)
            {
                Image? original = ImageManager.Instance.GetImage(key);
                if (original != null)
                {
                    Bitmap scaled = new Bitmap(targetWidth, targetHeight);
                    using (Graphics g = Graphics.FromImage(scaled))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.DrawImage(original, 0, 0, targetWidth, targetHeight);
                    }
                    _preScaledBackgrounds[key] = scaled;
                }
            }

            _currentBackground = GetScaledBackground(_currentBackgroundKey);
        }

        private Image? GetScaledBackground(string key)
        {
            if (_preScaledBackgrounds.TryGetValue(key, out Image? scaled))
            {
                return scaled;
            }
            return ImageManager.Instance.GetImage(key);
        }

        private void EnableDoubleBuffer(Control control)
        {
            var property = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            property?.SetValue(control, true, null);

            foreach (Control child in control.Controls)
            {
                EnableDoubleBuffer(child);
            }
        }

        private void ConfigureWindow()
        {
            SetupFullScreen();
            BackColor = Color.FromArgb(13, 13, 13);
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private void SetupFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            if (Screen.PrimaryScreen != null)
            {
                this.Bounds = Screen.PrimaryScreen.Bounds;
            }
        }

        private void ConfigureBackgroundFadeTimer()
        {
            _backgroundFadeTimer.Interval = BackgroundFadeIntervalMs;
            _backgroundFadeTimer.Tick += BackgroundFadeTimer_Tick;
        }

        private void ConfigureTitleLabel()
        {
            lblTitle.Text = "LEVELS";
            lblTitle.Font = new Font("Courier New", 72, FontStyle.Bold);
            lblTitle.ForeColor = MatrixGreen;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void UpdateLevelButtons()
        {
            SetupButton(btnLevel1, new LevelButtonInfo(1, "LEVEL 1: LOOPS", Constants.IMG_BG_LEVEL1));
            SetupButton(btnLevel2, new LevelButtonInfo(2, "LEVEL 2: METHODS", Constants.IMG_BG_LEVEL2));
            SetupButton(btnLevel3, new LevelButtonInfo(3, "LEVEL 3: STRINGS", Constants.IMG_BG_LEVEL3));
            SetupButton(btnLevel4, new LevelButtonInfo(4, "LEVEL 4: ARRAYS", Constants.IMG_BG_LEVEL4));
            SetupButton(btnLevel5, new LevelButtonInfo(5, "LEVEL 5: CLASSES", Constants.IMG_BG_LEVEL5));
        }

        private void SetupButton(Button button, LevelButtonInfo info)
        {
            _buttonInfo[button] = info;

            bool isUnlocked = ProgressManager.Instance.IsLevelUnlocked(info.Level);
            StyleLevelButton(button, info, isUnlocked);

            if (_wiredButtons.Contains(button))
            {
                return;
            }

            _wiredButtons.Add(button);
            button.MouseEnter += LevelButton_MouseEnter;
            button.MouseLeave += LevelButton_MouseLeave;
            button.Click += LevelButton_Click;
        }

        private void StyleLevelButton(Button button, LevelButtonInfo info, bool isUnlocked)
        {
            if (isUnlocked)
            {
                button.Enabled = true;
                button.Cursor = Cursors.Hand;
                MenuButtonStyle.Apply(button, info.Text);
                return;
            }

            ApplyLockedLevelStyle(button, info.Level);
        }

        private void StyleBackButton()
        {
            MenuButtonStyle.Apply(btnBack, "[BACK]");
            btnBack.Size = new Size(200, 50);
            btnBack.Click += BackButton_Click;
        }

        private Button? _hoveredLevelButton;

        private void LevelButton_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is not Button button || !_buttonInfo.TryGetValue(button, out LevelButtonInfo? info))
            {
                return;
            }

            _hoveredLevelButton = button;
            BeginBackgroundFade(info.BackgroundKey);
        }

        private void LevelButton_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button button && _hoveredLevelButton == button)
            {
                _hoveredLevelButton = null;
            }
        
            BeginInvoke(new Action(() =>
            {
                if (!IsDisposed && !IsPointerOverAnyLevelButton())
                {
                    BeginBackgroundFade(Constants.IMG_BG_MENU);
                }
            }));
        }

        private void LevelButton_Click(object? sender, EventArgs e)
        {
            if (FormTransitionManager.IsTransitioning ||
                sender is not Button button ||
                !_buttonInfo.TryGetValue(button, out LevelButtonInfo? info))
            {
                return;
            }

            if (!IsLevelUnlocked(info.Level))
            {
                TerminalMessageBox.Show(
                    this,
                    $"Complete Level {info.Level - 1} to unlock this level.",
                    "Level Locked",
                    TerminalMessageType.Warning);
                return;
            }

            OpenLevel(info.Level);
        }

        private static bool IsLevelUnlocked(int level)
        {
            return ProgressManager.Instance.IsLevelUnlocked(level);
        }


        private static void ApplyLockedLevelStyle(Button button, int level)
        {
            MenuButtonStyle.ApplyLocked(button, $"[LOCKED] LEVEL {level}");
        }

        private void BackButton_Click(object? sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            Close();
        }

        private void OpenLevel(int level)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            BattleLoaderForm loader = new BattleLoaderForm(level);

            loader.OnComplete = () =>
            {
                BattleArenaForm arena = new BattleArenaForm(level);
                FormTransitionManager.ForceEndTransition(this);
                LaunchLevel(arena);

                // Seamlessly close the loader form after the Battle Arena has fully faded in (200ms)
                Task.Delay(200).ContinueWith(_ =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (loader is { IsDisposed: false, Disposing: false })
                        {
                            loader.Close();
                        }
                    }));
                });
            };

            bool shown = FormTransitionManager.ShowChild(this, loader, () =>
            {
                if (loader.LoadSuccessful)
                {
                    return false;
                }

                UpdateLevelButtons();
                return true;
            });

            if (!shown)
            {
                loader.Dispose();
            }
        }

        private void BeginBackgroundFade(string backgroundKey)
        {
            if (string.Equals(_targetBackgroundKey, backgroundKey, StringComparison.OrdinalIgnoreCase) ||
                (!_backgroundFadeTimer.Enabled && string.Equals(_currentBackgroundKey, backgroundKey, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            Image? background = GetScaledBackground(backgroundKey) ?? GetScaledBackground(Constants.IMG_BG_MENU);
            if (background == null)
            {
                return;
            }

            if (_backgroundFadeTimer.Enabled)
            {
                Bitmap? blendedBackground = CaptureCurrentBackgroundBlend();
                if (blendedBackground != null)
                {
                    ReplaceTransitionBaseBackground(blendedBackground);
                    _currentBackground = _transitionBaseBackground;
                    _currentBackgroundKey = "__TRANSITION__";
                    _currentShadeAlpha = GetCurrentInterpolatedShadeAlpha();
                }
            }

            _targetBackgroundKey = backgroundKey;
            _nextBackground = background;
            _backgroundFade = 0f;
            _startShadeAlpha = _currentShadeAlpha;
            _targetShadeAlpha = GetShadeAlpha(backgroundKey);
            _backgroundFadeTimer.Start();
            Invalidate();
        }

        private void BackgroundFadeTimer_Tick(object? sender, EventArgs e)
        {
            _backgroundFade += BackgroundFadeStep;
            if (_backgroundFade >= 1f)
            {
                _backgroundFade = 1f;
                _currentBackground = _nextBackground;
                _currentBackgroundKey = _targetBackgroundKey;
                _nextBackground = null;
                _currentShadeAlpha = _targetShadeAlpha;
                _startShadeAlpha = _currentShadeAlpha;
                _backgroundFadeTimer.Stop();
                ReplaceTransitionBaseBackground(null);
            }
            else
            {
                _currentShadeAlpha = GetCurrentInterpolatedShadeAlpha();
            }

            Invalidate();
        }

        private bool IsPointerOverAnyLevelButton()
        {
            foreach (Button button in _buttonInfo.Keys)
            {
                Point clientPoint = button.PointToClient(Cursor.Position);
                if (button.ClientRectangle.Contains(clientPoint))
                {
                    return true;
                }
            }

            return false;
        }

        private Bitmap? CaptureCurrentBackgroundBlend()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0 || _currentBackground == null)
            {
                return null;
            }

            Bitmap blended = new Bitmap(ClientSize.Width, ClientSize.Height);
            using Graphics graphics = Graphics.FromImage(blended);
            graphics.Clear(Color.FromArgb(13, 13, 13));
            DrawBackgroundImage(graphics, _currentBackground, 1f);

            if (_nextBackground != null && _backgroundFade > 0f)
            {
                DrawBackgroundImage(graphics, _nextBackground, _backgroundFade);
            }

            return blended;
        }

        private void ReplaceTransitionBaseBackground(Bitmap? background)
        {
            if (_transitionBaseBackground != null && !ReferenceEquals(_transitionBaseBackground, background))
            {
                _transitionBaseBackground.Dispose();
            }

            _transitionBaseBackground = background;
        }

        private float GetCurrentInterpolatedShadeAlpha()
        {
            return _startShadeAlpha + ((_targetShadeAlpha - _startShadeAlpha) * Math.Max(0f, Math.Min(1f, _backgroundFade)));
        }

        private static float GetShadeAlpha(string backgroundKey)
        {
            return string.Equals(backgroundKey, Constants.IMG_BG_MENU, StringComparison.OrdinalIgnoreCase)
                ? 0f
                : LevelBackgroundShadeAlpha;
        }

        private void LaunchLevel(Form levelForm)
        {
            if (!FormTransitionManager.ShowChild(this, levelForm, () =>
            {
                if (Tag?.ToString() == "EXIT_TO_MENU")
                {
                    Close();
                    return false;
                }

                UpdateLevelButtons();
                return true;
            }))
            {
                levelForm.Dispose();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterControls();
            if (Width > 0 && Height > 0)
            {
                PreScaleBackgrounds();
            }
        }

        private void CenterControls()
        {
            lblTitle.Size = new Size(Width, 150);
            lblTitle.Location = new Point(0, 50);

            int btnWidth = 600;
            int btnHeight = 80;
            int gap = 20;
            int startY = lblTitle.Bottom + 50;

            Button[] buttons = { btnLevel1, btnLevel2, btnLevel3, btnLevel4, btnLevel5 };
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Size = new Size(btnWidth, btnHeight);
                buttons[i].Location = new Point((Width - btnWidth) / 2, startY + i * (btnHeight + gap));
            }

            btnBack.Size = new Size(200, 50);
            btnBack.Location = new Point(50, 50);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(13, 13, 13));
            DrawBackgroundImage(e.Graphics, _currentBackground, 1f);

            if (_nextBackground != null && _backgroundFade > 0f)
            {
                DrawBackgroundImage(e.Graphics, _nextBackground, _backgroundFade);
            }

            int shadeAlpha = (int)Math.Round(_backgroundFadeTimer.Enabled ? GetCurrentInterpolatedShadeAlpha() : _currentShadeAlpha);
            if (shadeAlpha > 0)
            {
                using SolidBrush brush = new SolidBrush(Color.FromArgb(Math.Min(255, shadeAlpha), 0, 0, 0));
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        private void DrawBackgroundImage(Graphics graphics, Image? image, float alpha)
        {
            if (image == null)
            {
                return;
            }

            using ImageAttributes attributes = new ImageAttributes();
            ColorMatrix matrix = new ColorMatrix { Matrix33 = Math.Max(0f, Math.Min(1f, alpha)) };
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(
                image,
                ClientRectangle,
                0,
                0,
                image.Width,
                image.Height,
                GraphicsUnit.Pixel,
                attributes);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _backgroundFadeTimer.Stop();
            ReplaceTransitionBaseBackground(null);

            foreach (var kvp in _preScaledBackgrounds)
            {
                kvp.Value.Dispose();
            }
            _preScaledBackgrounds.Clear();

            base.OnFormClosed(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                SyncMenuVisualState();
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            SyncMenuVisualState();
        }

        private void SyncMenuVisualState()
        {
            CenterControls();

            Button[] buttons = { btnLevel1, btnLevel2, btnLevel3, btnLevel4, btnLevel5, btnBack };
            foreach (Button btn in buttons)
            {
                MenuButtonStyle.SyncHoverVisualState(btn);
            }
        }
    }
}

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeRift.Core;
using CodeRift.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift
{
    public partial class Form1 : Form
    {
        private Image? _backgroundImage;
        private Image? _titleImage;
        private readonly System.Windows.Forms.Timer _animationTimer = new System.Windows.Forms.Timer();
        private readonly System.Diagnostics.Stopwatch _splashTimer = new System.Diagnostics.Stopwatch();
        private double _displayPercent;
        private int _targetPercent;
        private bool _fadeOutRequested;
        private bool _transitionStarted;

        public Form1()
        {
            InitializeComponent();
            LanguageManager.Instance.Load(Constants.LANG_EN);
            Opacity = 0;
            _animationTimer.Interval = 16;
            _animationTimer.Tick += AnimationTimer_Tick;
            AssetBootstrapper.LoadSplashArtwork();
            ApplySplashArtwork();
            ClientSize = new Size(Constants.SPLASH_WIDTH, Constants.SPLASH_HEIGHT);
            percentLabel.Text = "0%";
            logLabel.Text = LanguageManager.Instance.Get("loading");
            progressFill.Width = 0;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _animationTimer.Start();
            _splashTimer.Restart();
            await LoadSplashAsync();
        }

        private async Task LoadSplashAsync()
        {
            SetStatus(LanguageManager.Instance.Get("loading"));

            var progress = new Progress<AssetLoadProgress>(UpdateProgress);
            await AssetBootstrapper.LoadAllAsync(progress);

            _targetPercent = 100;
            SetStatus(LanguageManager.Instance.Get("loading_done"));

            while (_splashTimer.ElapsedMilliseconds < Constants.SPLASH_MIN_MS || _displayPercent < 99.5)
            {
                await Task.Delay(16);
            }

            await Task.Delay(150);
            _fadeOutRequested = true;
        }

        private void ApplySplashArtwork()
        {
            Image? splashBackground = ImageManager.Instance.GetImage(AssetBootstrapper.SplashBackgroundKey);
            Image? splashTitle = ImageManager.Instance.GetImage(AssetBootstrapper.SplashTitleKey);

            if (splashBackground != null)
            {
                _backgroundImage?.Dispose();
                _backgroundImage = new Bitmap(splashBackground);
                BackgroundImage = _backgroundImage;
                BackgroundImageLayout = ImageLayout.Stretch;
            }

            if (splashTitle != null)
            {
                _titleImage?.Dispose();
                _titleImage = new Bitmap(splashTitle);
                titleBox.Image = _titleImage;
            }
        }

        private void UpdateProgress(AssetLoadProgress progress)
        {
            _targetPercent = progress.Percent;
            SetStatus(progress.Message);
        }

        private void SetStatus(string message)
        {
            logLabel.Text = message;
            logLabel.ForeColor = Color.FromArgb(0, 255, 65);
            logLabel.Refresh();
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            if (Opacity < 1.0 && !_fadeOutRequested)
            {
                Opacity = Math.Min(1.0, Opacity + 0.08);
            }
            else if (_fadeOutRequested)
            {
                Opacity = Math.Max(0.0, Opacity - 0.08);
                if (Opacity <= 0.0 && !_transitionStarted)
                {
                    _transitionStarted = true;
                    _animationTimer.Stop();
                    TransitionToNextForm();
                    return;
                }
            }

            double delta = _targetPercent - _displayPercent;
            if (Math.Abs(delta) > 0.1)
            {
                _displayPercent += delta * 0.15;
            }
            else
            {
                _displayPercent = _targetPercent;
            }

            int shownPercent = (int)Math.Round(_displayPercent);
            percentLabel.Text = $"{shownPercent}%";
            progressFill.Width = (int)Math.Round(396f * _displayPercent / 100f);
        }

        private void TransitionToNextForm()
        {
            this.Hide();
            // Using the existing 'menu' form as the MainMenuForm replacement
            menu mainMenu = new menu();
            mainMenu.FormClosed += (s, args) => this.Close();
            mainMenu.Show();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _animationTimer.Stop();
            _backgroundImage?.Dispose();
            _titleImage?.Dispose();
            base.OnFormClosed(e);
        }
    }
}

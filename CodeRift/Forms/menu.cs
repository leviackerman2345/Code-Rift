
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;
using CodeRift.Core;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    // Main menu hub: routes player into story/levels/settings/credits.
    public partial class MenuForm : Form
    {
        private Image? _backgroundImage;

        public MenuForm()

        {
            InitializeComponent();
            LoadAssets();
            SetupFullScreen();
            SetupRendering();
            SetupButtonHovers();
            ApplyLanguage();
            btnPlay.Click += btnPlay_Click;
        }

        private void btnPlay_Click(object? sender, EventArgs e)
        {
            OpenPrologue();
        }

        private void SetupButtonHovers()
        {
            if (buttonContainer != null)
            {
                foreach (Control ctrl in buttonContainer.Controls)
                {
                    if (ctrl is Button btn)
                    {
                        MenuButtonStyle.Apply(btn, btn.Text, useMenuSize: true, playClickSound: true, useHoverImage: true);
                    }
                }
            }

            SyncMenuVisualState();
        }

        private void SetupRendering()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            if (buttonContainer != null)
            {
                typeof(Control)
                    .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.SetValue(buttonContainer, true, null);
            }
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

        private void LoadAssets()
        {
            // Pull preloaded assets from managers (loaded during splash).
            Image? menuBg = ImageManager.Instance.GetImage(Constants.IMG_BG_MENU);
            if (menuBg != null)
            {
                Bitmap dimmed = new Bitmap(menuBg.Width, menuBg.Height);
                using (Graphics g = Graphics.FromImage(dimmed))
                {
                    ColorMatrix matrix = new ColorMatrix { Matrix33 = 0.4f }; // Even darker for menu
                    ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(menuBg, new Rectangle(0, 0, dimmed.Width, dimmed.Height), 0, 0, menuBg.Width, menuBg.Height, GraphicsUnit.Pixel, attributes);
                }
                _backgroundImage = dimmed;
                this.BackgroundImage = _backgroundImage;
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            // Reuse splash title for menu branding.
            if (titleBox != null)
            {
                titleBox.Image = ImageManager.Instance.GetImage(AssetBootstrapper.SplashTitleKey);
            }

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterControls();
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

        private void CenterControls()
        {
            if (titleBox == null || buttonContainer == null) return;

            // Set scaling factors
            float titleHeightPercent = 0.4f; // Increased from 0.25f
            int verticalGap = (int)(this.Height * 0.03); // Slightly tighter gap

            // 1. Calculate and scale Title Box
            titleBox.Width = (int)(this.Width * 0.85); // Increased from 0.7
            titleBox.Height = (int)(this.Height * titleHeightPercent);

            // 2. Calculate Total Content Block Height
            int totalContentHeight = titleBox.Height + verticalGap + buttonContainer.Height;

            // 3. Center the entire block vertically
            int startY = (this.Height - totalContentHeight) / 2;

            // 4. Apply Horizontal Centering
            titleBox.Left = (this.Width - titleBox.Width) / 2;
            buttonContainer.Left = (this.Width - buttonContainer.Width) / 2;

            // 5. Apply Vertical Positions
            titleBox.Top = startY;
            buttonContainer.Top = titleBox.Bottom + verticalGap;
        }

        private void SyncMenuVisualState()
        {
            CenterControls();

            if (buttonContainer == null)
            {
                return;
            }

            foreach (Control ctrl in buttonContainer.Controls)
            {
                if (ctrl is Button btn)
                {
                    MenuButtonStyle.SyncHoverVisualState(btn);
                }
            }
        }

        private void btnExit_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSettings_Click(object? sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm())
            {
                settingsForm.ShowDialog(this);
            }
        }

        private void btnCredits_Click(object? sender, EventArgs e)
        {
            using (CreditsForm creditsForm = new CreditsForm())
            {
                creditsForm.ShowDialog(this);
            }
        }

        private void btnLevels_Click(object? sender, EventArgs e)
        {
            if (FormTransitionManager.IsTransitioning)
            {
                return;
            }

            OpenLevelsMenu();
        }

        private void OpenPrologue()
        {
            if (FormTransitionManager.IsTransitioning)
            {
                return;
            }

            // Story route: menu -> prologue.
            ShowChildAndResumeMenuMusic(new StoryForm(StoryScripts.CreatePrologue()));
        }

        private void OpenLevelsMenu()
        {
            // Direct route: menu -> levels.
            ShowChildAndResumeMenuMusic(new LevelsMenuForm());
        }

        private void ShowChildAndResumeMenuMusic(Form childForm)
        {
            AudioManager.Instance.StopMusic();
            if (!FormTransitionManager.ShowChild(this, childForm, ResumeMenuMusicAndStayOpen))
            {
                childForm.Dispose();
                AudioManager.Instance.PlayMusic(Constants.MUSIC_MENU);
            }
        }

        private static bool ResumeMenuMusicAndStayOpen()
        {
            AudioManager.Instance.PlayMusic(Constants.MUSIC_MENU);
            return true;
        }

        public void ApplyLanguage()
        {
            if (btnPlay != null) btnPlay.Text = $"[{LanguageManager.Instance.Get("menu_play").ToUpperInvariant()}]";
            if (btnLevels != null) btnLevels.Text = "[LEVELS]";
            if (btnSettings != null) btnSettings.Text = $"[{LanguageManager.Instance.Get("menu_settings").ToUpperInvariant()}]";
            if (btnCredits != null) btnCredits.Text = $"[{LanguageManager.Instance.Get("menu_credits").ToUpperInvariant()}]";
            if (btnExit != null) btnExit.Text = $"[{LanguageManager.Instance.Get("menu_quit").ToUpperInvariant()}]";
        }

        private void menu_Load(object? sender, EventArgs e)
        {
            ApplyLanguage();
            AudioManager.Instance.PlayMusic(Constants.MUSIC_MENU);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Ignore Enter and Space keys to prevent accidental activation of focused buttons
            if (keyData == Keys.Enter || keyData == Keys.Space)
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}

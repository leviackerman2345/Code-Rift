
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        private Image? _hoverImage;

        public MenuForm()

        {
            InitializeComponent();
            LoadAssets();
            SetupFullScreen();
            SetupButtonHovers();
            ApplyLanguage();
            btnPlay.Click += btnPlay_Click;
        }

        private void btnPlay_Click(object? sender, EventArgs e)
        {
            // Story route: menu -> prologue.
            AudioManager.Instance.StopMusic();
            PrologueForm prologue = new PrologueForm();
            // When downstream flow closes, return to this menu.
            prologue.FormClosed += (s, args) => 
            {
                this.Show();
                AudioManager.Instance.PlayMusic(Constants.MUSIC_MENU);
            };
            prologue.Shown += (s, args) => this.Hide();
            prologue.Show();
        }

        private void SetupButtonHovers()
        {
            Color matrixGreen = Color.FromArgb(0, 255, 65);
            if (buttonContainer != null)
            {
                foreach (Control ctrl in buttonContainer.Controls)
                {
                    if (ctrl is Button btn)
                    {
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 2;
                        btn.FlatAppearance.BorderColor = matrixGreen;
                        
                        btn.FlatAppearance.MouseOverBackColor = matrixGreen;
                        btn.Click += (s, e) => AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
                        btn.MouseEnter += (s, e) =>
                        {
                            AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
                            btn.ForeColor = Color.Black;
                            btn.FlatAppearance.BorderColor = Color.Black;
                            if (_hoverImage != null)
                            {
                                btn.BackgroundImage = _hoverImage;
                                btn.BackgroundImageLayout = ImageLayout.Stretch;
                            }
                        };
                        btn.MouseLeave += (s, e) =>
                        {
                            btn.ForeColor = matrixGreen;
                            btn.FlatAppearance.BorderColor = matrixGreen;
                            btn.BackgroundImage = null;
                        };
                    }
                }
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

            // Shared hover texture for all menu buttons.
            _hoverImage = ImageManager.Instance.GetImage(Constants.IMG_UI_BUTTON);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterControls();
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
            // Direct route: menu -> levels.
            AudioManager.Instance.StopMusic();
            LevelsMenuForm levelsMenu = new LevelsMenuForm();
            levelsMenu.FormClosed += (s, args) => 
            {
                this.Show();
                AudioManager.Instance.PlayMusic(Constants.MUSIC_MENU);
            };
            levelsMenu.Shown += (s, args) => this.Hide();
            levelsMenu.Show();
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

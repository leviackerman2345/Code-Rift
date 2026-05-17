
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using CodeRift.Managers;

namespace CodeRift.Forms
{
    public partial class menu : Form
    {
        private Image? _backgroundImage;
        private Image? _hoverImage;

        public menu()

        {
            InitializeComponent();
            LoadAssets();
            SetupFullScreen();
            SetupButtonHovers();
            ApplyLanguage();
            btnPlay.Click += btnPlay_Click;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.Hide();
            PrologueForm prologue = new PrologueForm();
            prologue.FormClosed += (s, args) => this.Show(); // Show menu again when prologue/game ends? Or close menu?
            // Usually, after epilogue, we return to menu. 
            // The prompt says "After the final dialogue line, show a 'Return to Main Menu' button or transition back to the main menu form".
            // So keeping menu hidden and showing it again on close is correct.
            prologue.Show();
        }

        private void SetupButtonHovers()
        {
            Color matrixGreen = Color.FromArgb(0, 255, 65);
            foreach (Control ctrl in buttonContainer.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatAppearance.MouseOverBackColor = matrixGreen;
                    btn.MouseEnter += (s, e) =>
                    {
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

        private void SetupFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        private void LoadAssets()
        {
            // Load Background (Dimmed like splash)
            string[] bgPaths = {
                Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\backgrounds\main_menu.png"),
                Path.Combine(Application.StartupPath, @"Assets\Images\backgrounds\main_menu.png"),
                @"D:\Christian things\CodeRift\CodeRift\Assets\Images\backgrounds\main_menu.png",
                Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\backgrounds\Splash background.jpeg"),
                Path.Combine(Application.StartupPath, @"Assets\Images\backgrounds\Splash background.jpeg")
            };

            foreach (var path in bgPaths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        Image original = Image.FromFile(path);
                        Bitmap dimmed = new Bitmap(original.Width, original.Height);
                        using (Graphics g = Graphics.FromImage(dimmed))
                        {
                            ColorMatrix matrix = new ColorMatrix { Matrix33 = 0.4f }; // Even darker for menu
                            ImageAttributes attributes = new ImageAttributes();
                            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            g.DrawImage(original, new Rectangle(0, 0, dimmed.Width, dimmed.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                        }
                        _backgroundImage = dimmed;
                        this.BackgroundImage = _backgroundImage;
                        this.BackgroundImageLayout = ImageLayout.Stretch;
                        original.Dispose();
                        break;
                    }
                    catch { }
                }
            }

            // Load Title
            string[] titlePaths = {
                Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\ui\Title.png"),
                Path.Combine(Application.StartupPath, @"Assets\Images\ui\Title.png"),
                @"D:\Christian things\CodeRift\CodeRift\Assets\Images\ui\Title.png"
            };

            foreach (var path in titlePaths)
            {
                if (File.Exists(path))
                {
                    try { titleBox.Image = Image.FromFile(path); break; } catch { }
                }
            }

            // Load Hover Image
            string[] hoverPaths = {
                Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\ui\button_hover.png"),
                Path.Combine(Application.StartupPath, @"Assets\Images\ui\button_hover.png"),
                @"D:\Christian things\CodeRift\CodeRift\Assets\Images\ui\button_hover.png"
            };

            foreach (var path in hoverPaths)
            {
                if (File.Exists(path))
                {
                    try { _hoverImage = Image.FromFile(path); break; } catch { }
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterControls();
        }

        private void CenterControls()
        {
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (settings settingsForm = new settings())
            {
                settingsForm.ShowDialog(this);
            }
        }

        private void btnCredits_Click(object sender, EventArgs e)
        {
            using (credits creditsForm = new credits())
            {
                creditsForm.ShowDialog(this);
            }
        }

        private void btnLevels_Click(object sender, EventArgs e)
        {
            this.Hide();
            LevelsMenuForm levelsMenu = new LevelsMenuForm();
            levelsMenu.FormClosed += (s, args) => this.Show();
            levelsMenu.Show();
        }

        public void ApplyLanguage()
        {
            btnPlay.Text = $"[{LanguageManager.Instance.Get("menu_play").ToUpperInvariant()}]";
            btnLevels.Text = "[LEVELS]";
            btnSettings.Text = $"[{LanguageManager.Instance.Get("menu_settings").ToUpperInvariant()}]";
            btnCredits.Text = $"[{LanguageManager.Instance.Get("menu_credits").ToUpperInvariant()}]";
            btnExit.Text = $"[{LanguageManager.Instance.Get("menu_quit").ToUpperInvariant()}]";
        }

        private void menu_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
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

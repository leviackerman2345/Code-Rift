using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CodeRift.Forms
{
    public partial class credits : Form
    {
        private Image? _backgroundImage;
        private Image? _hoverImage;
        private readonly System.Windows.Forms.Timer _creditsScrollTimer = new System.Windows.Forms.Timer();
        private const int CreditsScrollSpeed = 2;
        private const int CreditsScrollIntervalMs = 16;

        public credits()
        {
            InitializeComponent();
            EnableOptimizedPainting();
            LoadAssets();
            SetupFullScreen();
            SetupButtonHovers();
            PopulateCredits();
            ConfigureCreditsScroll();
        }

        private void EnableOptimizedPainting()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            EnableDoubleBuffer(mainContainer);
            EnableDoubleBuffer(tableLayoutPanel1);
        }

        private static void EnableDoubleBuffer(Control control)
        {
            typeof(Control)
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(control, true, null);
        }

        private void ConfigureCreditsScroll()
        {
            _creditsScrollTimer.Interval = CreditsScrollIntervalMs;
            _creditsScrollTimer.Tick += CreditsScrollTimer_Tick;
        }

        private void CreditsScrollTimer_Tick(object? sender, EventArgs e)
        {
            lblTitle.Top -= CreditsScrollSpeed;
            tableLayoutPanel1.Top -= CreditsScrollSpeed;

            if (tableLayoutPanel1.Bottom < 0)
            {
                ResetCreditsPosition();
            }
        }

        private void SetupButtonHovers()
        {
            Color matrixGreen = Color.FromArgb(0, 255, 65);
            btnBack.FlatAppearance.MouseOverBackColor = matrixGreen;
            btnBack.MouseEnter += (s, e) => {
                btnBack.ForeColor = Color.Black;
                btnBack.FlatAppearance.BorderColor = Color.Black;
                if (_hoverImage != null)
                {
                    btnBack.BackgroundImage = _hoverImage;
                    btnBack.BackgroundImageLayout = ImageLayout.Stretch;
                }
            };
            btnBack.MouseLeave += (s, e) => {
                btnBack.ForeColor = matrixGreen;
                btnBack.FlatAppearance.BorderColor = matrixGreen;
                btnBack.BackgroundImage = null;
            };
        }

        private void SetupFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        private void LoadAssets()
        {
            // Load Background (Dimmed like main menu)
            string[] bgPaths = {
                Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\backgrounds\main_menu.png"),
                Path.Combine(Application.StartupPath, @"Assets\Images\backgrounds\main_menu.png"),
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
                            ColorMatrix matrix = new ColorMatrix { Matrix33 = 0.3f }; // Very dark for credits
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

            // Load Hover Image
            string[] hoverPaths = {
                Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\ui\button_hover.png"),
                Path.Combine(Application.StartupPath, @"Assets\Images\ui\button_hover.png")
            };

            foreach (var path in hoverPaths)
            {
                if (File.Exists(path))
                {
                    try { _hoverImage = Image.FromFile(path); break; } catch { }
                }
            }
        }

        private void PopulateCredits()
        {
            tableLayoutPanel1.SuspendLayout();
            var creditsData = new[]
            {
                new { Position = "Technical_Director", Name = "Christian_Lawrence_De_Goma" },
                new { Position = "Game_Designer", Name = "Ryza_Nicole_Chavez" },
                new { Position = "Art_Director", Name = "Psalms_Gycleff_P._Rivera" },
                new { Position = "Programmer", Name = "Lian_James_Petil" },
                new { Position = "QA_Tester", Name = "Javin_Martin_Urete" }
            };

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = creditsData.Length;
            
            // Adjust row heights
            tableLayoutPanel1.RowStyles.Clear();
            for (int i = 0; i < creditsData.Length; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / creditsData.Length));
            }

            Font fontPos = new Font("Courier New", 20F, FontStyle.Bold);
            Font fontName = new Font("Courier New", 20F, FontStyle.Regular);
            Color matrixGreen = Color.FromArgb(0, 255, 65);
            Color nameColor = Color.White;

            for (int i = 0; i < creditsData.Length; i++)
            {
                Label lblPos = new Label
                {
                    Text = creditsData[i].Position + " / ",
                    Font = fontPos,
                    ForeColor = matrixGreen,
                    TextAlign = ContentAlignment.MiddleRight,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    BackColor = Color.Transparent
                };

                Label lblName = new Label
                {
                    Text = creditsData[i].Name,
                    Font = fontName,
                    ForeColor = nameColor,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    BackColor = Color.Transparent
                };

                tableLayoutPanel1.Controls.Add(lblPos, 0, i);
                tableLayoutPanel1.Controls.Add(lblName, 1, i);
            }
            tableLayoutPanel1.ResumeLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterControls();
            ResetCreditsPosition();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ResetCreditsPosition();
            _creditsScrollTimer.Start();
        }

        private void CenterControls()
        {
            // Move back button to the form level to avoid transparency overlap issues
            if (btnBack.Parent != this)
            {
                this.Controls.Add(btnBack);
                btnBack.BringToFront();
            }

            // Center mainContainer in the screen
            int padding = 40;
            
            // Size main container to full screen so credits start from the very edge
            mainContainer.Width = this.Width;
            mainContainer.Height = this.Height;
            mainContainer.Left = 0;
            mainContainer.Top = 0;

            // Position back button at top left of the screen
            btnBack.Width = 200;
            btnBack.Left = padding;
            btnBack.Top = padding;

            // Center Title horizontally
            lblTitle.Left = (mainContainer.Width - lblTitle.Width) / 2;

            // Size and center table layout
            tableLayoutPanel1.Width = (int)(mainContainer.Width * 0.8);
            tableLayoutPanel1.Height = (int)(mainContainer.Height * 0.6);
            tableLayoutPanel1.Left = (mainContainer.Width - tableLayoutPanel1.Width) / 2;
        }

        private void ResetCreditsPosition()
        {
            lblTitle.Top = mainContainer.Height;
            tableLayoutPanel1.Top = lblTitle.Bottom + 40;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _creditsScrollTimer.Stop();
            base.OnFormClosing(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Space)
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}

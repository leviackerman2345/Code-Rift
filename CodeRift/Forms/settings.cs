using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public partial class settings : Form
    {
        private Image? _hoverImage;
        private Bitmap _iconVolume;
        private Bitmap _iconMute;
        private Bitmap _flagPH;
        private Bitmap _flagEN;
        private bool _isMuted = false;
        private int _lastVolume = 80;

        public settings()
        {
            InitializeComponent();
            Generate8BitAssets();
            LoadAssets();
            SetupButtonHovers();
            SetupEvents();
            SetupCustomTitleBar();
            ApplyLanguageSelection();
        }

        private void SetupCustomTitleBar()
        {
            // Custom Paint for perfect circular dots with slight glow
            Action<Panel, Color> paintDot = (p, c) => {
                p.Paint += (s, e) => {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (SolidBrush b = new SolidBrush(c)) {
                        e.Graphics.FillEllipse(b, 1, 1, p.Width - 2, p.Height - 2);
                    }
                };
            };

            paintDot(btnClose, Color.FromArgb(255, 95, 86));
            paintDot(btnMin, Color.FromArgb(255, 189, 46));
            paintDot(btnMax, Color.FromArgb(39, 201, 63));

            // Form Dragging Logic
            bool dragging = false;
            Point dragCursorPoint = new Point(0, 0);
            Point dragFormPoint = new Point(0, 0);

            titleBar.MouseDown += (s, e) => {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            };

            titleBar.MouseMove += (s, e) => {
                if (dragging) {
                    Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                    this.Location = Point.Add(dragFormPoint, new Size(dif));
                }
            };

            titleBar.MouseUp += (s, e) => dragging = false;

            btnClose.Click += (s, e) => this.Close();
            btnClose.Cursor = Cursors.Hand;

            // Optional: Hover effect for dots
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.FromArgb(200, 70, 60);
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.Transparent;
        }

        private void Generate8BitAssets()
        {
            // ... (rest of Generate8BitAssets remains the same)
            // 1. Volume Icon (8-bit style)
            _iconVolume = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(_iconVolume))
            {
                g.Clear(Color.Transparent);
                Brush green = new SolidBrush(Color.FromArgb(0, 255, 65));
                // Speaker body
                g.FillRectangle(green, 4, 10, 8, 12); 
                g.FillPolygon(green, new Point[] { new Point(12, 10), new Point(22, 4), new Point(22, 28), new Point(12, 22) });
                // Waves
                g.FillRectangle(green, 26, 10, 2, 12);
                g.FillRectangle(green, 30, 6, 2, 20);
            }

            // 2. Mute Icon (8-bit style)
            _iconMute = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(_iconMute))
            {
                g.Clear(Color.Transparent);
                Brush green = new SolidBrush(Color.FromArgb(0, 255, 65));
                Brush red = new SolidBrush(Color.Red);
                // Speaker body
                g.FillRectangle(green, 4, 10, 8, 12);
                g.FillPolygon(green, new Point[] { new Point(12, 10), new Point(22, 4), new Point(22, 28), new Point(12, 22) });
                // X mark
                g.FillRectangle(red, 24, 12, 8, 2);
                g.FillRectangle(red, 27, 9, 2, 8);
            }

            // 3. PH Flag (8-bit style - Detailed)
            _flagPH = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(_flagPH))
            {
                g.Clear(Color.White);
                g.FillRectangle(Brushes.Blue, 10, 0, 22, 16);
                g.FillRectangle(Brushes.Red, 10, 16, 22, 16);
                g.FillPolygon(Brushes.White, new Point[] { new Point(0, 0), new Point(12, 16), new Point(0, 32) });
                // Sun (Simplified)
                g.FillEllipse(Brushes.Yellow, 2, 13, 6, 6);
            }

            // 4. EN Flag (8-bit style - Detailed cross)
            _flagEN = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(_flagEN))
            {
                g.Clear(Color.DarkBlue);
                Pen whitePen = new Pen(Color.White, 6);
                Pen redPen = new Pen(Color.Red, 2);
                // White Cross
                g.DrawLine(whitePen, 0, 0, 32, 32);
                g.DrawLine(whitePen, 32, 0, 0, 32);
                g.DrawLine(whitePen, 16, 0, 16, 32);
                g.DrawLine(whitePen, 0, 16, 32, 16);
                // Red Cross
                g.DrawLine(redPen, 16, 0, 16, 32);
                g.DrawLine(redPen, 0, 16, 32, 16);
            }

            volIcon.Image = _iconVolume;
            phFlagIcon.Image = _flagPH;
            enFlagIcon.Image = _flagEN;
        }

        private void LoadAssets()
        {
            string hoverPath = Path.Combine(Application.StartupPath, @"Assets\Images\ui\button_hover.png");
            if (!File.Exists(hoverPath)) hoverPath = Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\ui\button_hover.png");
            
            if (File.Exists(hoverPath))
            {
                try { _hoverImage = Image.FromFile(hoverPath); } catch { }
            }
        }

        private void SetupButtonHovers()
        {
            Color matrixGreen = Color.FromArgb(0, 255, 65);
            foreach (Control ctrl in terminalBody.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatAppearance.MouseOverBackColor = matrixGreen;
                    btn.MouseEnter += (s, e) => {
                        btn.ForeColor = Color.Black;
                        btn.FlatAppearance.BorderColor = Color.Black;
                        if (_hoverImage != null)
                        {
                            btn.BackgroundImage = _hoverImage;
                            btn.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    };
                    btn.MouseLeave += (s, e) => {
                        btn.ForeColor = matrixGreen;
                        btn.FlatAppearance.BorderColor = matrixGreen;
                        btn.BackgroundImage = null;
                    };
                }
            }
        }

        private void SetupEvents()
        {
            btnBack.Click += (s, e) => this.Close();

            volIcon.Click += (s, e) => {
                _isMuted = !_isMuted;
                if (_isMuted) {
                    _lastVolume = volSlider.Value;
                    volSlider.Value = 0;
                    volIcon.Image = _iconMute;
                } else {
                    volSlider.Value = _lastVolume > 0 ? _lastVolume : 80;
                    volIcon.Image = _iconVolume;
                }
            };

            volSlider.ValueChanged += (s, e) => {
                if (volSlider.Value > 0) {
                    _isMuted = false;
                    volIcon.Image = _iconVolume;
                } else {
                    _isMuted = true;
                    volIcon.Image = _iconMute;
                }
            };

            btnFilipino.Click += (s, e) => SwitchLanguage(Constants.LANG_PH);
            btnEnglish.Click += (s, e) => SwitchLanguage(Constants.LANG_EN);
        }

        private void SwitchLanguage(string languageCode)
        {
            LanguageManager.Instance.Switch(languageCode);
            ApplyLanguageSelection();

            if (Owner is menu mainMenu)
            {
                mainMenu.ApplyLanguage();
            }
        }

        private void ApplyLanguageSelection()
        {
            var lm = LanguageManager.Instance;
            Color matrixGreen = Color.FromArgb(0, 255, 65);
            bool isFilipino = lm.CurrentLanguage == Constants.LANG_PH;

            btnFilipino.BackColor = isFilipino ? matrixGreen : Color.Black;
            btnFilipino.ForeColor = isFilipino ? Color.Black : matrixGreen;

            btnEnglish.BackColor = !isFilipino ? matrixGreen : Color.Black;
            btnEnglish.ForeColor = !isFilipino ? Color.Black : matrixGreen;

            // Apply translations
            lblTitle.Text = lm.Get("settings_title");
            lblVolume.Text = lm.Get("settings_volume");
            lblSubtitles.Text = lm.Get("settings_subtitles");
            btnBack.Text = lm.Get("settings_exit");
            btnFilipino.Text = lm.Get("lang_ph");
            btnEnglish.Text = lm.Get("lang_en");
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    // Credits screen with direct in-form Star Wars-style crawl logic.
    public partial class CreditsForm : Form
    {
        private static readonly Color MatrixGreen = Color.FromArgb(0, 255, 65);
        private static readonly Color CrawlTextColor = Color.FromArgb(255, 230, 95);

        private Image? _backgroundImage;
        private Image? _hoverImage;

        private readonly System.Windows.Forms.Timer _creditsScrollTimer = new System.Windows.Forms.Timer();
        private readonly List<string> _creditsLines = new List<string>();

        private float _crawlOffset;

        private const float CreditsScrollSpeed = 1.8f;
        private const int CreditsScrollIntervalMs = 16;
        private const float CrawlLineSpacing = 64f;
        private const float CrawlMinScale = 0.16f;
        private const float CrawlPerspectiveStrength = 0.82f;
        private const float CrawlPerspectiveYOffset = 260f;
        private const float CrawlMaxWidthRatio = 0.95f;
        private const float CrawlMinWidthRatio = 0.08f;

        public CreditsForm()
        {
            InitializeComponent();

            ConfigureRenderingPerformance();
            ConfigureFullScreenWindow();
            ConfigureButtonHoverEffects();
            ConfigureCreditsAnimation();

            LoadVisualAssets();
            BuildCreditsContent();
        }

        private void ConfigureRenderingPerformance()
        {
            // Microcomment: reduce flicker during fast redraw loops.
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

        private void ConfigureFullScreenWindow()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            if (Screen.PrimaryScreen != null)
            {
                Bounds = Screen.PrimaryScreen.Bounds;
            }
        }

        private void ConfigureButtonHoverEffects()
        {
            btnBack.FlatAppearance.MouseOverBackColor = MatrixGreen;

            btnBack.MouseEnter += (_, _) =>
            {
                btnBack.ForeColor = Color.Black;
                btnBack.FlatAppearance.BorderColor = Color.Black;

                if (_hoverImage != null)
                {
                    btnBack.BackgroundImage = _hoverImage;
                    btnBack.BackgroundImageLayout = ImageLayout.Stretch;
                }
            };

            btnBack.MouseLeave += (_, _) =>
            {
                btnBack.ForeColor = MatrixGreen;
                btnBack.FlatAppearance.BorderColor = MatrixGreen;
                btnBack.BackgroundImage = null;
            };
        }

        private void ConfigureCreditsAnimation()
        {
            _creditsScrollTimer.Interval = CreditsScrollIntervalMs;
            _creditsScrollTimer.Tick += CreditsScrollTimer_Tick;
            mainContainer.Paint += MainContainer_Paint;
        }

        private void LoadVisualAssets()
        {
            _backgroundImage = TryLoadDimmedBackground(alpha: 0.3f);
            if (_backgroundImage != null)
            {
                BackgroundImage = _backgroundImage;
                BackgroundImageLayout = ImageLayout.Stretch;
            }

            _hoverImage = TryLoadHoverImage();
        }

        private void BuildCreditsContent()
        {
            _creditsLines.Clear();
            _creditsLines.Add("[ C R E D I T S ]");
            _creditsLines.Add(string.Empty);
            _creditsLines.Add(string.Empty);

            var credits = new[]
            {
                new { Role = "TECHNICAL DIRECTOR", Name = "CHRISTIAN LAWRENCE DE GOMA" },
                new { Role = "GAME DESIGNER", Name = "RYZA NICOLE CHAVEZ" },
                new { Role = "ART DIRECTOR", Name = "PSALMS GYCLEFF P. RIVERA" },
                new { Role = "PROGRAMMER", Name = "LIAN JAMES PETIL" },
                new { Role = "QA TESTER", Name = "JAVIN MARTIN URETE" }
            };

            foreach (var credit in credits)
            {
                _creditsLines.Add($"[ {credit.Role} ]");
                _creditsLines.Add(credit.Name);
                _creditsLines.Add(string.Empty);
            }

            // Microcomment: hide legacy designer labels; crawl is custom painted.
            lblTitle.Visible = false;
            tableLayoutPanel1.Visible = false;
        }

        private void CreditsScrollTimer_Tick(object? sender, EventArgs e)
        {
            _crawlOffset += CreditsScrollSpeed;

            if (_crawlOffset > GetTotalCrawlHeight() + mainContainer.Height + CrawlLineSpacing)
            {
                ResetCreditsPosition();
            }

            mainContainer.Invalidate();
        }

        private void MainContainer_Paint(object? sender, PaintEventArgs e)
        {
            if (_creditsLines.Count == 0)
            {
                return;
            }

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            float panelWidth = mainContainer.ClientSize.Width;
            float panelHeight = mainContainer.ClientSize.Height;
            float baseY = panelHeight - _crawlOffset;

            for (int i = 0; i < _creditsLines.Count; i++)
            {
                float worldY = baseY + (i * CrawlLineSpacing);
                if (worldY < -160f || worldY > panelHeight + CrawlLineSpacing)
                {
                    continue;
                }

                DrawCrawlLine(g, panelWidth, panelHeight, _creditsLines[i], worldY);
            }
        }

        private static Image? TryLoadHoverImage()
        {
            string[] paths =
            {
                Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\ui\button_hover.png"),
                Path.Combine(Application.StartupPath, @"Assets\Images\ui\button_hover.png")
            };

            foreach (string path in paths)
            {
                if (!File.Exists(path))
                {
                    continue;
                }

                try
                {
                    return Image.FromFile(path);
                }
                catch
                {
                }
            }

            return null;
        }

        private static Image? TryLoadDimmedBackground(float alpha)
        {
            string[] paths =
            {
                Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\backgrounds\main_menu.png"),
                Path.Combine(Application.StartupPath, @"Assets\Images\backgrounds\main_menu.png"),
                Path.Combine(Application.StartupPath, @"..\..\..\Assets\Images\backgrounds\Splash background.jpeg"),
                Path.Combine(Application.StartupPath, @"Assets\Images\backgrounds\Splash background.jpeg")
            };

            foreach (string path in paths)
            {
                if (!File.Exists(path))
                {
                    continue;
                }

                try
                {
                    using Image original = Image.FromFile(path);
                    Bitmap dimmed = new Bitmap(original.Width, original.Height);

                    using Graphics g = Graphics.FromImage(dimmed);
                    ColorMatrix matrix = new ColorMatrix { Matrix33 = alpha };
                    using ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    g.DrawImage(
                        original,
                        new Rectangle(0, 0, dimmed.Width, dimmed.Height),
                        0,
                        0,
                        original.Width,
                        original.Height,
                        GraphicsUnit.Pixel,
                        attributes);

                    return dimmed;
                }
                catch
                {
                }
            }

            return null;
        }

        private void DrawCrawlLine(Graphics g, float panelWidth, float panelHeight, string line, float worldY)
        {
            float yNorm = Math.Clamp(worldY / panelHeight, 0f, 1f);
            float perspective = 1f - yNorm; // 0 near bottom, 1 near top

            float scale = Math.Max(CrawlMinScale, 1f - (perspective * CrawlPerspectiveStrength));
            float projectedY = worldY - (perspective * perspective * CrawlPerspectiveYOffset);

            float maxWidth = panelWidth * CrawlMaxWidthRatio;
            float minWidth = panelWidth * CrawlMinWidthRatio;
            float lineWidth = Math.Max(minWidth, maxWidth - (perspective * panelWidth * CrawlPerspectiveStrength));
            float x = (panelWidth - lineWidth) / 2f;

            FontStyle style = line.StartsWith("[", StringComparison.Ordinal) ? FontStyle.Bold : FontStyle.Regular;
            float fontSize = 40f * scale;

            using Font font = new Font("Arial Black", fontSize, style, GraphicsUnit.Pixel);
            using SolidBrush brush = new SolidBrush(CrawlTextColor);
            using StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Near
            };

            g.DrawString(line, font, brush, new RectangleF(x, projectedY, lineWidth, CrawlLineSpacing * 1.2f), format);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ResetCreditsPosition();
            _creditsScrollTimer.Start();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutMainControls();
            ResetCreditsPosition();
        }

        private void LayoutMainControls()
        {
            // Microcomment: placing button on form avoids panel transparency artifacting.
            if (btnBack.Parent != this)
            {
                Controls.Add(btnBack);
                btnBack.BringToFront();
            }

            const int padding = 40;

            mainContainer.Width = Width;
            mainContainer.Height = Height;
            mainContainer.Left = 0;
            mainContainer.Top = 0;

            btnBack.Width = 200;
            btnBack.Left = padding;
            btnBack.Top = padding;
        }

        private float GetTotalCrawlHeight()
        {
            return _creditsLines.Count * CrawlLineSpacing;
        }

        private void ResetCreditsPosition()
        {
            _crawlOffset = 0f;
            mainContainer.Invalidate();
        }

        private void btnBack_MouseEnter(object? sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
        }

        private void btnBack_Click(object? sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Space)
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _creditsScrollTimer.Stop();
            mainContainer.Paint -= MainContainer_Paint;

            _hoverImage?.Dispose();
            _hoverImage = null;

            base.OnFormClosing(e);
        }
    }
}

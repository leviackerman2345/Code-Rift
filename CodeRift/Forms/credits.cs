using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    // Credits screen with direct in-form Star Wars-style crawl logic.
    public partial class CreditsForm : Form
    {
        private static readonly Color MatrixGreen = Color.FromArgb(0, 255, 65);
        private static readonly Color CrawlTextColor = Color.FromArgb(255, 230, 95);

        private Image _backgroundImage;

        private readonly System.Windows.Forms.Timer _creditsScrollTimer = new System.Windows.Forms.Timer();
        private readonly List<string> _creditsLines = new List<string>();

        private float _crawlOffset;
        private bool _isAutoClosing;

        private const float CreditsScrollSpeed = 3.0f;
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
            var prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            if (prop != null) prop.SetValue(control, true, null);
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
                _creditsLines.Add(string.Format("[ {0} ]", credit.Role));
                _creditsLines.Add(credit.Name);
                _creditsLines.Add(string.Empty);
            }

            // Microcomment: hide legacy designer labels; crawl is custom painted.
            lblTitle.Visible = false;
            tableLayoutPanel1.Visible = false;
        }

        private void CreditsScrollTimer_Tick(object sender, EventArgs e)
        {
            _crawlOffset += CreditsScrollSpeed;

            if (_crawlOffset > GetTotalCrawlHeight() + mainContainer.Height + CrawlLineSpacing)
            {
                FinishCredits();
                return;
            }

            mainContainer.Invalidate();
        }

        private void MainContainer_Paint(object sender, PaintEventArgs e)
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

        private static Image TryLoadDimmedBackground(float alpha)
        {
            string[] relativePaths =
            {
                Path.Combine("Assets", "Images", "backgrounds", "main_menu.png"),
                Path.Combine("Assets", "Images", "backgrounds", "Splash background.jpeg")
            };

            foreach (string relativePath in relativePaths)
            {
                string path = AssetPathHelper.ResolveAssetPath(relativePath);
                if (!File.Exists(path))
                {
                    continue;
                }

                try
                {
                    using (Image original = Image.FromFile(path))
                    {
                        Bitmap dimmed = new Bitmap(original.Width, original.Height);

                        using (Graphics g = Graphics.FromImage(dimmed))
                        {
                            ColorMatrix matrix = new ColorMatrix { Matrix33 = alpha };
                            using (ImageAttributes attributes = new ImageAttributes())
                            {
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
                            }
                        }

                        return dimmed;
                    }
                }
                catch
                {
                }
            }

            return null;
        }

        private void DrawCrawlLine(Graphics g, float panelWidth, float panelHeight, string line, float worldY)
        {
            float yNorm = Math.Min(1f, Math.Max(0f, worldY / panelHeight));
            float perspective = 1f - yNorm; // 0 near bottom, 1 near top

            float scale = Math.Max(CrawlMinScale, 1f - (perspective * CrawlPerspectiveStrength));
            float projectedY = worldY - (perspective * perspective * CrawlPerspectiveYOffset);

            float maxWidth = panelWidth * CrawlMaxWidthRatio;
            float minWidth = panelWidth * CrawlMinWidthRatio;
            float lineWidth = Math.Max(minWidth, maxWidth - (perspective * panelWidth * CrawlPerspectiveStrength));
            float x = (panelWidth - lineWidth) / 2f;

            FontStyle style = line.StartsWith("[", StringComparison.Ordinal) ? FontStyle.Bold : FontStyle.Regular;
            float fontSize = 40f * scale;

            using (Font font = new Font("Arial Black", fontSize, style, GraphicsUnit.Pixel))
            using (SolidBrush brush = new SolidBrush(CrawlTextColor))
            using (StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Near
            })
            {
                g.DrawString(line, font, brush, new RectangleF(x, projectedY, lineWidth, CrawlLineSpacing * 1.2f), format);
            }
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
            mainContainer.Width = Width;
            mainContainer.Height = Height;
            mainContainer.Left = 0;
            mainContainer.Top = 0;
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

        private void FinishCredits()
        {
            if (_isAutoClosing)
            {
                return;
            }

            _isAutoClosing = true;
            _creditsScrollTimer.Stop();
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
            _creditsScrollTimer.Dispose();
            mainContainer.Paint -= MainContainer_Paint;
            BackgroundImage = null;
            if (_backgroundImage != null) _backgroundImage.Dispose();
            _backgroundImage = null;


            base.OnFormClosing(e);
        }
    }
}

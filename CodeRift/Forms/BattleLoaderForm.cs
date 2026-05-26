using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeRift.Core;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public partial class BattleLoaderForm : Form
    {
        public int Level { get; private set; }
        public bool LoadSuccessful = false;
        public Action? OnComplete;

        private Image? _currentBackground;
        private readonly Dictionary<string, Image> _preScaledBackgrounds = new();
        private float _targetProgress = 0f;
        private float _currentProgress = 0f;
        private readonly System.Windows.Forms.Timer _smoothTimer = new();
        private bool _loadStarted = false;
        private string _currentStatus = "INITIALIZING DECRYPTION PROTOCOLS...";

        public BattleLoaderForm(int level = 1)
        {
            InitializeComponent();
            Level = level;

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            // Configure smooth interpolation timer (60 FPS tick)
            _smoothTimer.Interval = 16;
            _smoothTimer.Tick += SmoothTimer_Tick;

            LoadBackgroundAsset();
        }

        private void LoadBackgroundAsset()
        {
            try
            {
                string loaderPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Images", "battle_loader", $"level{Level}.png");
                if (File.Exists(loaderPath))
                {
                    _currentBackground = Image.FromFile(loaderPath);
                }
                else
                {
                    // Fallback to normal level background if specific loader image is missing
                    string fallbackPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Images", "backgrounds", "level_background", $"level{Level}.png");
                    if (File.Exists(fallbackPath))
                    {
                        _currentBackground = Image.FromFile(fallbackPath);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Loader Background Load Error: " + ex.Message);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CenterControls();
            PreScaleBackgrounds();
            _smoothTimer.Start();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!_loadStarted)
            {
                _loadStarted = true;
                Task.Run(ExecuteLoadingRoutineAsync);
            }
        }

        private async Task ExecuteLoadingRoutineAsync()
        {
            try
            {
                // Kick off progress-aware asset pre-warming
                await BattleArenaForm.PrewarmLevelWithProgressAsync(Level, (status, progress) =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        _currentStatus = status;
                        _targetProgress = (float)progress;
                    }));
                });

                // Let the smooth timer run until 100% is visually fully reached
                _currentStatus = "DECRYPTION SYNCHRONIZED. READY FOR COMBAT.";
                _targetProgress = 100f;
            }
            catch (Exception ex)
            {
                BeginInvoke(new Action(() =>
                {
                    _currentStatus = "ERROR: INTERFACE PROTOCOL BREACHED.";
                    _targetProgress = 100f;
                }));
                System.Diagnostics.Debug.WriteLine("Loader Task Error: " + ex.Message);
            }
        }

        private void SmoothTimer_Tick(object? sender, EventArgs e)
        {
            // Smooth exponential slide to target progress
            _currentProgress += (_targetProgress - _currentProgress) * 0.08f;

            if (_targetProgress >= 100f && Math.Abs(100f - _currentProgress) < 0.5f)
            {
                _currentProgress = 100f;
                lblStatus.Text = $"> {_currentStatus} [ 100% ]";
                _smoothTimer.Stop();

                // Graceful short delay before closing to let player appreciate the 100% completion state
                Task.Delay(500).ContinueWith(_ =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        LoadSuccessful = true;
                        OnComplete?.Invoke();
                    }));
                });
            }
            else
            {
                lblStatus.Text = $"> {_currentStatus} [ {(int)Math.Round(_currentProgress)}% ]";
            }

            Invalidate();
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
            lblTitle.Visible = false;
            lblPercent.Visible = false;

            int barWidth = 600;
            int barX = (Width - barWidth) / 2;
            int barY = Height - 100;

            lblStatus.Font = new Font("Courier New", 12F, FontStyle.Bold);
            lblStatus.Size = new Size(barWidth, 30);
            lblStatus.Location = new Point(barX, barY - 35);
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void PreScaleBackgrounds()
        {
            if (_currentBackground == null || Width <= 0 || Height <= 0)
            {
                return;
            }

            string cacheKey = $"{Width}x{Height}";
            if (_preScaledBackgrounds.ContainsKey(cacheKey))
            {
                return;
            }

            // Dispose old cached scalings
            foreach (var kvp in _preScaledBackgrounds)
            {
                kvp.Value.Dispose();
            }
            _preScaledBackgrounds.Clear();

            Bitmap scaled = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(scaled))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(_currentBackground, 0, 0, Width, Height);
            }

            _preScaledBackgrounds[cacheKey] = scaled;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Draw pre-scaled high quality background
            string cacheKey = $"{Width}x{Height}";
            if (_preScaledBackgrounds.TryGetValue(cacheKey, out Image? bg))
            {
                g.DrawImage(bg, 0, 0);
            }
            else if (_currentBackground != null)
            {
                g.DrawImage(_currentBackground, ClientRectangle);
            }
            else
            {
                g.Clear(Color.FromArgb(13, 13, 13));
            }

            // Draw custom sci-fi segmented progress bar at center bottom
            int barWidth = 600;
            int barHeight = 18;
            int barX = (Width - barWidth) / 2;
            int barY = Height - 100;

            // Outer glowing dark-green container
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(180, 8, 16, 8)))
            {
                g.FillRectangle(bgBrush, barX, barY, barWidth, barHeight);
            }

            using (Pen borderPen = new Pen(Color.FromArgb(0, 255, 65), 1.5f))
            {
                g.DrawRectangle(borderPen, barX, barY, barWidth, barHeight);
            }

            // Segmented blocks calculations
            int maxSegments = 25;
            int segmentGap = 3;
            int totalGapsWidth = (maxSegments - 1) * segmentGap;
            int segmentWidth = (barWidth - 6 - totalGapsWidth) / maxSegments;

            int filledSegments = (int)Math.Floor((_currentProgress / 100f) * maxSegments);

            for (int i = 0; i < filledSegments; i++)
            {
                int segX = barX + 3 + i * (segmentWidth + segmentGap);
                int segY = barY + 3;
                int segH = barHeight - 6;

                // Glowing neon cyber green
                using (SolidBrush segBrush = new SolidBrush(Color.FromArgb(0, 255, 65)))
                {
                    g.FillRectangle(segBrush, segX, segY, segmentWidth, segH);
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _smoothTimer.Stop();
            _smoothTimer.Dispose();

            _currentBackground?.Dispose();
            foreach (var kvp in _preScaledBackgrounds)
            {
                kvp.Value.Dispose();
            }
            _preScaledBackgrounds.Clear();

            base.OnFormClosed(e);
        }
    }
}

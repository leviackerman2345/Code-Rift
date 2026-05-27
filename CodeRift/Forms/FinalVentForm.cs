using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public sealed class FinalVentForm : Form
    {
        private readonly PictureBox _imageBox = new PictureBox();
        private readonly Button _btnContinue = new Button();
        private readonly List<FinalVentImageRef> _images;
        private int _currentImageIndex;

        private sealed class FinalVentImageRef
        {
            public FinalVentImageRef(string fileName, string path)
            {
                FileName = fileName;
                Path = path;
                Key = "FINAL_VENT_" + fileName;
            }

            public string FileName { get; private set; }

            public string Path { get; private set; }

            public string Key { get; private set; }
        }

        public FinalVentForm()
        {
            _images = LoadFinalVentImageRefs();
            InitializeAndShow();
        }

        public FinalVentForm(int level, bool playerWon)
        {
            string resultFileName = BuildResultFileName(level, playerWon);
            List<FinalVentImageRef> allImages = LoadFinalVentImageRefs();
            FinalVentImageRef resultImage = allImages.FirstOrDefault(
                image => string.Equals(image.FileName, resultFileName, StringComparison.OrdinalIgnoreCase));

            if (resultImage != null)
            {
                _images = new List<FinalVentImageRef> { resultImage };
            }
            else
            {
                _images = allImages;
                Console.WriteLine("Asset Warning: Final vent result image missing: " + resultFileName);
            }

            InitializeAndShow();
        }

        private static string BuildResultFileName(int level, bool playerWon)
        {
            return string.Format("level{0}_{1}.png", level, playerWon ? "win" : "lose");
        }

        private void InitializeAndShow()
        {
            ConfigureForm();
            ShowCurrentImage();
        }

        private void ConfigureForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.Black;
            KeyPreview = true;
            DoubleBuffered = true;

            _imageBox.Dock = DockStyle.Fill;
            _imageBox.BackColor = Color.Black;
            _imageBox.SizeMode = PictureBoxSizeMode.Zoom;
            _imageBox.TabStop = false;
            Controls.Add(_imageBox);

            ConfigureContinueButton();
            Controls.Add(_btnContinue);
            _btnContinue.BringToFront();

            // Enable double buffering recursively on all panels/controls to prevent transition flickering.
            EnableDoubleBuffer(this);
        }

        private void EnableDoubleBuffer(Control control)
        {
            var property = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (property != null) property.SetValue(control, true, null);

            foreach (Control child in control.Controls)
            {
                EnableDoubleBuffer(child);
            }
        }

        private void ConfigureContinueButton()
        {
            MenuButtonStyle.Apply(_btnContinue, "Continue", useMenuSize: true);
            _btnContinue.Click += ContinueButton_Click;
        }

        private static List<FinalVentImageRef> LoadFinalVentImageRefs()
        {
            string folderPath = AssetPathHelper.ResolveAssetPath("Assets", "Images", "final_vent");
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Asset Warning: Final vent folder missing: " + folderPath);
                return new List<FinalVentImageRef>();
            }

            return Directory.GetFiles(folderPath, "*.png")
                .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
                .Select(path => new FinalVentImageRef(Path.GetFileName(path), path))
                .ToList();
        }

        private void ShowCurrentImage()
        {
            if (_images.Count == 0)
            {
                _imageBox.Image = null;
                return;
            }

            _currentImageIndex = Math.Max(0, Math.Min(_currentImageIndex, _images.Count - 1));
            FinalVentImageRef image = _images[_currentImageIndex];
            _imageBox.Image = ImageManager.Instance.GetOrLoadImage(image.Key, image.Path);
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            AdvanceImageOrClose();
        }

        private void AdvanceImageOrClose()
        {
            if (_currentImageIndex < _images.Count - 1)
            {
                _currentImageIndex++;
                ShowCurrentImage();
                return;
            }

            Close();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            const int bottomPadding = 40;
            _btnContinue.Location = new Point(
                Math.Max(0, (Width - _btnContinue.Width) / 2),
                Math.Max(0, Height - _btnContinue.Height - bottomPadding));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Space)
            {
                ContinueButton_Click(this, EventArgs.Empty);
                return true;
            }

            if (keyData == Keys.Escape || keyData == Keys.Back)
            {
                Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _imageBox.Image = null;
            base.OnFormClosing(e);
        }
    }
}

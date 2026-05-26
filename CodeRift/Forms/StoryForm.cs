using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public class StoryForm : Form
    {
        private readonly StoryConfig _config;
        private readonly PictureBox _dialogueBox = new PictureBox();
        private readonly Label _dialogueLabel = new Label();
        private readonly Label _clickHintLabel = new Label();
        private readonly Button _backButton = new Button();
        private readonly Button _skipButton = new Button();
        private readonly Button _finishButton = new Button();
        private readonly System.Windows.Forms.Timer _sceneFadeTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer _typeTimer = new System.Windows.Forms.Timer();

        private int _currentStep;
        private int _fadeAlpha;
        private int _fadeStep;
        private Action? _pendingSceneUpdate;
        private bool _isSceneTransitioning;
        private string _fullText = string.Empty;
        private int _typeIndex;
        private bool _isTyping;
        private bool _isFinishing;
        private int _lastRenderedTypeIndex;

        public StoryForm(StoryConfig config)
        {
            _config = config;
            ConfigureForm();
            ConfigureDialogueBox();
            ConfigureButtons();
            ConfigureTimers();

            AudioManager.Instance.PlayMusic(_config.MusicKey);
            UpdateScene();
        }

        private void ConfigureForm()
        {
            Text = _config.Title;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.Black;
            KeyPreview = true;
            DoubleBuffered = true;
            ClientSize = new Size(1280, 720);
            BackgroundImageLayout = ImageLayout.Stretch;

            Controls.Add(_dialogueBox);
            Controls.Add(_finishButton);
            Controls.Add(_skipButton);
            Controls.Add(_backButton);

            Click += Form_Click;
        }

        private void ConfigureDialogueBox()
        {
            _dialogueBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            _dialogueBox.BackColor = Color.Transparent;
            _dialogueBox.Location = new Point(100, 520);
            _dialogueBox.Size = new Size(1080, 150);
            _dialogueBox.SizeMode = PictureBoxSizeMode.StretchImage;
            _dialogueBox.Image = ImageManager.Instance.GetImage(Constants.IMG_UI_DIALOGUE);
            _dialogueBox.Click += DialogueElement_Click;

            _dialogueLabel.ForeColor = Color.FromArgb(0, 255, 65);
            _dialogueLabel.Font = new Font("Courier New", 18, FontStyle.Bold);
            _dialogueLabel.BackColor = Color.Transparent;
            _dialogueLabel.Parent = _dialogueBox;
            _dialogueLabel.Location = new Point(50, 25);
            _dialogueLabel.Size = new Size(_dialogueBox.Width - 100, _dialogueBox.Height - 55);
            _dialogueLabel.TextAlign = ContentAlignment.MiddleCenter;
            _dialogueLabel.Click += DialogueElement_Click;

            _clickHintLabel.Text = "[Click anywhere to continue]";
            _clickHintLabel.Font = new Font("Courier New", 11, FontStyle.Bold | FontStyle.Italic);
            _clickHintLabel.ForeColor = Color.FromArgb(0, 255, 65);
            _clickHintLabel.BackColor = Color.Transparent;
            _clickHintLabel.Parent = _dialogueBox;
            _clickHintLabel.AutoSize = true;
            _clickHintLabel.Location = new Point(_dialogueBox.Width - 300, _dialogueBox.Height - 35);
            _clickHintLabel.Click += DialogueElement_Click;
            _clickHintLabel.BringToFront();
        }

        private void ConfigureButtons()
        {
            MenuButtonStyle.Apply(_backButton, "[BACK]");
            _backButton.Size = new Size(180, 50);
            _backButton.Location = new Point(28, 24);
            _backButton.Click += BackButton_Click;

            MenuButtonStyle.Apply(_skipButton, "[SKIP]");
            _skipButton.Size = new Size(180, 50);
            _skipButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _skipButton.Location = new Point(Width - _skipButton.Width - 28, 24);
            _skipButton.Click += SkipButton_Click;

            MenuButtonStyle.Apply(_finishButton, _config.FinishButtonText, useMenuSize: true);
            _finishButton.Visible = false;
            _finishButton.Click += FinishButton_Click;
        }

        private void ConfigureTimers()
        {
            _sceneFadeTimer.Interval = 16;
            _sceneFadeTimer.Tick += SceneFadeTimer_Tick;

            _typeTimer.Interval = 30;
            _typeTimer.Tick += TypeTimer_Tick;
        }

        private void UpdateScene()
        {
            if (_currentStep >= _config.Steps.Count)
            {
                FinishStory();
                return;
            }

            StoryStep step = _config.Steps[_currentStep];
            string? previousImageKey = _currentStep > 0 ? _config.Steps[_currentStep - 1].ImageKey : null;

            if (step.ImageKey != previousImageKey)
            {
                StartSceneFade(step.ImageKey);
            }
            else
            {
                AudioManager.Instance.PlaySFX(Constants.SFX_CG_CLICK);
                StartTyping();
            }

            UpdateFinishButtonState();
        }

        private void StartSceneFade(string imageKey)
        {
            _isSceneTransitioning = true;
            _pendingSceneUpdate = () =>
            {
                BackgroundImage = ImageManager.Instance.GetImage(imageKey);
                AudioManager.Instance.PlaySFX(Constants.SFX_CG_CLICK);
            };

            _fadeAlpha = 0;
            _fadeStep = 15;
            _dialogueLabel.Text = string.Empty;
            _isTyping = false;
            _typeTimer.Stop();
            _sceneFadeTimer.Start();
        }

        private void SceneFadeTimer_Tick(object? sender, EventArgs e)
        {
            _fadeAlpha += _fadeStep;
            if (_fadeAlpha >= 255)
            {
                _fadeAlpha = 255;
                _fadeStep = -15;
                _pendingSceneUpdate?.Invoke();
                _pendingSceneUpdate = null;
            }
            else if (_fadeAlpha <= 0)
            {
                _fadeAlpha = 0;
                _sceneFadeTimer.Stop();
                _isSceneTransitioning = false;
                StartTyping();
            }

            Invalidate();
        }

        private void TypeTimer_Tick(object? sender, EventArgs e)
        {
            if (_typeIndex < _fullText.Length)
            {
                _typeIndex++;

                // Render every 2 chars to cut label/string churn while keeping the same typing feel.
                bool shouldRenderNow = _typeIndex == _fullText.Length || (_typeIndex - _lastRenderedTypeIndex) >= 2;
                if (shouldRenderNow)
                {
                    _dialogueLabel.Text = _fullText.Substring(0, _typeIndex);
                    _lastRenderedTypeIndex = _typeIndex;
                }

                return;
            }

            _isTyping = false;
            _typeTimer.Stop();
        }

        private void StartTyping()
        {
            if (_currentStep >= _config.Steps.Count)
            {
                return;
            }

            _fullText = _config.Steps[_currentStep].Text;
            _typeIndex = 0;
            _lastRenderedTypeIndex = 0;
            _dialogueLabel.Text = string.Empty;
            _isTyping = true;
            _typeTimer.Start();
        }

        private void Form_Click(object? sender, EventArgs e)
        {
            AdvanceDialogue();
        }

        private void DialogueElement_Click(object? sender, EventArgs e)
        {
            AdvanceDialogue();
        }

        private void BackButton_Click(object? sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            Close();
        }

        private void SkipButton_Click(object? sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            FinishStory();
        }

        private void FinishButton_Click(object? sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            FinishStory();
        }

        private void AdvanceDialogue()
        {
            if (_isSceneTransitioning || _isFinishing)
            {
                return;
            }

            if (_isTyping)
            {
                _isTyping = false;
                _typeTimer.Stop();
                _dialogueLabel.Text = _fullText;
                return;
            }

            if (_config.ShowFinishButtonOnLastStep && _currentStep >= _config.Steps.Count - 1)
            {
                return;
            }

            _currentStep++;
            UpdateScene();
        }

        private void UpdateFinishButtonState()
        {
            bool showFinishButton = _config.ShowFinishButtonOnLastStep && _currentStep == _config.Steps.Count - 1;
            _finishButton.Visible = showFinishButton;
            _skipButton.Visible = !showFinishButton;

            if (showFinishButton)
            {
                _finishButton.BringToFront();
            }
        }

        private void FinishStory()
        {
            if (_isFinishing)
            {
                return;
            }

            _isFinishing = true;
            _sceneFadeTimer.Stop();
            _typeTimer.Stop();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(Constants.SFX_CG_END);
            _config.FinishAction(this);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            _dialogueBox.SetBounds(100, Height - 200, Math.Max(300, Width - 200), 150);
            _dialogueLabel.Location = new Point(50, 25);
            _dialogueLabel.Size = new Size(_dialogueBox.Width - 100, _dialogueBox.Height - 55);
            _clickHintLabel.Location = new Point(Math.Max(0, _dialogueBox.Width - 300), _dialogueBox.Height - 35);

            _skipButton.Location = new Point(Math.Max(28, Width - _skipButton.Width - 28), 24);
            _finishButton.Location = new Point(
                Math.Max(0, (Width - _finishButton.Width) / 2),
                Math.Max(0, Height - _finishButton.Height - 40));
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (_fadeAlpha > 0)
            {
                using SolidBrush brush = new SolidBrush(Color.FromArgb(_fadeAlpha, 0, 0, 0));
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _sceneFadeTimer.Stop();
            _typeTimer.Stop();
            AudioManager.Instance.StopMusic();
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

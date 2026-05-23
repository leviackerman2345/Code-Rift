using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    // Ending cutscene form: advances linear dialogue with transitions, then returns user to credits/main flow.
    public partial class EpilogueForm : Form
    {
        private int _currentStep = 0;

        private struct EpilogueStep
        {
            public string Text;
            public string ImageKey;
        }

        private readonly List<EpilogueStep> _steps = new List<EpilogueStep>();

        private System.Windows.Forms.Timer _transitionTimer;
        private System.Windows.Forms.Timer _typeTimer;
        private int _fadeAlpha = 0;
        private int _fadeStep = 0;
        private Action _pendingSceneUpdate;
        private bool _isTransitioning = false;
        private string _fullText = "";
        private int _typeIndex = 0;
        private bool _isTyping = false;

        public EpilogueForm()
        {
            InitializeComponent();
            InitializeScript();
            SetupForm();
            UpdateScene();
        }

        private void InitializeScript()
        {
            // SLIDE 1
            AddStep("After a long and devastating battle, Elias finally defeats the supreme Bug known as: The Null King.", Constants.EP_01);
            AddStep("The origin of all corruption born from the Digital Rift.", Constants.EP_01);

            // SLIDE 2
            AddStep("As the Null King falls, its body begins to break apart into streams of green code and fragmented binary.", Constants.EP_02);
            AddStep("The entire Digital Rift Realm trembles violently as the corruption holding it together starts to collapse.", Constants.EP_02);
            AddStep("Then… Something unexpected happens.", Constants.EP_02);

            // SLIDE 3
            AddStep("The corrupted world does not explode. It begins to heal.", Constants.EP_03);
            AddStep("Across the Digital Rift, broken structures repair themselves. Glitched skies stabilize.", Constants.EP_03);
            AddStep("Distorted landscapes return to normal. The endless streams of corrupted code slowly reorganize into clean flowing data.", Constants.EP_03);

            // SLIDE 4
            AddStep("The Bugs — once terrifying monsters of destruction — begin disappearing one by one.", Constants.EP_04);
            AddStep("Not dying. But being fixed.", Constants.EP_04);
            AddStep("Their corrupted code was finally repaired.", Constants.EP_04);

            // SLIDE 5
            AddStep("For the first time in over a century, silence fills the world.", Constants.EP_05);
            AddStep("The Digital Rift itself starts closing the massive fissures scattered across reality.", Constants.EP_05);
            AddStep("One by one, the portals disappear, sealing the connection between the human world and the corrupted realm.", Constants.EP_05);

            // SLIDE 6
            AddStep("Far across the northern mountains, the remaining survivors watch the skies as the green light slowly fades away.", Constants.EP_06);
            AddStep("Humanity was finally free.", Constants.EP_06);

            // SLIDE 7
            AddStep("As Elias stands within the collapsing Rift Realm, the spirit of the Great Compiler appears before him one final time.", Constants.EP_07);
            AddStep("The Great Compiler smiles. “You did not destroy the future, Elias… You corrected it.”", Constants.EP_07);

            // SLIDE 8
            AddStep("The Console begins losing its glow. Its purpose had finally been fulfilled.", Constants.EP_08);
            AddStep("Before disappearing, the Great Compiler thanks Elias for giving humanity another chance.", Constants.EP_08);
            AddStep("Then, like fading code, his spirit vanishes peacefully into the light.", Constants.EP_08);

            // SLIDE 9
            AddStep("Moments later, Elias escapes the Digital Rift just before the final portal closes forever.", Constants.EP_09);
            AddStep("Years later, humanity slowly begins rebuilding civilization. Cities rise again.", Constants.EP_09);
            AddStep("The survivors no longer live in fear of the Bugs.", Constants.EP_09);
            AddStep("The story of Elias and the Great Compiler becomes a legend passed down across generations.", Constants.EP_09);
            AddStep("A reminder that even the most corrupted systems can still be repaired.", Constants.EP_09);

            // SLIDE 10
            AddStep("But deep beneath the ruins of the old world… A tiny green symbol suddenly flickers in the darkness.", Constants.EP_10);
            AddStep("System.Reboot();", Constants.EP_10);
        }

        private void AddStep(string text, string imageKey)
        {
            _steps.Add(new EpilogueStep { Text = text, ImageKey = imageKey });
        }

        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(13, 13, 13); // Near black
            this.KeyPreview = true; // Essential for capturing keys regardless of focused control
            this.DoubleBuffered = true;

            _transitionTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _transitionTimer.Tick += TransitionTimer_Tick;

            _typeTimer = new System.Windows.Forms.Timer { Interval = 30 };
            _typeTimer.Tick += TypeTimer_Tick;

            dialogueBox.Image = ImageManager.Instance.GetImage(Constants.IMG_UI_DIALOGUE);
            dialogueBox.SizeMode = PictureBoxSizeMode.StretchImage;

            dialogueLabel.ForeColor = Color.FromArgb(0, 255, 65);
            dialogueLabel.Font = new Font("Courier New", 18, FontStyle.Bold);
            dialogueLabel.BackColor = Color.Transparent;
            dialogueLabel.Parent = dialogueBox;
            dialogueLabel.Location = new Point(50, 30);
            dialogueLabel.Size = new Size(dialogueBox.Width - 100, dialogueBox.Height - 60);
            dialogueLabel.TextAlign = ContentAlignment.MiddleCenter;

            lblClickHint.Text = "[Click anywhere to continue]";
            lblClickHint.Font = new Font("Courier New", 11, FontStyle.Bold | FontStyle.Italic);
            lblClickHint.ForeColor = Color.FromArgb(0, 255, 65); // Fully opaque
            lblClickHint.BackColor = Color.Transparent;
            lblClickHint.Parent = dialogueBox;
            lblClickHint.AutoSize = true;
            lblClickHint.Location = new Point(dialogueBox.Width - 300, dialogueBox.Height - 35);
            lblClickHint.BringToFront(); // Ensure hint is above the dialogue text

            btnReturn.Visible = false;
            btnReturn.TabStop = false; // Prevents button from stealing focus
            btnReturn.FlatStyle = FlatStyle.Flat;
            btnReturn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            btnReturn.ForeColor = Color.FromArgb(0, 255, 65);
            btnReturn.BackColor = Color.Black;
            btnReturn.Font = new Font("Courier New", 18, FontStyle.Bold);

            if (btnBack != null)
            {
                StyleNavigationButton(btnBack);
                btnBack.TabStop = false;
            }

            if (btnSkip != null)
            {
                StyleNavigationButton(btnSkip);
                btnSkip.TabStop = false;
            }

            this.Click += dialogueBox_Click; // Allow clicking anywhere on the background to progress
            this.Focus();

            AudioManager.Instance.PlayMusic(Constants.MUSIC_EPILOGUE);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _transitionTimer?.Stop();
            _typeTimer?.Stop();
            AudioManager.Instance.StopMusic();
            base.OnFormClosing(e);
        }

        private void UpdateScene()
        {
            if (_currentStep < _steps.Count)
            {
                var step = _steps[_currentStep];
                string prevImageKey = _currentStep > 0 ? _steps[_currentStep - 1].ImageKey : null;

                if (step.ImageKey != prevImageKey)
                {
                    _isTransitioning = true;
                    _pendingSceneUpdate = () => 
                    {
                        this.BackgroundImage = ImageManager.Instance.GetImage(step.ImageKey);
                        this.BackgroundImageLayout = ImageLayout.Stretch;
                        AudioManager.Instance.PlaySFX(Constants.SFX_CG_CLICK);
                    };
                    _fadeStep = 15;
                    _fadeAlpha = 0;
                    
                    dialogueLabel.Text = "";
                    _isTyping = false;
                    if (_typeTimer != null) _typeTimer.Stop();

                    _transitionTimer.Start();
                }
                else
                {
                    AudioManager.Instance.PlaySFX(Constants.SFX_CG_CLICK);
                    StartTyping();
                }

                if (_currentStep == _steps.Count - 1)
                {
                    // End state: expose explicit return action and hide skip.
                    btnReturn.Visible = true;
                    btnReturn.TabStop = true; // Allow tab to reach it at the end
                    btnReturn.Focus();
                    btnSkip.Visible = false;
                }
            }
        }

        private void TransitionTimer_Tick(object sender, EventArgs e)
        {
            _fadeAlpha += _fadeStep;
            if (_fadeAlpha >= 255)
            {
                _fadeAlpha = 255;
                _fadeStep = -15; // start fading in
                _pendingSceneUpdate?.Invoke();
                _pendingSceneUpdate = null;
            }
            else if (_fadeAlpha <= 0)
            {
                _fadeAlpha = 0;
                _transitionTimer.Stop();
                _isTransitioning = false;
                StartTyping();
            }
            this.Invalidate(); // trigger repaint of background
        }

        private void TypeTimer_Tick(object sender, EventArgs e)
        {
            if (_typeIndex < _fullText.Length)
            {
                _typeIndex++;
                dialogueLabel.Text = _fullText.Substring(0, _typeIndex);
            }
            else
            {
                _isTyping = false;
                _typeTimer.Stop();
            }
        }

        private void StartTyping()
        {
            if (_currentStep >= _steps.Count) return;
            
            _fullText = _steps[_currentStep].Text;
            _typeIndex = 0;
            dialogueLabel.Text = "";
            _isTyping = true;
            _typeTimer.Start();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (_fadeAlpha > 0)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(_fadeAlpha, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            }
        }

        private void AdvanceDialogue()
        {
            if (_isTransitioning) return;

            if (_isTyping)
            {
                _isTyping = false;
                _typeTimer.Stop();
                dialogueLabel.Text = _fullText;
            }
            else
            {
                if (_currentStep < _steps.Count - 1)
                {
                    _currentStep++;
                    UpdateScene();
                }
            }
        }

        private void dialogueBox_Click(object? sender, EventArgs e)
        {
            AdvanceDialogue();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            ShowCreditsAndReturn();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            _transitionTimer?.Stop();
            _typeTimer?.Stop();
            this.Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
            _transitionTimer?.Stop();
            _typeTimer?.Stop();
            ShowCreditsAndReturn();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (dialogueBox != null && dialogueLabel != null)
            {
                dialogueLabel.Size = new Size(dialogueBox.Width - 100, dialogueBox.Height - 60);
                dialogueLabel.Location = new Point(50, 30);
            }
        }

        private void ShowCreditsAndReturn()
        {
            // Ending route: epilogue -> credits -> signal levels menu to exit back to main menu.
            CreditsForm creditsForm = new CreditsForm();
            creditsForm.FormClosed += (s, args) => 
            {
                // Signal LevelsMenuForm to close so we return all the way to the main menu
                foreach (Form f in Application.OpenForms)
                {
                    if (f is LevelsMenuForm)
                    {
                        f.Tag = "EXIT_TO_MENU";
                        break;
                    }
                }
                this.Close(); 
            };
            creditsForm.Shown += (s, args) => this.Hide();
            creditsForm.Show();
        }

        private void EpilogueForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Keyboard progression disabled as per strict input controls
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Space)
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static void StyleNavigationButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 255, 65);
            button.ForeColor = Color.FromArgb(0, 255, 65);
            button.BackColor = Color.Black;
            button.Font = new Font("Courier New", 12, FontStyle.Bold);
        }
    }
}

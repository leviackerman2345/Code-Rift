using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    // Ending cutscene form: advances linear dialogue, then returns user to credits/main flow.
    public partial class EpilogueForm : Form
    {
        private int _currentDialogue = 0;
        private readonly string[] _dialogues = {
            "Epilogue — The Final Compilation",
            "After a long and devastating battle, Elias finally defeats the supreme Bug known as: The Null King.",
            "The origin of all corruption born from the Digital Rift.",
            "As the Null King falls, its body begins to break apart into streams of green code and fragmented binary.",
            "The entire Digital Rift Realm trembles violently as the corruption holding it together starts to collapse.",
            "Then… Something unexpected happens.",
            "The corrupted world does not explode. It begins to heal.",
            "Across the Digital Rift, broken structures repair themselves. Glitched skies stabilize.",
            "Distorted landscapes return to normal. The endless streams of corrupted code slowly reorganize into clean flowing data.",
            "The Bugs — once terrifying monsters of destruction — begin disappearing one by one.",
            "Not dying. But being fixed.",
            "Their corrupted code was finally repaired.",
            "For the first time in over a century, silence fills the world.",
            "The Digital Rift itself starts closing the massive fissures scattered across reality.",
            "One by one, the portals disappear, sealing the connection between the human world and the corrupted realm.",
            "Far across the northern mountains, the remaining survivors watch the skies as the green light slowly fades away.",
            "Humanity was finally free.",
            "As Elias stands within the collapsing Rift Realm, the spirit of the Great Compiler appears before him one final time.",
            "The Great Compiler smiles. “You did not destroy the future, Elias… You corrected it.”",
            "The Console begins losing its glow. Its purpose had finally been fulfilled.",
            "Before disappearing, the Great Compiler thanks Elias for giving humanity another chance.",
            "Then, like fading code, his spirit vanishes peacefully into the light.",
            "Moments later, Elias escapes the Digital Rift just before the final portal closes forever.",
            "Years later, humanity slowly begins rebuilding civilization. Cities rise again.",
            "The survivors no longer live in fear of the Bugs.",
            "The story of Elias and the Great Compiler becomes a legend passed down across generations.",
            "A reminder that even the most corrupted systems can still be repaired.",
            "But deep beneath the ruins of the old world… A tiny green symbol suddenly flickers in the darkness.",
            "System.Reboot();"
        };

        public EpilogueForm()
        {
            InitializeComponent();
            SetupForm();
            UpdateDialogue();
        }

        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(13, 13, 13); // Near black
            this.KeyPreview = true; // Essential for capturing keys regardless of focused control

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
        }

        private void UpdateDialogue()
        {
            if (_currentDialogue < _dialogues.Length)
            {
                dialogueLabel.Text = _dialogues[_currentDialogue];
                AudioManager.Instance.PlaySFX(Constants.SFX_CG_CLICK);
                
                if (_currentDialogue == _dialogues.Length - 1)
                {
                    // End state: expose explicit return action and hide skip.
                    btnReturn.Visible = true;
                    btnReturn.TabStop = true; // Allow tab to reach it at the end
                    btnReturn.Focus();
                    btnSkip.Visible = false;
                }
            }
        }

        private void AdvanceDialogue()
        {
            if (_currentDialogue < _dialogues.Length - 1)
            {
                _currentDialogue++;
                UpdateDialogue();
            }
        }

        private void dialogueBox_Click(object? sender, EventArgs e)
        {
            AdvanceDialogue();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            ShowCreditsAndReturn();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            ShowCreditsAndReturn();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (dialogueBox != null && dialogueLabel != null)
            {
                dialogueLabel.Size = dialogueBox.Size;
                dialogueLabel.Location = new Point(0, 0);
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

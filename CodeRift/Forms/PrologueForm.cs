using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    // Story cutscene form: click advances narrative, then transitions to levels menu.
    public partial class PrologueForm : Form
    {
        private int _currentStep = 0;

        private struct PrologueStep
        {
            public string Text;
            public string ImageKey;
        }

        private readonly List<PrologueStep> _steps = new List<PrologueStep>();

        public PrologueForm()
        {
            InitializeComponent();
            InitializeScript();
            SetupForm();
            UpdateScene();
        }

        private void InitializeScript()
        {
            // Script is linear; each step binds dialogue text to one CG image key.
            // SLIDE 1
            AddStep("In the age before the collapse, humanity lived in peace. Nations flourished, technology advanced rapidly, and the world believed its future was secure.", Constants.CG_01);

            // SLIDE 2
            AddStep("Then came the quake.", Constants.CG_02);
            AddStep("Without warning, the earth trembled violently across the globe. Cities crumbled, oceans roared, and the skies darkened. When the disaster finally ended, humanity witnessed something impossible.", Constants.CG_02);
            AddStep("Across different parts of the world, enormous fractures had appeared in reality itself.", Constants.CG_02);

            // SLIDE 3
            AddStep("They glowed with an eerie green light, filled with streams of floating binary digits and distorted symbols. Scientists could not explain them. Survivors would later give them a single name:", Constants.CG_03);
            AddStep("— The Digital Rift —", Constants.CG_03);

            // SLIDE 4
            AddStep("At first, people believed the rifts were harmless.", Constants.CG_04);
            AddStep("They were wrong.", Constants.CG_04);
            AddStep("From the depths of the rifts emerged terrifying creatures known as the Bugs — corrupted beings formed from broken code, failed logic, and digital corruption. These monsters spread rapidly across cities, destroying everything in their path.", Constants.CG_04);

            // SLIDE 5
            AddStep("Weapons had little effect against them, and entire civilizations fell within months.", Constants.CG_05);
            AddStep("Humanity was pushed to the brink of extinction.", Constants.CG_05);
            AddStep("The remaining survivors fled north, hiding deep within frozen mountains where the Bugs struggled to reach. There, humanity built its final sanctuary: the Stronghold.", Constants.CG_05);

            // SLIDE 6
            AddStep("Years passed.", Constants.CG_06);
            AddStep("During an expedition beneath an ancient ruined temple, survivors discovered a mysterious artifact unlike anything they had ever seen.", Constants.CG_06);
            AddStep("It was a black metallic device covered in glowing symbols and shifting lines of code.", Constants.CG_06);
            AddStep("— The Console —", Constants.CG_06);

            // SLIDE 7
            AddStep("Legends claimed the Console possessed the power to destroy the Bugs permanently. However, the artifact could not be activated by ordinary people.", Constants.CG_07);
            AddStep("According to ancient records left within the temple, only a chosen individual worthy of the 'Great Compiler' would one day wield its power.", Constants.CG_07);
            AddStep("Most believed it was only a myth.", Constants.CG_07);
            AddStep("A false hope created to comfort humanity during its final days.", Constants.CG_07);
            AddStep("But the Bugs feared the Console.", Constants.CG_07);
            AddStep("And deep within the Rift, something was watching.", Constants.CG_07);

            // SLIDE 8
            AddStep("— One Hundred Years Later —", Constants.CG_08);
            AddStep("For nearly a century, humanity remained hidden under the protection of the Great Compiler — a mysterious guardian who watched over the Stronghold and kept the Bugs away.", Constants.CG_08);
            AddStep("Peace, however, never lasts forever.", Constants.CG_08);
            AddStep("One night, the mountains shook once again.", Constants.CG_08);
            AddStep("The Bugs had returned.", Constants.CG_08);

            // SLIDE 9
            AddStep("This time, they came not as scattered monsters, but as an army.", Constants.CG_09);
            AddStep("The Stronghold was overwhelmed. Walls collapsed. Entire districts were consumed by corruption. Thousands were slaughtered while others were dragged away into the darkness beyond the mountains.", Constants.CG_09);
            AddStep("At the center of the invasion stood the Great Compiler himself, fighting endlessly to protect the last remnants of humanity.", Constants.CG_09);
            AddStep("But even he could not stop them forever.", Constants.CG_09);

            // SLIDE 10
            AddStep("The Bugs had finally discovered the truth behind the Console.", Constants.CG_10);
            AddStep("They had come to destroy it.", Constants.CG_10);
            AddStep("Because as long as the Console existed, the Bugs could never truly win.", Constants.CG_10);
            AddStep("Mortally wounded and nearing death, the Great Compiler made one final decision.", Constants.CG_10);

            // SLIDE 11
            AddStep("Using the last of his strength, he merged his spirit into the Console itself, sealing his knowledge, power, and consciousness within the artifact.", Constants.CG_11);
            AddStep("Before disappearing, he left behind a final message:", Constants.CG_11);
            AddStep("'One day, a worthy soul will compile the future humanity could not.'", Constants.CG_11);
            AddStep("As the Stronghold burned, the Console vanished into the chaos.", Constants.CG_11);
            AddStep("Lost. Forgotten. Waiting.", Constants.CG_11);

            // SLIDE 12
            AddStep("Far from the ruins of the capital, in a small hidden village near the northern mountains, a young boy named Elias lived an ordinary life.", Constants.CG_12);
            AddStep("Orphaned during earlier Bug attacks, Elias spent his days helping rebuild the village alongside the remaining survivors.", Constants.CG_12);
            AddStep("Like many others, he had grown up hearing stories about the Digital Rift, the Bugs, and the legendary Great Compiler.", Constants.CG_12);
            AddStep("Stories he never truly believed.", Constants.CG_12);

            // SLIDE 13
            AddStep("Until the day everything changed.", Constants.CG_13);
            AddStep("While searching through the remains of a destroyed caravan near the forest, Elias discovered a strange black device buried beneath the snow.", Constants.CG_13);
            AddStep("The moment he touched it, the Console activated.", Constants.CG_13);
            AddStep("Green symbols illuminated the darkness. Code flowed across its surface. And a voice echoed within his mind.", Constants.CG_13);
            AddStep("The spirit of the Great Compiler had awakened.", Constants.CG_13);
            AddStep("The Console recognized Elias as its new wielder.", Constants.CG_13);
            AddStep("Through the Console, Elias gained access to the ancient language capable of fighting the Bugs — the power of C#.", Constants.CG_13);
            AddStep("Under the guidance of the Great Compiler, Elias begins his journey across the corrupted world, learning to master code, battle the Bugs, and uncover the truth behind the Digital Rift before humanity disappears forever.", Constants.CG_13);
        }

        private void AddStep(string text, string imageKey)
        {
            _steps.Add(new PrologueStep { Text = text, ImageKey = imageKey });
        }

        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;
            this.KeyPreview = true; // Essential for capturing keys regardless of focused control

            // Dialogue box setup
            dialogueBox.Image = ImageManager.Instance.GetImage(Constants.IMG_UI_DIALOGUE);
            dialogueBox.SizeMode = PictureBoxSizeMode.StretchImage;

            dialogueLabel.ForeColor = Color.FromArgb(0, 255, 65); // Terminal Green
            dialogueLabel.Font = new Font("Courier New", 18, FontStyle.Bold);
            dialogueLabel.BackColor = Color.Transparent;
            dialogueLabel.Parent = dialogueBox;
            dialogueLabel.Location = new Point(0, 0);
            dialogueLabel.Size = dialogueBox.Size;
            dialogueLabel.TextAlign = ContentAlignment.MiddleCenter;

            lblClickHint.Text = "[Click anywhere to continue]";
            lblClickHint.Font = new Font("Courier New", 11, FontStyle.Bold | FontStyle.Italic);
            lblClickHint.ForeColor = Color.FromArgb(0, 255, 65); // Fully opaque
            lblClickHint.BackColor = Color.Transparent;
            lblClickHint.Parent = dialogueBox;
            lblClickHint.AutoSize = true;
            lblClickHint.Location = new Point(dialogueBox.Width - 300, dialogueBox.Height - 35);
            lblClickHint.BringToFront(); // Ensure hint is above the dialogue text

            StyleNavigationButton(btnBack);
            btnBack.TabStop = false; // Prevents button from stealing focus
            StyleNavigationButton(btnSkip);
            btnSkip.TabStop = false;
            this.Click += dialogueBox_Click; // Allow clicking anywhere on the background to progress
            this.Focus();
        }

        private void UpdateScene()
        {
            if (_currentStep < _steps.Count)
            {
                var step = _steps[_currentStep];
                // Images were preloaded in splash; this only swaps references.
                this.BackgroundImage = ImageManager.Instance.GetImage(step.ImageKey);
                this.BackgroundImageLayout = ImageLayout.Stretch;
                dialogueLabel.Text = step.Text;
                AudioManager.Instance.PlaySFX(Constants.SFX_CG_CLICK);
            }
            else
            {
                TransitionToLevelsMenu();
            }
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

        private void PrologueForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Keyboard progression disabled as per strict input controls
        }

        private void dialogueBox_Click(object? sender, EventArgs e)
        {
            AdvanceDialogue();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            TransitionToLevelsMenu();
        }

        private void AdvanceDialogue()
        {
            _currentStep++;
            UpdateScene();
        }

        private void TransitionToLevelsMenu()
        {
            // Story route: prologue -> levels selection.
            AudioManager.Instance.PlaySFX(Constants.SFX_CG_END);
            this.Hide();
            var levelsMenu = new LevelsMenuForm();
            levelsMenu.FormClosed += (s, args) => this.Close();
            levelsMenu.Show();
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

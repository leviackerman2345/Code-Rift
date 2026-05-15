using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public class PrologueForm : Form
    {
        private Label lblStory;
        private Label lblCounter;
        private Button btnNext;
        private Button btnBack;
        private Button btnSkip;
        private int currentSlide = 0;

        private readonly string[] slides = new string[]
        {
            "In the age before the collapse, the world was a seamless lattice of logic and light.",
            "Systems hummed in perfect synchronicity, governed by the Great Compiler.",
            "But a shadow fell across the source code.",
            "An anomaly, ancient and nameless, began to unspool the threads of reality.",
            "They called it the Null King.",
            "Corruption spread like a wildfire through the silicon veins of the world.",
            "Data screamed as it was rewritten into nonsensical loops and jagged arrays.",
            "Elias, once a humble apprentice of the Syntax Sanctum, watched as his masters were swallowed by the rising tide of Bugs.",
            "The Sanctum fell.",
            "The Great Compiler was silenced.",
            "The world became a fragmented wasteland known as the Code Rift.",
            "But Elias carried a secret.",
            "A terminal, forged from the purest logic, capable of speaking the language of the old world.",
            "With this terminal, he could challenge the corruption.",
            "He could debug the world, one line at a time.",
            "His journey would take him through the Loop Plains, where time itself repeated in endless, agonizing cycles.",
            "He would traverse the Method Mountains, where the very laws of cause and effect were twisted into forbidden functions.",
            "He would brave the String Seas, where communication was a cacophony of broken characters and hollow promises.",
            "And he would cross the Array Abyss, a graveyard of multi-dimensional structures where logic went to die.",
            "At the heart of it all waited the Null King, sitting upon a throne of unhandled exceptions.",
            "Elias gripped his terminal.",
            "The screen flickered to life.",
            "The spirit of the Great Compiler had awakened."
        };

        public PrologueForm()
        {
            InitializeComponent();
            ShowSlide();
        }

        private void InitializeComponent()
        {
            this.Text = "Code Rift - Prologue";
            this.BackColor = ColorTranslator.FromHtml("#0D0D0D");
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            lblStory = new Label();
            lblStory.Font = new Font("Segoe UI", 20);
            lblStory.ForeColor = Color.White;
            lblStory.AutoSize = false;
            lblStory.Size = new Size(Screen.PrimaryScreen.Bounds.Width - 400, 300);
            lblStory.Location = new Point(200, (Screen.PrimaryScreen.Bounds.Height - 500) / 2 + 50);
            lblStory.TextAlign = ContentAlignment.MiddleCenter;

            lblCounter = new Label();
            lblCounter.Font = new Font("Segoe UI", 14);
            lblCounter.ForeColor = ColorTranslator.FromHtml("#00FF41");
            lblCounter.AutoSize = true;
            lblCounter.Location = new Point((Screen.PrimaryScreen.Bounds.Width - 50) / 2, Screen.PrimaryScreen.Bounds.Height - 150);

            btnNext = CreateStyledButton("NEXT →", Screen.PrimaryScreen.Bounds.Width - 200);
            btnNext.Click += (s, e) => {
                if (currentSlide < slides.Length - 1)
                {
                    currentSlide++;
                    ShowSlide();
                }
                else
                {
                    SkipToLevelSelect();
                }
            };

            btnBack = CreateStyledButton("← BACK", 50);
            btnBack.Click += (s, e) => {
                if (currentSlide > 0)
                {
                    currentSlide--;
                    ShowSlide();
                }
                else
                {
                    new MainMenuForm().Show();
                    this.Hide();
                }
            };

            btnSkip = CreateStyledButton("SKIP", 50);
            btnSkip.Location = new Point(50, Screen.PrimaryScreen.Bounds.Height - 170); // Placed above Back button
            btnSkip.Click += (s, e) => SkipToLevelSelect();

            this.Controls.Add(lblStory);
            this.Controls.Add(lblCounter);
            this.Controls.Add(btnNext);
            this.Controls.Add(btnBack);
            this.Controls.Add(btnSkip);
        }

        private Button CreateStyledButton(string text, int x)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(150, 50);
            btn.Location = new Point(x, Screen.PrimaryScreen.Bounds.Height - 100);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#00FF41");
            btn.ForeColor = ColorTranslator.FromHtml("#00FF41");
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.BackColor = ColorTranslator.FromHtml("#111111");
            return btn;
        }

        private void ShowSlide()
        {
            lblCounter.Text = $"{currentSlide + 1} / {slides.Length}";
            lblStory.Text = slides[currentSlide]; // No typewriter effect as requested
        }

        private void SkipToLevelSelect()
        {
            new LevelSelectForm().Show();
            this.Hide();
        }
    }
}

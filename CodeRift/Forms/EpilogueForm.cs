using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public class EpilogueForm : Form
    {
        private Label lblStory;
        private Button btnNext;
        private Button btnBack;
        private int currentSlide = 0;

        private readonly string[] slides = new string[]
        {
            "After a long and devastating battle, the Null King let out a final, discordant shriek before dissolving into a cloud of unallocated memory.",
            "The Code Rift trembled.",
            "The fragmented data structures began to align.",
            "The loops unrolled, and the broken strings found their meaning again.",
            "Elias stood at the center of the rebooting world, his terminal glowing with a steady, warm light.",
            "The Great Compiler was restored.",
            "Life returned to the lattice.",
            "The silicon veins once again hummed with the rhythm of perfect logic.",
            "The darkness had been debugged.",
            "But Elias knew his work was not over.",
            "In a world of infinite code, new anomalies would always arise.",
            "He would be ready.",
            "System.Reboot();"
        };

        public EpilogueForm()
        {
            InitializeComponent();
            ShowSlide();
        }

        private void InitializeComponent()
        {
            this.Text = "Code Rift - Epilogue";
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
            // ASSET SWAP: Epilogue illustrations per slide

            btnNext = CreateStyledButton("NEXT →", Screen.PrimaryScreen.Bounds.Width - 200);
            btnNext.Click += (s, e) => {
                if (currentSlide < slides.Length - 1)
                {
                    currentSlide++;
                    ShowSlide();
                }
                else
                {
                    EndGame();
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
                    new LevelSelectForm().Show();
                    this.Hide();
                }
            };

            this.Controls.Add(lblStory);
            this.Controls.Add(btnNext);
            this.Controls.Add(btnBack);
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
            if (currentSlide == slides.Length - 1)
            {
                lblStory.Font = new Font("Consolas", 36, FontStyle.Bold);
                lblStory.ForeColor = ColorTranslator.FromHtml("#00FF41");
            }
            else
            {
                lblStory.Font = new Font("Segoe UI", 20);
                lblStory.ForeColor = Color.White;
            }
            lblStory.Text = slides[currentSlide];
        }

        private void EndGame()
        {
            lblStory.Text = "THE END";
            lblStory.Font = new Font("Segoe UI", 72, FontStyle.Bold);
            lblStory.ForeColor = Color.White;
            btnNext.Text = "MAIN MENU";
            btnNext.Click -= (s, e) => { }; // This is hacky, but works for prototype
            btnNext.Click += (s, e) => { new MainMenuForm().Show(); this.Hide(); };
        }
    }
}

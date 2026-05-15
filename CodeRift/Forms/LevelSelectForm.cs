using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;

namespace CodeRift.Forms
{
    public class LevelSelectForm : Form
    {
        private Label lblTitle;
        private Button[] levelButtons;
        private Button btnBack;

        public LevelSelectForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Code Rift - Select Level";
            this.BackColor = ColorTranslator.FromHtml("#0D0D0D");
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            // ASSET SWAP: PictureBox for map/world background

            lblTitle = new Label();
            lblTitle.Text = "SELECT YOUR LEVEL";
            lblTitle.Font = new Font("Segoe UI", 36, FontStyle.Bold);
            lblTitle.ForeColor = ColorTranslator.FromHtml("#00FF41");
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point((Screen.PrimaryScreen.Bounds.Width - 450) / 2, 50);

            string[] levelNames = {
                "LEVEL 1 — LOOPS\nDifficulty: Easy",
                "LEVEL 2 — METHODS\nDifficulty: Easy–Medium",
                "LEVEL 3 — STRINGS\nDifficulty: Medium",
                "LEVEL 4 — ARRAYS\nDifficulty: Medium–Hard",
                "LEVEL 5 — FINAL COMPILATION\nDifficulty: Hard"
            };

            levelButtons = new Button[5];
            for (int i = 0; i < 5; i++)
            {
                int level = i + 1;
                levelButtons[i] = new Button();
                levelButtons[i].Text = GameManager.Instance.IsLevelUnlocked(level) ? levelNames[i] : "🔒 LOCKED";
                levelButtons[i].Size = new Size(500, 100);
                levelButtons[i].Location = new Point((Screen.PrimaryScreen.Bounds.Width - 500) / 2, 180 + (i * 120));
                levelButtons[i].FlatStyle = FlatStyle.Flat;
                levelButtons[i].Font = new Font("Segoe UI", 14, FontStyle.Bold);
                
                if (GameManager.Instance.IsLevelUnlocked(level))
                {
                    levelButtons[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#00FF41");
                    levelButtons[i].ForeColor = ColorTranslator.FromHtml("#00FF41");
                    levelButtons[i].BackColor = ColorTranslator.FromHtml("#111111");
                    levelButtons[i].Click += (s, e) => StartLevel(level);
                }
                else
                {
                    levelButtons[i].FlatAppearance.BorderColor = Color.Gray;
                    levelButtons[i].ForeColor = Color.Gray;
                    levelButtons[i].BackColor = ColorTranslator.FromHtml("#222222");
                    levelButtons[i].Enabled = false;
                }
                this.Controls.Add(levelButtons[i]);
            }

            btnBack = new Button();
            btnBack.Text = "← BACK";
            btnBack.Size = new Size(150, 50);
            btnBack.Location = new Point(50, 50);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#00FF41");
            btnBack.ForeColor = ColorTranslator.FromHtml("#00FF41");
            btnBack.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnBack.Click += (s, e) => {
                new MainMenuForm().Show();
                this.Hide();
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnBack);
        }

        private void StartLevel(int level)
        {
            GameManager.Instance.CurrentLevel = level;
            GameManager.Instance.ResetPlayerHP();
            
            LevelManager lm = new LevelManager();
            lm.LoadLevel(level);
            GameManager.Instance.CurrentEnemy = lm.GetEnemyForLevel(level);

            BattleForm battle = new BattleForm(lm);
            battle.Show();
            this.Hide();
        }
    }
}

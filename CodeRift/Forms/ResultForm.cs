using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;

namespace CodeRift.Forms
{
    public class ResultForm : Form
    {
        private bool isWin;
        private Label lblResult = null!, lblSubResult = null!, lblStats = null!;
        private Button? btnNext, btnRetry, btnLevelSelect, btnMainMenu;

        public ResultForm(bool win)
        {
            this.isWin = win;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Code Rift - Result";
            this.BackColor = ColorTranslator.FromHtml("#0D0D0D");
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            lblResult = new Label();
            lblResult.Text = isWin ? "VICTORY!" : "DEFEATED...";
            lblResult.Font = new Font("Segoe UI", 48, FontStyle.Bold);
            lblResult.ForeColor = isWin ? ColorTranslator.FromHtml("#FFD700") : ColorTranslator.FromHtml("#FF3333");
            lblResult.AutoSize = true;
            lblResult.Location = new Point((screenWidth - 400) / 2, 100);

            lblSubResult = new Label();
            lblSubResult.Text = isWin ? "The Bug has been defeated!" : "Elias has fallen. The Bugs grow stronger.";
            lblSubResult.Font = new Font("Segoe UI", 18);
            lblSubResult.ForeColor = Color.White;
            lblSubResult.AutoSize = true;
            lblSubResult.Location = new Point((screenWidth - 450) / 2, 200);

            lblStats = new Label();
            lblStats.Text = $"Correct Answers: {GameManager.Instance.CurrentPlayer.CorrectAnswers}\nWrong Answers: {GameManager.Instance.CurrentPlayer.WrongAnswers}";
            lblStats.Font = new Font("Segoe UI", 14);
            lblStats.ForeColor = Color.White;
            lblStats.AutoSize = true;
            lblStats.Location = new Point((screenWidth - 200) / 2, 300);

            // ASSET SWAP: Victory animation / illustration or Defeat illustration
            if (isWin)
            {
                if (GameManager.Instance.CurrentLevel < 5)
                {
                    GameManager.Instance.UnlockLevel(GameManager.Instance.CurrentLevel + 1);
                    btnNext = CreateStyledButton("NEXT LEVEL", 450);
                    btnNext.Click += (s, e) => StartLevel(GameManager.Instance.CurrentLevel + 1);
                    this.Controls.Add(btnNext);
                }
                else
                {
                    btnNext = CreateStyledButton("VIEW EPILOGUE", 450);
                    btnNext.Click += (s, e) => { new EpilogueForm().Show(); this.Hide(); };
                    this.Controls.Add(btnNext);
                }
            }
            else
            {
                btnRetry = CreateStyledButton("RETRY", 450);
                btnRetry.Click += (s, e) => StartLevel(GameManager.Instance.CurrentLevel);
                this.Controls.Add(btnRetry);
            }

            btnLevelSelect = CreateStyledButton("LEVEL SELECT", isWin || GameManager.Instance.CurrentLevel == 5 ? 530 : 530);
            btnLevelSelect.Click += (s, e) => { new LevelSelectForm().Show(); this.Hide(); };

            btnMainMenu = CreateStyledButton("MAIN MENU", 610);
            btnMainMenu.Click += (s, e) => { new MainMenuForm().Show(); this.Hide(); };

            this.Controls.AddRange(new Control[] { lblResult, lblSubResult, lblStats, btnLevelSelect, btnMainMenu });
        }

        private Button CreateStyledButton(string text, int top)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(300, 60);
            btn.Location = new Point((Screen.PrimaryScreen.Bounds.Width - 300) / 2, top);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#00FF41");
            btn.BackColor = ColorTranslator.FromHtml("#111111");
            btn.ForeColor = ColorTranslator.FromHtml("#00FF41");
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            return btn;
        }

        private void StartLevel(int level)
        {
            GameManager.Instance.CurrentLevel = level;
            GameManager.Instance.ResetPlayerHP();
            LevelManager lm = new LevelManager();
            lm.LoadLevel(level);
            GameManager.Instance.CurrentEnemy = lm.GetEnemyForLevel(level);
            new BattleForm(lm).Show();
            this.Hide();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Models;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public class BattleForm : Form
    {
        private LevelManager levelManager;
        private Question currentQuestion = null!;

        // UI Components
        private Panel pnlEnemy = null!, pnlPlayer = null!;
        private Label lblEnemyName = null!, lblEnemyHP = null!, lblPlayerName = null!, lblPlayerHP = null!;
        private ProgressBar pbEnemyHP = null!, pbPlayerHP = null!;
        private Label lblQuestionNum = null!, lblQuestionText = null!;
        private Panel pnlManual = null!;
        private Label lblFeedback = null!;
        private Button btnBack = null!;
        
        // Manual Controls
        private TextBox txtManualAnswer = null!;
        private Button btnSubmitManual = null!;

        private System.Windows.Forms.Timer animationTimer = null!;
        private int targetEnemyHP, targetPlayerHP;

        public BattleForm(LevelManager lm)
        {
            this.levelManager = lm;
            InitializeComponent();
            LoadNextQuestion();
        }

        private void InitializeComponent()
        {
            this.Text = "Code Rift - Battle";
            this.BackColor = ColorTranslator.FromHtml("#0D0D0D");
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Player Panel (TOP LEFT)
            pnlPlayer = new Panel { Size = new Size(500, 140), Location = new Point(50, 50), BackColor = Color.Transparent };
            lblPlayerName = new Label { Text = "ELIAS", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#00FF41"), AutoSize = true, Location = new Point(10, 10) };
            pbPlayerHP = new ProgressBar { Size = new Size(400, 30), Location = new Point(10, 50), Maximum = GameManager.Instance.CurrentPlayer.MaxHP, Value = GameManager.Instance.CurrentPlayer.CurrentHP };
            lblPlayerHP = new Label { Text = $"HP: {pbPlayerHP.Value} / {pbPlayerHP.Maximum}", ForeColor = Color.White, AutoSize = true, Location = new Point(10, 90) };
            pnlPlayer.Controls.AddRange(new Control[] { lblPlayerName, pbPlayerHP, lblPlayerHP });

            // Enemy Panel (TOP RIGHT)
            pnlEnemy = new Panel { Size = new Size(500, 140), Location = new Point(screenWidth - 550, 50), BackColor = Color.Transparent };
            lblEnemyName = new Label { Text = GameManager.Instance.CurrentEnemy!.Name, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(10, 10) };
            pbEnemyHP = new ProgressBar { Size = new Size(400, 30), Location = new Point(10, 50), Maximum = GameManager.Instance.CurrentEnemy.MaxHP, Value = GameManager.Instance.CurrentEnemy.CurrentHP };
            lblEnemyHP = new Label { Text = $"HP: {pbEnemyHP.Value} / {pbEnemyHP.Maximum}", ForeColor = Color.White, AutoSize = true, Location = new Point(10, 90) };
            pnlEnemy.Controls.AddRange(new Control[] { lblEnemyName, pbEnemyHP, lblEnemyHP });

            // Feedback Overlay (Larger and Centered)
            lblFeedback = new Label { 
                Text = "", 
                Font = new Font("Segoe UI", 48, FontStyle.Bold), 
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(screenWidth, 100),
                Location = new Point(0, 200),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false 
            };

            // Question Section (Centralized)
            lblQuestionNum = new Label { Text = "Question X / 5", Font = new Font("Segoe UI", 18), ForeColor = ColorTranslator.FromHtml("#00FF41"), AutoSize = true, Location = new Point((screenWidth - 200) / 2, 350) };
            lblQuestionText = new Label { 
                Text = "Question Text", 
                Font = new Font("Consolas", 18, FontStyle.Bold), 
                ForeColor = Color.White, 
                Size = new Size(screenWidth - 200, 150), 
                Location = new Point(100, 400),
                TextAlign = ContentAlignment.TopCenter
            };

            btnBack = new Button { 
                Text = "← BACK", 
                Size = new Size(120, 40), 
                Location = new Point(screenWidth / 2 - 60, 10),
                FlatStyle = FlatStyle.Flat,
                ForeColor = ColorTranslator.FromHtml("#00FF41"),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#111111")
            };
            btnBack.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#00FF41");
            btnBack.Click += (s, e) => {
                new LevelSelectForm().Show();
                this.Hide();
            };

            SetupManualPanel(screenWidth);

            this.Controls.AddRange(new Control[] { pnlEnemy, pnlPlayer, lblQuestionNum, lblQuestionText, pnlManual, lblFeedback, btnBack });
            lblFeedback.BringToFront();
            btnBack.BringToFront();

            animationTimer = new System.Windows.Forms.Timer { Interval = 30 };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void SetupManualPanel(int screenWidth)
        {
            pnlManual = new Panel { Size = new Size(800, 300), Location = new Point((screenWidth - 800) / 2, 600), Visible = true };
            txtManualAnswer = new TextBox { Font = new Font("Consolas", 20), Size = new Size(600, 60), Location = new Point(100, 35), BackColor = ColorTranslator.FromHtml("#222222"), ForeColor = Color.White, TextAlign = HorizontalAlignment.Center };
            
            txtManualAnswer.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter) {
                    e.SuppressKeyPress = true;
                    ProcessAnswer(txtManualAnswer.Text);
                }
            };

            btnSubmitManual = new Button { 
                Text = "EXECUTE ATTACK", 
                Size = new Size(300, 60), 
                Location = new Point(250, 80), 
                FlatStyle = FlatStyle.Flat, 
                ForeColor = ColorTranslator.FromHtml("#00FF41"), 
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#111111")
            };
            btnSubmitManual.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#00FF41");
            btnSubmitManual.Click += (s, e) => ProcessAnswer(txtManualAnswer.Text);
            
            pnlManual.Controls.Add(new Label { Text = "ENTER CODE TO DEBUG:", ForeColor = Color.White, AutoSize = true, Font = new Font("Segoe UI", 12), Location = new Point(310, 10) });
            pnlManual.Controls.Add(txtManualAnswer);
            pnlManual.Controls.Add(btnSubmitManual);
        }

        private void LoadNextQuestion()
        {
            if (!levelManager.HasMoreQuestions())
            {
                // Only show end screen if HP reached zero
                CheckBattleState();
                return;
            }

            currentQuestion = levelManager.GetNextQuestion()!;
            lblQuestionNum.Text = $"LOGIC UNIT {levelManager.CurrentQuestionNumber} / {levelManager.TotalQuestions}";
            lblQuestionText.Text = currentQuestion.Text;

            txtManualAnswer.Clear();
            txtManualAnswer.Enabled = true;
            btnSubmitManual.Enabled = true;
            txtManualAnswer.Focus();
        }

        private void ProcessAnswer(string answer)
        {
            if (string.IsNullOrEmpty(answer)) return;

            bool correct = string.Equals(answer.Trim().Replace(";", ""), currentQuestion.CorrectAnswer.Trim().Replace(";", ""), StringComparison.OrdinalIgnoreCase);

            pnlManual.Enabled = false;
            targetEnemyHP = GameManager.Instance.CurrentEnemy!.CurrentHP;
            targetPlayerHP = GameManager.Instance.CurrentPlayer.CurrentHP;

            if (correct)
            {
                lblFeedback.Text = "✔ CODE VALIDATED";
                lblFeedback.ForeColor = ColorTranslator.FromHtml("#FFD700");
                targetEnemyHP = Math.Max(0, GameManager.Instance.CurrentEnemy.CurrentHP - (GameManager.Instance.CurrentEnemy.MaxHP / 5));
                GameManager.Instance.CurrentPlayer.CorrectAnswers++;
            }
            else
            {
                lblFeedback.Text = "✘ SYNTAX ERROR";
                lblFeedback.ForeColor = ColorTranslator.FromHtml("#FF3333");
                targetPlayerHP = Math.Max(0, GameManager.Instance.CurrentPlayer.CurrentHP - (GameManager.Instance.CurrentPlayer.MaxHP / 5));
                GameManager.Instance.CurrentPlayer.WrongAnswers++;
            }

            lblFeedback.Visible = true;
            animationTimer.Start();
            
            System.Windows.Forms.Timer nextQuestionTimer = new System.Windows.Forms.Timer { Interval = 2000 };
            nextQuestionTimer.Tick += (s, e) => {
                nextQuestionTimer.Stop();
                lblFeedback.Visible = false;
                
                GameManager.Instance.CurrentEnemy!.CurrentHP = targetEnemyHP;
                GameManager.Instance.CurrentPlayer.CurrentHP = targetPlayerHP;
                
                pnlManual.Enabled = true;

                if (GameManager.Instance.CurrentEnemy.CurrentHP <= 0 || GameManager.Instance.CurrentPlayer.CurrentHP <= 0)
                {
                    CheckBattleState();
                }
                else
                {
                    LoadNextQuestion();
                }
            };
            nextQuestionTimer.Start();
        }

        private void CheckBattleState()
        {
            // Victory Condition: 3 or more correct answers
            if (GameManager.Instance.CurrentPlayer.CorrectAnswers >= 3)
            {
                new ResultForm(true).Show();
                this.Hide();
            }
            else
            {
                // Defeat if less than 3 correct answers
                new ResultForm(false).Show();
                this.Hide();
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            bool enemyDone = false, playerDone = false;
            
            if (pbEnemyHP.Value > targetEnemyHP) pbEnemyHP.Value--; else enemyDone = true;
            if (pbPlayerHP.Value > targetPlayerHP) pbPlayerHP.Value--; else playerDone = true;
            
            lblEnemyHP.Text = $"HP: {pbEnemyHP.Value} / {pbEnemyHP.Maximum}";
            lblPlayerHP.Text = $"HP: {pbPlayerHP.Value} / {pbPlayerHP.Maximum}";

            if (enemyDone && playerDone) animationTimer.Stop();
        }
    }
}

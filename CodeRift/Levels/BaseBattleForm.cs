using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Core;
using CodeRift.Managers;
using CodeRift.Utils;
using CodeRift.Forms;

namespace CodeRift.Levels
{
    public partial class BaseBattleForm : Form
    {
        protected int PlayerHealth = 100;
        protected int PlayerMaxHealth = 100;
        protected int EnemyHealth = 100;
        protected int EnemyMaxHealth = 100;
        protected string LevelName = "Level";
        protected int CurrentLevel = 1;
        
        private int _currentQuestionIndex = 0;
        private int _correctCount = 0;
        private int _incorrectCount = 0;
        private List<Question> _questions;

        public BaseBattleForm()
        {
            InitializeComponent();
        }

        protected void SetupBaseForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(13, 13, 13);
            
            pbBackground.Image = ImageManager.Instance.GetImage(Constants.IMG_BG_LEVEL1);
            pbBackground.SizeMode = PictureBoxSizeMode.StretchImage;
            pbBackground.SendToBack();

            pbPlayer.SizeMode = PictureBoxSizeMode.Zoom;
            pbPlayerAction.SizeMode = PictureBoxSizeMode.Zoom;
            pbEnemy.SizeMode = PictureBoxSizeMode.Zoom;
            pbEnemyAction.SizeMode = PictureBoxSizeMode.Zoom;
            
            pbPlayer.Image = ImageManager.Instance.GetImage(Constants.IMG_PLAYER_IDLE);
            pbPlayerAction.Image = ImageManager.Instance.GetImage(Constants.IMG_PLAYER_ATTACK);
            pbEnemy.Image = ImageManager.Instance.GetImage(Constants.IMG_ENEMY_BASIC);
            pbEnemyAction.Image = ImageManager.Instance.GetImage(Constants.IMG_ENEMY_BASIC);

            // Adjust Question Label to fit choices
            lblQuestion.Location = new Point(650, 420);
            lblQuestion.Size = new Size(602, 160);
            lblQuestion.TextAlign = ContentAlignment.TopLeft;

            // Allow Enter for new lines in the answer box
            txtCodeAnswer.AcceptsReturn = true;

            StyleBattleButton(btnBack);
            StyleBattleButton(btnExecuteCode);

            ApplyLocalization();

            _questions = QuestionManager.GetQuestionsForLevel(CurrentLevel, LanguageManager.Instance.CurrentLanguage);
            UpdateUI();
            txtCodeAnswer.Focus();
        }

        private void ApplyLocalization()
        {
            var lm = LanguageManager.Instance;
            btnBack.Text = lm.Get("back");
            btnExecuteCode.Text = lm.Get("execute_code");
            btnContinue.Text = lm.Get("continue");
        }

        protected virtual void OnAttack()
        {
            if (_questions == null || _currentQuestionIndex >= _questions.Count) return;

            Question q = _questions[_currentQuestionIndex];
            bool correct = q.IsCorrect(txtCodeAnswer.Text);

            if (correct)
            {
                _correctCount++;
                EnemyHealth -= 20;
                // Visual feedback for correct answer could go here
            }
            else
            {
                _incorrectCount++;
                PlayerHealth -= 20;
                // Visual feedback for incorrect answer could go here
            }

            _currentQuestionIndex++;
            UpdateUI();

            if (_currentQuestionIndex >= 5 || PlayerHealth <= 0 || EnemyHealth <= 0)
            {
                EndLevel();
            }
        }

        private void EndLevel()
        {
            bool victory = _correctCount >= 3;
            
            if (victory)
            {
                // Unlock the next level immediately upon victory
                CodeRift.Managers.ProgressManager.Instance.UnlockNextLevel(CurrentLevel);
                
                // If it's the final level and the player won, skip the result form and go straight to Epilogue
                if (CurrentLevel == 5)
                {
                    TransitionToNext();
                    return;
                }
            }

            var resultForm = new BattleResultForm(victory ? BattleResultForm.ResultType.Victory : BattleResultForm.ResultType.Defeat, CurrentLevel);
            var dialogResult = resultForm.ShowDialog(this);

            switch (dialogResult)
            {
                case DialogResult.OK: // Proceed to Next Level
                    TransitionToNext();
                    break;
                case DialogResult.Retry: // Try Again
                    ResetLevel();
                    break;
                case DialogResult.Yes: // Levels Menu
                    this.Close();
                    break;
                case DialogResult.Abort: // Main Menu
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f is CodeRift.Forms.LevelsMenuForm)
                        {
                            f.Tag = "EXIT_TO_MENU";
                            break;
                        }
                    }
                    this.Close();
                    break;
                default:
                    this.Close();
                    break;
            }
        }

        private void ResetLevel()
        {
            PlayerHealth = 100;
            EnemyHealth = 100;
            _currentQuestionIndex = 0;
            _correctCount = 0;
            _incorrectCount = 0;
            UpdateUI();
            txtCodeAnswer.Clear();
            txtCodeAnswer.Focus();
        }

        protected virtual void UpdateUI()
        {
            lblLevelTitle.Text = LevelName;
            lblEnemyName.Text = GetEnemyName();

            PlayerHealth = Math.Clamp(PlayerHealth, 0, PlayerMaxHealth);
            EnemyHealth = Math.Clamp(EnemyHealth, 0, EnemyMaxHealth);

            prgEliasHealth.Maximum = PlayerMaxHealth;
            prgEnemyHealth.Maximum = EnemyMaxHealth;
            prgEliasHealth.Value = PlayerHealth;
            prgEnemyHealth.Value = EnemyHealth;

            lblEliasHealthValue.Text = $"{PlayerHealth}/{PlayerMaxHealth}";
            lblEnemyHealthValue.Text = $"{EnemyHealth}/{EnemyMaxHealth}";

            if (_questions != null && _currentQuestionIndex < _questions.Count)
            {
                Question q = _questions[_currentQuestionIndex];
                string questionText = q.Text;
                if (q.Options.Count > 0)
                {
                    questionText += "\n" + string.Join("\n", q.Options);
                }
                lblQuestion.Text = questionText;
            }
        }

        protected virtual void WinLevel()
        {
            lblLevelTitle.Text = "SYSTEM CLEANSED - VICTORY";
            btnContinue.Visible = true;
            btnContinue.BringToFront();
            btnExecuteCode.Enabled = false;
            txtCodeAnswer.Enabled = false;
        }

        protected virtual void TransitionToNext()
        {
            // Override in derived classes
        }

        private void btnExecuteCode_Click(object sender, EventArgs e)
        {
            OnAttack();
            txtCodeAnswer.Clear();
            txtCodeAnswer.Focus();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            TransitionToNext();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string GetEnemyName()
        {
            int separatorIndex = LevelName.IndexOf(':');
            return separatorIndex >= 0 && separatorIndex < LevelName.Length - 1
                ? LevelName[(separatorIndex + 1)..].Trim().ToUpperInvariant()
                : "ENEMY";
        }

        private static void StyleBattleButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 255, 65);
            button.ForeColor = Color.FromArgb(0, 255, 65);
            button.BackColor = Color.Black;

            // Change text color to black on hover so it is visible against the green background
            button.MouseEnter += (s, e) => button.ForeColor = Color.Black;
            button.MouseLeave += (s, e) => button.ForeColor = Color.FromArgb(0, 255, 65);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Allow the spacebar and enter to function normally if the user is typing in the answer textbox
            if ((keyData == Keys.Space || keyData == Keys.Enter) && txtCodeAnswer.Focused)
            {
                return false; 
            }

            if (keyData == Keys.Enter || keyData == Keys.Space)
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}

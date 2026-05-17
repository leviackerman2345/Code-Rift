using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Levels
{
    public partial class BaseBattleForm : Form
    {
        protected int PlayerHealth = 100;
        protected int PlayerMaxHealth = 100;
        protected int EnemyHealth = 100;
        protected int EnemyMaxHealth = 100;
        protected string LevelName = "Level";
        private int _currentQuestionIndex = 0;
        private readonly string[] _questionPlaceholders =
        {
            "Question 1/5: Write your answer or code here.",
            "Question 2/5: Write your answer or code here.",
            "Question 3/5: Write your answer or code here.",
            "Question 4/5: Write your answer or code here.",
            "Question 5/5: Write your answer or code here."
        };

        public BaseBattleForm()
        {
            InitializeComponent();
            SetupBaseForm();
        }

        private void SetupBaseForm()
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

            StyleBattleButton(btnBack);
            StyleBattleButton(btnExecuteCode);
            txtCodeAnswer.Focus();
        }

        protected virtual void OnAttack()
        {
            EnemyHealth -= 20;
            _currentQuestionIndex++;
            UpdateUI();
            if (EnemyHealth <= 0)
            {
                WinLevel();
            }
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
            lblQuestion.Text = _questionPlaceholders[Math.Min(_currentQuestionIndex, _questionPlaceholders.Length - 1)];
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

using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;

namespace CodeRift.Forms
{
    public class MainMenuForm : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnNewGame;
        private Button btnContinue;
        private Button btnExit;

        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Code Rift - Main Menu";
            this.BackColor = ColorTranslator.FromHtml("#0D0D0D");
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            // ASSET SWAP: Background PictureBox for main menu art

            lblTitle = new Label();
            lblTitle.Text = "CODE RIFT";
            lblTitle.Font = new Font("Segoe UI", 64, FontStyle.Bold);
            lblTitle.ForeColor = ColorTranslator.FromHtml("#00FF41");
            lblTitle.AutoSize = true;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Location = new Point((Screen.PrimaryScreen.Bounds.Width - 450) / 2, 100);

            lblSubtitle = new Label();
            lblSubtitle.Text = "The Great Compiler Awaits";
            lblSubtitle.Font = new Font("Segoe UI", 18, FontStyle.Italic);
            lblSubtitle.ForeColor = Color.White;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point((Screen.PrimaryScreen.Bounds.Width - 300) / 2, lblTitle.Bottom + 10);

            btnNewGame = CreateStyledButton("NEW GAME", lblSubtitle.Bottom + 100);
            btnContinue = CreateStyledButton("CONTINUE", btnNewGame.Bottom + 20);
            btnExit = CreateStyledButton("EXIT", btnContinue.Bottom + 20);

            btnContinue.Enabled = GameManager.Instance.UnlockedLevels.Count > 1;
            if (!btnContinue.Enabled) btnContinue.ForeColor = Color.Gray;

            btnNewGame.Click += (s, e) => {
                GameManager.Instance.ResetProgress();
                new PrologueForm().Show();
                this.Hide();
            };

            btnContinue.Click += (s, e) => {
                new LevelSelectForm().Show();
                this.Hide();
            };

            btnExit.Click += (s, e) => Application.Exit();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(btnNewGame);
            this.Controls.Add(btnContinue);
            this.Controls.Add(btnExit);
        }

        private Button CreateStyledButton(string text, int top)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(300, 60);
            btn.Location = new Point((Screen.PrimaryScreen.Bounds.Width - 300) / 2, top);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#00FF41");
            btn.FlatAppearance.BorderSize = 2;
            btn.BackColor = ColorTranslator.FromHtml("#111111");
            btn.ForeColor = ColorTranslator.FromHtml("#00FF41");
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            return btn;
        }
    }
}

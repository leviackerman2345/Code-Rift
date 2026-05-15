using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public class SplashForm : Form
    {
        private Label lblTitle;
        private Label lblStatus;
        private System.Windows.Forms.Timer splashTimer;

        public SplashForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Code Rift";
            this.BackColor = ColorTranslator.FromHtml("#0D0D0D");
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;

            lblTitle = new Label();
            lblTitle.Text = "CODE RIFT";
            lblTitle.Font = new Font("Segoe UI", 48, FontStyle.Bold);
            lblTitle.ForeColor = ColorTranslator.FromHtml("#00FF41");
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point((Screen.PrimaryScreen.Bounds.Width - 400) / 2, (Screen.PrimaryScreen.Bounds.Height - 100) / 2);
            // ASSET SWAP: Replace this with PictureBox for logo image
            
            lblStatus = new Label();
            lblStatus.Text = "Loading...";
            lblStatus.Font = new Font("Segoe UI", 14);
            lblStatus.ForeColor = Color.White;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point((Screen.PrimaryScreen.Bounds.Width - 150) / 2, lblTitle.Bottom + 100);

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblStatus);

            splashTimer = new System.Windows.Forms.Timer();
            splashTimer.Interval = 2000;
            splashTimer.Tick += SplashTimer_Tick;
            splashTimer.Start();

            this.KeyDown += SplashForm_KeyDown;
        }

        private void SplashTimer_Tick(object sender, EventArgs e)
        {
            splashTimer.Stop();
            lblStatus.Text = "Press any key to continue";
        }

        private void SplashForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (lblStatus.Text == "Press any key to continue")
            {
                MainMenuForm mainMenu = new MainMenuForm();
                mainMenu.Show();
                this.Hide();
            }
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public partial class LevelsMenuForm : Form
    {
        public LevelsMenuForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(13, 13, 13);
            
            this.BackgroundImage = ImageManager.Instance.GetImage(Constants.IMG_BG_MENU);
            this.BackgroundImageLayout = ImageLayout.Stretch;

            lblTitle.Text = "LEVELS";
            lblTitle.Font = new Font("Courier New", 72, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 255, 65);
            lblTitle.BackColor = Color.Transparent;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Register handlers once
            btnLevel1.Click += (s, e) => LaunchLevel(new Levels.Level1Form());
            btnLevel2.Click += (s, e) => LaunchLevel(new Levels.Level2Form());
            btnLevel3.Click += (s, e) => LaunchLevel(new Levels.Level3Form());
            btnLevel4.Click += (s, e) => LaunchLevel(new Levels.Level4Form());
            btnLevel5.Click += (s, e) => LaunchLevel(new Levels.Level5Form());
            btnBack.Click += (s, e) => this.Close();

            UpdateLevelButtons();
            StyleLevelButton(btnBack, "[BACK]");
        }

        private void UpdateLevelButtons()
        {
            SetupButtonState(btnLevel1, 1, "LEVEL 1: LOOPS");
            SetupButtonState(btnLevel2, 2, "LEVEL 2: METHODS");
            SetupButtonState(btnLevel3, 3, "LEVEL 3: STRINGS");
            SetupButtonState(btnLevel4, 4, "LEVEL 4: ARRAYS");
            SetupButtonState(btnLevel5, 5, "LEVEL 5: CLASSES");
        }

        private void SetupButtonState(Button btn, int level, string text)
        {
            bool isUnlocked = ProgressManager.Instance.IsLevelUnlocked(level);
            StyleLevelButton(btn, isUnlocked ? text : "[LOCKED]");
            btn.Enabled = isUnlocked;
        }

        private void StyleLevelButton(Button btn, string text)
        {
            btn.Text = text;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            
            bool isUnlocked = !text.Contains("LOCKED");

            if (isUnlocked)
            {
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 255, 65);
                btn.ForeColor = Color.FromArgb(0, 255, 65);
                btn.BackColor = Color.Black;
                btn.Font = new Font("Courier New", 20, FontStyle.Bold);
                btn.MouseEnter += (s, e) => btn.ForeColor = Color.Black;
                btn.MouseLeave += (s, e) => btn.ForeColor = Color.FromArgb(0, 255, 65);
            }
            else
            {
                btn.ForeColor = Color.Gray;
                btn.FlatAppearance.BorderColor = Color.Gray;
                btn.BackColor = Color.FromArgb(20, 20, 20);
                btn.Font = new Font("Courier New", 20, FontStyle.Bold | FontStyle.Italic);
            }
        }

        private void LaunchLevel(Form levelForm)
        {
            this.Hide();
            levelForm.FormClosed += (s, args) => 
            {
                if (this.Tag?.ToString() == "EXIT_TO_MENU")
                {
                    this.Close();
                    return;
                }
                UpdateLevelButtons(); // Refresh in case a level was just unlocked
                this.Show();
            };
            levelForm.Show();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterControls();
        }

        private void CenterControls()
        {
            lblTitle.Size = new Size(this.Width, 150);
            lblTitle.Location = new Point(0, 50);

            int btnWidth = 600;
            int btnHeight = 80;
            int gap = 20;
            int startY = lblTitle.Bottom + 50;

            Button[] buttons = { btnLevel1, btnLevel2, btnLevel3, btnLevel4, btnLevel5 };
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Size = new Size(btnWidth, btnHeight);
                buttons[i].Location = new Point((this.Width - btnWidth) / 2, startY + i * (btnHeight + gap));
            }

            btnBack.Size = new Size(200, 50);
            btnBack.Location = new Point(50, 50);
        }
    }
}

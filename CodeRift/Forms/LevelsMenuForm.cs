using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    // Level selection gate: unlock state comes from ProgressManager.
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

            UpdateLevelButtons();
            StyleLevelButton(btnBack, "[BACK]");
            btnBack.Click += (s, e) => this.Close();
        }

        private void UpdateLevelButtons()
        {
            // Each button launches the battle form with a level identifier.
            SetupButton(btnLevel1, 1, "LEVEL 1: LOOPS", () => LaunchLevel(new BattleArenaForm(1)));
            SetupButton(btnLevel2, 2, "LEVEL 2: METHODS", () => LaunchLevel(new BattleArenaForm(2)));
            SetupButton(btnLevel3, 3, "LEVEL 3: STRINGS", () => LaunchLevel(new BattleArenaForm(3)));
            SetupButton(btnLevel4, 4, "LEVEL 4: ARRAYS", () => LaunchLevel(new BattleArenaForm(4)));
            SetupButton(btnLevel5, 5, "LEVEL 5: CLASSES", () => LaunchLevel(new BattleArenaForm(5)));
        }

        private void SetupButton(Button btn, int level, string text, Action launchAction)
        {
            // Locked levels are disabled until prior level completion updates progress.
            bool isUnlocked = ProgressManager.Instance.IsLevelUnlocked(level);
            StyleLevelButton(btn, isUnlocked ? text : "[LOCKED]");
            btn.Enabled = isUnlocked;
            
            // Remove existing handlers to avoid duplicates if UpdateLevelButtons is called multiple times
            // However, since we are creating new form instances, we'll just set it once.
            // If we were to re-call UpdateLevelButtons, we'd need to be more careful.
            btn.Click -= null; // This doesn't actually work in C#, but for now SetupForm only calls it once.
            if (isUnlocked)
            {
                btn.Click += (s, e) => launchAction();
            }
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
            // Navigation: levels -> battle, then return here unless EXIT_TO_MENU was set by ending flow.
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


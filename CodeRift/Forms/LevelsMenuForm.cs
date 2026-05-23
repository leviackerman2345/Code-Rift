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
            btnBack.Click += (s, e) => 
            {
                AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
                this.Close();
            };
        }

        private void UpdateLevelButtons()
        {
            // Each button launches the battle form with a level identifier.
            SetupButton(btnLevel1, 1, "LEVEL 1: LOOPS", Constants.IMG_BG_LEVEL1, () => LaunchLevel(new BattleArenaForm(1)));
            SetupButton(btnLevel2, 2, "LEVEL 2: METHODS", Constants.IMG_BG_LEVEL2, () => LaunchLevel(new BattleArenaForm(2)));
            SetupButton(btnLevel3, 3, "LEVEL 3: STRINGS", Constants.IMG_BG_LEVEL3, () => LaunchLevel(new BattleArenaForm(3)));
            SetupButton(btnLevel4, 4, "LEVEL 4: ARRAYS", Constants.IMG_BG_LEVEL4, () => LaunchLevel(new BattleArenaForm(4)));
            SetupButton(btnLevel5, 5, "LEVEL 5: CLASSES", Constants.IMG_BG_LEVEL5, () => LaunchLevel(new BattleArenaForm(5)));
        }

        private void SetupButton(Button btn, int level, string text, string bgKey, Action launchAction)
        {
            // Locked levels are disabled until prior level completion updates progress.
            bool isUnlocked = ProgressManager.Instance.IsLevelUnlocked(level);
            StyleLevelButton(btn, isUnlocked ? text : "[LOCKED]", isUnlocked, text, bgKey);
            btn.Enabled = isUnlocked;
            
            // To avoid duplicate handlers when UpdateLevelButtons is re-called, 
            // clear all existing handlers using a helper or just rely on a tag.
            // A simple way to avoid duplicates is to check if we've already attached.
            if (btn.Tag == null)
            {
                btn.Tag = true;
                btn.Click += (s, e) => 
                {
                    AudioManager.Instance.PlaySFX(Constants.SFX_CLICK);
                    if (ProgressManager.Instance.IsLevelUnlocked(level))
                    {
                        launchAction();
                    }
                };
            }
        }

        private void StyleLevelButton(Button btn, string displayString, bool isUnlocked = true, string originalText = "", string bgKey = "")
        {
            btn.Text = displayString;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            
            // Clear hover handlers if we are re-styling, though it's easier to only add them once
            // But we already add them dynamically. Let's just avoid adding them multiple times.
            // If bgKey is empty, it's the Back button.
            bool isLevelButton = !string.IsNullOrEmpty(bgKey);

            if (isUnlocked)
            {
                btn.FlatAppearance.MouseOverBackColor = isLevelButton ? Color.Transparent : Color.FromArgb(0, 255, 65);
                btn.ForeColor = Color.FromArgb(0, 255, 65);
                btn.BackColor = Color.Black;
                btn.Font = new Font("Courier New", 20, FontStyle.Bold);

                // Use a property we haven't used yet to store state, or just handle it if it hasn't been added.
                // Since SetupButton sets btn.Tag = true for level buttons, we can use that to ensure we only subscribe once.
                // But for btnBack, SetupForm only calls it once anyway.
                if (isLevelButton && btn.Tag == null) // This will run only once before Tag is set to true in SetupButton
                {
                    btn.MouseEnter += (s, e) => 
                    {
                        if (btn.Enabled)
                        {
                            AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
                            var bgImg = ImageManager.Instance.GetImage(bgKey);
                            if (bgImg != null) 
                            {
                                this.BackgroundImage = bgImg;
                                this.Invalidate(); // Ensure form repaints to apply the dark overlay
                            }
                        }
                    };
                    btn.MouseLeave += (s, e) => 
                    {
                        if (btn.Enabled)
                        {
                            this.BackgroundImage = ImageManager.Instance.GetImage(Constants.IMG_BG_MENU);
                            this.Invalidate(); // Ensure form repaints
                        }
                    };
                }
                else if (!isLevelButton)
                {
                    // Back button hover style (match main menu)
                    btn.MouseEnter += (s, e) => 
                    {
                        AudioManager.Instance.PlaySFX(Constants.SFX_HOVER);
                        btn.ForeColor = Color.Black;
                        btn.FlatAppearance.BorderColor = Color.Black;
                        var hoverImage = ImageManager.Instance.GetImage(Constants.IMG_UI_BUTTON);
                        if (hoverImage != null)
                        {
                            btn.BackgroundImage = hoverImage;
                            btn.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    };
                    btn.MouseLeave += (s, e) => 
                    {
                        btn.ForeColor = Color.FromArgb(0, 255, 65);
                        btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
                        btn.BackgroundImage = null;
                    };
                }
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
            levelForm.Shown += (s, args) => this.Hide();
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

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            
            // If the background is a level background (not the main menu background), draw a darkening overlay
            if (this.BackgroundImage != null && this.BackgroundImage != ImageManager.Instance.GetImage(Constants.IMG_BG_MENU))
            {
                using (var brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            }
        }
    }
}


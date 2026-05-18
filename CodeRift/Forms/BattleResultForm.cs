using System;
using System.Drawing;
using System.Windows.Forms;
using CodeRift.Managers;

namespace CodeRift.Forms
{
    public partial class BattleResultForm : Form
    {
        public enum ResultType { Victory, Defeat }
        private readonly ResultType _result;
        private readonly int _level;

        public BattleResultForm(ResultType result, int level)
        {
            InitializeComponent();
            _result = result;
            _level = level;
            SetupUI();
        }

        private void SetupUI()
        {
            var lm = LanguageManager.Instance;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.Size = new Size(500, 550); // Increased height to 550 to allow lowering buttons more

            Label lblTitle = new Label
            {
                Text = _result == ResultType.Victory ? lm.Get("victory") : lm.Get("defeated"),
                Font = new Font("Courier New", 36, FontStyle.Bold),
                ForeColor = _result == ResultType.Victory ? Color.FromArgb(0, 255, 65) : Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 120
            };
            this.Controls.Add(lblTitle);

            // Using a container panel
            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(contentPanel);

            int btnWidth = 340;
            int btnHeight = 60;
            int gap = 20;
            
            // Shift buttons even lower to ensure visibility
            int currentY = 140; 
            int centerX = (500 - btnWidth) / 2;

            if (_result == ResultType.Victory)
            {
                AddButton(contentPanel, lm.Get("proceed_next"), (s, e) => {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }, new Point(centerX, currentY), btnWidth, btnHeight);
                currentY += btnHeight + gap;
            }
            else
            {
                AddButton(contentPanel, lm.Get("try_again"), (s, e) => {
                    this.DialogResult = DialogResult.Retry;
                    this.Close();
                }, new Point(centerX, currentY), btnWidth, btnHeight);
                currentY += btnHeight + gap;
            }

            AddButton(contentPanel, lm.Get("levels"), (s, e) => {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }, new Point(centerX, currentY), btnWidth, btnHeight);
            currentY += btnHeight + gap;

            AddButton(contentPanel, lm.Get("main_menu"), (s, e) => {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }, new Point(centerX, currentY), btnWidth, btnHeight);
        }

        private void AddButton(Control parent, string text, EventHandler onClick, Point location, int width, int height)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                Location = location,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(0, 255, 65),
                BackColor = Color.Black,
                Font = new Font("Courier New", 12, FontStyle.Bold),
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 255, 65);
            
            // Hover effect
            btn.MouseEnter += (s, e) => btn.ForeColor = Color.Black;
            btn.MouseLeave += (s, e) => btn.ForeColor = Color.FromArgb(0, 255, 65);
            
            btn.Click += onClick;
            parent.Controls.Add(btn);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.FromArgb(0, 255, 65), ButtonBorderStyle.Solid);
        }
    }

    // Designer partial class
    public partial class BattleResultForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Name = "BattleResultForm";
            this.Text = "Battle Result";
            this.ResumeLayout(false);
        }
    }
}

namespace CodeRift.Forms
{
    partial class SettingsForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.terminalBody = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.enFlagIcon = new System.Windows.Forms.PictureBox();
            this.phFlagIcon = new System.Windows.Forms.PictureBox();
            this.btnEnglish = new System.Windows.Forms.Button();
            this.btnFilipino = new System.Windows.Forms.Button();
            this.lblSubtitles = new System.Windows.Forms.Label();
            this.volSlider = new System.Windows.Forms.TrackBar();
            this.volIcon = new System.Windows.Forms.PictureBox();
            this.lblVolume = new System.Windows.Forms.Label();
            this.titleBar = new System.Windows.Forms.Panel();
            this.titleDotContainer = new System.Windows.Forms.Panel();
            this.btnMax = new System.Windows.Forms.Panel();
            this.btnMin = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.settingsPanel.SuspendLayout();
            this.terminalBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.enFlagIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.phFlagIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volIcon)).BeginInit();
            this.titleBar.SuspendLayout();
            this.titleDotContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.settingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.settingsPanel.Controls.Add(this.terminalBody);
            this.settingsPanel.Controls.Add(this.titleBar);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(550, 500);
            this.settingsPanel.TabIndex = 0;
            // 
            // terminalBody
            // 
            this.terminalBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.terminalBody.Controls.Add(this.btnBack);
            this.terminalBody.Controls.Add(this.enFlagIcon);
            this.terminalBody.Controls.Add(this.phFlagIcon);
            this.terminalBody.Controls.Add(this.btnEnglish);
            this.terminalBody.Controls.Add(this.btnFilipino);
            this.terminalBody.Controls.Add(this.lblSubtitles);
            this.terminalBody.Controls.Add(this.volSlider);
            this.terminalBody.Controls.Add(this.volIcon);
            this.terminalBody.Controls.Add(this.lblVolume);
            this.terminalBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.terminalBody.Location = new System.Drawing.Point(0, 35);
            this.terminalBody.Name = "terminalBody";
            this.terminalBody.Size = new System.Drawing.Size(550, 465);
            this.terminalBody.TabIndex = 11;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Black;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnBack.Location = new System.Drawing.Point(175, 380);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(200, 45);
            this.btnBack.TabIndex = 9;
            this.btnBack.Text = "[ EXIT_CONFIG ]";
            this.btnBack.UseVisualStyleBackColor = false;
            // 
            // enFlagIcon
            // 
            this.enFlagIcon.Location = new System.Drawing.Point(60, 269);
            this.enFlagIcon.Name = "enFlagIcon";
            this.enFlagIcon.Size = new System.Drawing.Size(32, 32);
            this.enFlagIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.enFlagIcon.TabIndex = 8;
            this.enFlagIcon.TabStop = false;
            // 
            // phFlagIcon
            // 
            this.phFlagIcon.Location = new System.Drawing.Point(60, 219);
            this.phFlagIcon.Name = "phFlagIcon";
            this.phFlagIcon.Size = new System.Drawing.Size(32, 32);
            this.phFlagIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.phFlagIcon.TabIndex = 7;
            this.phFlagIcon.TabStop = false;
            // 
            // btnEnglish
            // 
            this.btnEnglish.BackColor = System.Drawing.Color.Black;
            this.btnEnglish.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnEnglish.FlatAppearance.BorderSize = 0;
            this.btnEnglish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnglish.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.btnEnglish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnEnglish.Location = new System.Drawing.Point(100, 265);
            this.btnEnglish.Name = "btnEnglish";
            this.btnEnglish.Size = new System.Drawing.Size(360, 40);
            this.btnEnglish.TabIndex = 6;
            this.btnEnglish.Text = "SET_LANG_EN";
            this.btnEnglish.UseVisualStyleBackColor = false;
            // 
            // btnFilipino
            // 
            this.btnFilipino.BackColor = System.Drawing.Color.Black;
            this.btnFilipino.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnFilipino.FlatAppearance.BorderSize = 0;
            this.btnFilipino.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilipino.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.btnFilipino.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnFilipino.Location = new System.Drawing.Point(100, 215);
            this.btnFilipino.Name = "btnFilipino";
            this.btnFilipino.Size = new System.Drawing.Size(360, 40);
            this.btnFilipino.TabIndex = 5;
            this.btnFilipino.Text = "SET_LANG_PH";
            this.btnFilipino.UseVisualStyleBackColor = false;
            // 
            // lblSubtitles
            // 
            this.lblSubtitles.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubtitles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblSubtitles.Location = new System.Drawing.Point(60, 180);
            this.lblSubtitles.Name = "lblSubtitles";
            this.lblSubtitles.Size = new System.Drawing.Size(250, 20);
            this.lblSubtitles.TabIndex = 4;
            this.lblSubtitles.Text = "root@coderift:~/subtitles$";
            // 
            // volSlider
            // 
            this.volSlider.AutoSize = false;
            this.volSlider.Location = new System.Drawing.Point(100, 115);
            this.volSlider.Maximum = 100;
            this.volSlider.Name = "volSlider";
            this.volSlider.Size = new System.Drawing.Size(360, 32);
            this.volSlider.TabIndex = 2;
            this.volSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volSlider.Value = 80;
            // 
            // volIcon
            // 
            this.volIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.volIcon.Location = new System.Drawing.Point(60, 115);
            this.volIcon.Name = "volIcon";
            this.volIcon.Size = new System.Drawing.Size(32, 32);
            this.volIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.volIcon.TabIndex = 1;
            this.volIcon.TabStop = false;
            // 
            // lblVolume
            // 
            this.lblVolume.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.lblVolume.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblVolume.Location = new System.Drawing.Point(60, 85);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(250, 20);
            this.lblVolume.TabIndex = 3;
            this.lblVolume.Text = "root@coderift:~/volume$";
            // 
            // titleBar
            // 
            this.titleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.titleBar.Controls.Add(this.titleDotContainer);
            this.titleBar.Controls.Add(this.lblTitle);
            this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(548, 35);
            this.titleBar.TabIndex = 10;
            // 
            // titleDotContainer
            // 
            this.titleDotContainer.Controls.Add(this.btnMax);
            this.titleDotContainer.Controls.Add(this.btnMin);
            this.titleDotContainer.Controls.Add(this.btnClose);
            this.titleDotContainer.Location = new System.Drawing.Point(10, 8);
            this.titleDotContainer.Name = "titleDotContainer";
            this.titleDotContainer.Size = new System.Drawing.Size(70, 20);
            this.titleDotContainer.TabIndex = 1;
            // 
            // btnMax
            // 
            this.btnMax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(201)))), ((int)(((byte)(63)))));
            this.btnMax.Location = new System.Drawing.Point(40, 4);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(12, 12);
            this.btnMax.TabIndex = 2;
            // 
            // btnMin
            // 
            this.btnMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(189)))), ((int)(((byte)(46)))));
            this.btnMin.Location = new System.Drawing.Point(20, 4);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(12, 12);
            this.btnMin.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(95)))), ((int)(((byte)(86)))));
            this.btnClose.Location = new System.Drawing.Point(0, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(12, 12);
            this.btnClose.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(548, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Coderift_settings";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.ClientSize = new System.Drawing.Size(550, 500);
            this.Controls.Add(this.settingsPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.settingsPanel.ResumeLayout(false);
            this.terminalBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.enFlagIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.phFlagIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volIcon)).EndInit();
            this.titleBar.ResumeLayout(false);
            this.titleDotContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Panel titleBar;
        private System.Windows.Forms.Panel titleDotContainer;
        private System.Windows.Forms.Panel btnClose;
        private System.Windows.Forms.Panel btnMin;
        private System.Windows.Forms.Panel btnMax;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel terminalBody;
        private System.Windows.Forms.PictureBox volIcon;
        private System.Windows.Forms.TrackBar volSlider;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Label lblSubtitles;
        private System.Windows.Forms.Button btnFilipino;
        private System.Windows.Forms.Button btnEnglish;
        private System.Windows.Forms.PictureBox phFlagIcon;
        private System.Windows.Forms.PictureBox enFlagIcon;
        private System.Windows.Forms.Button btnBack;
    }
}
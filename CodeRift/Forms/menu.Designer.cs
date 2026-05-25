namespace CodeRift.Forms
{
    partial class MenuForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (_backgroundImage != null)
            {
                _backgroundImage.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            titleBox = new PictureBox();
            buttonContainer = new Panel();
            btnPlay = new Button();
            btnLevels = new Button();
            btnSettings = new Button();
            btnCredits = new Button();
            btnExit = new Button();
            ((System.ComponentModel.ISupportInitialize)titleBox).BeginInit();
            buttonContainer.SuspendLayout();
            SuspendLayout();
            // 
            // titleBox
            // 
            titleBox.BackColor = Color.Transparent;
            titleBox.Location = new Point(64, 20);
            titleBox.Name = "titleBox";
            titleBox.Size = new Size(1152, 350);
            titleBox.SizeMode = PictureBoxSizeMode.Zoom;
            titleBox.TabIndex = 0;
            titleBox.TabStop = false;
            // 
            // buttonContainer
            // 
            buttonContainer.BackColor = Color.Transparent;
            buttonContainer.Controls.Add(btnPlay);
            buttonContainer.Controls.Add(btnLevels);
            buttonContainer.Controls.Add(btnSettings);
            buttonContainer.Controls.Add(btnCredits);
            buttonContainer.Controls.Add(btnExit);
            buttonContainer.Location = new Point(390, 380);
            buttonContainer.Name = "buttonContainer";
            buttonContainer.Size = new Size(500, 400);
            buttonContainer.TabIndex = 1;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.Black;
            btnPlay.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            this.btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.FlatAppearance.MouseDownBackColor = Color.FromArgb(26, 107, 26);
            btnPlay.FlatAppearance.MouseOverBackColor = Color.Black;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.Font = new Font("Courier New", 18F, FontStyle.Bold);
            btnPlay.ForeColor = Color.FromArgb(0, 255, 65);
            btnPlay.Location = new Point(50, 0);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(400, 60);
            btnPlay.TabIndex = 0;
            btnPlay.Text = "[PLAY]";
            btnPlay.UseVisualStyleBackColor = false;
            // 
            // btnLevels
            // 
            btnLevels.BackColor = Color.Black;
            btnLevels.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            btnLevels.FlatAppearance.BorderSize = 0;
            btnLevels.FlatAppearance.MouseDownBackColor = Color.FromArgb(26, 107, 26);
            btnLevels.FlatAppearance.MouseOverBackColor = Color.Black;
            btnLevels.FlatStyle = FlatStyle.Flat;
            btnLevels.Font = new Font("Courier New", 18F, FontStyle.Bold);
            btnLevels.ForeColor = Color.FromArgb(0, 255, 65);
            btnLevels.Location = new Point(50, 80);
            btnLevels.Name = "btnLevels";
            btnLevels.Size = new Size(400, 60);
            btnLevels.TabIndex = 1;
            btnLevels.Text = "[LEVELS]";
            btnLevels.UseVisualStyleBackColor = false;
            btnLevels.Click += btnLevels_Click;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.Black;
            btnSettings.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatAppearance.MouseDownBackColor = Color.FromArgb(26, 107, 26);
            btnSettings.FlatAppearance.MouseOverBackColor = Color.Black;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Courier New", 18F, FontStyle.Bold);
            btnSettings.ForeColor = Color.FromArgb(0, 255, 65);
            btnSettings.Location = new Point(50, 160);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(400, 60);
            btnSettings.TabIndex = 2;
            btnSettings.Text = "[SETTINGS]";
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnCredits
            // 
            btnCredits.BackColor = Color.Black;
            btnCredits.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            btnCredits.FlatAppearance.BorderSize = 0;
            btnCredits.FlatAppearance.MouseDownBackColor = Color.FromArgb(26, 107, 26);
            btnCredits.FlatAppearance.MouseOverBackColor = Color.Black;
            btnCredits.FlatStyle = FlatStyle.Flat;
            btnCredits.Font = new Font("Courier New", 18F, FontStyle.Bold);
            btnCredits.ForeColor = Color.FromArgb(0, 255, 65);
            btnCredits.Location = new Point(50, 240);
            btnCredits.Name = "btnCredits";
            btnCredits.Size = new Size(400, 60);
            btnCredits.TabIndex = 3;
            btnCredits.Text = "[CREDITS]";
            btnCredits.UseVisualStyleBackColor = false;
            btnCredits.Click += btnCredits_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.Black;
            btnExit.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatAppearance.MouseDownBackColor = Color.FromArgb(26, 107, 26);
            btnExit.FlatAppearance.MouseOverBackColor = Color.Black;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Courier New", 18F, FontStyle.Bold);
            btnExit.ForeColor = Color.FromArgb(0, 255, 65);
            btnExit.Location = new Point(50, 320);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(400, 60);
            btnExit.TabIndex = 4;
            btnExit.Text = "[EXIT]";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // MenuForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(13, 13, 13);
            ClientSize = new Size(1280, 720);
            Controls.Add(buttonContainer);
            Controls.Add(titleBox);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "MenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Code Rift - Main Menu";
            WindowState = FormWindowState.Maximized;
            Load += menu_Load;
            ((System.ComponentModel.ISupportInitialize)titleBox).EndInit();
            buttonContainer.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox titleBox;
        private System.Windows.Forms.Panel buttonContainer;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnLevels;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnCredits;
        private System.Windows.Forms.Button btnExit;
    }
}
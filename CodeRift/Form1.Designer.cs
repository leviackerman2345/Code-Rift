namespace CodeRift
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            titleBox = new PictureBox();
            progressBg = new Panel();
            progressFill = new Panel();
            percentLabel = new Label();
            logBorder = new Panel();
            logContainer = new Panel();
            logLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)titleBox).BeginInit();
            progressBg.SuspendLayout();
            logBorder.SuspendLayout();
            logContainer.SuspendLayout();
            SuspendLayout();
            // 
            // titleBox
            // 
            titleBox.BackColor = Color.Transparent;
            titleBox.Location = new Point(50, 30);
            titleBox.Name = "titleBox";
            titleBox.Size = new Size(500, 120);
            titleBox.SizeMode = PictureBoxSizeMode.Zoom;
            titleBox.TabIndex = 0;
            titleBox.TabStop = false;
            // 
            // progressBg
            // 
            progressBg.BackColor = Color.FromArgb(26, 107, 26);
            progressBg.Controls.Add(progressFill);
            progressBg.Location = new Point(70, 180);
            progressBg.Name = "progressBg";
            progressBg.Padding = new Padding(2);
            progressBg.Size = new Size(400, 20);
            progressBg.TabIndex = 1;
            // 
            // progressFill
            // 
            progressFill.BackColor = Color.FromArgb(0, 255, 65);
            progressFill.Location = new Point(2, 2);
            progressFill.Name = "progressFill";
            progressFill.Size = new Size(140, 16);
            progressFill.TabIndex = 0;
            // 
            // percentLabel
            // 
            percentLabel.AutoSize = true;
            percentLabel.BackColor = Color.Transparent;
            percentLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            percentLabel.ForeColor = Color.White;
            percentLabel.Location = new Point(480, 178);
            percentLabel.Name = "percentLabel";
            percentLabel.Size = new Size(51, 21);
            percentLabel.TabIndex = 2;
            percentLabel.Text = "100%";
            // 
            // logBorder
            // 
            logBorder.BackColor = Color.FromArgb(0, 255, 65);
            logBorder.Controls.Add(logContainer);
            logBorder.Location = new Point(70, 215);
            logBorder.Name = "logBorder";
            logBorder.Padding = new Padding(1);
            logBorder.Size = new Size(460, 24);
            logBorder.TabIndex = 3;
            // 
            // logContainer
            // 
            logContainer.BackColor = Color.Black;
            logContainer.Controls.Add(logLabel);
            logContainer.Dock = DockStyle.Fill;
            logContainer.Location = new Point(1, 1);
            logContainer.Name = "logContainer";
            logContainer.Size = new Size(458, 22);
            logContainer.TabIndex = 0;
            // 
            // logLabel
            // 
            logLabel.AutoEllipsis = true;
            logLabel.BackColor = Color.Transparent;
            logLabel.Dock = DockStyle.Fill;
            logLabel.Font = new Font("Consolas", 9F);
            logLabel.ForeColor = Color.FromArgb(0, 255, 65);
            logLabel.Location = new Point(0, 0);
            logLabel.Name = "logLabel";
            logLabel.Padding = new Padding(5, 0, 5, 0);
            logLabel.Size = new Size(458, 22);
            logLabel.TabIndex = 0;
            logLabel.Text = "Loading Asset: Images\\enemies\\bug_corrupted_data_01.png";
            logLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(13, 13, 13);
            ClientSize = new Size(600, 340);
            Controls.Add(logBorder);
            Controls.Add(percentLabel);
            Controls.Add(progressBg);
            Controls.Add(titleBox);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)titleBox).EndInit();
            progressBg.ResumeLayout(false);
            logBorder.ResumeLayout(false);
            logContainer.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox titleBox;
        private System.Windows.Forms.Panel progressBg;
        private System.Windows.Forms.Panel progressFill;
        private System.Windows.Forms.Label percentLabel;
        private System.Windows.Forms.Panel logBorder;
        private System.Windows.Forms.Panel logContainer;
        private System.Windows.Forms.Label logLabel;
    }
}
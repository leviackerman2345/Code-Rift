namespace CodeRift.Forms
{
    partial class EpilogueForm
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
            this.dialogueBox = new System.Windows.Forms.PictureBox();
            this.dialogueLabel = new System.Windows.Forms.Label();
            this.lblClickHint = new System.Windows.Forms.Label();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dialogueBox)).BeginInit();
            this.SuspendLayout();
            // 
            // dialogueBox
            // 
            this.dialogueBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogueBox.BackColor = System.Drawing.Color.Transparent;
            this.dialogueBox.Location = new System.Drawing.Point(100, 520);
            this.dialogueBox.Name = "dialogueBox";
            this.dialogueBox.Size = new System.Drawing.Size(1080, 150);
            this.dialogueBox.TabIndex = 0;
            this.dialogueBox.TabStop = false;
            this.dialogueBox.Click += new System.EventHandler(this.dialogueBox_Click);
            // 
            // dialogueLabel
            // 
            this.dialogueLabel.BackColor = System.Drawing.Color.Transparent;
            this.dialogueLabel.Location = new System.Drawing.Point(0, 0);
            this.dialogueLabel.Name = "dialogueLabel";
            this.dialogueLabel.Size = new System.Drawing.Size(100, 23);
            this.dialogueLabel.TabIndex = 1;
            this.dialogueLabel.Text = "Dialogue Text";
            this.dialogueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dialogueLabel.Click += new System.EventHandler(this.dialogueBox_Click);
            // 
            // lblClickHint
            // 
            this.lblClickHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClickHint.AutoSize = true;
            this.lblClickHint.BackColor = System.Drawing.Color.Transparent;
            this.lblClickHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblClickHint.Location = new System.Drawing.Point(950, 120);
            this.lblClickHint.Name = "lblClickHint";
            this.lblClickHint.Size = new System.Drawing.Size(110, 15);
            this.lblClickHint.TabIndex = 4;
            this.lblClickHint.Text = "[Click to continue]";
            this.lblClickHint.Click += new System.EventHandler(this.dialogueBox_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnReturn.Location = new System.Drawing.Point(540, 335);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(200, 50);
            this.btnReturn.TabIndex = 2;
            this.btnReturn.Text = "[RETURN]";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Black;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnBack.Location = new System.Drawing.Point(28, 24);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(130, 42);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "[BACK]";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSkip.BackColor = System.Drawing.Color.Black;
            this.btnSkip.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnSkip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSkip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnSkip.Location = new System.Drawing.Point(1122, 24);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(130, 42);
            this.btnSkip.TabIndex = 5;
            this.btnSkip.Text = "[SKIP]";
            this.btnSkip.UseVisualStyleBackColor = false;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // EpilogueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.dialogueBox);
            this.DoubleBuffered = true;
            this.Name = "EpilogueForm";
            this.Text = "Epilogue";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EpilogueForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dialogueBox)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.PictureBox dialogueBox;
        private System.Windows.Forms.Label dialogueLabel;
        private System.Windows.Forms.Label lblClickHint;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSkip;
    }
}

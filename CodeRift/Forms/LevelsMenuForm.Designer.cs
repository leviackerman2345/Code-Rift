using System.Drawing;
using System.Windows.Forms;

namespace CodeRift.Forms
{
    partial class LevelsMenuForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnLevel1 = new System.Windows.Forms.Button();
            this.btnLevel2 = new System.Windows.Forms.Button();
            this.btnLevel3 = new System.Windows.Forms.Button();
            this.btnLevel4 = new System.Windows.Forms.Button();
            this.btnLevel5 = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.TabIndex = 0;
            // 
            // btnLevel1
            // 
            this.btnLevel1.Name = "btnLevel1";
            this.btnLevel1.TabIndex = 1;
            this.btnLevel1.UseVisualStyleBackColor = true;
            // 
            // btnLevel2
            // 
            this.btnLevel2.Name = "btnLevel2";
            this.btnLevel2.TabIndex = 2;
            this.btnLevel2.UseVisualStyleBackColor = true;
            // 
            // btnLevel3
            // 
            this.btnLevel3.Name = "btnLevel3";
            this.btnLevel3.TabIndex = 3;
            this.btnLevel3.UseVisualStyleBackColor = true;
            // 
            // btnLevel4
            // 
            this.btnLevel4.Name = "btnLevel4";
            this.btnLevel4.TabIndex = 4;
            this.btnLevel4.UseVisualStyleBackColor = true;
            // 
            // btnLevel5
            // 
            this.btnLevel5.Name = "btnLevel5";
            this.btnLevel5.TabIndex = 5;
            this.btnLevel5.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            this.btnBack.Name = "btnBack";
            this.btnBack.TabIndex = 6;
            this.btnBack.UseVisualStyleBackColor = true;
            // 
            // LevelsMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnLevel5);
            this.Controls.Add(this.btnLevel4);
            this.Controls.Add(this.btnLevel3);
            this.Controls.Add(this.btnLevel2);
            this.Controls.Add(this.btnLevel1);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.Name = "LevelsMenuForm";
            this.Text = "Levels";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLevel1;
        private System.Windows.Forms.Button btnLevel2;
        private System.Windows.Forms.Button btnLevel3;
        private System.Windows.Forms.Button btnLevel4;
        private System.Windows.Forms.Button btnLevel5;
        private System.Windows.Forms.Button btnBack;
    }
}

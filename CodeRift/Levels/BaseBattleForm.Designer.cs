namespace CodeRift.Levels
{
    partial class BaseBattleForm
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
            this.pbBackground = new System.Windows.Forms.PictureBox();
            this.pbPlayer = new System.Windows.Forms.PictureBox();
            this.pbPlayerAction = new System.Windows.Forms.PictureBox();
            this.pbEnemy = new System.Windows.Forms.PictureBox();
            this.pbEnemyAction = new System.Windows.Forms.PictureBox();
            this.lblLevelTitle = new System.Windows.Forms.Label();
            this.lblEliasName = new System.Windows.Forms.Label();
            this.lblEnemyName = new System.Windows.Forms.Label();
            this.lblEliasHealthValue = new System.Windows.Forms.Label();
            this.lblEnemyHealthValue = new System.Windows.Forms.Label();
            this.prgEliasHealth = new System.Windows.Forms.ProgressBar();
            this.prgEnemyHealth = new System.Windows.Forms.ProgressBar();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.txtCodeAnswer = new System.Windows.Forms.TextBox();
            this.btnExecuteCode = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.pnlSkillCards = new System.Windows.Forms.Panel();
            this.pbSkillCard4 = new System.Windows.Forms.PictureBox();
            this.pbSkillCard3 = new System.Windows.Forms.PictureBox();
            this.pbSkillCard2 = new System.Windows.Forms.PictureBox();
            this.pbSkillCard1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayerAction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEnemy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEnemyAction)).BeginInit();
            this.pnlSkillCards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillCard4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillCard3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillCard2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillCard1)).BeginInit();
            this.SuspendLayout();
            // 
            // pbBackground
            // 
            this.pbBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.pbBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbBackground.Location = new System.Drawing.Point(0, 0);
            this.pbBackground.Name = "pbBackground";
            this.pbBackground.Size = new System.Drawing.Size(1280, 720);
            this.pbBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBackground.TabIndex = 0;
            this.pbBackground.TabStop = false;
            // 
            // pbPlayer
            // 
            this.pbPlayer.BackColor = System.Drawing.Color.Transparent;
            this.pbPlayer.Location = new System.Drawing.Point(145, 238);
            this.pbPlayer.Name = "pbPlayer";
            this.pbPlayer.Size = new System.Drawing.Size(230, 260);
            this.pbPlayer.TabIndex = 1;
            this.pbPlayer.TabStop = false;
            // 
            // pbPlayerAction
            // 
            this.pbPlayerAction.BackColor = System.Drawing.Color.Transparent;
            this.pbPlayerAction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPlayerAction.Location = new System.Drawing.Point(390, 298);
            this.pbPlayerAction.Name = "pbPlayerAction";
            this.pbPlayerAction.Size = new System.Drawing.Size(96, 96);
            this.pbPlayerAction.TabIndex = 2;
            this.pbPlayerAction.TabStop = false;
            // 
            // pbEnemy
            // 
            this.pbEnemy.BackColor = System.Drawing.Color.Transparent;
            this.pbEnemy.Location = new System.Drawing.Point(905, 238);
            this.pbEnemy.Name = "pbEnemy";
            this.pbEnemy.Size = new System.Drawing.Size(230, 260);
            this.pbEnemy.TabIndex = 3;
            this.pbEnemy.TabStop = false;
            // 
            // pbEnemyAction
            // 
            this.pbEnemyAction.BackColor = System.Drawing.Color.Transparent;
            this.pbEnemyAction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbEnemyAction.Location = new System.Drawing.Point(794, 298);
            this.pbEnemyAction.Name = "pbEnemyAction";
            this.pbEnemyAction.Size = new System.Drawing.Size(96, 96);
            this.pbEnemyAction.TabIndex = 4;
            this.pbEnemyAction.TabStop = false;
            // 
            // lblLevelTitle
            // 
            this.lblLevelTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLevelTitle.Font = new System.Drawing.Font("Courier New", 16F, System.Drawing.FontStyle.Bold);
            this.lblLevelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblLevelTitle.Location = new System.Drawing.Point(416, 28);
            this.lblLevelTitle.Name = "lblLevelTitle";
            this.lblLevelTitle.Size = new System.Drawing.Size(448, 36);
            this.lblLevelTitle.TabIndex = 5;
            this.lblLevelTitle.Text = "Level";
            this.lblLevelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEliasName
            // 
            this.lblEliasName.BackColor = System.Drawing.Color.Transparent;
            this.lblEliasName.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.lblEliasName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblEliasName.Location = new System.Drawing.Point(28, 82);
            this.lblEliasName.Name = "lblEliasName";
            this.lblEliasName.Size = new System.Drawing.Size(300, 24);
            this.lblEliasName.TabIndex = 6;
            this.lblEliasName.Text = "ELIAS";
            // 
            // lblEnemyName
            // 
            this.lblEnemyName.BackColor = System.Drawing.Color.Transparent;
            this.lblEnemyName.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.lblEnemyName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblEnemyName.Location = new System.Drawing.Point(952, 82);
            this.lblEnemyName.Name = "lblEnemyName";
            this.lblEnemyName.Size = new System.Drawing.Size(300, 24);
            this.lblEnemyName.TabIndex = 7;
            this.lblEnemyName.Text = "ENEMY";
            this.lblEnemyName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblEliasHealthValue
            // 
            this.lblEliasHealthValue.BackColor = System.Drawing.Color.Transparent;
            this.lblEliasHealthValue.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Bold);
            this.lblEliasHealthValue.ForeColor = System.Drawing.Color.White;
            this.lblEliasHealthValue.Location = new System.Drawing.Point(334, 82);
            this.lblEliasHealthValue.Name = "lblEliasHealthValue";
            this.lblEliasHealthValue.Size = new System.Drawing.Size(90, 24);
            this.lblEliasHealthValue.TabIndex = 8;
            this.lblEliasHealthValue.Text = "100/100";
            // 
            // lblEnemyHealthValue
            // 
            this.lblEnemyHealthValue.BackColor = System.Drawing.Color.Transparent;
            this.lblEnemyHealthValue.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Bold);
            this.lblEnemyHealthValue.ForeColor = System.Drawing.Color.White;
            this.lblEnemyHealthValue.Location = new System.Drawing.Point(856, 82);
            this.lblEnemyHealthValue.Name = "lblEnemyHealthValue";
            this.lblEnemyHealthValue.Size = new System.Drawing.Size(90, 24);
            this.lblEnemyHealthValue.TabIndex = 9;
            this.lblEnemyHealthValue.Text = "100/100";
            this.lblEnemyHealthValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // prgEliasHealth
            // 
            this.prgEliasHealth.Location = new System.Drawing.Point(28, 110);
            this.prgEliasHealth.Name = "prgEliasHealth";
            this.prgEliasHealth.Size = new System.Drawing.Size(396, 24);
            this.prgEliasHealth.TabIndex = 10;
            this.prgEliasHealth.Value = 100;
            // 
            // prgEnemyHealth
            // 
            this.prgEnemyHealth.Location = new System.Drawing.Point(856, 110);
            this.prgEnemyHealth.Name = "prgEnemyHealth";
            this.prgEnemyHealth.Size = new System.Drawing.Size(396, 24);
            this.prgEnemyHealth.TabIndex = 11;
            this.prgEnemyHealth.Value = 100;
            // 
            // lblQuestion
            // 
            this.lblQuestion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblQuestion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblQuestion.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Bold);
            this.lblQuestion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblQuestion.Location = new System.Drawing.Point(650, 520);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.lblQuestion.Size = new System.Drawing.Size(602, 64);
            this.lblQuestion.TabIndex = 12;
            this.lblQuestion.Text = "Question placeholder";
            // 
            // txtCodeAnswer
            // 
            this.txtCodeAnswer.BackColor = System.Drawing.Color.Black;
            this.txtCodeAnswer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCodeAnswer.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.txtCodeAnswer.ForeColor = System.Drawing.Color.White;
            this.txtCodeAnswer.Location = new System.Drawing.Point(650, 598);
            this.txtCodeAnswer.Multiline = true;
            this.txtCodeAnswer.Name = "txtCodeAnswer";
            this.txtCodeAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCodeAnswer.Size = new System.Drawing.Size(430, 82);
            this.txtCodeAnswer.TabIndex = 13;
            // 
            // btnExecuteCode
            // 
            this.btnExecuteCode.BackColor = System.Drawing.Color.Black;
            this.btnExecuteCode.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnExecuteCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecuteCode.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.btnExecuteCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnExecuteCode.Location = new System.Drawing.Point(1100, 598);
            this.btnExecuteCode.Name = "btnExecuteCode";
            this.btnExecuteCode.Size = new System.Drawing.Size(152, 82);
            this.btnExecuteCode.TabIndex = 14;
            this.btnExecuteCode.Text = "EXECUTE CODE";
            this.btnExecuteCode.UseVisualStyleBackColor = false;
            this.btnExecuteCode.Click += new System.EventHandler(this.btnExecuteCode_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Black;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnBack.Location = new System.Drawing.Point(28, 24);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(130, 42);
            this.btnBack.TabIndex = 15;
            this.btnBack.Text = "[BACK]";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.Black;
            this.btnContinue.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this.btnContinue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnContinue.Location = new System.Drawing.Point(540, 310);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(200, 60);
            this.btnContinue.TabIndex = 17;
            this.btnContinue.Text = "CONTINUE";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Visible = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // pnlSkillCards
            // 
            this.pnlSkillCards.BackColor = System.Drawing.Color.Transparent;
            this.pnlSkillCards.Controls.Add(this.pbSkillCard4);
            this.pnlSkillCards.Controls.Add(this.pbSkillCard3);
            this.pnlSkillCards.Controls.Add(this.pbSkillCard2);
            this.pnlSkillCards.Controls.Add(this.pbSkillCard1);
            this.pnlSkillCards.Location = new System.Drawing.Point(28, 548);
            this.pnlSkillCards.Name = "pnlSkillCards";
            this.pnlSkillCards.Size = new System.Drawing.Size(532, 132);
            this.pnlSkillCards.TabIndex = 16;
            // 
            // pbSkillCard4
            // 
            this.pbSkillCard4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pbSkillCard4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSkillCard4.Location = new System.Drawing.Point(399, 0);
            this.pbSkillCard4.Name = "pbSkillCard4";
            this.pbSkillCard4.Size = new System.Drawing.Size(124, 132);
            this.pbSkillCard4.TabIndex = 3;
            this.pbSkillCard4.TabStop = false;
            // 
            // pbSkillCard3
            // 
            this.pbSkillCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pbSkillCard3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSkillCard3.Location = new System.Drawing.Point(266, 0);
            this.pbSkillCard3.Name = "pbSkillCard3";
            this.pbSkillCard3.Size = new System.Drawing.Size(124, 132);
            this.pbSkillCard3.TabIndex = 2;
            this.pbSkillCard3.TabStop = false;
            // 
            // pbSkillCard2
            // 
            this.pbSkillCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pbSkillCard2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSkillCard2.Location = new System.Drawing.Point(133, 0);
            this.pbSkillCard2.Name = "pbSkillCard2";
            this.pbSkillCard2.Size = new System.Drawing.Size(124, 132);
            this.pbSkillCard2.TabIndex = 1;
            this.pbSkillCard2.TabStop = false;
            // 
            // pbSkillCard1
            // 
            this.pbSkillCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pbSkillCard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSkillCard1.Location = new System.Drawing.Point(0, 0);
            this.pbSkillCard1.Name = "pbSkillCard1";
            this.pbSkillCard1.Size = new System.Drawing.Size(124, 132);
            this.pbSkillCard1.TabIndex = 0;
            this.pbSkillCard1.TabStop = false;
            // 
            // BaseBattleForm
            // 
            this.AcceptButton = this.btnExecuteCode;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.pnlSkillCards);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnExecuteCode);
            this.Controls.Add(this.txtCodeAnswer);
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.prgEnemyHealth);
            this.Controls.Add(this.prgEliasHealth);
            this.Controls.Add(this.lblEnemyHealthValue);
            this.Controls.Add(this.lblEliasHealthValue);
            this.Controls.Add(this.lblEnemyName);
            this.Controls.Add(this.lblEliasName);
            this.Controls.Add(this.lblLevelTitle);
            this.Controls.Add(this.pbEnemyAction);
            this.Controls.Add(this.pbEnemy);
            this.Controls.Add(this.pbPlayerAction);
            this.Controls.Add(this.pbPlayer);
            this.Controls.Add(this.pbBackground);
            this.DoubleBuffered = true;
            this.Name = "BaseBattleForm";
            this.Text = "Battle";
            ((System.ComponentModel.ISupportInitialize)(this.pbBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayerAction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEnemy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEnemyAction)).EndInit();
            this.pnlSkillCards.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillCard4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillCard3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillCard2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillCard1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        protected System.Windows.Forms.PictureBox pbBackground;
        protected System.Windows.Forms.PictureBox pbPlayer;
        protected System.Windows.Forms.PictureBox pbPlayerAction;
        protected System.Windows.Forms.PictureBox pbEnemy;
        protected System.Windows.Forms.PictureBox pbEnemyAction;
        protected System.Windows.Forms.Label lblLevelTitle;
        protected System.Windows.Forms.Label lblEliasName;
        protected System.Windows.Forms.Label lblEnemyName;
        protected System.Windows.Forms.Label lblEliasHealthValue;
        protected System.Windows.Forms.Label lblEnemyHealthValue;
        protected System.Windows.Forms.ProgressBar prgEliasHealth;
        protected System.Windows.Forms.ProgressBar prgEnemyHealth;
        protected System.Windows.Forms.Label lblQuestion;
        protected System.Windows.Forms.TextBox txtCodeAnswer;
        protected System.Windows.Forms.Button btnExecuteCode;
        protected System.Windows.Forms.Button btnBack;
        protected System.Windows.Forms.Button btnContinue;
        protected System.Windows.Forms.Panel pnlSkillCards;
        protected System.Windows.Forms.PictureBox pbSkillCard1;
        protected System.Windows.Forms.PictureBox pbSkillCard2;
        protected System.Windows.Forms.PictureBox pbSkillCard3;
        protected System.Windows.Forms.PictureBox pbSkillCard4;
    }
}

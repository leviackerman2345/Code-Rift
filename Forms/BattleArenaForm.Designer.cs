using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeRift.Forms
{
    partial class BattleArenaForm
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlTitleBar = new Panel();
            btnBack = new Button();
            lblLevelTitle = new Label();
            pnlHUDStrip = new Panel();
            picPlayerThumb = new PictureBox();
            lblPlayerName = new Label();
            lblPlayerHP = new Label();
            lblPlayerHPMax = new Label();
            pnlPlayerHealthBg = new Panel();
            pnlPlayerHealthFill = new Panel();
            lblVS = new Label();
            lblTimer = new Label();
            picEnemyThumb = new PictureBox();
            lblEnemyName = new Label();
            lblEnemyHP = new Label();
            lblEnemyHPMax = new Label();
            pnlEnemyHealthBg = new Panel();
            pnlEnemyHealthFill = new Panel();
            pnlMainContent = new Panel();
            pnlBattleZone = new Panel();
            picPlayerPortrait = new PictureBox();
            picEnemyPortrait = new PictureBox();
            pnlBottomZone = new TableLayoutPanel();
            flpPlayerCards = new FlowLayoutPanel();
            picPlayerCard1 = new PictureBox();
            picPlayerCard2 = new PictureBox();
            picPlayerCard3 = new PictureBox();
            picPlayerCard4 = new PictureBox();
            picPlayerCard5 = new PictureBox();
            flpEnemyCards = new FlowLayoutPanel();
            picEnemyCard1 = new PictureBox();
            picEnemyCard2 = new PictureBox();
            picEnemyCard3 = new PictureBox();
            picEnemyCard4 = new PictureBox();
            picEnemyCard5 = new PictureBox();

            pnlTitleBar.SuspendLayout();
            pnlHUDStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPlayerThumb).BeginInit();
            pnlPlayerHealthBg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picEnemyThumb).BeginInit();
            pnlEnemyHealthBg.SuspendLayout();
            pnlMainContent.SuspendLayout();
            pnlBattleZone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPlayerPortrait).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyPortrait).BeginInit();
            pnlBottomZone.SuspendLayout();
            flpPlayerCards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPlayerCard1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPlayerCard2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPlayerCard3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPlayerCard4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPlayerCard5).BeginInit();
            flpEnemyCards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picEnemyCard1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyCard2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyCard3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyCard4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyCard5).BeginInit();
            SuspendLayout();
            // 
            // pnlTitleBar
            // 
            pnlTitleBar.BackColor = Color.Transparent;
            pnlTitleBar.Controls.Add(btnBack);
            pnlTitleBar.Controls.Add(lblLevelTitle);
            pnlTitleBar.Dock = DockStyle.Top;
            pnlTitleBar.Location = new Point(0, 0);
            pnlTitleBar.Name = "pnlTitleBar";
            pnlTitleBar.Size = new Size(1280, 58);
            pnlTitleBar.TabIndex = 1;
            // 
            // btnBack
            // 
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Courier New", 12F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(16, 7);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(120, 44);
            btnBack.TabIndex = 0;
            btnBack.Text = "< BACK";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // lblLevelTitle
            // 
            lblLevelTitle.Dock = DockStyle.Fill;
            lblLevelTitle.Font = new Font("Courier New", 14F, FontStyle.Bold);
            lblLevelTitle.ForeColor = Color.White;
            lblLevelTitle.Location = new Point(0, 0);
            lblLevelTitle.Name = "lblLevelTitle";
            lblLevelTitle.Size = new Size(1280, 58);
            lblLevelTitle.TabIndex = 1;
            lblLevelTitle.Text = "// LEVEL 1 : LOOPBUG //";
            lblLevelTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlHUDStrip
            // 
            pnlHUDStrip.BackColor = Color.Transparent;
            pnlHUDStrip.Controls.Add(picPlayerThumb);
            pnlHUDStrip.Controls.Add(lblPlayerName);
            pnlHUDStrip.Controls.Add(lblPlayerHP);
            pnlHUDStrip.Controls.Add(lblPlayerHPMax);
            pnlHUDStrip.Controls.Add(pnlPlayerHealthBg);
            pnlHUDStrip.Controls.Add(lblVS);
            pnlHUDStrip.Controls.Add(lblTimer);
            pnlHUDStrip.Controls.Add(picEnemyThumb);
            pnlHUDStrip.Controls.Add(lblEnemyName);
            pnlHUDStrip.Controls.Add(lblEnemyHP);
            pnlHUDStrip.Controls.Add(lblEnemyHPMax);
            pnlHUDStrip.Controls.Add(pnlEnemyHealthBg);
            pnlHUDStrip.Dock = DockStyle.Top;
            pnlHUDStrip.Location = new Point(0, 58);
            pnlHUDStrip.Name = "pnlHUDStrip";
            pnlHUDStrip.Size = new Size(1280, 130);
            pnlHUDStrip.TabIndex = 2;
            // 
            // picPlayerThumb
            // 
            picPlayerThumb.Location = new Point(20, 20);
            picPlayerThumb.Name = "picPlayerThumb";
            picPlayerThumb.Size = new Size(80, 80);
            picPlayerThumb.SizeMode = PictureBoxSizeMode.StretchImage;
            picPlayerThumb.TabIndex = 0;
            picPlayerThumb.TabStop = false;
            // 
            // lblPlayerName
            // 
            lblPlayerName.AutoSize = true;
            lblPlayerName.Font = new Font("Courier New", 12F, FontStyle.Bold);
            lblPlayerName.ForeColor = Color.FromArgb(0, 255, 65);
            lblPlayerName.Location = new Point(110, 20);
            lblPlayerName.Name = "lblPlayerName";
            lblPlayerName.Size = new Size(58, 18);
            lblPlayerName.TabIndex = 1;
            lblPlayerName.Text = "ELIAS";
            // 
            // lblPlayerHP
            // 
            lblPlayerHP.AutoSize = true;
            lblPlayerHP.Font = new Font("Courier New", 22F, FontStyle.Bold);
            lblPlayerHP.ForeColor = Color.White;
            lblPlayerHP.Location = new Point(110, 75);
            lblPlayerHP.Name = "lblPlayerHP";
            lblPlayerHP.Size = new Size(120, 34);
            lblPlayerHP.TabIndex = 2;
            lblPlayerHP.Text = "100";
            // 
            // lblPlayerHPMax
            // 
            lblPlayerHPMax.AutoSize = true;
            lblPlayerHPMax.Font = new Font("Courier New", 10F);
            lblPlayerHPMax.ForeColor = Color.LightGray;
            lblPlayerHPMax.Location = new Point(230, 88);
            lblPlayerHPMax.Name = "lblPlayerHPMax";
            lblPlayerHPMax.Size = new Size(72, 17);
            lblPlayerHPMax.TabIndex = 3;
            lblPlayerHPMax.Text = "/ 100 HP";
            // 
            // pnlPlayerHealthBg
            // 
            pnlPlayerHealthBg.BackColor = Color.FromArgb(20, 60, 20);
            pnlPlayerHealthBg.Controls.Add(pnlPlayerHealthFill);
            pnlPlayerHealthBg.Location = new Point(110, 45);
            pnlPlayerHealthBg.Name = "pnlPlayerHealthBg";
            pnlPlayerHealthBg.Size = new Size(450, 25);
            pnlPlayerHealthBg.TabIndex = 4;
            // 
            // pnlPlayerHealthFill
            // 
            pnlPlayerHealthFill.BackColor = Color.FromArgb(0, 255, 65);
            pnlPlayerHealthFill.Dock = DockStyle.Left;
            pnlPlayerHealthFill.Location = new Point(0, 0);
            pnlPlayerHealthFill.Name = "pnlPlayerHealthFill";
            pnlPlayerHealthFill.Size = new Size(450, 25);
            pnlPlayerHealthFill.TabIndex = 0;
            // 
            // lblVS
            // 
            lblVS.Anchor = AnchorStyles.Top;
            lblVS.AutoSize = true;
            lblVS.Font = new Font("Courier New", 22F, FontStyle.Bold);
            lblVS.ForeColor = Color.White;
            lblVS.Location = new Point(615, 25);
            lblVS.Name = "lblVS";
            lblVS.Size = new Size(51, 34);
            lblVS.TabIndex = 5;
            lblVS.Text = "VS";
            // 
            // lblTimer
            // 
            lblTimer.Anchor = AnchorStyles.Top;
            lblTimer.AutoSize = false;
            lblTimer.Font = new Font("Courier New", 16F, FontStyle.Bold);
            lblTimer.ForeColor = Color.White;
            lblTimer.Location = new Point(440, 70);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(400, 30);
            lblTimer.TabIndex = 6;
            lblTimer.Text = "00:00";
            lblTimer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picEnemyThumb
            // 
            picEnemyThumb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picEnemyThumb.Location = new Point(1180, 20);
            picEnemyThumb.Name = "picEnemyThumb";
            picEnemyThumb.Size = new Size(80, 80);
            picEnemyThumb.SizeMode = PictureBoxSizeMode.StretchImage;
            picEnemyThumb.TabIndex = 11;
            picEnemyThumb.TabStop = false;
            // 
            // lblEnemyName
            // 
            lblEnemyName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEnemyName.Font = new Font("Courier New", 12F, FontStyle.Bold);
            lblEnemyName.ForeColor = Color.FromArgb(0, 255, 65);
            lblEnemyName.Location = new Point(720, 20);
            lblEnemyName.Name = "lblEnemyName";
            lblEnemyName.Size = new Size(450, 25);
            lblEnemyName.TabIndex = 10;
            lblEnemyName.Text = "LOOPBUG";
            lblEnemyName.TextAlign = ContentAlignment.TopRight;
            // 
            // lblEnemyHP
            // 
            lblEnemyHP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEnemyHP.Font = new Font("Courier New", 22F, FontStyle.Bold);
            lblEnemyHP.ForeColor = Color.White;
            lblEnemyHP.Location = new Point(1052, 75);
            lblEnemyHP.Name = "lblEnemyHP";
            lblEnemyHP.Size = new Size(120, 33);
            lblEnemyHP.TabIndex = 9;
            lblEnemyHP.Text = "100";
            lblEnemyHP.TextAlign = ContentAlignment.TopRight;
           
            // 
            // lblEnemyHPMax
            // 
            lblEnemyHPMax.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEnemyHPMax.AutoSize = true;
            lblEnemyHPMax.Font = new Font("Courier New", 10F);
            lblEnemyHPMax.ForeColor = Color.LightGray;
            lblEnemyHPMax.Location = new Point(970, 88);
            lblEnemyHPMax.Name = "lblEnemyHPMax";
            lblEnemyHPMax.Size = new Size(72, 17);
            lblEnemyHPMax.TabIndex = 8;
            lblEnemyHPMax.Text = "100 HP /";
            // 
            // pnlEnemyHealthBg
            // 
            pnlEnemyHealthBg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlEnemyHealthBg.BackColor = Color.FromArgb(20, 60, 20);
            pnlEnemyHealthBg.Controls.Add(pnlEnemyHealthFill);
            pnlEnemyHealthBg.Location = new Point(720, 45);
            pnlEnemyHealthBg.Name = "pnlEnemyHealthBg";
            pnlEnemyHealthBg.Size = new Size(450, 25);
            pnlEnemyHealthBg.TabIndex = 7;
            // 
            // pnlEnemyHealthFill
            // 
            pnlEnemyHealthFill.BackColor = Color.Red;
            pnlEnemyHealthFill.Dock = DockStyle.Right;
            pnlEnemyHealthFill.Location = new Point(0, 0);
            pnlEnemyHealthFill.Name = "pnlEnemyHealthFill";
            pnlEnemyHealthFill.Size = new Size(450, 25);
            pnlEnemyHealthFill.TabIndex = 0;
            // 
            // pnlMainContent
            // 
            pnlMainContent.BackColor = Color.Transparent;
            pnlMainContent.Controls.Add(pnlBattleZone);
            pnlMainContent.Dock = DockStyle.Fill;
            pnlMainContent.Location = new Point(0, 188);
            pnlMainContent.Name = "pnlMainContent";
            pnlMainContent.Size = new Size(1280, 532);
            pnlMainContent.TabIndex = 12;
            // 
            // pnlBattleZone
            // 
            pnlBattleZone.BackColor = Color.Transparent;
            pnlBattleZone.Controls.Add(picPlayerPortrait);
            pnlBattleZone.Controls.Add(picEnemyPortrait);
            pnlBattleZone.Dock = DockStyle.Fill;
            pnlBattleZone.Location = new Point(0, 0);
            pnlBattleZone.Name = "pnlBattleZone";
            pnlBattleZone.Size = new Size(1280, 532);
            pnlBattleZone.TabIndex = 3;
            // 
            // picPlayerPortrait
            // 
            picPlayerPortrait.BackColor = Color.Transparent;
            picPlayerPortrait.Location = new Point(-450, 200);
            picPlayerPortrait.Name = "picPlayerPortrait";
            picPlayerPortrait.Size = new Size(450, 450);
            picPlayerPortrait.SizeMode = PictureBoxSizeMode.Zoom;
            picPlayerPortrait.TabIndex = 0;
            picPlayerPortrait.TabStop = false;
            // 
            // picEnemyPortrait
            // 
            picEnemyPortrait.BackColor = Color.Transparent;
            picEnemyPortrait.Location = new Point(1280, 200);
            picEnemyPortrait.Name = "picEnemyPortrait";
            picEnemyPortrait.Size = new Size(450, 450);
            picEnemyPortrait.SizeMode = PictureBoxSizeMode.Zoom;
            picEnemyPortrait.TabIndex = 1;
            picEnemyPortrait.TabStop = false;
            // 
            // pnlBottomZone
            // 
            pnlBottomZone.BackColor = Color.Transparent;
            pnlBottomZone.ColumnCount = 2;
            pnlBottomZone.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlBottomZone.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlBottomZone.Controls.Add(flpPlayerCards, 0, 0);
            pnlBottomZone.Controls.Add(flpEnemyCards, 1, 0);
            pnlBottomZone.Dock = DockStyle.Bottom;
            pnlBottomZone.Location = new Point(0, 570);
            pnlBottomZone.Name = "pnlBottomZone";
            pnlBottomZone.RowCount = 1;
            pnlBottomZone.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlBottomZone.Size = new Size(1280, 150);
            pnlBottomZone.TabIndex = 4;
            // 
            // flpPlayerCards
            // 
            flpPlayerCards.Anchor = AnchorStyles.Left;
            flpPlayerCards.AutoSize = true;
            flpPlayerCards.Controls.Add(picPlayerCard1);
            flpPlayerCards.Controls.Add(picPlayerCard2);
            flpPlayerCards.Controls.Add(picPlayerCard3);
            flpPlayerCards.Controls.Add(picPlayerCard4);
            flpPlayerCards.Controls.Add(picPlayerCard5);
            flpPlayerCards.Location = new Point(40, 2);
            flpPlayerCards.Name = "flpPlayerCards";
            flpPlayerCards.Padding = new Padding(0, 5, 0, 0);
            flpPlayerCards.Size = new Size(494, 139);
            flpPlayerCards.TabIndex = 1;
            // 
            // picPlayerCard1
            // 
            picPlayerCard1.BorderStyle = BorderStyle.FixedSingle;
            picPlayerCard1.Location = new Point(0, 5);
            picPlayerCard1.Name = "picPlayerCard1";
            picPlayerCard1.Size = new Size(90, 134);
            picPlayerCard1.SizeMode = PictureBoxSizeMode.StretchImage;
            picPlayerCard1.TabIndex = 0;
            picPlayerCard1.TabStop = false;
            // 
            // picPlayerCard2
            // 
            picPlayerCard2.BorderStyle = BorderStyle.FixedSingle;
            picPlayerCard2.Location = new Point(101, 5);
            picPlayerCard2.Name = "picPlayerCard2";
            picPlayerCard2.Size = new Size(90, 134);
            picPlayerCard2.SizeMode = PictureBoxSizeMode.StretchImage;
            picPlayerCard2.TabIndex = 1;
            picPlayerCard2.TabStop = false;
            // 
            // picPlayerCard3
            // 
            picPlayerCard3.BorderStyle = BorderStyle.FixedSingle;
            picPlayerCard3.Location = new Point(202, 5);
            picPlayerCard3.Name = "picPlayerCard3";
            picPlayerCard3.Size = new Size(90, 134);
            picPlayerCard3.SizeMode = PictureBoxSizeMode.StretchImage;
            picPlayerCard3.TabIndex = 2;
            picPlayerCard3.TabStop = false;
            // 
            // picPlayerCard4
            // 
            picPlayerCard4.BorderStyle = BorderStyle.FixedSingle;
            picPlayerCard4.Location = new Point(303, 5);
            picPlayerCard4.Name = "picPlayerCard4";
            picPlayerCard4.Size = new Size(90, 134);
            picPlayerCard4.SizeMode = PictureBoxSizeMode.StretchImage;
            picPlayerCard4.TabIndex = 3;
            picPlayerCard4.TabStop = false;
            // 
            // picPlayerCard5
            // 
            picPlayerCard5.BorderStyle = BorderStyle.FixedSingle;
            picPlayerCard5.Location = new Point(404, 5);
            picPlayerCard5.Name = "picPlayerCard5";
            picPlayerCard5.Size = new Size(90, 134);
            picPlayerCard5.SizeMode = PictureBoxSizeMode.StretchImage;
            picPlayerCard5.TabIndex = 4;
            picPlayerCard5.TabStop = false;
            // 
            // flpEnemyCards
            // 
            flpEnemyCards.Anchor = AnchorStyles.Right;
            flpEnemyCards.AutoSize = true;
            flpEnemyCards.Controls.Add(picEnemyCard1);
            flpEnemyCards.Controls.Add(picEnemyCard2);
            flpEnemyCards.Controls.Add(picEnemyCard3);
            flpEnemyCards.Controls.Add(picEnemyCard4);
            flpEnemyCards.Controls.Add(picEnemyCard5);
            flpEnemyCards.Location = new Point(735, 2);
            flpEnemyCards.Name = "flpEnemyCards";
            flpEnemyCards.Padding = new Padding(0, 5, 0, 0);
            flpEnemyCards.RightToLeft = RightToLeft.Yes;
            flpEnemyCards.Size = new Size(505, 139);
            flpEnemyCards.TabIndex = 1;
            // 
            // picEnemyCard1
            // 
            picEnemyCard1.BorderStyle = BorderStyle.FixedSingle;
            picEnemyCard1.Location = new Point(404, 5);
            picEnemyCard1.Name = "picEnemyCard1";
            picEnemyCard1.Size = new Size(90, 134);
            picEnemyCard1.SizeMode = PictureBoxSizeMode.StretchImage;
            picEnemyCard1.TabIndex = 0;
            picEnemyCard1.TabStop = false;
            // 
            // picEnemyCard2
            // 
            picEnemyCard2.BorderStyle = BorderStyle.FixedSingle;
            picEnemyCard2.Location = new Point(303, 5);
            picEnemyCard2.Name = "picEnemyCard2";
            picEnemyCard2.Size = new Size(90, 134);
            picEnemyCard2.SizeMode = PictureBoxSizeMode.StretchImage;
            picEnemyCard2.TabIndex = 1;
            picEnemyCard2.TabStop = false;
            // 
            // picEnemyCard3
            // 
            picEnemyCard3.BorderStyle = BorderStyle.FixedSingle;
            picEnemyCard3.Location = new Point(202, 5);
            picEnemyCard3.Name = "picEnemyCard3";
            picEnemyCard3.Size = new Size(90, 134);
            picEnemyCard3.SizeMode = PictureBoxSizeMode.StretchImage;
            picEnemyCard3.TabIndex = 2;
            picEnemyCard3.TabStop = false;
            // 
            // picEnemyCard4
            // 
            picEnemyCard4.BorderStyle = BorderStyle.FixedSingle;
            picEnemyCard4.Location = new Point(101, 5);
            picEnemyCard4.Name = "picEnemyCard4";
            picEnemyCard4.Size = new Size(90, 134);
            picEnemyCard4.SizeMode = PictureBoxSizeMode.StretchImage;
            picEnemyCard4.TabIndex = 3;
            picEnemyCard4.TabStop = false;
            // 
            // picEnemyCard5
            // 
            picEnemyCard5.BorderStyle = BorderStyle.FixedSingle;
            picEnemyCard5.Location = new Point(0, 5);
            picEnemyCard5.Name = "picEnemyCard5";
            picEnemyCard5.Size = new Size(90, 134);
            picEnemyCard5.SizeMode = PictureBoxSizeMode.StretchImage;
            picEnemyCard5.TabIndex = 4;
            picEnemyCard5.TabStop = false;
            // 
            // BattleArenaForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 13, 8);
            ClientSize = new Size(1280, 720);
            Controls.Add(pnlMainContent);
            Controls.Add(pnlBottomZone);
            Controls.Add(pnlHUDStrip);
            Controls.Add(pnlTitleBar);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "BattleArenaForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Battle Arena";
            Load += BattleArenaForm_Load;
            pnlTitleBar.ResumeLayout(false);
            pnlHUDStrip.ResumeLayout(false);
            pnlHUDStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPlayerThumb).EndInit();
            pnlPlayerHealthBg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picEnemyThumb).EndInit();
            pnlEnemyHealthBg.ResumeLayout(false);
            pnlMainContent.ResumeLayout(false);
            pnlBattleZone.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPlayerPortrait).EndInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyPortrait).EndInit();
            pnlBottomZone.ResumeLayout(false);
            pnlBottomZone.PerformLayout();
            flpPlayerCards.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPlayerCard1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPlayerCard2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPlayerCard3).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPlayerCard4).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPlayerCard5).EndInit();
            flpEnemyCards.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picEnemyCard1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyCard2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyCard3).EndInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyCard4).EndInit();
            ((System.ComponentModel.ISupportInitialize)picEnemyCard5).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblLevelTitle;
        
        private System.Windows.Forms.Panel pnlHUDStrip;
        private System.Windows.Forms.PictureBox picPlayerThumb;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.Label lblPlayerHP;
        private System.Windows.Forms.Label lblPlayerHPMax;
        private System.Windows.Forms.Panel pnlPlayerHealthBg;
        private System.Windows.Forms.Panel pnlPlayerHealthFill;
        
        private System.Windows.Forms.Label lblVS;
        private System.Windows.Forms.Label lblTimer;
        
        private System.Windows.Forms.PictureBox picEnemyThumb;
        private System.Windows.Forms.Label lblEnemyName;
        private System.Windows.Forms.Label lblEnemyHP;
        private System.Windows.Forms.Label lblEnemyHPMax;
        private System.Windows.Forms.Panel pnlEnemyHealthBg;
        private System.Windows.Forms.Panel pnlEnemyHealthFill;
        
        private System.Windows.Forms.Panel pnlBattleZone;
        private System.Windows.Forms.PictureBox picPlayerPortrait;
        private System.Windows.Forms.PictureBox picEnemyPortrait;
        
        private System.Windows.Forms.TableLayoutPanel pnlBottomZone;
        private System.Windows.Forms.FlowLayoutPanel flpPlayerCards;
        private System.Windows.Forms.PictureBox picPlayerCard1;
        private System.Windows.Forms.PictureBox picPlayerCard2;
        private System.Windows.Forms.PictureBox picPlayerCard3;
        private System.Windows.Forms.PictureBox picPlayerCard4;
        private System.Windows.Forms.PictureBox picPlayerCard5;
        private System.Windows.Forms.FlowLayoutPanel flpEnemyCards;
        private System.Windows.Forms.PictureBox picEnemyCard1;
        private System.Windows.Forms.PictureBox picEnemyCard2;
        private System.Windows.Forms.PictureBox picEnemyCard3;
        private System.Windows.Forms.PictureBox picEnemyCard4;
        private System.Windows.Forms.PictureBox picEnemyCard5;
        private System.Windows.Forms.Panel pnlMainContent;
    }
}

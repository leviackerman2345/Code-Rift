namespace CodeRift.Forms
{
    partial class BattleArenaQuestionForm
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
            this.btnBack = new System.Windows.Forms.Button();
            this.lblQuestionCounter = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lblQuestionTag = new System.Windows.Forms.Label();
            this.lblQuestion = new System.Windows.Forms.RichTextBox();
            this.pnlAnswerZone = new System.Windows.Forms.Panel();
            this.pnlCodeInput = new System.Windows.Forms.Panel();
            this.txtCodeInput = new System.Windows.Forms.RichTextBox();
            this.lblCodeTag = new System.Windows.Forms.Label();
            this.pnlMultiChoice = new System.Windows.Forms.Panel();
            this.btnOptionD = new System.Windows.Forms.Button();
            this.btnOptionC = new System.Windows.Forms.Button();
            this.btnOptionB = new System.Windows.Forms.Button();
            this.btnOptionA = new System.Windows.Forms.Button();
            this.lblMCTag = new System.Windows.Forms.Label();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblLineNumbers = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblSystemId = new System.Windows.Forms.Label();
            this.pnlQuestion = new System.Windows.Forms.Panel();
            this.pnlBottomBar = new System.Windows.Forms.Panel();
            this.pnlMainLayout = new System.Windows.Forms.Panel();
            this.pnlContentFrame = new System.Windows.Forms.Panel();
            this.pnlAnswerZone.SuspendLayout();
            this.pnlCodeInput.SuspendLayout();
            this.pnlMultiChoice.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.pnlTopBar.SuspendLayout();
            this.pnlQuestion.SuspendLayout();
            this.pnlBottomBar.SuspendLayout();
            this.pnlMainLayout.SuspendLayout();
            this.pnlContentFrame.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnBack.Location = new System.Drawing.Point(0, 0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(150, 44);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "[ESC]BACK";
            this.btnBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblQuestionCounter
            // 
            this.lblQuestionCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQuestionCounter.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold);
            this.lblQuestionCounter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblQuestionCounter.Location = new System.Drawing.Point(150, 0);
            this.lblQuestionCounter.Name = "lblQuestionCounter";
            this.lblQuestionCounter.Size = new System.Drawing.Size(700, 44);
            this.lblQuestionCounter.TabIndex = 1;
            this.lblQuestionCounter.Text = "/// TASK_CMD: 01_OF_05 ///";
            this.lblQuestionCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimer
            // 
            this.lblTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimer.Font = new System.Drawing.Font("Courier New", 24F, System.Drawing.FontStyle.Bold);
            this.lblTimer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblTimer.Location = new System.Drawing.Point(620, 115);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(210, 40);
            this.lblTimer.TabIndex = 2;
            this.lblTimer.Text = "00:00:00";
            this.lblTimer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblQuestionTag
            // 
            this.lblQuestionTag.AutoSize = true;
            this.lblQuestionTag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(8)))));
            this.lblQuestionTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblQuestionTag.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Bold);
            this.lblQuestionTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblQuestionTag.Location = new System.Drawing.Point(20, -1);
            this.lblQuestionTag.Name = "lblQuestionTag";
            this.lblQuestionTag.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.lblQuestionTag.Size = new System.Drawing.Size(162, 22);
            this.lblQuestionTag.TabIndex = 3;
            this.lblQuestionTag.Text = "PROBLEM_STATEMENT";
            // 
            // lblQuestion
            // 
            this.lblQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuestion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(8)))));
            this.lblQuestion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblQuestion.Font = new System.Drawing.Font("Courier New", 20F, System.Drawing.FontStyle.Bold);
            this.lblQuestion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblQuestion.Location = new System.Drawing.Point(20, 40);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.ReadOnly = true;
            this.lblQuestion.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.lblQuestion.Size = new System.Drawing.Size(810, 80);
            this.lblQuestion.TabIndex = 4;
            this.lblQuestion.Text = "Question text will appear here...";
            // 
            // pnlAnswerZone
            // 
            this.pnlAnswerZone.Controls.Add(this.pnlMultiChoice);
            this.pnlAnswerZone.Controls.Add(this.pnlCodeInput);
            this.pnlAnswerZone.Controls.Add(this.pnlSidebar);
            this.pnlAnswerZone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAnswerZone.Location = new System.Drawing.Point(0, 170);
            this.pnlAnswerZone.Name = "pnlAnswerZone";
            this.pnlAnswerZone.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.pnlAnswerZone.Size = new System.Drawing.Size(850, 326);
            this.pnlAnswerZone.TabIndex = 5;
            // 
            // pnlCodeInput
            // 
            this.pnlCodeInput.Controls.Add(this.txtCodeInput);
            this.pnlCodeInput.Controls.Add(this.lblCodeTag);
            this.pnlCodeInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCodeInput.Location = new System.Drawing.Point(60, 30);
            this.pnlCodeInput.Name = "pnlCodeInput";
            this.pnlCodeInput.Size = new System.Drawing.Size(790, 296);
            this.pnlCodeInput.TabIndex = 0;
            // 
            // txtCodeInput
            // 
            this.txtCodeInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCodeInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(8)))));
            this.txtCodeInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCodeInput.Font = new System.Drawing.Font("Courier New", 16F);
            this.txtCodeInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.txtCodeInput.Location = new System.Drawing.Point(10, 40);
            this.txtCodeInput.Name = "txtCodeInput";
            this.txtCodeInput.Size = new System.Drawing.Size(770, 246);
            this.txtCodeInput.TabIndex = 1;
            this.txtCodeInput.Text = "type here..";
            // 
            // lblCodeTag
            // 
            this.lblCodeTag.AutoSize = true;
            this.lblCodeTag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(8)))));
            this.lblCodeTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCodeTag.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Bold);
            this.lblCodeTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblCodeTag.Location = new System.Drawing.Point(0, -1);
            this.lblCodeTag.Name = "lblCodeTag";
            this.lblCodeTag.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.lblCodeTag.Size = new System.Drawing.Size(114, 22);
            this.lblCodeTag.TabIndex = 0;
            this.lblCodeTag.Text = "USER_INPUT";
            // 
            // pnlMultiChoice
            // 
            this.pnlMultiChoice.Controls.Add(this.btnOptionD);
            this.pnlMultiChoice.Controls.Add(this.btnOptionC);
            this.pnlMultiChoice.Controls.Add(this.btnOptionB);
            this.pnlMultiChoice.Controls.Add(this.btnOptionA);
            this.pnlMultiChoice.Controls.Add(this.lblMCTag);
            this.pnlMultiChoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMultiChoice.Location = new System.Drawing.Point(60, 30);
            this.pnlMultiChoice.Name = "pnlMultiChoice";
            this.pnlMultiChoice.Size = new System.Drawing.Size(790, 296);
            this.pnlMultiChoice.TabIndex = 1;
            this.pnlMultiChoice.Visible = false;
            // 
            // btnOptionD
            // 
            this.btnOptionD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptionD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(5)))));
            this.btnOptionD.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnOptionD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptionD.Font = new System.Drawing.Font("Courier New", 16F, System.Drawing.FontStyle.Bold);
            this.btnOptionD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnOptionD.Location = new System.Drawing.Point(0, 220);
            this.btnOptionD.Name = "btnOptionD";
            this.btnOptionD.Size = new System.Drawing.Size(790, 50);
            this.btnOptionD.TabIndex = 4;
            this.btnOptionD.Text = "[ D ]  option text here";
            this.btnOptionD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOptionD.UseVisualStyleBackColor = false;
            this.btnOptionD.MouseEnter += new System.EventHandler(this.InvertedButton_MouseEnter);
            this.btnOptionD.MouseLeave += new System.EventHandler(this.InvertedButton_MouseLeave);
            // 
            // btnOptionC
            // 
            this.btnOptionC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptionC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(5)))));
            this.btnOptionC.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnOptionC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptionC.Font = new System.Drawing.Font("Courier New", 16F, System.Drawing.FontStyle.Bold);
            this.btnOptionC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnOptionC.Location = new System.Drawing.Point(0, 160);
            this.btnOptionC.Name = "btnOptionC";
            this.btnOptionC.Size = new System.Drawing.Size(790, 50);
            this.btnOptionC.TabIndex = 3;
            this.btnOptionC.Text = "[ C ]  option text here";
            this.btnOptionC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOptionC.UseVisualStyleBackColor = false;
            this.btnOptionC.MouseEnter += new System.EventHandler(this.InvertedButton_MouseEnter);
            this.btnOptionC.MouseLeave += new System.EventHandler(this.InvertedButton_MouseLeave);
            // 
            // btnOptionB
            // 
            this.btnOptionB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptionB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(5)))));
            this.btnOptionB.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnOptionB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptionB.Font = new System.Drawing.Font("Courier New", 16F, System.Drawing.FontStyle.Bold);
            this.btnOptionB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnOptionB.Location = new System.Drawing.Point(0, 100);
            this.btnOptionB.Name = "btnOptionB";
            this.btnOptionB.Size = new System.Drawing.Size(790, 50);
            this.btnOptionB.TabIndex = 2;
            this.btnOptionB.Text = "[ B ]  option text here";
            this.btnOptionB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOptionB.UseVisualStyleBackColor = false;
            this.btnOptionB.MouseEnter += new System.EventHandler(this.InvertedButton_MouseEnter);
            this.btnOptionB.MouseLeave += new System.EventHandler(this.InvertedButton_MouseLeave);
            // 
            // btnOptionA
            // 
            this.btnOptionA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptionA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(5)))));
            this.btnOptionA.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnOptionA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptionA.Font = new System.Drawing.Font("Courier New", 16F, System.Drawing.FontStyle.Bold);
            this.btnOptionA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnOptionA.Location = new System.Drawing.Point(0, 40);
            this.btnOptionA.Name = "btnOptionA";
            this.btnOptionA.Size = new System.Drawing.Size(790, 50);
            this.btnOptionA.TabIndex = 1;
            this.btnOptionA.Text = "[ A ]  option text here";
            this.btnOptionA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOptionA.UseVisualStyleBackColor = false;
            this.btnOptionA.MouseEnter += new System.EventHandler(this.InvertedButton_MouseEnter);
            this.btnOptionA.MouseLeave += new System.EventHandler(this.InvertedButton_MouseLeave);
            // 
            // lblMCTag
            // 
            this.lblMCTag.AutoSize = true;
            this.lblMCTag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(8)))));
            this.lblMCTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMCTag.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Bold);
            this.lblMCTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblMCTag.Location = new System.Drawing.Point(0, -1);
            this.lblMCTag.Name = "lblMCTag";
            this.lblMCTag.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.lblMCTag.Size = new System.Drawing.Size(146, 22);
            this.lblMCTag.TabIndex = 0;
            this.lblMCTag.Text = "SELECT_COMMAND";
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.Controls.Add(this.lblLineNumbers);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 30);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(60, 296);
            this.pnlSidebar.TabIndex = 2;
            // 
            // lblLineNumbers
            // 
            this.lblLineNumbers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLineNumbers.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this.lblLineNumbers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(74)))), ((int)(((byte)(26)))));
            this.lblLineNumbers.Location = new System.Drawing.Point(0, 0);
            this.lblLineNumbers.Name = "lblLineNumbers";
            this.lblLineNumbers.Padding = new System.Windows.Forms.Padding(0, 40, 0, 0);
            this.lblLineNumbers.Size = new System.Drawing.Size(60, 296);
            this.lblLineNumbers.TabIndex = 0;
            this.lblLineNumbers.Text = "01\r\n02\r\n03\r\n04\r\n05\r\n06\r\n07\r\n08\r\n09\r\n10";
            this.lblLineNumbers.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblHint
            // 
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblHint.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.lblHint.Location = new System.Drawing.Point(20, 10);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(500, 40);
            this.lblHint.TabIndex = 6;
            this.lblHint.Text = "> Or press CODE-? for help";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(8)))));
            this.btnSubmit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSubmit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Courier New", 16F, System.Drawing.FontStyle.Bold);
            this.btnSubmit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(65)))));
            this.btnSubmit.Location = new System.Drawing.Point(610, 10);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(220, 40);
            this.btnSubmit.TabIndex = 7;
            this.btnSubmit.Text = "CONFIRM_RUN";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnSubmit.MouseEnter += new System.EventHandler(this.InvertedButton_MouseEnter);
            this.btnSubmit.MouseLeave += new System.EventHandler(this.InvertedButton_MouseLeave);
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.Controls.Add(this.lblQuestionCounter);
            this.pnlTopBar.Controls.Add(this.btnBack);
            this.pnlTopBar.Controls.Add(this.lblSystemId);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(60, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(850, 44);
            this.pnlTopBar.TabIndex = 8;
            this.pnlTopBar.Paint += new System.Windows.Forms.PaintEventHandler(this.TopBar_Paint);
            // 
            // lblSystemId
            // 
            this.lblSystemId.AutoSize = true;
            this.lblSystemId.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Bold);
            this.lblSystemId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(74)))), ((int)(((byte)(26)))));
            this.lblSystemId.Location = new System.Drawing.Point(0, 44);
            this.lblSystemId.Name = "lblSystemId";
            this.lblSystemId.Size = new System.Drawing.Size(136, 16);
            this.lblSystemId.TabIndex = 3;
            this.lblSystemId.Text = "SYS_ID: CR-098-X";
            // 
            // pnlQuestion
            // 
            this.pnlQuestion.Controls.Add(this.lblTimer);
            this.pnlQuestion.Controls.Add(this.lblQuestion);
            this.pnlQuestion.Controls.Add(this.lblQuestionTag);
            this.pnlQuestion.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuestion.Location = new System.Drawing.Point(0, 0);
            this.pnlQuestion.Name = "pnlQuestion";
            this.pnlQuestion.Padding = new System.Windows.Forms.Padding(0, 15, 0, 15);
            this.pnlQuestion.Size = new System.Drawing.Size(850, 170);
            this.pnlQuestion.TabIndex = 9;
            // 
            // pnlBottomBar
            // 
            this.pnlBottomBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(8)))));
            this.pnlBottomBar.Controls.Add(this.btnSubmit);
            this.pnlBottomBar.Controls.Add(this.lblHint);
            this.pnlBottomBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomBar.Location = new System.Drawing.Point(60, 540);
            this.pnlBottomBar.Name = "pnlBottomBar";
            this.pnlBottomBar.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlBottomBar.Size = new System.Drawing.Size(850, 60);
            this.pnlBottomBar.TabIndex = 10;
            // 
            // pnlMainLayout
            // 
            this.pnlMainLayout.Controls.Add(this.pnlContentFrame);
            this.pnlMainLayout.Controls.Add(this.pnlTopBar);
            this.pnlMainLayout.Controls.Add(this.pnlBottomBar);
            this.pnlMainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlMainLayout.Name = "pnlMainLayout";
            this.pnlMainLayout.Padding = new System.Windows.Forms.Padding(60, 0, 60, 0);
            this.pnlMainLayout.Size = new System.Drawing.Size(970, 600);
            this.pnlMainLayout.TabIndex = 11;
            // 
            // pnlContentFrame
            // 
            this.pnlContentFrame.Controls.Add(this.pnlAnswerZone);
            this.pnlContentFrame.Controls.Add(this.pnlQuestion);
            this.pnlContentFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContentFrame.Location = new System.Drawing.Point(60, 44);
            this.pnlContentFrame.Name = "pnlContentFrame";
            this.pnlContentFrame.Size = new System.Drawing.Size(850, 496);
            this.pnlContentFrame.TabIndex = 12;
            this.pnlContentFrame.Paint += new System.Windows.Forms.PaintEventHandler(this.DoubleLine_Paint);
            // 
            // BattleArenaQuestionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(8)))));
            this.ClientSize = new System.Drawing.Size(970, 600);
            this.Controls.Add(this.pnlMainLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BattleArenaQuestionForm";
            this.Text = "BattleArenaQuestionForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlAnswerZone.ResumeLayout(false);
            this.pnlCodeInput.ResumeLayout(false);
            this.pnlCodeInput.PerformLayout();
            this.pnlMultiChoice.ResumeLayout(false);
            this.pnlMultiChoice.PerformLayout();
            this.pnlSidebar.ResumeLayout(false);
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.pnlQuestion.ResumeLayout(false);
            this.pnlQuestion.PerformLayout();
            this.pnlBottomBar.ResumeLayout(false);
            this.pnlMainLayout.ResumeLayout(false);
            this.pnlContentFrame.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblQuestionCounter;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblQuestionTag;
        private System.Windows.Forms.RichTextBox lblQuestion;
        private System.Windows.Forms.Panel pnlAnswerZone;
        private System.Windows.Forms.Panel pnlCodeInput;
        private System.Windows.Forms.Label lblCodeTag;
        private System.Windows.Forms.RichTextBox txtCodeInput;
        private System.Windows.Forms.Panel pnlMultiChoice;
        private System.Windows.Forms.Label lblMCTag;
        private System.Windows.Forms.Button btnOptionA;
        private System.Windows.Forms.Button btnOptionB;
        private System.Windows.Forms.Button btnOptionC;
        private System.Windows.Forms.Button btnOptionD;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Panel pnlQuestion;
        private System.Windows.Forms.Panel pnlBottomBar;
        private System.Windows.Forms.Panel pnlMainLayout;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLineNumbers;
        private System.Windows.Forms.Label lblSystemId;
        private System.Windows.Forms.Panel pnlContentFrame;
    }
}

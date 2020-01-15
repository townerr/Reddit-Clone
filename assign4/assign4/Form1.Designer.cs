namespace assign4
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.redditLogoPictureBox = new System.Windows.Forms.PictureBox();
            this.subredditsComboBox = new System.Windows.Forms.ComboBox();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.postPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.loginButton = new System.Windows.Forms.PictureBox();
            this.userName = new System.Windows.Forms.Label();
            this.commentScore = new System.Windows.Forms.Label();
            this.postScore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.redditLogoPictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loginButton)).BeginInit();
            this.SuspendLayout();
            // 
            // redditLogoPictureBox
            // 
            this.redditLogoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("redditLogoPictureBox.Image")));
            this.redditLogoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.redditLogoPictureBox.Name = "redditLogoPictureBox";
            this.redditLogoPictureBox.Size = new System.Drawing.Size(112, 45);
            this.redditLogoPictureBox.TabIndex = 0;
            this.redditLogoPictureBox.TabStop = false;
            // 
            // subredditsComboBox
            // 
            this.subredditsComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.subredditsComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.subredditsComboBox.ForeColor = System.Drawing.Color.White;
            this.subredditsComboBox.FormattingEnabled = true;
            this.subredditsComboBox.Items.AddRange(new object[] {
            "Home"});
            this.subredditsComboBox.Location = new System.Drawing.Point(118, 12);
            this.subredditsComboBox.Name = "subredditsComboBox";
            this.subredditsComboBox.Size = new System.Drawing.Size(247, 21);
            this.subredditsComboBox.TabIndex = 1;
            this.subredditsComboBox.SelectedIndexChanged += new System.EventHandler(this.subredditsComboBox_SelectedIndexChanged);
            // 
            // searchBox
            // 
            this.searchBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.searchBox.ForeColor = System.Drawing.Color.White;
            this.searchBox.Location = new System.Drawing.Point(371, 12);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(406, 20);
            this.searchBox.TabIndex = 2;
            this.searchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchBox_KeyDown);
            // 
            // postPanel
            // 
            this.postPanel.AutoScroll = true;
            this.postPanel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.postPanel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.postPanel.Location = new System.Drawing.Point(107, 60);
            this.postPanel.Name = "postPanel";
            this.postPanel.Size = new System.Drawing.Size(829, 543);
            this.postPanel.TabIndex = 3;
            this.postPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.PostPanel_Paint);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.loginButton);
            this.panel1.Controls.Add(this.userName);
            this.panel1.Controls.Add(this.commentScore);
            this.panel1.Controls.Add(this.postScore);
            this.panel1.Location = new System.Drawing.Point(108, -2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(953, 45);
            this.panel1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.LightGray;
            this.button1.Location = new System.Drawing.Point(678, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 30);
            this.button1.TabIndex = 9;
            this.button1.Text = "NEW POST";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.createPostButton_Click);
            // 
            // loginButton
            // 
            this.loginButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loginButton.Image = ((System.Drawing.Image)(resources.GetObject("loginButton.Image")));
            this.loginButton.Location = new System.Drawing.Point(790, 2);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(130, 41);
            this.loginButton.TabIndex = 5;
            this.loginButton.TabStop = false;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // userName
            // 
            this.userName.AutoSize = true;
            this.userName.BackColor = System.Drawing.Color.Transparent;
            this.userName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userName.ForeColor = System.Drawing.Color.White;
            this.userName.Location = new System.Drawing.Point(787, 5);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(45, 16);
            this.userName.TabIndex = 6;
            this.userName.Text = "label1";
            // 
            // commentScore
            // 
            this.commentScore.AutoSize = true;
            this.commentScore.BackColor = System.Drawing.Color.Transparent;
            this.commentScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commentScore.ForeColor = System.Drawing.Color.White;
            this.commentScore.Location = new System.Drawing.Point(787, 25);
            this.commentScore.Name = "commentScore";
            this.commentScore.Size = new System.Drawing.Size(41, 15);
            this.commentScore.TabIndex = 8;
            this.commentScore.Text = "label1";
            // 
            // postScore
            // 
            this.postScore.AutoSize = true;
            this.postScore.BackColor = System.Drawing.Color.Transparent;
            this.postScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.postScore.ForeColor = System.Drawing.Color.White;
            this.postScore.Location = new System.Drawing.Point(867, 25);
            this.postScore.Name = "postScore";
            this.postScore.Size = new System.Drawing.Size(41, 15);
            this.postScore.TabIndex = 7;
            this.postScore.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1056, 682);
            this.Controls.Add(this.postPanel);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.subredditsComboBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.redditLogoPictureBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "Welcome to Reddit";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.redditLogoPictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loginButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox redditLogoPictureBox;
        private System.Windows.Forms.ComboBox subredditsComboBox;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Panel postPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox loginButton;
        private System.Windows.Forms.Label userName;
        private System.Windows.Forms.Label commentScore;
        private System.Windows.Forms.Label postScore;
        private System.Windows.Forms.Button button1;
    }
}
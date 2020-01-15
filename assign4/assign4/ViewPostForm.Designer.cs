namespace assign4
{
    partial class ViewPostForm
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
            this.postPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // postPanel
            // 
            this.postPanel.AutoScroll = true;
            this.postPanel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.postPanel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.postPanel.Location = new System.Drawing.Point(12, 12);
            this.postPanel.Name = "postPanel";
            this.postPanel.Size = new System.Drawing.Size(831, 552);
            this.postPanel.TabIndex = 4;
            // 
            // ViewPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(855, 682);
            this.Controls.Add(this.postPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ViewPostForm";
            this.Text = "Post Content";
            this.Load += new System.EventHandler(this.ViewPostForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel postPanel;
    }
}
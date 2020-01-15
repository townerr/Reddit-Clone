using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assign4
{
    public partial class CreatePost : Form
    {
        public CreatePost()
        {
            InitializeComponent();
        }

        private Form1 f1 = null;

        public CreatePost(Form form1)
        {
            f1 = form1 as Form1;
            InitializeComponent();
        }

        private void CreatePost_Load(object sender, EventArgs e)
        {
            //load combo
            foreach (Subreddit sub in Program.subreddits)
            {
                subsCombo.Items.Add(sub);
            }
            subsCombo.Items.RemoveAt(0); //remove "all"
            subsCombo.SelectedIndex = 0; //set default to top
        }

        // Create post
        private void Button1_Click(object sender, EventArgs e)
        {
            Subreddit sub = subsCombo.SelectedItem as Subreddit;

            if(titleText.Text != null || titleText.Text != "")
            {
                if (postText.Text != null || postText.Text != "")
                {
                    f1.CreatePost(sub, titleText.Text, postText.Text);
                    MessageBox.Show("Post Created Successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Post Contents.");
                }
            }
            else
            {
                MessageBox.Show("Invalid Post Title.");
            }

        }
    }
}

/***********************************************************
CSCI 473 - Assignment 4 - Fall 2019

Programmer: Ryan Towner
Programmer: Matthew Beardsley

Date Due: 10/31/19

Purpose: Better Reddit Clone
************************************************************/

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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Program.ReadInputFiles();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //fill subreddit listbox
            foreach (Subreddit sub in Program.subreddits)
            {
                subredditsComboBox.Items.Add(sub);
            }
            subredditsComboBox.SelectedIndex = 0;
        }

        private void subredditsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Subreddit curSubreddit = subredditsComboBox.SelectedItem as Subreddit;

            Program.displayPosts.Clear();
            postPanel.Controls.Clear();


            // Displays all posts when "all" or home is selected
            if (subredditsComboBox.SelectedIndex == 1 || subredditsComboBox.SelectedIndex == 0) // "all" or home was selected
            {
                foreach (Subreddit s in Program.subreddits)
                {
                    //numMembersLabel.Text = "";
                    //numActiveLabel.Text = "";

                    foreach (Post p in s)
                    {
                        DisplayPost dp = new DisplayPost(p, 0);
                        dp.PostPanel.Location = new Point(0, Program.displayPosts.Count * DisplayPost.TYPE0_HEIGHT);

                        Program.displayPosts.Add(dp);
                        postPanel.Controls.Add(dp.PostPanel);
                    }
                }
            } // Displays all of the selected subreddits posts
            else if (curSubreddit != null)
            {

                foreach (Post p in curSubreddit)
                {
                    DisplayPost dp = new DisplayPost(p, 0);
                    dp.PostPanel.Location = new Point(0, Program.displayPosts.Count * DisplayPost.TYPE0_HEIGHT);

                    Program.displayPosts.Add(dp);
                    postPanel.Controls.Add(dp.PostPanel);
                    
                }
            }

        }

        private void PostPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm(this);
            login.Show();
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            //get search phrase
            string searchphrase = searchBox.Text;

            //If enter is pressed filter the posts with search phrase
            if(e.KeyCode == Keys.Enter)
            {
                Program.displayPosts.Clear();
                postPanel.Controls.Clear();

                foreach (Subreddit s in Program.subreddits)
                {
                    foreach (Post p in s)
                    {
                        if(p.Title.Contains(searchphrase) == true || p.PostContent.Contains(searchphrase) == true)
                        {
                            DisplayPost dp = new DisplayPost(p, 0);
                            dp.PostPanel.Location = new Point(0, Program.displayPosts.Count * DisplayPost.TYPE0_HEIGHT);

                            Program.displayPosts.Add(dp);
                            postPanel.Controls.Add(dp.PostPanel);
                        }
                    }
                }
            }
        }
        public void LoginSuccess(User user)
        {
            //update login area top right
            loginButton.Visible = false;
            loginButton.Enabled = false;

            userName.Text = user.Name;
            postScore.Text = "PS: " + user.PostScore.ToString();
            commentScore.Text = "CS: " + user.CommentScore.ToString();

        }

        private void createPostButton_Click(object sender, EventArgs e)
        {
            CreatePost createpost = new CreatePost(this);
            createpost.Show();
        }

        public void CreatePost(Subreddit sub, string title, string content)
        {
            if (Program.loggedInUserID == -1)
            {
                MessageBox.Show("Must be logged in to create a post.");
                return;
            }

            try // Check for foul language
            {
                Program.CheckFoulLanguage(content);
                Program.CheckFoulLanguage(title);
            }
            catch (Program.FoulLanguageException except) // Caught foul language
            {
                MessageBox.Show("Caught a FoulLanguageException: " + except);
                return;
            }

            DateTime timestamp = DateTime.Now;
            Post newPost = new Post(0, Program.GenerateID(), sub.ID, (uint)Program.loggedInUserID, title, content, timestamp, 0, 0, 0, 0, 0, 0);
            Program.idDictionary.Add(newPost.ID, newPost);
            Subreddit postSubreddit = Program.idDictionary[newPost.SubHome] as Subreddit;
            postSubreddit.addPost(newPost);

            //append to file new comment
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"..\\..\\..\\posts.txt", true))
            {
                file.WriteLine(
                    "0" + "\t" +
                    newPost.ID + "\t" +
                    Program.loggedInUserID + "\t" +
                    newPost.Title + "\t" +
                    newPost.PostContent + "\t" +
                    newPost.SubHome + "\t" +
                    newPost.UpVotes + "\t" +
                    newPost.DownVotes + "\t" +
                    newPost.Weight + "\t" +
                    newPost.TimeStamp.Year + "\t" +
                    newPost.TimeStamp.Month + "\t" +
                    newPost.TimeStamp.Day + "\t" +
                    newPost.TimeStamp.Hour + "\t" +
                    newPost.TimeStamp.Minute + "\t" +
                    newPost.TimeStamp.Second + "\t" +
                    newPost.Silver + "\t" +
                    newPost.Gold + "\t" +
                    newPost.Platinum);
            }

            //refresh posts
            Program.displayPosts.Clear();
            postPanel.Controls.Clear();

            subredditsComboBox.SelectedIndex = 0;

            foreach (Subreddit s in Program.subreddits)
            {
                foreach (Post p in s)
                { 
                    DisplayPost dp = new DisplayPost(p, 0);
                    dp.PostPanel.Location = new Point(0, Program.displayPosts.Count * DisplayPost.TYPE0_HEIGHT);

                    Program.displayPosts.Add(dp);
                    postPanel.Controls.Add(dp.PostPanel);
                }
            }
        }
    }
}

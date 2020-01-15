/***********************************************************
CSCI 473 - Assignment 4 - Fall 2019

Programmer: Ryan Towner
Programmer: Matthew Beardsley

Date Due: 10/31/19


Purpose: This class represents a Post that is displayed on our
main form's Panel. It is also used when displaying an individual post
************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace assign4
{
    public class DisplayPost
    {
        public const int BUFFER = 5;

        public const int WIDTH = 800;
        public const int TYPE0_HEIGHT = 100;
        public const int TYPE1_HEIGHT = 125;
        public const int INFOLABEL_HEIGHT = 13;
        public const int VOTEBUTTONS_OFFSET_TOP = 7;
        public const int VOTEBUTTONS_OFFSET_LEFT = 8;
        public const int VOTEBUTTONS_WIDTH = 25;
        public const int VOTEBUTTONS_HEIGHT = 25;
        public const int SCORE_HEIGHT = 12;
        public const int SCORE_WIDTH = 40;
        public const int CONTENT_HEIGHT = 26;
        public const int TITLE_HEIGHT = 40;

        // Type 1 is normal DisplayPost for browsing posts
        // Type 2 displays the post content as well
        private int type;

        private ViewPostForm form;

        private bool upVoted = false;
        private bool downVoted = false;

        private Post post;

        private Panel postPanel;
        private Label postInfoLabel;
        private Button upVote;
        private Label score;
        private Button downVote;
        private Label title;
        private RichTextBox content;
        private PictureBox commentIcon;
        private Label commentLabel;
        private RichTextBox commentBox;
        private PictureBox commentSubmit;

        // Constructor 
        public DisplayPost(Post p, int t)
        {
            post = p;
            type = t;

            // Construct panel
            postPanel = new Panel();
            postPanel.BackColor = Color.FromArgb(21, 21, 21);
            if (type == 0)
                postPanel.Size = new Size(WIDTH, TYPE0_HEIGHT);
            else if (Program.loggedInUserID == -1) // type == 1
                postPanel.Size = new Size(WIDTH, TYPE1_HEIGHT);
            else
                postPanel.Size = new Size(WIDTH, TYPE1_HEIGHT + 60);
            postPanel.Paint += new PaintEventHandler(drawBorder);
            postPanel.MouseClick += new MouseEventHandler(openPost_MouseClick);

            // Construct upVote Button
            upVote = new Button();
            upVote.TabStop = false;
            upVote.FlatStyle = FlatStyle.Flat;
            upVote.FlatAppearance.BorderSize = 0;
            upVote.Size = new Size(VOTEBUTTONS_WIDTH, VOTEBUTTONS_HEIGHT);
            upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_grey.png");
            upVote.Location = new Point(VOTEBUTTONS_OFFSET_LEFT, VOTEBUTTONS_OFFSET_TOP);
            upVote.MouseEnter += new EventHandler(upVote_MouseEnter);
            upVote.MouseLeave += new EventHandler(upVote_MouseLeave);
            upVote.MouseClick += new MouseEventHandler(upVote_MouseClick);

            // Construct score Label
            score = new Label();
            score.Size = new Size(SCORE_WIDTH, SCORE_HEIGHT);
            score.TextAlign = ContentAlignment.MiddleCenter;
            score.Location = new Point(1, upVote.Bottom + BUFFER);
            score.ForeColor = Color.White;
            score.Text = Convert.ToString(p.Score);

            // Constrcut downVote Button
            downVote = new Button();
            downVote.TabStop = false;
            downVote.FlatStyle = FlatStyle.Flat;
            downVote.FlatAppearance.BorderSize = 0;
            downVote.Size = new Size(VOTEBUTTONS_WIDTH, VOTEBUTTONS_HEIGHT);
            downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_grey.png");
            downVote.Location = new Point(VOTEBUTTONS_OFFSET_LEFT, score.Bottom + BUFFER);
            downVote.MouseEnter += new EventHandler(downVote_MouseEnter);
            downVote.MouseLeave += new EventHandler(downVote_MouseLeave);
            downVote.MouseClick += new MouseEventHandler(downVote_MouseClick);

            // Update UpVote and DownVote buttons
            updateVoteButtons();

            // Construct postInfoLabel 
            postInfoLabel = new Label();
            postInfoLabel.Location = new Point(BUFFER + upVote.Width, 1);
            postInfoLabel.Size = new Size(300, INFOLABEL_HEIGHT);
            postInfoLabel.ForeColor = Color.White;
            postInfoLabel.Text = p.ToString("DisplayPost");
            postInfoLabel.MouseClick += new MouseEventHandler(openPost_MouseClick);
            
            // Construct title Label
            title = new Label();
            title.Location = new Point(score.Width, postInfoLabel.Bottom + BUFFER);
            title.Size = new Size(WIDTH - SCORE_WIDTH - BUFFER * 2, TITLE_HEIGHT);
            title.Font = new Font(title.Font.FontFamily, 12);
            title.ForeColor = Color.White;
            title.Text = p.Title;
            title.MouseClick += new MouseEventHandler(openPost_MouseClick);

            // Only for ViewPostForm
            if (type == 1)
            {   // Construct content
                content = new RichTextBox();
                content.ReadOnly = true;
                content.Location = new Point(SCORE_WIDTH + 1, title.Bottom);
                content.Size = new Size(WIDTH - SCORE_WIDTH - 2, CONTENT_HEIGHT);
                content.BackColor = Color.FromArgb(21, 21, 21);
                content.ForeColor = Color.White;
                content.BorderStyle = BorderStyle.None;
                content.Text = p.PostContent;
            }

            int offset = type * CONTENT_HEIGHT; // 0 if form type 0, otherwise offsets for the content box

            // Constructs comment icon
            commentIcon = new PictureBox();
            commentIcon.Size = new Size(23, 21);
            commentIcon.Image = Bitmap.FromFile("..\\..\\images\\comment_icon.png");
            commentIcon.Location = new Point(title.Left, title.Bottom + BUFFER + offset);
            commentIcon.MouseClick += new MouseEventHandler(openPost_MouseClick);
            
            // Constructs comment label
            commentLabel = new Label();
            commentLabel.Location = new Point(commentIcon.Right, title.Bottom + BUFFER * 2 + offset);
            commentLabel.ForeColor = Color.White;
            int total = 0;
            foreach (Comment c in post) // Counts total # of comments and replies
            { 
                total++;
                total += c.CommentReplies.Count;
            }
            commentLabel.Text = total.ToString() + " comment";
            if (total != 1)
                commentLabel.Text += "s";
            commentLabel.MouseClick += new MouseEventHandler(openPost_MouseClick);

            // Construct commentBox
            if (type == 1 && Program.loggedInUserID != -1)
            {
                commentBox = new RichTextBox();
                commentBox.Size = new Size(WIDTH - SCORE_WIDTH * 2 - 2, CONTENT_HEIGHT);
                commentBox.Location = new Point(commentIcon.Left, commentIcon.Bottom + 10);
                commentBox.BackColor = Color.FromArgb(21, 21, 21);
                commentBox.ForeColor = Color.White;
                commentBox.BorderStyle = BorderStyle.FixedSingle;
              
                commentSubmit = new PictureBox();
                commentSubmit.Image = Image.FromFile("..\\..\\images\\comment_button.png");
                commentSubmit.Size = new Size(94, 30);
                commentSubmit.Location = new Point(commentBox.Right - commentSubmit.Width, commentBox.Bottom);
                commentSubmit.MouseEnter += new EventHandler(commentSubmit_MouseEnter);
                commentSubmit.MouseLeave += new EventHandler(commentSubmit_MouseLeave);
                commentSubmit.MouseClick += new MouseEventHandler(commentSubmit_MouseClick);
            }

            // Add all controls to panel
            postPanel.Controls.Add(postInfoLabel);
            postPanel.Controls.Add(upVote);
            postPanel.Controls.Add(score);
            postPanel.Controls.Add(downVote);
            postPanel.Controls.Add(title);
            if (type == 1) // Only for ViewPostForm
                postPanel.Controls.Add(content);
            postPanel.Controls.Add(commentIcon);
            postPanel.Controls.Add(commentLabel);
            if (type == 1 && Program.loggedInUserID != -1)
            {   // Only for ViewPostForm and User is logged in
                postPanel.Controls.Add(commentBox);
                postPanel.Controls.Add(commentSubmit);
            }
        }
     
        // Used to pass postPanel to main panel
        public Panel PostPanel
        {
            get { return postPanel; }
        }

        // Draws white border around object
        void drawBorder(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(new Pen(Color.White), 0, 0, postPanel.Width - 1, postPanel.Height - 1);
        }

        // changes upvote when mouse hovers over button
        void upVote_MouseEnter(object sender, EventArgs e)
        {
            if (upVoted == false)
            {
                upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_red.png");
            }
        }

        // changes back to normal when mouse leaves
        void upVote_MouseLeave(object sender, EventArgs e)
        {
            if (upVoted == false)
            {
                upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_grey.png");
            }
        }

        // Post is upvoted
        void upVote_MouseClick(object sender, EventArgs e)
        {
            // Verify user is logged in
            if (Program.loggedInUserID == -1)
            {
                MessageBox.Show("Must be logged in to vote.");
                return;
            }

            User u = Program.idDictionary[(uint)Program.loggedInUserID] as User;


            // upVote post
            if (upVoted == false)
            {
                post.UpVotes++;
                upVoted = true;
                upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_red.png");

                if (downVoted == true)
                {
                    post.DownVotes--;
                    downVoted = false;
                    downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_grey.png");
                }
            }
            else // upVoted == true
            {    // Post is already upvoted, change to nothing
                post.UpVotes--;
                upVoted = false;
                upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_grey.png");
            }

            // Update text
            score.Text = Convert.ToString(post.Score);

            // Update Users upVoted/downVoted items
            updateUserVotes();
        }

        // Changes downVote button look when mouse is hovering over it
        void downVote_MouseEnter(object sender, EventArgs e)
        {
            if (downVoted == false)
            {
                downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_blue.png");
            }
        }

        // Changes downVote back to normal when mouse leaves
        void downVote_MouseLeave(object sender, EventArgs e)
        {
            if (downVoted == false)
            {
                downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_grey.png");
            }
        }

        // Post is downVoted 
        void downVote_MouseClick(object sender, EventArgs e)
        {
            // Verify user is logged in
            if (Program.loggedInUserID == -1)
            {
                MessageBox.Show("Must be logged in to vote.");
                return;
            }

            if (downVoted == false)
            {
                post.DownVotes++;
                downVoted = true;
                downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_blue.png");

                if (upVoted == true)
                {
                    post.UpVotes--;
                    upVoted = false;
                    upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_grey.png");
                }

            }
            else // downVoted == true
            {
                post.DownVotes--;
                downVoted = false;
                downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_grey.png");
            }

            // Update text
            score.Text = Convert.ToString(post.Score);

            // Update Users upVoted/downVoted items
            updateUserVotes();
        }

        // Opens a post
        void openPost_MouseClick(object sender, EventArgs e)
        {
            if (type == 0)
            {
                ViewPostForm viewPost = new ViewPostForm(post);
                viewPost.Show();
            }
        }

        // Changes commentSubmit look when mouse is hovering it
        void commentSubmit_MouseEnter(object sender, EventArgs e)
        {
            commentSubmit.Image = Image.FromFile("..\\..\\images\\comment_button_hover.png");
        }

        // Changes commentSubmit back to normal when mouse leaves it
        void commentSubmit_MouseLeave(object sender, EventArgs e)
        {
            commentSubmit.Image = Image.FromFile("..\\..\\images\\comment_button.png");
        }

        // Submits comment
        void commentSubmit_MouseClick(object sender, EventArgs e)
        {
            string commentContent = commentBox.Text;

            try // Check for foul language
            {
                Program.CheckFoulLanguage(commentContent);
            }
            catch (Program.FoulLanguageException except) // Caught foul language
            {
               MessageBox.Show("Caught a FoulLanguageException: " + except);
                return;
            }

            if (post.Locked == true) // Post is locked
            {
                MessageBox.Show("Post is marked as 'Locked' -- replies are disabled.");
            }
            else if (commentBox.Text.Equals(""))
            {
                MessageBox.Show("Please enter a comment.");
            }
            else // Add comment to post
            {
                Comment newComment = new Comment(commentContent, (uint)Program.loggedInUserID, post.ID);
                post.addComment(newComment);
                Program.idDictionary.Add(newComment.ID, newComment);
                commentBox.Text = "";
                form.addComment(newComment);
                form.updatePanelLocations();

                //append to file new comment
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"..\\..\\..\\comments.txt", true))
                {
                    file.WriteLine(newComment.ID + "\t" +
                        Program.loggedInUserID + "\t" +
                        newComment.Content + "\t" +
                        newComment.ParentID + "\t" +
                        newComment.UpVotes + "\t" +
                        newComment.DownVotes + "\t" +
                        newComment.TimeStamp.Year + "\t" +
                        newComment.TimeStamp.Month + "\t" +
                        newComment.TimeStamp.Day + "\t" +
                        newComment.TimeStamp.Hour + "\t" +
                        newComment.TimeStamp.Minute + "\t" +
                        newComment.TimeStamp.Second + "\t" +
                        newComment.Silver + "\t" +
                        newComment.Gold + "\t" +
                        newComment.Platinum);
                }
            }
        }

        // Update Users upVoted/downVoted items
        public void updateUserVotes()
        {
            User u = Program.idDictionary[(uint)Program.loggedInUserID] as User;

            if (upVoted == true && u.UpVotedIDs.Contains(post.ID) == false)
                u.UpVotedIDs.Add(post.ID);
            if (downVoted == true && u.DownVotedIDs.Contains(post.ID) == false)
                u.DownVotedIDs.Add(post.ID);
            if (upVoted == false && u.UpVotedIDs.Contains(post.ID) == true)
                u.UpVotedIDs.Remove(post.ID);
            if (downVoted == false && u.UpVotedIDs.Contains(post.ID) == true)
                u.DownVotedIDs.Remove(post.ID);
        }

        public void updateVoteButtons()
        {
            if (Program.loggedInUserID != -1) // User is logged in
            {
                User u = Program.idDictionary[(uint)Program.loggedInUserID] as User;
                if (u.UpVotedIDs.Contains(post.ID) == true) // User already UpVoted content
                {
                    upVoted = true;
                    upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_red.png");
                }
                if (u.DownVotedIDs.Contains(post.ID) == true) // User already DownVoted content
                {
                    downVoted = true;
                    downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_blue.png");
                }
            }
        }

        // References to main form
        public void addFormToRefresh(ViewPostForm f)
        {
            form = f;
        }
    }

}

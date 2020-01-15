/***********************************************************
CSCI 473 - Assignment 4 - Fall 2019

Programmer: Ryan Towner
Programmer: Matthew Beardsley

Date Due: 10/31/19

Purpose: Creates panels that hold either a comment or a reply
for a single post. Also allows for replies to the comment.
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

    public class CommentAndReplyPanel
    {
        // Constants
        public const int BUFFER = 5;
        public const int REPLY_OFFSET = 25;
        public const int WIDTH = 800;
        public const int HEIGHT = 80;
        public const int INFOLABEL_HEIGHT = 13;
        public const int VOTEBUTTON_WIDTH = 25;
        public const int VOTEBUTTON_HEIGHT = 25;
        public const int VOTEBUTTONS_OFFSET_LEFT = 8;
        public const int SCORE_HEIGHT = 12;
        public const int SCORE_WIDTH = 40;
        public const int COMMENT_CONTENT_HEIGHT = 26;
        public const int TITLE_HEIGHT = 40;
        public const int ADDITIONAL_REPLY_SPACE = 70;

        private bool upVoted = false;
        private bool downVoted = false;

        private ViewPostForm form;
        public Comment comment;

        private Panel panel;
        private Label commentInfoLabel;
        private Button upVote;
        private Label score;
        private Button downVote;
        private RichTextBox commentContent;
        private PictureBox replyIcon;
        private RichTextBox replyBox;
        private PictureBox cancelReplyButton;
        private PictureBox replyButton;

        // Constructs panel from comment
        public CommentAndReplyPanel(Comment c)
        {
            comment = c;

            // Offsets replies to the right
            int replyOffset = 0;
            if (c.isReply())
                replyOffset += REPLY_OFFSET;

            // Constructs Panel
            panel = new Panel();
            panel.BackColor = Color.FromArgb(21, 21, 21);
            panel.Size = new Size(WIDTH, HEIGHT);
            panel.Paint += new PaintEventHandler(drawBorder);

            // Constructs upVote button
            upVote = new Button();
            upVote.TabStop = false;
            upVote.FlatStyle = FlatStyle.Flat;
            upVote.FlatAppearance.BorderSize = 0;
            upVote.Size = new Size(VOTEBUTTON_WIDTH, VOTEBUTTON_HEIGHT);
            upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_grey.png");
            upVote.Location = new Point(VOTEBUTTONS_OFFSET_LEFT + replyOffset, 1);
            upVote.MouseEnter += new EventHandler(upVote_MouseEnter);
            upVote.MouseLeave += new EventHandler(upVote_MouseLeave);
            upVote.MouseClick += new MouseEventHandler(upVote_MouseClick);

            // Constructs score label
            score = new Label();
            score.Size = new Size(SCORE_WIDTH, SCORE_HEIGHT);
            score.TextAlign = ContentAlignment.MiddleCenter;
            score.Location = new Point(1 + replyOffset, upVote.Bottom + BUFFER);
            score.ForeColor = Color.White;
            score.Text = Convert.ToString(c.Score);

            // Constructs downVote Button
            downVote = new Button();
            downVote.TabStop = false;
            downVote.FlatStyle = FlatStyle.Flat;
            downVote.FlatAppearance.BorderSize = 0;
            downVote.Size = new Size(VOTEBUTTON_WIDTH, VOTEBUTTON_HEIGHT);
            downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_grey.png");
            downVote.Location = new Point(VOTEBUTTONS_OFFSET_LEFT + replyOffset, score.Bottom + BUFFER);
            downVote.MouseEnter += new EventHandler(downVote_MouseEnter);
            downVote.MouseLeave += new EventHandler(downVote_MouseLeave);
            downVote.MouseClick += new MouseEventHandler(downVote_MouseClick);

            // Update UpVote and DownVote buttons
            updateVoteButtons();

            // constructs commentInfoLabel Label
            commentInfoLabel = new Label();
            commentInfoLabel.Location = new Point(BUFFER + upVote.Right, 1);
            commentInfoLabel.Size = new Size(300, INFOLABEL_HEIGHT);
            commentInfoLabel.ForeColor = Color.White;
            commentInfoLabel.Text = c.ToString("CommentAndReplyPanel");

            // Constrcucts commmentContent rich text box
            commentContent = new RichTextBox();
            commentContent.ReadOnly = true;
            commentContent.BackColor = Color.FromArgb(21, 21, 21);
            commentContent.ForeColor = Color.White;
            commentContent.BorderStyle = BorderStyle.None;
            commentContent.Location = new Point(score.Right, commentInfoLabel.Bottom + BUFFER);
            commentContent.Size = new Size(WIDTH - SCORE_WIDTH - BUFFER * 2 - replyOffset, COMMENT_CONTENT_HEIGHT);
            commentContent.ForeColor = Color.White;
            commentContent.Text = c.Content;

            // Constructs reply icon
            replyIcon = new PictureBox();
            replyIcon.Size = new Size(67, 26);
            replyIcon.Image = Bitmap.FromFile("..\\..\\images\\reply_icon.png");
            replyIcon.Location = new Point(commentContent.Left, commentContent.Bottom + BUFFER);
            replyIcon.MouseEnter += new EventHandler(replyIcon_MouseEnter);
            replyIcon.MouseLeave += new EventHandler(replyIcon_MouseLeave);
            replyIcon.MouseClick += new MouseEventHandler(replyIcon_MouseClick);

            // Constructs reply box
            replyBox = new RichTextBox();
            replyBox.Size = new Size(WIDTH - SCORE_WIDTH * 2 - 2, COMMENT_CONTENT_HEIGHT);
            replyBox.Location = new Point(replyIcon.Left, replyIcon.Bottom + 10);
            replyBox.BackColor = Color.FromArgb(21, 21, 21);
            replyBox.ForeColor = Color.White;
            replyBox.BorderStyle = BorderStyle.FixedSingle;
            replyBox.LostFocus += new EventHandler(replyBox_LostFocus);
            replyBox.Text = "What are your thoughts";

            // Constructs reply button
            replyButton = new PictureBox();
            replyButton.Image = Image.FromFile("..\\..\\images\\reply_button.png");
            replyButton.Size = new Size(94, 30);
            replyButton.Location = new Point(replyBox.Right - replyButton.Width, replyBox.Bottom);
            replyButton.MouseEnter += new EventHandler(replyButton_MouseEnter);
            replyButton.MouseLeave += new EventHandler(replyButton_MouseLeave);
            replyButton.MouseClick += new MouseEventHandler(replyButton_MouseClick);

            // Constructs cancel button
            cancelReplyButton = new PictureBox();
            cancelReplyButton.Image = Image.FromFile("..\\..\\images\\cancel_button.png");
            cancelReplyButton.Size = new Size(89, 30);
            cancelReplyButton.Location = new Point(replyButton.Left - cancelReplyButton.Width, replyBox.Bottom);
            cancelReplyButton.MouseEnter += new EventHandler(cancelReplyButton_MouseEnter);
            cancelReplyButton.MouseLeave += new EventHandler(cancelReplyButton_MouseLeave);
            cancelReplyButton.MouseClick += new MouseEventHandler(cancelReplyButton_MouseClick);

            // Adds all Controls to panel
            panel.Controls.Add(upVote);
            panel.Controls.Add(score);
            panel.Controls.Add(downVote);
            panel.Controls.Add(commentInfoLabel);
            panel.Controls.Add(commentContent);
            panel.Controls.Add(replyIcon);
            panel.Controls.Add(replyBox);
            panel.Controls.Add(replyButton);
            panel.Controls.Add(cancelReplyButton);

        }

        // Returns panel so it can be added to main panel
        public Panel Panel
        {
            get { return panel; }
        }

        // Draws white border around posts and reply box
        void drawBorder(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.White);
            g.DrawRectangle(p, 0, 0, panel.Width - 1, panel.Height - 1);

            if (comment.isReply())
            {
                g.DrawLine(p, 15, 10, 15, 40);
                g.DrawLine(p, 15, 40, 25, 40);
            }
        }

        // Changes upVote picture when moused over
        void upVote_MouseEnter(object sender, EventArgs e)
        {
            if (upVoted == false)
            {
                upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_red.png");
            }
        }

        // Changes upVote picture back when mouse leaves if it has not been selected
        void upVote_MouseLeave(object sender, EventArgs e)
        {
            if (upVoted == false)
            {
                upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_grey.png");
            }
        }

        // Logic for when upVote button is clicked
        void upVote_MouseClick(object sender, EventArgs e)
        {
            // Verify user is logged in
            if (Program.loggedInUserID == -1)
            {
                MessageBox.Show("Must be logged in to vote.");
                return;
            }

            if (upVoted == false)
            {
                comment.UpVotes++;
                upVoted = true;
                upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_red.png");

                if (downVoted == true)
                {
                    comment.DownVotes--;
                    downVoted = false;
                    downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_grey.png");
                }

            }
            else // upVoted == true
            {
                comment.UpVotes--;
                upVoted = false;
                upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_grey.png");
            }

            // Update text
            score.Text = Convert.ToString(comment.Score);

            // Update Users upVoted/downVoted items
            upDateUserVotes();
        }

        // Change picture when downVote is hovered over
        void downVote_MouseEnter(object sender, EventArgs e)
        {
            if (downVoted == false)
            {
                downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_blue.png");
            }
        }

        // Changes downVote picture back when mouse leaves if it has not been selected
        void downVote_MouseLeave(object sender, EventArgs e)
        {
            if (downVoted == false)
            {
                downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_grey.png");
            }
        }

        // Logic for when downVote button is clicked
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
                comment.DownVotes++;
                downVoted = true;
                downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_blue.png");

                if (upVoted == true)
                {
                    comment.UpVotes--;
                    upVoted = false;
                    upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_grey.png");
                }

            }
            else // upVoted == true
            {
                comment.DownVotes--;
                downVoted = false;
                downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_grey.png");
            }

            // Update text
            score.Text = Convert.ToString(comment.Score);

            // Update Users upVoted/downVoted items
            upDateUserVotes();
        }

        // Highlights reply button when moused over
        public void replyIcon_MouseEnter(object sender, EventArgs e)
        {
            replyIcon.Image = Image.FromFile("..\\..\\images\\reply_icon_hover.png");
        }

        // reply button goes back to normal when mouse leaves
        public void replyIcon_MouseLeave(object sender, EventArgs e)
        {
            replyIcon.Image = Image.FromFile("..\\..\\images\\reply_icon.png");
        }

        // Displays reply box when clicked if user is logged in
        public void replyIcon_MouseClick(object sender, EventArgs e)
        {
            // Verifies that a user is logged in
            if (Program.loggedInUserID != -1)
            {
                panel.Height = HEIGHT + ADDITIONAL_REPLY_SPACE;
                form.updatePanelLocations();
            }
            else
            {
                MessageBox.Show("You must be logged in to reply to a post.");
            }
        }

        // Highlights cancel button when moused over
        public void cancelReplyButton_MouseEnter(object sender, EventArgs e)
        {
            cancelReplyButton.Image = Image.FromFile("..\\..\\images\\cancel_button_hover.png");
        }

        // cancel button goes back to normal when mouse leaves
        public void cancelReplyButton_MouseLeave(object sender, EventArgs e)
        {
            cancelReplyButton.Image = Image.FromFile("..\\..\\images\\cancel_button.png");
        }

        // closes the reply box when clicked and fixes the panel positions in the main panel
        public void cancelReplyButton_MouseClick(object sender, EventArgs e)
        {
            replyBox.Text = "What are your thoughts?";
            panel.Height = HEIGHT;
            form.updatePanelLocations();
        }

        // highlights reply button when moused over
        public void replyButton_MouseEnter(object sender, EventArgs e)
        {
            replyButton.Image = Image.FromFile("..\\..\\images\\reply_button_hover.png");
        }

        // reply button goes back to normal when mouse leaves
        public void replyButton_MouseLeave(object sender, EventArgs e)
        {
            replyButton.Image = Image.FromFile("..\\..\\images\\reply_button.png");

        }

        // Adds reply if reply is valid and posts it below the comment
        public void replyButton_MouseClick(object sender, EventArgs e)
        {
            string commentContent = replyBox.Text;

            try // Check for foul language
            {
                Program.CheckFoulLanguage(commentContent);
            }
            catch (Program.FoulLanguageException except) // Caught foul language
            {
                MessageBox.Show("Caught a FoulLanguageException: " + except);
                return;
            }

            if (comment.isReply()) // Post is locked
            {
                MessageBox.Show("Replies to comment - replies are not supported.");
                return;
            }
            else if (commentContent.Equals(""))
            {
                MessageBox.Show("Please enter a comment.");
            }
            else // Add comment to post
            {
                Comment newReply = new Comment(commentContent, (uint)Program.loggedInUserID, comment.ID);
                comment.addComment(newReply);
                Program.idDictionary.Add(newReply.ID, newReply);
                replyBox.Text = "What are your thoughts?";
                form.addReply(newReply);
                panel.Height = HEIGHT;
                form.updatePanelLocations();

                //append to file new comment
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"..\\..\\..\\comments.txt", true))
                {
                    file.WriteLine(newReply.ID + "\t" +
                        Program.loggedInUserID + "\t" +
                        newReply.Content + "\t" +
                        newReply.ParentID + "\t" +
                        newReply.UpVotes + "\t" +
                        newReply.DownVotes + "\t" +
                        newReply.TimeStamp.Year + "\t" +
                        newReply.TimeStamp.Month + "\t" +
                        newReply.TimeStamp.Day + "\t" +
                        newReply.TimeStamp.Hour + "\t" +
                        newReply.TimeStamp.Minute + "\t" +
                        newReply.TimeStamp.Second + "\t" +
                        newReply.Silver + "\t" +
                        newReply.Gold + "\t" +
                        newReply.Platinum);
                }
            }
        }

        // "What are your thoughts?" displays if nothing is entered in the box
        public void replyBox_LostFocus(object sender, EventArgs e)
        {
            if (replyBox.Text.Equals(""))
            {
                replyBox.Text = "What are your thoughts?";
            }
        }

        // Update Users upVoted/downVoted items
        public void upDateUserVotes()
        {
            User u = Program.idDictionary[(uint)Program.loggedInUserID] as User;

            if (upVoted == true && u.UpVotedIDs.Contains(comment.ID) == false)
                u.UpVotedIDs.Add(comment.ID);
            if (downVoted == true && u.DownVotedIDs.Contains(comment.ID) == false)
                u.DownVotedIDs.Add(comment.ID);
            if (upVoted == false && u.UpVotedIDs.Contains(comment.ID) == true)
                u.UpVotedIDs.Remove(comment.ID);
            if (downVoted == false && u.UpVotedIDs.Contains(comment.ID) == true)
                u.DownVotedIDs.Remove(comment.ID);
        }

        public void updateVoteButtons()
        {
            if (Program.loggedInUserID != -1) // User is logged in
            {
                User u = Program.idDictionary[(uint)Program.loggedInUserID] as User;
                if (u.UpVotedIDs.Contains(comment.ID) == true) // User already UpVoted content
                {
                    upVoted = true;
                    upVote.BackgroundImage = Image.FromFile("..\\..\\images\\upVote_red.png");
                }
                if (u.DownVotedIDs.Contains(comment.ID) == true) // User already DownVoted content
                {
                    downVoted = true;
                    downVote.BackgroundImage = Image.FromFile("..\\..\\images\\downVote_blue.png");
                }
            }
        }

        // Passes reference of main form to this object so it can the method to reposition all the panels
        public void addFormToRefresh(ViewPostForm f)
        {
            form = f;
        }
    }
}

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
    public partial class ViewPostForm : Form
    {
        private Post post;
        private DisplayPost displayPost;
        private List<CommentAndReplyPanel> commentPanels;

        public ViewPostForm()
        {
            InitializeComponent();
        }

        public ViewPostForm(Post p)
        {
            post = p;
            displayPost = new DisplayPost(p, 1);
            displayPost.addFormToRefresh(this);

            InitializeComponent();
        }

        private void ViewPostForm_Load(object sender, EventArgs e)
        {
            commentPanels = new List<CommentAndReplyPanel>();
            postPanel.Controls.Add(displayPost.PostPanel);
            displayComments();            
        }

        private void displayComments()
        {
            int offset = displayPost.PostPanel.Height;

            foreach (Comment comment in post)
            {
                CommentAndReplyPanel commentPanel = new CommentAndReplyPanel(comment);
                commentPanel.addFormToRefresh(this);
                commentPanels.Add(commentPanel);
                commentPanel.Panel.Location = new Point(0, offset);
                offset += commentPanel.Panel.Height;
                postPanel.Controls.Add(commentPanel.Panel);

                foreach (Comment reply in comment)
                {
                    CommentAndReplyPanel replyPanel = new CommentAndReplyPanel(reply);
                    replyPanel.addFormToRefresh(this);
                    commentPanels.Add(replyPanel);
                    replyPanel.Panel.Location = new Point(0, offset);
                    offset += replyPanel.Panel.Height;
                    postPanel.Controls.Add(replyPanel.Panel);
                }
            }
        }

        public void updatePanelLocations()
        {
            int offset = displayPost.PostPanel.Height;
            foreach (CommentAndReplyPanel carp in commentPanels)
            {
                carp.Panel.Location = new Point(0, offset);
                offset += carp.Panel.Height;
            }
        }

        public void addComment(Comment comment)
        {
            CommentAndReplyPanel commentPanel = new CommentAndReplyPanel(comment);
            commentPanel.addFormToRefresh(this);
            commentPanels.Insert(0,commentPanel); // Insert at beginning
            postPanel.Controls.Add(commentPanel.Panel);
        }

        public void addReply(Comment reply)
        {
            CommentAndReplyPanel replyPanel = new CommentAndReplyPanel(reply);
            replyPanel.addFormToRefresh(this);
            postPanel.Controls.Add(replyPanel.Panel);

            for (int i = 0; i < commentPanels.Count; ++i)
            {
                if (commentPanels[i].comment.ID == reply.ParentID)
                {
                    commentPanels.Insert(i + 1, replyPanel);
                }
            }
        }
    }
}

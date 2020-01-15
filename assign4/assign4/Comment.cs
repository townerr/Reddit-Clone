/***********************************************************
CSCI 473 - Assignment 2 - Fall 2019

Programmer: Ryan Towner
Programmer: Matthew Beardsley

Date Due: 10/31/19


Purpose: This class represents a Comment in our reddit clone.
************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace assign4
{
    public class Comment : IComparable, IEnumerable
    {
        private uint id;
        private uint authorID;
        private string content;
        private uint parentID;
        private uint upVotes;
        private uint downVotes;
        private DateTime timeStamp;
        private uint silver;        // Amount of silver awarded to comment
        private uint gold;          // Amount of gold awarded to comment
        private uint platinum;      // Amount of platinum awarded to comment
        private SortedSet<Comment> commentReplies = new SortedSet<Comment>();

        // Default Constructor
        public Comment()
        {
            id = 0;
            authorID = 0;
            Content = "";
            parentID = 0;
            UpVotes = 0;
            DownVotes = 0;
            timeStamp = DateTime.MinValue;
            Silver = silver;
            Gold = gold;
            Platinum = platinum;
        }

        // New Comment Constructor
        public Comment(string content, uint authorID, uint parentID)
        {
            id = Program.GenerateID();

            this.authorID = authorID;
            Content = content;
            this.parentID = parentID;
            UpVotes = 1;
            DownVotes = 0;
            timeStamp = DateTime.Now;
            silver = 0;
            gold = 0;
            platinum = 0;
            Program.idArray.Add(id);

        }

        // Old Comment Constructor
        public Comment(uint id, uint authorID, string content, uint parentID, uint upVotes, uint downVotes, DateTime timeStamp, uint silver, uint  gold, uint platinum)
        {
            this.id = id;
            this.authorID = authorID;
            Content = content;
            this.parentID = parentID;
            UpVotes = upVotes;
            DownVotes = downVotes;
            this.timeStamp = timeStamp;
            Silver = silver;
            Gold = gold;
            Platinum = platinum;
            Program.idArray.Add(id);

        }

        // Indexer for Comment rewards
        public uint this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return silver;
                    case 1:
                        return gold;
                    case 2:
                        return platinum;
                }
                return 0;
            }
            set
            {
                switch (index)
                {
                    case 0:
                        silver = value;
                        break;
                    case 1:
                        gold = value;
                        break;
                    case 2:
                        platinum = value;
                        break;
                }
            }
        }

        // get and set methods 
        public uint ID
        {
            get { return id; }
        }

        public uint AuthorID
        {
            get { return authorID; }
        }
 
        // Validates input for content
        public string Content
        {
            get { return content; }
            set
            {
                if (value.Length >= 1 && value.Length <= 1000)
                {
                    Program.CheckFoulLanguage(value);
                    content = value;
                }
            }
        }

        public uint ParentID
        {
            get { return parentID; }
        }

        public uint UpVotes
        {
            get { return upVotes; }
            set { upVotes = value; }
        }

        public uint DownVotes
        {
            get { return downVotes; }
            set { downVotes = value; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        public uint Silver
        {
            get { return silver; }
            set { silver = value; }
        }

        public uint Gold
        {
            get { return gold; }
            set { gold = value; }
        }

        public uint Platinum
        {
            get { return platinum; }
            set { platinum = value; }
        }

        public int Score
        {
            get { return (int)(upVotes - downVotes); }
        }

        public string DisplayListBox
        {
            get
            {
                return ToString("ListBox");
            }
        }

        public SortedSet<Comment> CommentReplies
        {
            get { return commentReplies; }
        }

        public void addComment(Comment c)
        {
            commentReplies.Add(c);
        }

        public void removeComment(Comment c)
        {
            commentReplies.Remove(c);
        }

        // Sorted by post rating
        public int CompareTo(object obj)
        {
            // Checks for null values
            if (obj == null) throw new ArgumentNullException();

            Comment rightOp = obj as Comment;

            // Protect against failed typcasting
            if (rightOp != null)
            {
                if (Score > rightOp.Score) //Largest at the beginning
                    return -1;
                else if (Score < rightOp.Score) //Smallest at the end
                    return 1;
                else // PostRating == rightOp.PostRating
                    return -1; // Comments are the same, pick any order
            }
            else
                throw new ArgumentException("[Comment]:CompareTo argument is not a Comment");
        }

        public bool isReply()
        {
            if (Program.idDictionary[ParentID].GetType() == typeof(Comment))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            User commentAuthor = Program.idDictionary[AuthorID] as User;

            // Tabs comment replies
            String tabReplies = "";

            return tabReplies + "<" + ID + "> (" + Score + ") " + Content + " - " + commentAuthor.Name + " |" + TimeStamp + "|";
        }

        // Overloaded ToString for alternatively built strings
        public string ToString(string s)
        {
            // Tabs comment replies
            String tabReplies = "";
            User commentAuthor = Program.idDictionary[AuthorID] as User;

            // ToString for ListBox
            if (s.Equals("ListBox"))
            {
                // Checks if Comment is a Comment-reply
                if (isReply())
                {
                    tabReplies += "\t";
                }

                // Shortens content
                string shortComment;
                if (Content.Length > 35)
                {
                    shortComment = Content.Substring(0, 35) + "...";
                }
                else
                {
                    shortComment = Content;
                }


                return tabReplies + "<" + ID + "> (" + Score + ") " + shortComment + " - " + commentAuthor.Name + " |" + TimeStamp + "|";
            }
            else if(s.Equals("Points"))
            {
                // Shortens content
                string shortComment;
                if (Content.Length > 30)
                {
                    shortComment = Content.Substring(0, 30);
                }
                else
                {
                    shortComment = Content;
                }

                return shortComment;
            }
            else if(s.Equals("CommentAndReplyPanel"))
            {
                User author = Program.idDictionary[AuthorID] as User;

                string output = author.Name + " | " + Score + " points ";

                output += Program.age(timeStamp);

                return output;
            }
            else
                return ToString();

        }



        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        // Implementation for the GetEnumerator method from IEnumerable.
        public IEnumerator GetEnumerator()
        {
            Comment[] commentRepliesArr = new Comment[commentReplies.Count];
            commentReplies.CopyTo(commentRepliesArr);
            return new PostEnum(commentRepliesArr);
        }

    }

    // Implementation of IEnumerator as required by the IEnumerable interface
    public class CommentEnum : IEnumerator
    {
        public Comment[] replies;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public CommentEnum(Comment[] list)
        {
            replies = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < replies.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Comment Current
        {
            get
            {
                try
                {
                    return replies[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}

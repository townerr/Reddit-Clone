/***********************************************************
CSCI 473 - Assignment 2 - Fall 2019

Programmer: Ryan Towner
Programmer: Matthew Beardsley

Date Due: 10/31/19


Purpose: This class represents a Post in our reddit clone.
************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace assign4
{
    public class Post : IEnumerable, IComparable
    {
        //vars
        private readonly uint id;
        private readonly uint subHome;
        private readonly uint authorID;
        private readonly DateTime timeStamp;
        private string title;
        private string postContent;
        private uint upVotes;
        private uint downVotes;
        private uint weight;
        private bool locked;
        private uint silver;        // Amount of silver awarded to comment
        private uint gold;          // Amount of gold awarded to comment
        private uint platinum;      // Amount of platinum awarded to comment
        private SortedSet<Comment> postComments = new SortedSet<Comment>();

        // default constructor
        public Post()
        {
            id = 0;
            subHome = 0;
            authorID = 0;
            title = "";
            postContent = "";
            upVotes = 0;
            downVotes = 0;
            weight = 0;
            locked = false;

            silver = 0;
            gold = 0;
            platinum = 0;
        }

        // new post constructor
        public Post(string _Title, string _PostContent, uint _AuthorID, uint _SubHome, int _locked)
        {
            //generate unique id
            uint _ID = Program.GenerateID();

            id = _ID;
            title = _Title;
            postContent = _PostContent;
            authorID = _AuthorID;
            timeStamp = DateTime.Now;
            subHome = _SubHome;
            weight = 0;
            upVotes = 1;
            downVotes = 0;
            silver = 0;
            gold = 0;
            platinum = 0;

            if (_locked == 1)
                locked = true;
            else if (_locked == 0)
                locked = false;

            Program.idArray.Add(_ID);
        }

        // existing post constructor
        public Post(int _locked, uint _ID, uint _SubHome, uint _AuthorID, string _Title, string _PostContent, DateTime _TimeStamp, uint _UpVotes, uint _DownVotes, uint _Weight, uint silver, uint gold, uint platinum)
        {
            if (Program.CheckID(_ID) == true)
            {
                id = _ID;
                Program.idArray.Add(_ID);
            }
            else
            {
                uint newID = Program.GenerateID();
                id = newID;
                Program.idArray.Add(newID);
            }

            subHome = _SubHome;
            authorID = _AuthorID;
            Title = _Title;
            PostContent = _PostContent;
            timeStamp = _TimeStamp;
            UpVotes = _UpVotes;
            DownVotes = _DownVotes;
            Weight = _Weight;

            if (_locked == 1)
                locked = true;
            else if (_locked == 0)
                locked = false;

            Silver = silver;
            Gold = gold;
            Platinum = platinum;
        }

        // Indexer for Post rewards
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

        //getters and setters
        public uint ID
        {
            get { return id; }
        }

        public uint SubHome
        {
            get { return subHome; }
        }

        public uint AuthorID
        {
            get { return authorID; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
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

        // Property includes input validation for PostContent's length and a foul language check
        public string PostContent
        {
            get { return postContent; }
            set
            {
                Program.CheckFoulLanguage(value);

                if (value.Length >= 1 && value.Length <= 1000)
                {
                    postContent = value;
                }
            }
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

        public uint Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public int Score
        {
            get { return (int)(UpVotes - DownVotes); }
        }

        // Property calculates PostRating based off Weight and Score
        public int PostRating
        {
            get
            {
                int returnValue = 0;

                if (Weight == 0)
                {
                    returnValue = Score;
                }
                else if (Weight == 1)
                {
                    double temp = .66 * Score;
                    returnValue = Convert.ToInt32(temp);
                }
                else if (Weight >= 2)
                {
                    returnValue = 0;
                }

                return returnValue;
            }

        }

        public string DisplayListBox
        {
            get
            {
                return ToString("ListBox");
            }
        }

        // Used for LINQ querys
        public SortedSet<Comment> PostComments
        {
            get { return postComments; }
        }

        // Accesser method for postComments
        public void addComment(Comment c)
        {
            postComments.Add(c);
        }

        // Accesser method for postComments
        public void removeComment(Comment c)
        {
            postComments.Remove(c);
        }

        // Implementation of CompareTo from IComparator
        // Sorts by post rating
        public int CompareTo(object obj)
        {
            // Checks for null values
            if (obj == null) throw new ArgumentNullException();

            Post rightOp = obj as Post;

            // Protect against failed typcasting
            if (rightOp != null)
            {
                if (PostRating > rightOp.PostRating)
                    return -1;
                else if (PostRating < rightOp.PostRating)
                    return 1;
                else // PostRating == rightOp.PostRating
                    return -1; // PostRatings are the same, pick any order
            }
            else
                throw new ArgumentException("[Subreddit]:CompareTo argument is not a Subreddit");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        // Implementation for the GetEnumerator method from IEnumerable.
        public IEnumerator GetEnumerator()
        {
            Comment[] postCommentsArr = new Comment[postComments.Count];
            postComments.CopyTo(postCommentsArr);
            return new PostEnum(postCommentsArr);
        }

        // ToString override
        public override string ToString()
        {
            User postAuthor = Program.idDictionary[AuthorID] as User;
            Subreddit sub = Program.idDictionary[SubHome] as Subreddit;

            // Allows for post to be displayed as locked
            string lockString = "";
            if (Locked == true)
            {
                lockString = "**LOCKED**";
            }

            return "<" + ID + "> [" + sub.Name + "] (" + Score + ") " + Title + " " + lockString + " - " + PostContent + " - " +
                            postAuthor.Name + " |" + TimeStamp + "|";
        }

        // Overloaded ToString for alternatively built strings
        public string ToString(string s)
        {
            if (s.Equals("ListBox"))
            {
                User postAuthor = Program.idDictionary[AuthorID] as User;
                Subreddit sub = Program.idDictionary[SubHome] as Subreddit;

                // Shortens long titles
                string shortTitle;
                if (Title.Length > 35)
                {
                    shortTitle = Title.Substring(0, 35) + "...";
                }
                else
                {
                    shortTitle = Title;
                }

                // Allows for post to be displayed as locked
                string lockString = "";
                if (Locked == true)
                {
                    lockString = "**LOCKED**";
                }

                return "<" + ID + "> [" + sub.Name + "] (" + Score + ") " + shortTitle + " " + lockString + " - " +
                                postAuthor.Name + " |" + TimeStamp + "|";
            }
            else if (s.Equals("Points"))
            {
                // Shortens content
                string shortComment;
                if (Title.Length > 30)
                {
                    shortComment = Title.Substring(0, 30);
                }
                else
                {
                    shortComment = Title;
                }

                return shortComment;
            }
            else if (s.Equals("DisplayPost"))
            {
                User postAuthor = Program.idDictionary[AuthorID] as User;
                Subreddit sub = Program.idDictionary[SubHome] as Subreddit;

                string output = "r/" + sub.Name + " | Posted by u/" + postAuthor.Name;

                output += Program.age(timeStamp);

                return output;
            }
            else
                return ToString();
        }
    }

    // Implementation of IEnumerator as required by the IEnumerable interface
    public class PostEnum : IEnumerator
    {
        public Comment[] comments;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public PostEnum(Comment[] list)
        {
            comments = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < comments.Length);
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
                    return comments[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}

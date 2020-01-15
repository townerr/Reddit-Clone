/***********************************************************
CSCI 473 - Assignment 2 - Fall 2019

Programmer: Ryan Towner
Programmer: Matthew Beardsley

Date Due: 10/31/19


Purpose: This class represents a Subreddit in our reddit clone.
************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace assign4
{
    public class Subreddit : IEnumerable, IComparable
    {
        //vars
        private readonly uint id;
        private string name;
        private uint members;
        private uint active;
        private SortedSet<Post> subPosts = new SortedSet<Post>();

        // default constructor
        public Subreddit()
        {
            id = 0;
            name = "";
            members = 0;
            active = 0;
        }

        // new Subreddit constructor
        public Subreddit(string _Name)
        {
            //generate an unique id for the subreddit
            uint _ID = Program.GenerateID();

            id = _ID;
            Name = _Name;
            Members = 0;
            Active = 0;

            Program.idArray.Add(_ID);
        }

        // existing Subreddit constructor
        public Subreddit(uint _ID, string _Name, uint _Members, uint _Active)
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

            Name = _Name;
            Members = _Members;
            Active = _Active;
        }

        // getters and setters
        public uint ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value.Length >= 3 && value.Length <= 21)
                    name = value;
                else
                    return;
            }
        }

        public uint Members
        {
            get { return members; }
            set { members = value; }
        }

        public uint Active
        {
            get { return active; }
            set { active = value; }
        }

        public SortedSet<Post> SubPosts
        {
            get { return subPosts; }
        }

        // Accessor method for adding a post to subreddit
        public void addPost(Post p)
        {
            subPosts.Add(p);
        }

        // Accessor method to remove a post from subreddit
        public void removePost(Post p)
        {
            subPosts.Remove(p);
        }

        //interfaces
        public int CompareTo(object obj)
        {
            // Checks for null values
            if (obj == null) throw new ArgumentNullException();

            Subreddit rightOp = obj as Subreddit;

            // Protect against failed typcasting
            if (rightOp != null)
                return name.CompareTo(rightOp.name);
            else
                throw new ArgumentException("[Subreddit]:CompareTo argument is not a Subreddit");
        }

        // ToString
        public override string ToString()
        {
            return Name;
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        // Implementation for the GetEnumerator method from IEnumerable.
        public IEnumerator GetEnumerator()
        {
            Post[] subPostArray = new Post[subPosts.Count];
            subPosts.CopyTo(subPostArray);
            return new SubEnum(subPostArray);
        }
    }

    // Implementation of IEnumerator as required by the IEnumerable interface
    public class SubEnum : IEnumerator
    {
        public Post[] posts;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public SubEnum(Post[] list)
        {
            posts = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < posts.Length);
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

        public Post Current
        {
            get
            {
                try
                {
                    return posts[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}

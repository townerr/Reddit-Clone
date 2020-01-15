/***********************************************************
CSCI 473 - Assignment 3 - Fall 2019

Programmer: Ryan Towner
Programmer: Matthew Beardsley

Date Due: 10/31/19

Purpose: This class represents a user in our reddit clone.
************************************************************/

using System;
using System.Text;
using System.Collections.Generic;

namespace assign4
{
    public class User : IComparable
    {
        // Vars
        private readonly uint id;
        private readonly string name;
        private int postScore;
        private int commentScore;
        private int userType;
        private string passwordHash;

        private enum UserClass { User, Moderator, Admin};

        // Keeps track of users upVoted and downVoted posts
        private List<uint> upVotedIDs = new List<uint>();
        private List<uint> downVotedIDs = new List<uint>();

        // defualt constructor
        public User()
        {
            id = 0;
            name = "";
            postScore = 0;
            commentScore = 0;
            userType = 0;
        }

        // new User constructor
        public User(string _name)
        {
            uint _ID = Program.GenerateID();

            id = _ID;

            if(_name.Length >= 5 && name.Length <= 21)
                name = _name;
            else
                throw new ArgumentException("Name must be at least 5 characters and 21 characters at longest.");

            Program.idArray.Add(_ID);
        }

        // existing User constructor
        public User(uint _ID, int _userType, string _name, string _passwordHash, int _postScore, int _commentScore)
        {
            //check if id is unique
            //If not create create a new unique id 
            if(Program.CheckID(_ID) == true)
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

            if (_name.Length >= 5 && _name.Length <= 21)
                name = _name;
            else
                throw new ArgumentException("Name must be at least 5 characters and 21 characters at longest."); postScore = _postScore;

            commentScore = _commentScore;
            userType = _userType;
            passwordHash = _passwordHash;
        }

        //getters and setters
        public int PostScore
        {
            get { return postScore; }
            set { postScore = value; }
        }

        public int CommentScore
        {
            get { return commentScore; }
            set { commentScore = value; }
        }

        public uint ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public int TotalScore()
        {
            return PostScore + CommentScore;
        }

        public int UserType
        {
            get { return userType; }
        }

        public List<uint> UpVotedIDs
        {
            get { return upVotedIDs; }
        }

        public List<uint> DownVotedIDs
        {
            get { return downVotedIDs; }
        }

        public string UserTypeText()
        {
            if (userType == 0)
                return "";
            else if (userType == 1)
                return "(M)";
            else if (userType == 2)
                return "(A)";

            return "";
        }

        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        // Name sorted in ascending order
        public int CompareTo(object obj)
        {
            // Checks for null values
            if (obj == null) throw new ArgumentNullException();

            User rightOp = obj as User;

            // Protect against failed typcasting
            if (rightOp != null)
                return name.CompareTo(rightOp.name);
            else
                throw new ArgumentException("[User]:CompareTo argument is not a User");
        }

        // ToString Override
        public override string ToString()
        {
            //return String.Format("{0, -15} {1} \t ({2} / {3})", Name, UserTypeText(), PostScore, CommentScore);
            return Name;
        }
    }
}

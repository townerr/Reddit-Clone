/***********************************************************
CSCI 473 - Assignment 3 - Fall 2019

Programmer: Ryan Towner
Programmer: Matthew Beardsley

Date Due: 10/31/19

Purpose: A GUI version of reddit.
************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assign4
{
    static class Program
    {
        public static int loggedInUserID = -1;

        // SortedSets that hold all class objects
        public static Dictionary<uint, object> idDictionary = new Dictionary<uint, object>();
        public static SortedSet<User> users = new SortedSet<User>();
        public static SortedSet<Subreddit> subreddits = new SortedSet<Subreddit>();
        public static List<DisplayPost> displayPosts = new List<DisplayPost>();

        // Array of vulgar words that are banned
        private static string[] vulgarWords = { "fudge", "shoot", "baddie", "butthead" };

        //list to keep all ids to make sure they are all unique
        public static List<uint> idArray = new List<uint>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new Form1());
        }
        /***************************************************************
        Login
        Use: Attempts to log the user in

        Parameters: user - the user object thats strying to be logged in
                    password - the password that has been input to test

        Returns: bool - true if login successful, false if it is not
        ***************************************************************/
        public static bool Login(User user, String password)
        {
            //get hash from user object
            string userHash = user.PasswordHash.ToString();

            //convert input password to hash
            string inputHash = password.GetHashCode().ToString("X");
            
            //test if hash is correct and return results
            if(userHash == inputHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /***************************************************************
        CheckID 
        Use: Check if ID is unique

        Parameters: uint _ID - The ID to be checked

        Returns: bool - true if the ID is unique, false if it is not
        ***************************************************************/
        public static bool CheckID(uint _ID)
        {
            foreach (uint id in idArray)
            {
                // If _ID is alread in the ID array
                if (id == _ID)
                {
                    return false;
                }
            }
            // ID is free to use, not in the array
            return true;
        }

        /***************************************************************
        GenerateID 
        Use: Fucntion to generate a unique id if the given one is already taken

        Parameters: none

        Returns: uint, an unique id that can be used by the caller.
        ***************************************************************/
        public static uint GenerateID()
        {
            Random rand = new Random();
            uint newID = 0;

            do
            {
                newID = Convert.ToUInt32(rand.Next(0, 9999));

            } while (CheckID(newID) == false); // While ID is not unique, find get a new id

            return newID;
        }

        /***************************************************************
        ReadInputFiles 
        Use: Reads through the input files for Users, Subreddits, Posts,
        and comments. For each class it reads in a line from the input file,
        tokenizes the line. Parses the tokens into something that can be used
        to create a new Object for each class and creates them. These Objects are
        then added to their respective SortedSets and the id dictionary.

        Parameters: none

        Returns: nothing
        ***************************************************************/
        public static void ReadInputFiles()
        {
            String line;
            String[] tokens;

            // Reading in the users
            using (StreamReader inFile = new StreamReader("..\\..\\..\\users.txt"))
            {
                line = inFile.ReadLine();

                // On the assignment site the format is, "unique ID | usertype | name | hashcode | upVotes | downVotes" but the class attributes are "unique ID, name, postScore, commentScore", not sure why  
                while (line != null)
                {
                    tokens = line.Split('\t');
                    User user = new User(Convert.ToUInt32(tokens[0]), Convert.ToInt32(tokens[1]), tokens[2], tokens[3], Convert.ToInt32(tokens[4]), Convert.ToInt32(tokens[5]));
                    users.Add(user);
                    idDictionary.Add(user.ID, user);
                    line = inFile.ReadLine();
                }
            }

            // Reads in the subreddits
            using (StreamReader inFile = new StreamReader("..\\..\\..\\subreddits.txt"))
            {
                line = inFile.ReadLine();

                while (line != null)
                {
                    tokens = line.Split('\t');
                    Subreddit sub = new Subreddit(Convert.ToUInt32(tokens[0]), tokens[1], Convert.ToUInt32(tokens[2]), Convert.ToUInt32(tokens[3]));
                    subreddits.Add(sub);
                    idDictionary.Add(sub.ID, sub);

                    line = inFile.ReadLine();
                }
            }

            // Reads in the posts and creates Post objects, putting them in the Subreddits sorted set of Posts
            // 0 locked | 1 unique ID | 2 authorID | 3 title | 4 content | 5 subredditID | 6 upVotes | 7 downVotes | 8 weight | 9 year | 10 month | 11  day | 12 hour | 13 min | 14 sec | 15 silver | 16 gold | 17 plat |
            // (uint _ID, uint _SubHome, uint _AuthorID, string _Title, string _PostContent, DateTime _TimeStamp, uint _UpVotes, uint _DownVotes, uint _Weight)
            using (StreamReader inFile = new StreamReader("..\\..\\..\\posts.txt"))
            {
                line = inFile.ReadLine();

                while (line != null)
                {
                    tokens = line.Split('\t');


                    DateTime timeStamp = new DateTime(Convert.ToInt32(tokens[9]), Convert.ToInt32(tokens[10]), Convert.ToInt32(tokens[11]),
                        Convert.ToInt32(tokens[12]), Convert.ToInt32(tokens[13]), Convert.ToInt32(tokens[14]));

                    Post post = new Post(Convert.ToInt32(tokens[0]), Convert.ToUInt32(tokens[1]), Convert.ToUInt32(tokens[5]), Convert.ToUInt32(tokens[2]), tokens[3], tokens[4], timeStamp,
                        Convert.ToUInt32(tokens[6]), Convert.ToUInt32(tokens[7]), Convert.ToUInt32(tokens[8]), Convert.ToUInt32(tokens[15]), Convert.ToUInt32(tokens[16]), Convert.ToUInt32(tokens[17]));

                    idDictionary.Add(post.ID, post);

                    Subreddit postSubreddit = idDictionary[post.SubHome] as Subreddit;
                    postSubreddit.addPost(post);

                    line = inFile.ReadLine();
                }
            }

            // Reads in comments and creates Comment objects, putting them in the comments SortedSet
            using (StreamReader inFile = new StreamReader("..\\..\\..\\comments.txt"))
            {
                line = inFile.ReadLine();

                while (line != null)
                {
                    tokens = line.Split('\t');

                    uint uniqueID = Convert.ToUInt32(tokens[0]);
                    uint authorID = Convert.ToUInt32(tokens[1]);
                    string content = tokens[2];
                    uint parentID = Convert.ToUInt32(tokens[3]);
                    uint upVotes = Convert.ToUInt32(tokens[4]);
                    uint downVotes = Convert.ToUInt32(tokens[5]);
                    DateTime timeStamp = new DateTime(Convert.ToInt32(tokens[6]), Convert.ToInt32(tokens[7]), Convert.ToInt32(tokens[8]), Convert.ToInt32(tokens[9]),
                        Convert.ToInt32(tokens[10]), Convert.ToInt32(tokens[11]));
                    uint silver = Convert.ToUInt32(tokens[12]);
                    uint gold = Convert.ToUInt32(tokens[13]);
                    uint platinum = Convert.ToUInt32(tokens[14]);

                    Comment comment = new Comment(uniqueID, authorID, content, parentID, upVotes, downVotes, timeStamp, silver, gold, platinum);

                    idDictionary.Add(comment.ID, comment);

                    if (idDictionary[comment.ParentID] is Post)
                    {
                        Post parent = idDictionary[comment.ParentID] as Post;
                        parent.addComment(comment);
                    }
                    else // Parent was a comment
                    {
                        Comment parent = idDictionary[comment.ParentID] as Comment;
                        parent.addComment(comment);
                    }

                    line = inFile.ReadLine();
                }
            }
        }

        /***************************************************************
        DeletePost 
        Use: Deletes a post from our reddit clone as specified by the user.
        This method iterates through all comments and replies and 
        removes all references of the post and its children from the 
        parent subreddit and the ID dictionary. 

        Parameters: Post - the post to be deleted

        Returns: nothing
        ***************************************************************/
        public static void DeletePost(Post post)
        {
            // Remove post from parent sub
            Subreddit parentSub = idDictionary[post.SubHome] as Subreddit;
            parentSub.removePost(post);

            // Remove all references to post, comments and replies

            // Remove reference to post in idDictionary
            idDictionary.Remove(post.ID);

            // Remove references to comments 
            foreach (Comment comment in post)
            {
                idDictionary.Remove(comment.ID);

                // Remove references to all comments
                foreach (Comment reply in comment)
                {
                    idDictionary.Remove(reply.ID);
                }
            }
        }

        /***************************************************************
        DeletePost 
        Use: Deletes a comment. This method first removes all references 
        from the parent Post or Comment and then deletes references to the 
        Comment from the idDictionary. Finally, the Comment removes all 
        references to its comments.

        Parameters: Post - the post to be deleted

        Returns: nothing
        ***************************************************************/
        public static void DeleteComment(Comment comment)
        {
            // Remove post from parent sub
            if (idDictionary[comment.ParentID].GetType() == typeof(Post))
            {
                Post parentPost = idDictionary[comment.ParentID] as Post;
                parentPost.removeComment(comment);
            }
            else // Parent was a comment, remove Comment from parent Comment
            {
                Comment parentComment = idDictionary[comment.ParentID] as Comment;
                parentComment.removeComment(comment);
            }

            // Remove reference to post in idDictionary
            idDictionary.Remove(comment.ID);

            // Remove references to comments 
            foreach (Comment reply in comment)
            {
                idDictionary.Remove(comment.ID);
            }
        }

        /***************************************************************
        age 
        Use: Returns a string for the age of a given timeStamp.
        Used to print the age of a post or comment.

        Parameters: DateTime timestamp - a DateTime for the creation of a 
            post or comment.

        Returns: A string representing how long ago the post or comment was made.
        ***************************************************************/
        public static string age(DateTime timeStamp)
        {
            string output = "";

            TimeSpan age = DateTime.Now.Subtract(timeStamp);

            // Minutes
            if (age.TotalHours < 1)
            {
                output += " " + age.Minutes;
                if (age.Minutes  == 1)
                    output += " minute ago";
                else
                    output += " minutes ago";
            }
            // Hours
            else if (age.TotalDays < 1)
            {
                output += " " + (int)age.TotalHours;
                if ((int)age.TotalHours == 1)
                    output += " hour ago";
                else
                    output += " hours ago";
            }
            // Days
            else if (age.TotalDays < 30.4167) // 30.4167 days in a month
            {
                output += " " + (int)age.TotalDays;
                if ((int)age.TotalDays == 1)
                    output += " day ago";
                else
                    output += " days ago";
            }
            // Months
            else if (age.TotalDays < 365.2425) // 365.2425 days in a year
            {
                output += " " + (int)(age.TotalDays / 30.4167);
                if ((int)(age.TotalDays / 30.4167) == 1)
                    output += " month ago";
                else
                    output += " months ago";
            }
            // Years
            else
            {
                output += " " + (int)(age.TotalDays / 365.2425);
                if ((int)(age.TotalDays / 365.2425) == 1)
                    output += " year ago";
                else
                    output += " years ago";
            }

            return output;
        }

        /***************************************************************
        CheckFoulLanguage 
        Use: Tokenizes input string and checks each word if it matches
        a foul language word from the array vulgarWords. If a foul word
        is found it throws a FoulLanguageException.

        Parameters: 1. input: A string of user input words

        Returns: nothing
        ***************************************************************/
        public static void CheckFoulLanguage(string input)
        {
            String[] words = input.Split();

            foreach (String word in words)
            {
                foreach (String vulgarWord in vulgarWords)
                {
                    if (vulgarWord.Equals(word.ToLower()))
                    {
                        throw new FoulLanguageException();
                    }
                }
            }
        }

        public class FoulLanguageException : Exception
        {
            public override string ToString()
            {
                return "No foul language allowed! Please refrain from using foul language.";
            }
        }

    }
}

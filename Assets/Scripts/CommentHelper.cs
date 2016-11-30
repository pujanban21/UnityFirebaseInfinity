using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class CommentHelper
    {
        private const string TAG = "CommentHelper";
        private string mComment;
        private DateTime mTimeStamp;
        private int mLikes;

        public CommentHelper() { }

        public CommentHelper(string comment, DateTime dateTime, int likes) {
            this.mComment = comment;
            this.mTimeStamp = dateTime;
            this.mLikes = likes;
        }

        //Dictionary to store and retrieve comments along with time stamp and likes
        public Dictionary<string, object> CommentRS() {

            Dictionary<string, object> commentStruct = new Dictionary<string, object>();
            commentStruct["comment"] = mComment;
            commentStruct["TimeStamp"] = mTimeStamp.ToString("dd/MM/yyyy");
            commentStruct["Likes"] = Likes.ToString();

            return commentStruct;
        }

        //Accessor for commment
        public string Comment {
            get {
                return mComment;
            }
            set {
                mComment = value;
            }
        }

        //Accessor for TimeStamp
        public DateTime TimeStamp
        {
            get {
                return mTimeStamp;
            }
            set {
                mTimeStamp = value;
            }
        }

        //Accessor for popularity of votes
        public int Likes {
        get {
                return mLikes;
            }
        set {
                mLikes = value;
            }
        }
    }
}

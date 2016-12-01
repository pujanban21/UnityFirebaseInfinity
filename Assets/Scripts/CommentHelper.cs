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
        private string mTimeStamp;
        private string mLikes;
        private string mUserName;

        public CommentHelper() { }

        public CommentHelper(string userName, string comment, string dateTime, string likes) {
            this.mUserName = userName;
            this.mComment = comment;
            this.mTimeStamp = dateTime;
            this.mLikes = likes;
        }

        //Dictionary to store and retrieve comments along with time stamp and likes
        public Dictionary<string, object> CommentRS() {

            Dictionary<string, object> commentStruct = new Dictionary<string, object>();
            commentStruct[StringValues.USER_NAME] = mUserName;
            commentStruct[StringValues.COMMENT] = mComment;
            commentStruct[StringValues.TIME_STAMP] = mTimeStamp;
            commentStruct[StringValues.LIKES] = mLikes;

            return commentStruct;
        }

        //Accessor for username
        public string UserName {
            get
            {
                return mUserName;
            }
            set {
                mUserName = value;
            }
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
        public string TimeStamp
        {
            get {
                return mTimeStamp;
            }
            set {
                mTimeStamp = value;
            }
        }

        //Accessor for popularity of votes
        public string Likes {
        get {
                return mLikes;
            }
        set {
                mLikes = value;
            }
        }
    }
}

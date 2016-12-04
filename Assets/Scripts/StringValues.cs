using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    static class  StringValues
    {
        #region String Variables for Comment 
        public static string LIKES = "likes";
        public static string COMMENT = "comment";
        public static string TIME_STAMP = "timestamp";
        public static string COMMENT_ID = "commentID";
        #endregion

        #region Location Coordinates
        public static string LATITUDE = "latitude";
        public static string LONGTITUDE = "longtitude";
        #endregion

        #region User Information 
        public static string USER_NAME = "userName";
        public static string EMAIL = "email";
        public static string PASSWORD = "password";
        public static string UID = "uid";
        #endregion

        #region Firebase Child Nodes
        public static string WORLDS_INFINITY_APP_ROOT = "WorldsInfinity";
        public static string USER_NODE = "Users";
        public static string LOCATIONS_NODE = "Locations";
        public static string COMMENT_NODE = "Comment";
       
        #endregion
    }
}

using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Unity.Editor;
using Firebase;
using UnityEngine;

namespace Assets.Scripts
{
    class FirebaseHelper
    {
        private const string TAG = "FirebaseHelper";

        private DatabaseReference rootReference;
        private FirebaseApp app;
        private string locationName = "";
        private string mUid = "";


        public FirebaseHelper() { }
        
        //URL of your database
        private const string getRef = "https://worldsinfinity-6f366.firebaseio.com/";

        //Get the URL of your database
        public string URLRef()
        {
            return getRef;
        }

        public void DatabaseRef()
        {
            app = FirebaseApp.DefaultInstance;
            app.SetEditorDatabaseUrl(URLRef());
        }

        //Root Node
        public DatabaseReference RootRef()
        {
            rootReference = FirebaseDatabase.DefaultInstance.RootReference;
            return rootReference;

        }
        //App Root Node
        public DatabaseReference AppRootRef() {
            return RootRef().Child("WorldsInfinity");
        }

        //Current User node
        public DatabaseReference UserRef()
        {
           return AppRootRef().Child("Users");
        }

        //Current User Visited Places Node
        public DatabaseReference VisitedLocationRef()
        {
            return UserID().Child("VisitedLocation");
        }

        public DatabaseReference UserID() {
            return UserRef().Child(mUid);
        }

        //New Location
        public DatabaseReference NewLocation()
        {
            return VisitedLocationRef().Child(locationName);
        }
        //Comment Node
        public DatabaseReference CommentRef()
        {
            return AppRootRef().Child("Comments");
        }

        //Get the location where comment is made
        public DatabaseReference CommentOnLocationRef() {
            return CommentRef().Child(locationName);
        }

              /*Not sure if I need thse nodes below*/
        //TimeStamp
        public DatabaseReference TimeStamp()
        {
            return CommentRef().Child("Date");
        }

        public DatabaseReference VoteComment()
        {
            return CommentRef().Child("Likes");
        }
        public DatabaseReference LocationComment() {
            return CommentRef().Child("Location");
        }

        //Accessor for the current Location Name
        public string LocationName{ 
            get
            { return locationName;}
            set
            { locationName = value;}
        }

        //Get the current user ID
        public string UserToken {
            get
            { return mUid; }
            set
            { mUid = value;}
        }
    }
}

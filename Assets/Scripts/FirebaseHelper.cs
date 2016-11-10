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
        private static string TAG = "FirebaseHelper";

        private DatabaseReference rootReference;
        private FirebaseApp app;

        public FirebaseHelper() { }

        private const string getRef = "https://worldsinfinity-6f366.firebaseio.com/";

        public string URLRef()
        {
            return getRef;
        }

        public void DatabaseRef()
        {
            Debug.Log(TAG);  //For Debugging
            app = FirebaseApp.DefaultInstance;
            app.SetEditorDatabaseUrl(URLRef());
        }

        public DatabaseReference RootRef()
        {
            rootReference = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log(rootReference.ToString()); //For Debugging
            return rootReference;

        }
       
        public DatabaseReference CurrentUserRef()
        {
            Debug.Log(RootRef().Child("Users")); //For Debugging

            return RootRef().Child("Users");
        }

        public DatabaseReference VisitedLocationRef()
        {
            Debug.Log("visitedLocations"); //For Debugging

            return CurrentUserRef().Child("VisitedLocation");
        }

        public DatabaseReference NewLocation()
        {
            return VisitedLocationRef().Child("LocationName");
        }

        public DatabaseReference CommentRef()
        {
            return NewLocation().Child("Comments");
        }

        public DatabaseReference TimeStamp()
        {
            return CommentRef().Child("Date");
        }

        public DatabaseReference VoteComment()
        {
            return CommentRef().Child("Likes");
        }

    }
}

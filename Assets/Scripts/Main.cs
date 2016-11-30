using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Firebase;
using System.Collections.Generic;
using Firebase.Database;
using System;

public class Main : MonoBehaviour {
    private const string TAG = "Main"; 
    private FirebaseHelper mFirebaseHelper;
    private VisitedPlaces_Info mVisitedLocation;
    private CommentHelper mCommentHelper;
    //private FacebookHelper mFacebookHelper;

    //For DummyLocation
    private string mLocationInfo;

    private string latitude = "";
    private string longtitude = "";
    private DateTime mTimeStamp;

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    void Start()
    {

        dependencyStatus = FirebaseApp.CheckDependencies();
        if (dependencyStatus != DependencyStatus.Available)
        {
            FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
                dependencyStatus = FirebaseApp.CheckDependencies();
                if (dependencyStatus == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    // This should never happen if we're only using Firebase Analytics.
                    // It does not rely on any external dependencies.
                    Debug.LogError(
                       "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
        else
        {
          InitializeFirebase();
        }
    }

    // Initialize the Firebase database:
    void InitializeFirebase()
    {
       // mFacebookHelper.AddLogButton();
        mLocationInfo = "Texas Tech University";
        Debug.Log("here");
        mFirebaseHelper = new FirebaseHelper();
        mFirebaseHelper.LocationName = mLocationInfo;
        mFirebaseHelper.DatabaseRef();

        GetListOfLocation();
       // AddLocation();
       // AddComment();
       // GetListOfLocation();

    }

    // Exit if escape (or back, on mobile) is pressed.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }



    //Adding visited Location
    public void AddLocation()
    {
        /**
         * remarks
            Will get the name and cordinates later from the user through the maps
         */
        
        mVisitedLocation = new VisitedPlaces_Info("dgdfg", "sdfsda");
        mFirebaseHelper.NewLocation().UpdateChildrenAsync(mVisitedLocation.SaveLocation());
       
    }

    //Adding new commments
    public void AddComment() {
        mCommentHelper = new CommentHelper("this is nice place", TimeStamp() , 5);
        string key = mFirebaseHelper.CurrentUserRef().Push().Key;
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[key] = mCommentHelper.CommentRS();
        mFirebaseHelper.CommentOnLocationRef().UpdateChildrenAsync(childUpdates);
    }

    //viewing comments about place
    public void ViewComment() {
    }

    //Getting coordinates of all visited Places
    public void GetListOfLocation() {
      
        mFirebaseHelper.VisitedLocationRef().ValueChanged += (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError != null) {
                Debug.LogError(args.DatabaseError);
            }

            if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0) {
                Debug.Log(args.Snapshot.ChildrenCount);
                foreach (var childSnapshot in args.Snapshot.Children){
                  
                   /* Need to get a better data structure to store all the location*/
                   latitude = childSnapshot.Child("latitude").Value.ToString();
                   longtitude = childSnapshot.Child("longitude").Value.ToString();

                   Debug.Log(latitude + "   "  + longtitude);
                }
                
            }
        };
    }

    public void GetLatitude(string latitude)
    {
        mVisitedLocation.Latitude = latitude;
    }

    public void GetLongitude(string longitude)
    {
        mVisitedLocation.Longitude = longitude;
        Debug.Log(longitude);
    }

    public void OnClick()
    {
    }

    private DateTime TimeStamp() {
        mTimeStamp = DateTime.Now;
        return mTimeStamp;
    }
}


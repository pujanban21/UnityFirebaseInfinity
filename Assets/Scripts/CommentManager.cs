using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Firebase;
using System.Collections.Generic;
using Firebase.Database;
using System;

public class CommentManager : MonoBehaviour {
    private const string TAG = "CommentManager"; 
    private FirebaseHelper mFirebaseHelper;
    private VisitedPlacesInfo mVisitedLocation;
    private CommentHelper mCommentHelper;
    //private FacebookHelper mFacebookHelper;

    //For DummyLocation
    private string mLocationInfo;

    private string latitude = "";
    private string longtitude = "";
    private DateTime mTimeStamp;

    private ArrayList mListofLocations;
    private ArrayList mListOfCommentsByLikes;
    private ArrayList mListOfCommentsByTime;
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

    #region Initialize Firebase and Get Location Name
    void InitializeFirebase()
    {
       //Put your location name here from LocationHelper
        mLocationInfo = "Computer Science";
        mFirebaseHelper = new FirebaseHelper();
        mFirebaseHelper.LocationName = mLocationInfo;
        mFirebaseHelper.DatabaseRef();

        //GetListOfLocation();
        // AddLocation();
        //  AddComment();
        // GetListOfLocation();
        DisplayByLike();

    }
    #endregion

    // Exit if escape (or back, on mobile) is pressed.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }



    #region Add New Location
    public void AddLocation()
    {
        /**
         * remarks
            Will get the name and cordinates later from the user through the maps
         */
        
        mVisitedLocation = new VisitedPlacesInfo("dgdfg", "sdfsda");
        mFirebaseHelper.NewLocation().UpdateChildrenAsync(mVisitedLocation.SaveLocation());
       
    }
    #endregion

    #region Add New Comment
    public void AddComment() {
        //Need to get comment string here from comment post
        //Get user name from Facebook
        mCommentHelper = new CommentHelper("Random Person", "this is nice place", TimeStamp() , "0");
        string key = mFirebaseHelper.CurrentUserRef().Push().Key;
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[key] = mCommentHelper.CommentRS();
        mFirebaseHelper.CommentOnLocationRef().UpdateChildrenAsync(childUpdates);
    }
    #endregion

    
    #region View Comment Based On Likes
    public ArrayList DisplayByLike() {
        mListOfCommentsByLikes = new ArrayList();
        mFirebaseHelper.CommentOnLocationRef().OrderByChild(StringValues.LIKES)
            .ValueChanged += (object sender, ValueChangedEventArgs args) =>
            {
                if (args.DatabaseError != null)
                {

                    Debug.LogError("Databse Error");
                }
                else {
                    if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0) {
                        foreach (var childSnapshot in args.Snapshot.Children)
                        {
                            mCommentHelper = new CommentHelper(childSnapshot.Child(StringValues.USER_NAME).Value.ToString(),
                                                                childSnapshot.Child(StringValues.COMMENT).Value.ToString(),
                                                                childSnapshot.Child(StringValues.TIME_STAMP).Value.ToString(),
                                                                childSnapshot.Child(StringValues.LIKES).Value.ToString());
                            mListOfCommentsByLikes.Add(mCommentHelper.CommentRS());
                        }
                    }
                 }
            };

        Debug.Log(mListOfCommentsByLikes.IndexOf(1).ToString());
        Debug.Log("Displayed By likes");

        return mListOfCommentsByLikes;

    }
    #endregion


    #region View Comment Based on Time
    public void DisplayByTime() {
        mFirebaseHelper.CommentOnLocationRef().OrderByChild(StringValues.TIME_STAMP)
          .ValueChanged += (object sender, ValueChangedEventArgs args) =>
          {
              if (args.DatabaseError != null)
              {

                  Debug.LogError("Databse Error");
              }
              else
              {
                  if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
                  {
                      foreach (var childSnapshot in args.Snapshot.Children)
                      {

                      }
                  }
              }
          };
        Debug.Log("Displayed By Time");
    }
    #endregion

    #region Get List Of Locations
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
                   latitude = childSnapshot.Child(StringValues.LATITUDE).Value.ToString();
                   longtitude = childSnapshot.Child(StringValues.LONGTITUDE).Value.ToString();

                   Debug.Log(latitude + "   "  + longtitude);
                }
                
            }
        };
    }
    #endregion

    public void GetLatitude(string latitude)
    {
        mVisitedLocation.Latitude = latitude;
    }

    public void GetLongitude(string longitude)
    {
        mVisitedLocation.Longitude = longitude;
        Debug.Log(longitude);
    }

    private string TimeStamp() {
        mTimeStamp = DateTime.Now;
        return mTimeStamp.ToString("mm/DD/yyyy");
    }
}


using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Firebase;
using System.Collections.Generic;
using Firebase.Database;
using System;

public class FirebaseFunctions : MonoBehaviour
{
    private const string TAG = "CommentManager";
    private FirebaseHelper mFirebaseHelper;
    private VisitedPlacesInfo mVisitedPlacesInfo;
    private CommentHelper mCommentHelper;
    //private FacebookHelper mFacebookHelper;
    Firebase.Auth.FirebaseAuth mAuth;
    Firebase.Auth.FirebaseUser mUser;

    //For DummyLocation
    private string mLocationInfo;

    private string latitude = "";
    private string longtitude = "";
    private DateTime mTimeStamp;

    private List<Dictionary<string, object>> mListOfLocations;
    private List<Dictionary<string, object>> mListOfCommentsByLikes;
    private List<Dictionary<string, object>> mListOfCommentsByTime;

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.

    #region Start Scene and Initialize Firebase
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
    #endregion

    #region Initialize Firebase and Get Location Name
    void InitializeFirebase()
    {
       
        //Put your location name here from LocationHelper
        mLocationInfo = "Texas Tech";
        mFirebaseHelper = new FirebaseHelper();
        mFirebaseHelper.LocationName = mLocationInfo;
        mFirebaseHelper.DatabaseRef();
        mAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        CreateUser("pujan_567@hotmail.com", "1234sf567");
        AddLocation();
        //  AddComment();
        // GetListOfLocation();
       // DisplayByLike();

    }
    #endregion

    #region Firebase Create User
    public void CreateUser(string email, string password) {
        mAuth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
            task =>
            {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    //TODO get user ID
                   // mFirebaseHelper.UserToken = mUser.UserId;
                }
                else {
                    Debug.LogError(task.ToString());
                    Debug.LogError("user creation failed");
                }
            });

    }
    #endregion

    #region Sign in Firebase Account
    public void SignInUser(string email, string password) {
        mAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
            task => 
            {
                if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted) {
                    mFirebaseHelper.UserToken = mUser.UserId;
                }
            });
    }
    #endregion

    #region On Back Button Pressed EXIT Game
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    #endregion

    #region Add New Location
    public void AddLocation()
    {
        /**
         * remarks
            Will get the name and cordinates later from the user through the maps
         */

        mVisitedPlacesInfo = new VisitedPlacesInfo("1313", "sfsdf");
        mFirebaseHelper.NewLocation().UpdateChildrenAsync(mVisitedPlacesInfo.LocationCordinates());

    }
    #endregion

    #region Add New Comment
    public void AddComment()
    {
        //Need to get comment string here from comment post
        //Get user name from Facebook
        mCommentHelper = new CommentHelper("Random Person", "this is nice place", TimeStamp(), "0");
        string key = mFirebaseHelper.UserRef().Push().Key;
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[key] = mCommentHelper.CommentRS();
        mFirebaseHelper.CommentOnLocationRef().UpdateChildrenAsync(childUpdates);
    }
    #endregion

    #region View Comment Based On Likes
    public List<Dictionary<string, object>> DisplayByLike()
    {
        mListOfCommentsByLikes = new List<Dictionary<string, object>>();
        mFirebaseHelper.CommentOnLocationRef().OrderByChild(StringValues.LIKES)
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
                            mCommentHelper = new CommentHelper(childSnapshot.Child(StringValues.USER_NAME).Value.ToString(),
                                                                childSnapshot.Child(StringValues.COMMENT).Value.ToString(),
                                                                childSnapshot.Child(StringValues.TIME_STAMP).Value.ToString(),
                                                                childSnapshot.Child(StringValues.LIKES).Value.ToString());
                            mListOfCommentsByLikes.Add(mCommentHelper.CommentRS());
                        }
                    }
                }
            };

        //Debug.Log(mListOfCommentsByLikes.IndexOf(1).ToString());
        Debug.Log("Displayed By likes");

        return mListOfCommentsByLikes;

    }
    #endregion

    #region View Comment Based on Time
    public List<Dictionary<string, object>> DisplayByTime()
    {
        mListOfCommentsByTime = new List<Dictionary<string, object>>();
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
                          mCommentHelper = new CommentHelper(childSnapshot.Child(StringValues.USER_NAME).Value.ToString(),
                                                               childSnapshot.Child(StringValues.COMMENT).Value.ToString(),
                                                               childSnapshot.Child(StringValues.TIME_STAMP).Value.ToString(),
                                                               childSnapshot.Child(StringValues.LIKES).Value.ToString());
                          mListOfCommentsByTime.Add(mCommentHelper.CommentRS());
                      }
                  }
              }
          };

        Debug.Log("Displayed By Time");

        return mListOfCommentsByTime;
    }
    #endregion

    #region Get List Of Locations
    public List<Dictionary<string, object>> GetListOfLocation()
    {
        mListOfLocations = new List<Dictionary<string, object>>();
        mFirebaseHelper.VisitedLocationRef().ValueChanged += (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError);
            }

            if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
            {
                Debug.Log(args.Snapshot.ChildrenCount);
                foreach (var childSnapshot in args.Snapshot.Children)
                {

                    mVisitedPlacesInfo = new VisitedPlacesInfo(childSnapshot.Child
                        (StringValues.LATITUDE).Value.ToString(), childSnapshot.Child(StringValues.LONGTITUDE).Value.ToString());
                    mListOfLocations.Add(mVisitedPlacesInfo.LocationCordinates());
                }

                /* testing purpose only
                foreach (Dictionary<string, object> locations in mListOfLocations) {
          
                    latitude = locations[StringValues.LATITUDE].ToString();
                    longtitude = locations[StringValues.LONGTITUDE].ToString();
                    Debug.Log(latitude + " " + longtitude);
                }
                */
            }
        };
        return mListOfLocations;
    }
    #endregion

    #region Get TimeStamp
    private string TimeStamp()
    {
        mTimeStamp = DateTime.Now;
        return mTimeStamp.ToString("mm/DD/yyyy");
    }
    #endregion
}


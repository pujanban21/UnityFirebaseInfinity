using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Firebase;
using System.Collections.Generic;
using Firebase.Database;
using System;
using System.Linq;

using System.Threading.Tasks;

public class FirebaseFunctions : MonoBehaviour
{

    private const string TAG = "FirebaseFunctions";
    private FirebaseHelper mFirebaseHelper;
    private VisitedPlacesInfo mVisitedPlacesInfo;
    private CommentHelper mCommentHelper;
    private User mUser;
    private bool isSignedInSuccessful = false;

    //For DummyLocation
    private string mLocationInfo;
    private string email;
    private string passowrd;
    private string username;
    private string latitude = "";
    private string longtitude = "";
    private bool T = false;
    public List<Dictionary<string, object>> mListOfLocations;
    public List<Dictionary<string, object>> mListOfCommentsByLikes;
    public List<Dictionary<string, object>> mListOfCommentsByTime;

    #region booleans for registration
    public bool createUserBoolean = true;
    public bool userHasBeenCreated = false;
    #endregion

    public bool loggedIn = false;

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
            FirebaseApp.FixDependenciesAsync().ContinueWith(task =>
            {
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

        //Put your location name here from LocationHelp
        mLocationInfo = "Eiffel Tower";
        mFirebaseHelper = new FirebaseHelper();
        mFirebaseHelper.LocationName = mLocationInfo;
        mFirebaseHelper.DatabaseRef();

        Debug.Log("db initialized");

        //AddComment();
        UserInfo("e", "3@hotmail.com", "46465");
        // SignInUser("3@hotmail.com", "46465");
        // GetListOfLocation();
        //        UserInfo("pujanban21", "4@hotmail.com", "46465");

    }
    #endregion

    public void setMLocationInfo(string currentLocation)
    {
        mLocationInfo = currentLocation;
    }

    public void UserInfo(string username, string email, string password)
    {
        this.username = username;
        this.email = email;
        this.passowrd = password;
        mFirebaseHelper.UserToken = username;

     //CreateUser(username, email, passowrd);
      AddLocation("21321", "123213");
        
    }

    #region Firebase Create User
    public void CreateUser(string username, string email, string password)
    {


        createUserBoolean = true;
        userHasBeenCreated = false;

        mFirebaseHelper.UserRef().ValueChanged += (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError != null)
            {

                Debug.LogError(args.DatabaseError);
            }


            if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
            {
                Debug.Log(createUserBoolean);
                foreach (var childSnapshot in args.Snapshot.Children)
                {
                    if (username.Equals(childSnapshot.Child(StringValues.USER_NAME).Value.ToString()))
                    {
                        Debug.Log("ff1.cs = " + createUserBoolean);
                        if (!userHasBeenCreated)
                        {
                            createUserBoolean = false;
                        }
                    }
                }
            }

            if (createUserBoolean)
            {


                mUser = new User(username, email, password);
                Dictionary<string, object> updateUser = new Dictionary<string, object>();
                updateUser[username] = mUser.UserInfo();

                Debug.Log("hi");
                mFirebaseHelper.UserRef().UpdateChildrenAsync(updateUser);
                Debug.Log("bye");

                userHasBeenCreated = true;

                Debug.Log("ff2.cs = " + createUserBoolean);
                Debug.Log("User Registered");

            }

        };

    }
    #endregion

    #region Sign in Firebase Account
    public void SignInUser(string username, string password)
    {
        loggedIn = false;

        mFirebaseHelper.UserRef().ValueChanged += (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError);
            }
            if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
            {
                foreach (var childSnapshot in args.Snapshot.Children)
                {
                    if (username.Equals(childSnapshot.Child(StringValues.USER_NAME).Value.ToString())
                    && password.Equals(childSnapshot.Child(StringValues.PASSWORD).Value.ToString()))
                    {
                        Debug.Log("Sign In Successful");
                        loggedIn = true;
                    }
                }

            }
        };
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
    public void AddLocation(string latitude, string longtitude)
    {
        //TODO Will get the name and cordinates later from the user through the maps
        mVisitedPlacesInfo = new VisitedPlacesInfo(latitude, longtitude);
        Dictionary<string, object> visitedDict = mVisitedPlacesInfo.LocationCordinates();
        //		InitializeFirebase();
        mFirebaseHelper.NewLocation().UpdateChildrenAsync(visitedDict);
    }


    #endregion

    #region Add New Comment
    public void AddComment(string location, string username, string comment)
    {
        //Need to get comment string here from comment post
        //Get user name from Facebook

        mLocationInfo = location;

        string key = mFirebaseHelper.UserRef().Push().Key;

        mCommentHelper = new CommentHelper(username, comment, TimeStamp(), "0");

        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[key] = mCommentHelper.CommentRS();
        mFirebaseHelper.CommentOnLocationRef().UpdateChildrenAsync(childUpdates);
    }
    #endregion

    #region View Comment Based On Likes
    public List<Dictionary<string, object>> DisplayByLike()
    {

        Debug.Log(mLocationInfo);
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
                            mCommentHelper = new CommentHelper(
                                childSnapshot.Child(StringValues.USER_NAME).Value.ToString(),
                                childSnapshot.Child(StringValues.COMMENT).Value.ToString(),
                                childSnapshot.Child(StringValues.TIME_STAMP).Value.ToString(),
                                childSnapshot.Child(StringValues.LIKES).Value.ToString());

                            Debug.Log(childSnapshot.Child(StringValues.USER_NAME).Value.ToString());

                            Dictionary<string, object> commentToDict = mCommentHelper.CommentRS();
                            Debug.Log("username: " + commentToDict[StringValues.USER_NAME]);
                            mListOfCommentsByLikes.Add(commentToDict);

                        }

                    }
                }
            };

        Debug.Log("!!!!Displayed By likes");

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
                          mCommentHelper = new CommentHelper(
                            childSnapshot.Child(StringValues.USER_NAME).Value.ToString(),
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
                    mVisitedPlacesInfo = new VisitedPlacesInfo(
                        childSnapshot.Child(StringValues.LATITUDE).Value.ToString(),
                        childSnapshot.Child(StringValues.LONGTITUDE).Value.ToString());
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
    public string TimeStamp()
    {
        var mTimeStamp = DateTime.Now;
        return mTimeStamp.ToString("dd/MM/yyyy");
    }
    #endregion

    public void CheckUser(bool T)
    {
        if (T) T = true;
        else T = false;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;
using Firebase;
using System.Collections.Generic;
using System;
using Firebase.Database;

public class CommentManager : MonoBehaviour {

    private const string TAG = "CommentManager";
    private const int NEW_COMMENT = 0;
    public Text locationText;

	public Dropdown sortOption;

    private FirebaseHelper mFirebaseHelper;
    private VisitedPlaces_Info mVisitedLocation;
    private CommentHelper mCommentHelper;
    private FirebaseInitiliazer mFirebaseInitializer;
    private CommentPost mCommentPost;
    private string mLocationInfo;
    private DateTime mTimeStamp;

    private string mLikes;
    private string mComment;
    private string mDate;

    void Start(){

        mFirebaseInitializer = new FirebaseInitiliazer();
        mFirebaseHelper = new FirebaseHelper();
        // mFacebookHelper.AddLogButton();
        mLocationInfo = "Dallas";
        Debug.Log(TAG + "start");
        mFirebaseHelper.LocationName = mLocationInfo;
        mFirebaseHelper.DatabaseRef();

        //PostComment();
        locationText.text = LevelManager.currentLocation;
		Dropdown sortOpt = sortOption.GetComponent<Dropdown>();

		sortOpt.onValueChanged.AddListener(DisplayOption);
	}

	public void DisplayOption (int option)
	{
		if (option == 0) {
			DisplayByLike ();
		} else if (option == 1) {
			DisplayByTime ();
		}
	}

	public void DisplayByLike(){
        //TODO: PUJANS CODE
        mFirebaseHelper.CommentOnLocationRef().OrderByChild("Likes")
            .ValueChanged += (object sender, ValueChangedEventArgs args) =>
            {
                if (args.DatabaseError != null) {
                    Debug.Log("Error");
                }
                else
                {
                    if (args.Snapshot != null && args.Snapshot.ChildrenCount >0)
                    {
                        Debug.Log(args.Snapshot.ChildrenCount);
                        foreach (var childSnapshot in args.Snapshot.Children)
                        {

                            /* Need to get a better data structure to store all the location*/
                            mLikes = childSnapshot.Child("Likes").Value.ToString();
                            mComment = childSnapshot.Child("comment").Value.ToString();
                            mDate = childSnapshot.Child("TimeStamp").Value.ToString();
                            Debug.Log(mLikes + "   " + mComment+ "  " + mDate);
                        }

                    }
                }

            };
		Debug.Log("Displayed by like");
	}

	public void DisplayByTime ()
	{
        //TODO: PUJANS CODE
        mFirebaseHelper.CommentOnLocationRef().OrderByChild("TimeStamp")
            .ValueChanged += (object sender, ValueChangedEventArgs args) =>
            {
                if (args.DatabaseError != null)
                {
                    Debug.Log("Error");
                }
                else
                {
                    if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
                    {
                        Debug.Log(args.Snapshot.ChildrenCount);
                        foreach (var childSnapshot in args.Snapshot.Children)
                        {

                            /* Need to get a better data structure to store all the location*/
                            mLikes = childSnapshot.Child("Likes").Value.ToString();
                            mComment = childSnapshot.Child("comment").Value.ToString();
                            mDate = childSnapshot.Child("TimeStamp").Value.ToString();
                            Debug.Log(mLikes + "   " + mComment + "  " + mDate);
                        }

                    }
                }

            };
        Debug.Log("Displayed by time");
	}

    #region Add New Comment In Firebase
    public void PostComment()
    {
        mCommentPost = new CommentPost();
        mCommentHelper = new CommentHelper(mCommentPost.CommentValue, TimeStamp(), NEW_COMMENT);
        string key = mFirebaseHelper.CurrentUserRef().Push().Key;
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[key] = mCommentHelper.CommentRS();
        mFirebaseHelper.CommentOnLocationRef().UpdateChildrenAsync(childUpdates);
    }
    #endregion

    #region Current Time Stamp
    private DateTime TimeStamp()
    {
        mTimeStamp = DateTime.Today;
        return mTimeStamp;
    }
    #endregion

}

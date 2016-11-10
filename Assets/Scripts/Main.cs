using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Firebase;
using System.Collections.Generic;

public class Main : MonoBehaviour {

    private FirebaseHelper mFirebaseHelper;
    private VisitedPlaces_Info mVisitedLocation;

    private string latitude = "";
    private string longtitude = "";

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
        Debug.Log("here");
        mFirebaseHelper = new FirebaseHelper();
        mFirebaseHelper.DatabaseRef();
        AddLocation();

    }

    // Exit if escape (or back, on mobile) is pressed.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    // A realtime database transaction receives MutableData which can be modified
    // and returns a TransactionResult which is either TransactionResult.Success(data) with
    // modified data or TransactionResult.Abort() which stops the transaction with no changes.


    public void AddLocation()
    {
        string name = "dallas";
        mVisitedLocation = new VisitedPlaces_Info("dgdfg", "sdfsda");
        string key = mFirebaseHelper.CurrentUserRef().Push().Key;
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[name] = mVisitedLocation.SaveLocation();
        mFirebaseHelper.VisitedLocationRef().UpdateChildrenAsync(childUpdates);
       
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
}


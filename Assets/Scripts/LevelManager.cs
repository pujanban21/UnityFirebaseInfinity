using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public float AutoLoadNextLevelAfter;
	public static string currentLocation= "none";
	public static bool visitedMode = false;

	public static float userLatitude;
	public static float userLongitude;

	private static int previousScene = 2;

	void Awake(){
		//if user location is not yet detected, set user location 
		if (userLatitude == 0 && userLongitude == 0){
			StartCoroutine(GetLocation());
		}
	}

	IEnumerator GetLocation ()
	{

		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser) {
			Debug.Log ("Please enable location service.");
			userLatitude = 48.8584f; //48.8584f
			userLongitude = 2.2945f; //2.2945f
			yield break;
		}

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
			string result = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
			userLatitude = Input.location.lastData.latitude;
			userLongitude = Input.location.lastData.longitude;
			Debug.Log(result);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

	void Start ()
	{

		if (Application.loadedLevel == 0) {
			Invoke ("LoadNextLevel", AutoLoadNextLevelAfter);
		} else if (Application.loadedLevelName == "05 Info") {
			Invoke ("LoadNextLevel", 0.5f);
		}

		if (Application.loadedLevelName == "03 ARCamera" || Application.loadedLevelName == "04 Visited" ){
			previousScene = Application.loadedLevel;
		}

	}

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		Debug.Log("prev scene:" + previousScene);
		Application.LoadLevel (name);
	}

	public void LoadPrevLevel(){
		Debug.Log("Load Prev Level requested");
		Application.LoadLevel(previousScene);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}

	public void LoadNextLevel(){
		Debug.Log("Load Next Level requested");
		Debug.Log(previousScene);
		Application.LoadLevel(Application.loadedLevel + 1);
	}

}

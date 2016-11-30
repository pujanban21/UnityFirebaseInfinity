using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;
using System;
using System.Collections.Generic;

public class CommentPost : MonoBehaviour {

	[SerializeField]
	private InputField input;
	[SerializeField]
	private Button postButton;
	[SerializeField]
	private Text confirmationText;

   
    private string comment;
    private DateTime mTimeStamp;
    private bool mNotfiy = false;

    public CommentPost() {}

	void Start(){
		Button postBtn = postButton.GetComponent<Button>();
		postBtn.onClick.AddListener(Post);
	}

	void Post(){

		comment = input.text;

		if (comment.Trim() == ""){ //comment cannot be empty or consists of just spaces.
			Debug.LogFormat("Please enter a valid comment.");

			confirmationText.text = "Please enter a valid comment.";
			confirmationText.color = new Color(0.784f, 0.302f, 0.247f);

		}else{ 
			//otherwise, post the comment.
			Debug.Log(input.text);
			//if succeed
			input.text = "";
            mNotfiy = true;
			confirmationText.text = "Thanks, your comment has been posted!";
			confirmationText.color = new Color(0.541f, 0.831f, 0.561f);
          	//otherwise
//			confirmationText.text = "Posting error, try again.";
//			confirmationText.color = new Color(0.784f, 0.302f, 0.247f);
		}
	}

    public string CommentValue { 
       get { return comment; }
       set { comment= value; }
    }

    public bool Notify {
        get {return mNotfiy; }
        set {mNotfiy= value; } }
}

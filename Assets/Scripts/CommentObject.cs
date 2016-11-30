using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommentObject : MonoBehaviour {

	private int commentID;
	private rateType rating;

	[SerializeField]
	private Text UserName;
	[SerializeField]
	private Text TimeStamp;
	[SerializeField]
	private Text CommentContent;
	[SerializeField]
	private Text LikeNum;
	[SerializeField]
	private Button LikeButton;
	[SerializeField]
	private Button DislikeButton;

	void Start(){
		Button Like = LikeButton.GetComponent<Button>();
		Like.onClick.AddListener(LikeComment);

		Button Dislike = DislikeButton.GetComponent<Button>();
		Dislike.onClick.AddListener(DislikeComment);

		rating = rateType.none;
	}

	public void LikeComment ()
	{
		if (rating == rateType.none) { //if not yet rated
			IncrementLike();
			rating = rateType.liked;
			LikeButton.GetComponent<Image>().color = new Color(0.541f, 0.831f, 0.561f);

		}else if (rating == rateType.liked){ //undo like
			DecrementLike();
			rating = rateType.none;
			LikeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
		}

		//otherwise, cannot rate more than once.
	}

	public void DislikeComment ()
	{
		if (rating == rateType.none) { //if not yet rated
			DecrementLike();
			rating = rateType.disliked;
			DislikeButton.GetComponent<Image>().color = new Color(0.784f, 0.302f, 0.247f);
		}else if (rating == rateType.disliked){ //undo dislike
			IncrementLike();
			rating = rateType.none;
			DislikeButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
		}

		//otherwise, cannot rate more than once.
	}

	private void IncrementLike ()
	{
		int updatelike = int.Parse (LikeNum.text) + 1;
		LikeNum.text = updatelike.ToString ();

		Debug.Log ("like increment");
	}

	private void DecrementLike ()
	{
		int updatelike = int.Parse (LikeNum.text) - 1;
		LikeNum.text = updatelike.ToString ();

		Debug.Log ("like decrement");
	}

	public void SetID(int id){
		commentID = id;
	}

	public void SetTimeStamp (string timeStamp)
	{
		
	}

	public void SetUserName(string userName){

	}

	public void SetCommentContent (string commentContent)
	{

	}

}

public enum rateType{
	liked,
	none,
	disliked
}

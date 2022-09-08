using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemDrop : MonoBehaviour {
	
	[Range(0f,100f)]
	public float dropRate = 10f;

	public float timeToDisappear = 2f;

	public float rotationSpeed = 1f;

	public float animationTime = 1f;

	public  abstract void OnItemPick (GameObject player);





	protected virtual  void Start () {

		DropUpAnimation ();
		Invoke ("DisappearAnimation",timeToDisappear);
	
	}
	

	private void DropUpAnimation() {

		Hashtable hashtableUp = new Hashtable ();
		hashtableUp.Add ("y", 2.5f);
		hashtableUp.Add ("time", animationTime);
		hashtableUp.Add ("easetype", iTween.EaseType.easeOutElastic);
		hashtableUp.Add ("ignoretimescale", true);
		hashtableUp.Add ("oncomplete", "OnCompleteUpAnimation");

		iTween.MoveTo (gameObject, hashtableUp);
	
	}


	private void OnCompleteUpAnimation() {

		Hashtable hashtableDown = new Hashtable ();
		hashtableDown.Add ("y", 0.5f);
		hashtableDown.Add ("time", animationTime);
		hashtableDown.Add ("easetype", iTween.EaseType.linear);
		hashtableDown.Add ("ignoretimescale", true);
		hashtableDown.Add ("oncomplete", "OnCompleteDropAnimation");

		iTween.MoveTo (gameObject,hashtableDown);

	}




	private void OnCompleteDropAnimation() {

		GetComponent<SphereCollider> ().enabled = true;

	}




	protected virtual void Update () {
		
		transform.Rotate(Vector3.up * Time.unscaledDeltaTime * rotationSpeed, Space.Self);

	}


	protected virtual void OnTriggerEnter(Collider collider) {

		if (collider.tag == Constants.PLAYER_TAG) {

			OnItemPick (collider.gameObject);
		}

	}



	private void DisappearAnimation() {

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("scale", Vector3.zero);
		hashtable.Add ("time", 1f);
		hashtable.Add ("easetype", iTween.EaseType.linear);
		hashtable.Add ("ignoretimescale", true);
		hashtable.Add ("oncomplete", "OnCompleteDisappearAnimation");

		iTween.ScaleTo (gameObject, hashtable);

	}

	private void OnCompleteDisappearAnimation() {

		GameObject.Destroy (gameObject);
	}

}

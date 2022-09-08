using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoney : ItemDrop {

	public int value = 1;

	private AudioSource audioSource;


	private void Start() {

		base.Start ();
		audioSource = GetComponent<AudioSource> ();

	}

	public override void OnItemPick(GameObject player) {

		audioSource.Play ();
		AnimateUp ();
	
	}


	private void AnimateUp() {

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("y",2f);
		hashtable.Add ("time", 0.7f);
		hashtable.Add ("easetype",iTween.EaseType.easeOutElastic);
		hashtable.Add ("ignoretimescale", true);
		hashtable.Add ("oncomplete", "ScaleDown");

		iTween.MoveTo (gameObject,hashtable);

	}

	private void ScaleDown() {

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("scale",Vector3.zero);
		hashtable.Add ("time", 0.7f);
		hashtable.Add ("easetype",iTween.EaseType.easeOutElastic);
		hashtable.Add ("ignoretimescale", true);
		hashtable.Add ("oncomplete", "OnAnimationFinish");

		iTween.ScaleTo (gameObject,hashtable);
	}

	private void OnAnimationFinish() {

		GameController.AddBattery (value);
		GameObject.Destroy (gameObject);
	
	}


	//move to huds
//	HudsController hudscontroller = GameObject.FindObjectOfType<HudsController> ();
//
//	Vector3 screenPoint = hudscontroller.txtPoints.transform.position + new Vector3(0,0,5);  //the "+ new Vector3(0,0,5)" ensures that the object is so close to the camera you dont see it
//	Vector3 worldPos = Camera.main.ScreenToWorldPoint( screenPoint );
//
//
//	Debug.Log ("pos = " + hudscontroller.txtPoints.transform.position);
//	Debug.Log ("screenPoint = " + screenPoint);
//	Debug.Log ("worldPos = " + worldPos);
//
//
//	Hashtable hashtable = new Hashtable ();
//	hashtable.Add ("position", worldPos);
//
//
//	iTween.MoveTo (gameObject,hashtable);

}

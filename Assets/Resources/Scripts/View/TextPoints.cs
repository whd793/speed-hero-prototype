using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPoints : MonoBehaviour {

	private TextMesh txtMesh;

	private void Awake() {
		txtMesh = GetComponent<TextMesh> ();
	}

	private void Start () {
		AnimateUp ();
	}


	public void SetText(string text) {

		txtMesh.text = text;

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

		GameObject.Destroy (gameObject);
	
	}

}

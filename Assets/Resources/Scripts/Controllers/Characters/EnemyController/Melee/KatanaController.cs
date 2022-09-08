using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaController : MeleeController {

	public BoxCollider attackCollider;
	public TrailRenderer trailRenderer;


	private void Start () {
		base.Start ();
	}

	public override void Attack() {

		attackCollider.enabled = true;
		trailRenderer.enabled = true;

//		Debug.Log ("Attack");
	}

	public override void InitAttack() {


	}

	public override void EndAttack() {

		attackCollider.enabled = false;
		trailRenderer.enabled = false;

//		Debug.Log ("EndAttack");
	}


}

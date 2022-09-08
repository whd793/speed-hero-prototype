using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : ItemDrop {

	public int healValue = 1;

	public ParticleSystem particleSystem;


	public override void OnItemPick(GameObject player) {

		GetComponent<MeshRenderer> ().enabled = false;
		particleSystem.Play ();

		Health playerHealth = player.GetComponent<Collider>().gameObject.GetComponent<Health> ();

		if (!playerHealth.isDead && playerHealth.currentHealth < playerHealth.maxHealth) {
			playerHealth.RecoverHealth (healValue);
		}


		GameObject.Destroy (gameObject, 1f);
	}






//	private void OnTriggerEnter(Collider collider) {
//
//		if (collider.transform.tag == Constants.PLAYER_TAG) {
//		}
//
//
//		base.OnTriggerEnter (collider);
//	}
}

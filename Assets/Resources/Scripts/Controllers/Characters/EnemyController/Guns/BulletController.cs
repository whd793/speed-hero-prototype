using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletSpeed {
	
	public float minSpeed, maxSpeed;
}


public abstract class BulletController : MonoBehaviour {

	public float speed;
	public int damage;
	public float timeToDisappear = 2f;

	public Vector3 direction;

	public BulletSpeed bulletSpeed;


	protected virtual void Start () {

		Physics.IgnoreLayerCollision (8, 9);
	

		GameObject.Destroy (gameObject, timeToDisappear);

	}
	

	protected virtual void Update () {

		float percentage = TimeController.currentTimeScale * 100f;

		speed = (percentage * bulletSpeed.maxSpeed) / 100f;
		speed = Mathf.Clamp (speed, bulletSpeed.minSpeed, bulletSpeed.maxSpeed);

direction.y = 0;
		gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, gameObject.transform.position + direction, 
			Time.timeScale * speed);

	}


	protected virtual void OnTriggerEnter(Collider collider) {

//		Debug.Log ("collider = " + collider);


		if (collider.tag == Constants.PLAYER_TAG) {
			GameObject.Destroy (gameObject);	
		}



	}
}

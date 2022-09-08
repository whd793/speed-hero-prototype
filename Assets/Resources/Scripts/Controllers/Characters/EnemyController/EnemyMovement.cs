using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public float movementSpeed = 1f;
	public float lookSpeed = 1f;

	public bool canMove = true;

	public float movementRange = 2f;



	private Transform target;




	private void Start () {

		GameObject targetObj = GameObject.FindGameObjectWithTag ("Player");

		if (targetObj != null) {
			target = targetObj.transform;
		}


	}
	

	private void Update () {

		if (canMove) {
			Movement ();
			LookToTarget ();
		}


	}


	private void Movement() {

		Vector3 direction = (gameObject.transform.position - target.position).normalized;
		Vector3 destination = target.position + direction * movementRange;

		float distance = Vector3.Distance (gameObject.transform.position, destination);

		if (distance > movementRange) {
			gameObject.transform.position = Vector3.Lerp (transform.position, destination, Time.deltaTime * movementSpeed);
		}

	}


	private void LookToTarget() {

		if (target == null) {
			return;
		}

		Vector3 relativePos = target.position - transform.position;
		Quaternion rotationQuaternion = Quaternion.LookRotation(relativePos);

		transform.rotation = Quaternion.Slerp (transform.rotation, rotationQuaternion, Time.deltaTime * lookSpeed);
	}

}

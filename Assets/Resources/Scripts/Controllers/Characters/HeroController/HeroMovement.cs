using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class MovementBoundary {
	public float xMin, xMax, zMin, zMax;
}


public class HeroMovement : MonoBehaviour {

	public bool canMove = true;
	public float movementSpeed = 10f;

	public Vector3 inputPosition;


	public float currentSpeed = 1f;
	public float maxSpeed = 1.2f;

	private float oldAngle,currentAngle;

	public MovementBoundary movementBoundary;


	private void Start () {
		
	}
	

	private void Update () {

		Movement ();

	
	}




	private void Movement() {

//		float h = Input.GetAxis ("Horizontal");
//		float v = Input.GetAxis ("Vertical");
//
//		Vector3 input = new Vector3 (h, 0 , v);
		Vector3 input = new Vector3 (inputPosition.x, 0 , inputPosition.y);

		if (input != Vector3.zero && canMove) {

			Vector3 direction = gameObject.transform.parent.position + input;

			Vector3 nextPosition  = Vector3.Lerp (gameObject.transform.parent.position, direction, Time.unscaledDeltaTime * movementSpeed);


			gameObject.transform.parent.position = new Vector3 (Mathf.Clamp (nextPosition.x, movementBoundary.xMin, movementBoundary.xMax)
														,nextPosition.y,
														Mathf.Clamp (nextPosition.z, movementBoundary.zMin, movementBoundary.zMax));

			Turning (input.x, input.z);
		}

					Vector3 directionz = gameObject.transform.parent.position + input;

			Vector3 nextPositiosn  = new Vector3 (gameObject.transform.parent.position.x, gameObject.transform.parent.position.y, gameObject.transform.parent.position.z);

// if (nextPositiosn.x < 3f && nextPositiosn.x > -3f){
// 	movementBoundary.zMax = 12.5f;
// } else {
// 		movementBoundary.zMax = 11f;

// }

	}


	private void Turning (float h, float v) {	

		if (currentAngle != 0 && currentAngle != -180) {
			oldAngle = currentAngle;
		}

		currentAngle = Angle (new Vector2 (h, v));

		if (h != 0 || v != 0) {

			gameObject.transform.parent.eulerAngles = new Vector3(0, currentAngle, 0);

		} else if (h == 0 && v == 0) {

			gameObject.transform.parent.eulerAngles = new Vector3(0, oldAngle, 0);
		}
	
	}

	private  float Angle(Vector2 p_vector2) {

		if (p_vector2.x < 0) {
			return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
		} else {
			return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
		}
	}















}

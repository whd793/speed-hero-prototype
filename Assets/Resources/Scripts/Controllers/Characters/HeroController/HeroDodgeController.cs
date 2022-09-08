using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDodgeController : MonoBehaviour {

	public int dodgePoints = 10;

	[SerializeField]
	private float fieldOfViewAngle = 110f;  

	//Delegate
	public delegate void OnDodgeBullet(int leftOrRight, Vector3 dodgePosition);
	public event  OnDodgeBullet onDodgeBullet;


	private SoundFX soundFx;

	// Use this for initialization
	private void Start () {

		soundFx = GetComponent<SoundFX> ();

	}
	



	private void OnTriggerEnter(Collider collider) {



		if (collider.tag == Constants.BULLET_TAG) {
			
			if (IsInSight (collider.gameObject, gameObject)) {

				var relativePoint = transform.InverseTransformPoint(collider.transform.position);

				int leftOrRight = 0;
				float stereo = 0f;

				if (relativePoint.x < 0f) {
					leftOrRight = 1; //left
					stereo = -0.7f;
				} else if (relativePoint.x > 0f) {
					leftOrRight = 2; //right
					stereo = 0.7f;
				}

				soundFx.GetAudioSource ().panStereo = stereo;
				soundFx.PlaySound ();

				if (onDodgeBullet != null) {
					onDodgeBullet (leftOrRight, collider.gameObject.transform.position);
				}
			}
		}
	}

	private bool IsInSight(GameObject target,GameObject objectLooker) {

		Vector3 direction = target.transform.position - objectLooker.transform.position;
		float angle = Vector3.Angle(direction, transform.forward);

		if (angle < fieldOfViewAngle * 0.5f) {
			return true;
		}

		return false;
	}
}

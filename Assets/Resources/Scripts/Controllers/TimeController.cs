using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

	public bool canTimeSlow = true;

	public float timeScale = 0.1f;
	private float initTimeScale;

	public Vector3 timeScaleValue;



	private float maxDistance = 1f;
	public float maxTimeScale = 1.2f;


	public static float currentTimeScale;

	private void Awake() {
		
	}



	private void Start () {

		if (canTimeSlow) {

			initTimeScale = timeScale;

		} else {
			
			timeScale = 1;
		}
	}
	


	private void Update () {

		if (canTimeSlow) {

			if (timeScaleValue == Vector3.zero) {
				timeScale = initTimeScale;

			} else {

//				Debug.Log ("timeScaleValue = "+ timeScaleValue);

				float x = Mathf.Abs (timeScaleValue.x);
				float y = Mathf.Abs (timeScaleValue.y);

				float valueInput = 0;

				if (x >= y) {
					valueInput = x;
				} else {
					valueInput = y;
				}

				float distancePercentage = (valueInput * 100f) / maxDistance;

				currentTimeScale = (maxTimeScale * distancePercentage) / 100f;
				timeScale = currentTimeScale;

				if (timeScale < initTimeScale) {
					timeScale = initTimeScale;
				}
			}
		} 


		Time.timeScale = timeScale;

	}

}

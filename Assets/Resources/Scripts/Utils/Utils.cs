using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

	//Returning value in pixels
	public static Vector3 CenterInputCoordinates(Vector3 currentInputPosition) {

		currentInputPosition.x -= Screen.width / 2;
		currentInputPosition.y -= Screen.height / 2;

		return currentInputPosition;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	public bool canInput;
	public bool holdingInput;

	[SerializeField]
	private int MovementRange = 100;

	private Vector3 inputInitPoint;

	//Delegate
	//Values in pixels
	public delegate void OnClick(Vector3 position);
	public event  OnClick onClick;

	public delegate void OnInputPositionChanged(Vector3 inputPosition);
	public event  OnInputPositionChanged onInputPositionChanged;

	public delegate void OnClickReleased(Vector3 position);
	public event  OnClickReleased onClickReleased;


	private void Start () {
		
	}


	private void Update () {
	
		if (canInput) {
			MouseInput ();
		}
	}

	private bool MouseHold() {

		if (Input.GetMouseButtonDown (0)) {

			inputInitPoint = Input.mousePosition;
			holdingInput = true;

			if (onClick != null) {
				onClick (inputInitPoint);
			}
		}


		if (Input.GetMouseButtonUp (0)) {

			holdingInput = false;

			if (onClickReleased != null) {
				onClickReleased (Input.mousePosition);
			}
		}

		return holdingInput;
	}


	private void MouseInput() {

		if (!MouseHold()) {
			return;
		}


		float deltaX =  (Input.mousePosition.x - inputInitPoint.x);
		deltaX = Mathf.Clamp(deltaX, -MovementRange, MovementRange);

		float deltaY =  (Input.mousePosition.y - inputInitPoint.y);
		deltaY = Mathf.Clamp(deltaY, -MovementRange,  MovementRange);


		Vector3 value = new Vector3(inputInitPoint.x + deltaX, inputInitPoint.y + deltaY);


//		Debug.Log ("deltaX = " + deltaX);
//		Debug.Log ("deltaY = " + deltaY);


		var delta = inputInitPoint - value;
		delta.x = -delta.x;
		delta.y = -delta.y;

		delta /= MovementRange;
	

		if (onInputPositionChanged != null) {
			onInputPositionChanged (delta);
		}

	}








}

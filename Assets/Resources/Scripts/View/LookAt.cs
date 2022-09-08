using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	private Transform target;


	private void Start () {

		target = Camera.main.gameObject.transform;

	}


	private	void Update () {

		transform.rotation = target.rotation;

	}
}

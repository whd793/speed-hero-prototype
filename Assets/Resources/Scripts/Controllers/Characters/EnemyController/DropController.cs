using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour {

	public GameObject[] itensDrop;


	// Use this for initialization
	void Start () {
		
	}
	



	public void Drop() {

		float dropValue = Random.Range (0f, 100f);

		for (int i = 0; i < itensDrop.Length; i++) {

			if (dropValue <= itensDrop [i].GetComponent<ItemDrop> ().dropRate) {

				GameObject.Instantiate (itensDrop [i], gameObject.transform.position, Quaternion.identity);

			}
		}
	}

}

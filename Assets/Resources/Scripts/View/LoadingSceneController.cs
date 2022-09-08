using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {

		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync (1);

	}
	

}

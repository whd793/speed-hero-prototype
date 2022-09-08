using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundController : MonoBehaviour {

	private AudioSource audioSource;

	public bool playBGByTime = false;

	public float timeScalePitch = 1f;
	public float minTimeScalePitch = 0.5f;
	public float maxTimeScalePitch = 1f;

	private CameraController cameraController;

	public AudioClip menuMusic;
	public AudioClip[] gameplayMusics;

	private int gameplayMusicIndex = 0;


	private void Awake() {
		
		audioSource = GetComponent<AudioSource> ();
		cameraController = GameObject.FindObjectOfType<CameraController> ();

		GameController.onGameStatusChanged += GameControllerOnGameStatusChanged;
	}



	private void GameControllerOnGameStatusChanged (GameController.GameStatus gameStatus) {

		if (gameStatus == GameController.GameStatus.Playing) {

			FadeOutMenuMusic ();

		} else if (gameStatus == GameController.GameStatus.Menu) {

			playBGByTime = false;

			audioSource.time = 0;
			audioSource.clip = menuMusic;
			audioSource.Play ();

		}

	}


	private void FadeOutMenuMusic() {

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("volume", 0);
		hashtable.Add ("time", 1);
		hashtable.Add ("oncomplete","FadeOutMenuMusicComplete");

		iTween.AudioTo (audioSource.gameObject,hashtable);
	}

	private void FadeOutMenuMusicComplete() {

		audioSource.time = 0;
		audioSource.volume = 0.7f;

		gameplayMusicIndex = Random.Range (0, gameplayMusics.Length - 1 );

		audioSource.clip = gameplayMusics [gameplayMusicIndex];
		audioSource.Play ();

		playBGByTime = true;
	}


	private void Start () {

		cameraController.onCinemaEffectInit += CameraControllerOnCinemaEffectInit;
		cameraController.onCinemaEffectEnd += CameraControllerOnCinemaEffectEnd;

	
	}

	private void CameraControllerOnCinemaEffectInit () {

		playBGByTime = false;
		timeScalePitch = 1;
	}

	private void CameraControllerOnCinemaEffectEnd () {

		playBGByTime = true;

	}



	private void Update () {

		if (playBGByTime) {

			if (Time.timeScale < minTimeScalePitch) {
				audioSource.pitch = minTimeScalePitch;
			} else if (Time.timeScale > maxTimeScalePitch) {
				audioSource.pitch = maxTimeScalePitch;
			} else {

				timeScalePitch = Time.timeScale;
				audioSource.pitch = timeScalePitch;
			}

			if (!audioSource.isPlaying) {

				if (gameplayMusicIndex < gameplayMusics.Length - 1) {
					gameplayMusicIndex++;
				} else {
					gameplayMusicIndex = 0;
				}

				audioSource.clip = gameplayMusics [gameplayMusicIndex];
				audioSource.Play ();
			}


		} else {

			audioSource.pitch = timeScalePitch;
		}

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour {

	[SerializeField]
	private float minTimeScalePitch = 0.5f;
	[SerializeField]
	private float maxTimeScalePitch = 1f;

	private AudioSource audioSource;


	public AudioSource GetAudioSource() {
		return audioSource;
	}

	private void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	

	private void Update () {

		if (Time.timeScale < minTimeScalePitch) {
			audioSource.pitch = minTimeScalePitch;
		} else if (Time.timeScale > maxTimeScalePitch) {
			audioSource.pitch = maxTimeScalePitch;
		} else {
			audioSource.pitch = Time.timeScale;
		}

	}


	public void PlaySound() {

		audioSource.Play ();
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTrailController : MonoBehaviour {

	public Transform target;
	public float speed = 1f;

	public float animationSpeed = 1f;

	private HeroController heroController;

	private HeroAnimationController heroAnimationController;

	private void Start () {

		heroController = target.GetChild(0).gameObject.GetComponent<HeroController> ();
		heroController.heroAttack.onAttack += HeroControllerHeroAttackOnAttack;


		heroController.heroDodgeController.onDodgeBullet += HeroDodgeControllerOnDodgeBullet;

		HeroController.onHeroStatusChanged += OnHeroStatusChanged;

		heroAnimationController = GetComponent<HeroAnimationController> ();

		GetComponent<Animator> ().speed = animationSpeed; 

	}

	private void OnHeroStatusChanged (HeroController.HeroStatus heroStatus) {

		if (heroStatus == HeroController.HeroStatus.Alive) {
			gameObject.SetActive (true);
		} else if (heroStatus == HeroController.HeroStatus.Dead) {
			gameObject.SetActive (false);
		}else if (heroStatus == HeroController.HeroStatus.End) {
			gameObject.SetActive (false);
		}


	}

	private void HeroControllerHeroAttackOnAttack (int attackType) {
		heroAnimationController.AttackAnimation (attackType);
	}

	private void HeroDodgeControllerOnDodgeBullet (int leftOrRight, Vector3 dodgePosition) {
		heroAnimationController.DodgeBullet (leftOrRight);
	}


	private void Update () {


		gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, target.transform.position, Time.unscaledDeltaTime * speed);
		gameObject.transform.rotation = target.transform.rotation;

	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {

	public enum HeroStatus {
		Alive,
		Dead,
		End,
	}

	private static HeroStatus _CurrentHeroStatus;
	public static HeroStatus CurrentHeroStatus {

		get { return _CurrentHeroStatus; }

		set {

			if (_CurrentHeroStatus != value) {

				_CurrentHeroStatus = value;

				if (onHeroStatusChanged != null) {
					onHeroStatusChanged (_CurrentHeroStatus);
				}
			}
		}
	}

	//Delegate
	public  delegate void OnHeroStatusChanged(HeroStatus heroStatus);
	public static event  OnHeroStatusChanged onHeroStatusChanged;


	private PlayerInput playerInput;
	private TimeController timeController;
    private PlayerController playerController;
    private FlagController flagController;


    public HeroMovement heroMovement;
	public HeroAttackController heroAttack;
	public HeroDodgeController heroDodgeController;

	public HeroAnimationController heroAnimation;

	private Health health;

	private HudsController hudsController;





	private CameraController cameraController;
public GameObject flyingTa;

public float powerz;
// public bool endAttack = false;

	public Health GetHealth() {
		return health;
	}


	private void Awake() {

		playerInput = GameObject.FindObjectOfType<PlayerInput> ();
		timeController = GameObject.FindObjectOfType<TimeController> ();
		hudsController = GameObject.FindObjectOfType<HudsController> ();
		health = GetComponent<Health> ();
		cameraController = GameObject.FindObjectOfType<CameraController> ();
        playerController = GameObject.FindObjectOfType<PlayerController>();
        flagController = GameObject.FindObjectOfType<FlagController>();

    }


    private void Start () {

		playerInput.onClick += PlayerInputOnClick;
		playerInput.onInputPositionChanged += PlayerInputOnInputPositionChanged;
		playerInput.onClickReleased += PlayerInputOnClickReleased;

		heroAttack.onAttack += HeroAttackOnAttack;
		heroAttack.onCombo += HeroAttackOnCombo;
		heroAttack.onComboFinished += HeroAttackOnComboFinished;

		heroAttack.onSpecialInit += HeroAttackOnSpecialInit;
		heroAttack.onSpecialFinished += HeroAttackOnSpecialFinished;

		heroAnimation.onAttackInit += HeroAnimationOnAttackInit;
		heroAnimation.onAttackEnd += HeroAnimationOnAttackEnd;

		heroMovement.maxSpeed = timeController.maxTimeScale;
		heroDodgeController.onDodgeBullet += HeroDodgeControllerOnDodgeBullet;


		health.onDamageTakenPosition += HealthOnDamageTaken;
		health.onHealthRecover += HealthOnHealthRecover;
		health.onDie += HealthOnDie;

		GameController.onGameStatusChanged += GameControllerOnGameStatusChanged;
		onHeroStatusChanged += HeroControllerOnHeroStatusChanged;

/*        PlayerController.onPlayerStateChanged += PlayerControllerOnPlayerStateChanged;
*/
        heroAttack.canAttack = false;
		heroMovement.canMove = false;
	}

	  private void HeroControllerOnHeroStatusChanged (HeroStatus heroStatus) {

		if (heroStatus == HeroStatus.End) {
			
heroAnimation.IdleAnimation ();

            // playerController.canSling = true;

		heroMovement.canMove = false;
            timeController.canTimeSlow = false;
					timeController.timeScale= 1.0f;

// cameraController.CinemaEffectEnding();
		// heroAnimation.StopAnimator (true);
					hudsController.UpdatePowerz();
		}

	}

	private void GameControllerOnGameStatusChanged (GameController.GameStatus gameStatus) {

		if (gameStatus == GameController.GameStatus.Playing) {

			heroAttack.canAttack = true;
			heroMovement.canMove = true;

			gameObject.transform.parent.position = Vector3.zero;
			gameObject.transform.parent.rotation = Quaternion.identity;

			heroAttack.SetSpecialCount (10);
			heroAttack.SetCurrentSpecialCount (0);
			heroAttack.specialChargeComplete = false;

			hudsController.SpecialCharge (0);

			Revive ();
		}

	}

	private void HeroAttackOnAttack (int attackType) {

		heroAnimation.AttackAnimation (attackType);

		if (CurrentHeroStatus != HeroStatus.End && !heroAttack.inSpecialAttack && 20f > Random.Range (0f, 100f)) {
			cameraController.CinemaEffect ();
		}

		if (CurrentHeroStatus == HeroStatus.End) {
			cameraController.CinemaEffectTwo ();
		}
	}

	private void HeroAnimationOnAttackInit (int attackType) {
		heroAttack.AttackInit (attackType);
	}

	private void HeroAnimationOnAttackEnd (int attackType) {
		heroAttack.AttackEnd (attackType);
	}

	private void HeroDodgeControllerOnDodgeBullet (int leftOrRight, Vector3 dodgePosition) {

		if (CurrentHeroStatus == HeroStatus.Alive) {

			heroAnimation.DodgeBullet (leftOrRight);
			hudsController.SpawnTxtPoints ("+" + heroDodgeController.dodgePoints , dodgePosition);
			GameController.AddPoints (heroDodgeController.dodgePoints);
		}
	}

	private void HeroAttackOnCombo (int comboCount) {

	}



	private void HeroAttackOnComboFinished (int comboCount) {

	}


	private void PlayerInputOnClick (Vector3 position) {
		
	}

	private void PlayerInputOnInputPositionChanged (Vector3 inputPosition) {

//		Debug.Log ("Input Position = " + inputPosition);

		heroMovement.inputPosition = inputPosition;
		timeController.timeScaleValue = inputPosition;

		heroMovement.currentSpeed = timeController.timeScale;
	}


	private void PlayerInputOnClickReleased (Vector3 inputPosition) {

		heroMovement.inputPosition = Vector3.zero;
		timeController.timeScaleValue = Vector3.zero;

		heroMovement.currentSpeed = timeController.timeScale;
	}
	



	public void SpecialAttack() {

		if (!health.isDead) {
			heroAttack.SpecialAttack ();
		}
	}

	private void HeroAttackOnSpecialInit () {

		timeController.timeScale = 1f;
		timeController.canTimeSlow = false;

		health.canTakeDamage = false;
	}

	private void HeroAttackOnSpecialFinished () {
		timeController.canTimeSlow = true;
		health.canTakeDamage = true;

		cameraController.CinemaEffect ();
	}

	private void HealthOnDamageTaken (int damage, Vector3 position) {
		DamageTaken (position);
	}


	private void HealthOnHealthRecover (int recovery) {
		hudsController.HealthBar (health.GetCurrentHealthPercentage());
	}

	private void DamageTaken(Vector3 position) {

		CameraController.CameraShake (0.1f);

		hudsController.HealthBar (health.GetCurrentHealthPercentage());

		hudsController.DamageImageFadeIn ();
		heroAnimation.DamageTaken (position);
	}


	private void HealthOnDie (Vector3 damagePosition) {


		DamageTaken (damagePosition);

		heroAnimation.Dead ();

		heroAttack.canAttack = false;
		heroMovement.canMove = false;

//		damageCollider.enabled = false;
		GetComponent<CapsuleCollider> ().enabled = false;

		timeController.canTimeSlow = false;
		timeController.timeScale = 1f;

		CurrentHeroStatus = HeroStatus.Dead;

		GameController.CurrentGameStatus = GameController.GameStatus.Dead;
	
	}




	private void OnTriggerEnter(Collider collider) {

//		Debug.Log ("collider tag = " + collider.tag);

		if (collider.tag == Constants.BULLET_TAG) {

			int damage = collider.gameObject.GetComponent<BulletController> ().damage;
			health.TakeDamage (damage, collider.transform.position);

		} else if (collider.tag == Constants.MELEE_TAG) {

			int damage = collider.gameObject.GetComponent<MeleeController> ().damage;
			health.TakeDamage (damage, collider.transform.position);
		}
        else if (collider.tag == "zone2")
        {
            // GameController.CurrentPlayerZone = GameController.PlayerZone.ZoneTwo;


        }
        else if (collider.tag == "Playerz")
        {
					CurrentHeroStatus = HeroStatus.End;

								// heroAnimation.AttackAnimation (2);
								// endAttack = true;
								// HeroAttackOnAttack(2);
// 																heroAnimation.IdleAnimation ();

//             // playerController.canSling = true;

// 		heroMovement.canMove = false;
//             timeController.canTimeSlow = false;

// // cameraController.CinemaEffectEnding();
// 		// heroAnimation.StopAnimator (true);
// 					hudsController.UpdatePowerz();
					// timeController.timeScale = 1f;


            /*            PlayerController.CurrentPlayerState = PlayerController.PlayerState.Sling;
            */

        }

    }


public void ShootSling(float power) {
								// HeroAttackOnAttack(2);
CinemaEffectzzz(power);

playerController.canSling = true;

heroAnimation.FlyAnimation();

// playerController.powerz = powerz;
	
}



public  void CinemaEffectzzz(float power) {
	        StartCoroutine(FlagShow());

Debug.Log("Power is  " + power);
//start finalenemy animation here
//20 ~ 80
if (power < 20f) {
power = 20f;
} else if (power > 80f) {
	power = 80f;
}
	Vector3 pos = new Vector3 (0, flyingTa.transform.position.y-1f, power);
        	Hashtable hashtable = new Hashtable ();
		hashtable.Add ("position", pos);
		hashtable.Add ("time",  3f);
		hashtable.Add ("easetype",  iTween.EaseType.easeOutQuart);
		hashtable.Add ("ignoretimescale", true);
            iTween.MoveTo(flyingTa, hashtable);


	}

    IEnumerator FlagShow()
    {
		        // yield return new WaitForSeconds(3f);
				// heroAnimation.FinalTargetsMoveDown();

        yield return new WaitForSeconds(3.5f);

	cameraController.canRotatez = true;
	Debug.Log("Show");
	        flagController.Show(flyingTa.transform.position);    }




    public void Revive() {

		CurrentHeroStatus = HeroStatus.Alive;

		heroAttack.canAttack = true;
		heroMovement.canMove = true;

		GetComponent<CapsuleCollider> ().enabled = true;



		timeController.timeScale = 0f;
		timeController.canTimeSlow = true;

		health.isDead = false;
		health.currentHealth = health.maxHealth;
		hudsController.HealthBar (health.GetCurrentHealthPercentage());

		heroAnimation.Revive ();

	}

}

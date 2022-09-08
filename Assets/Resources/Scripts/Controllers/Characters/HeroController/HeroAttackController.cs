using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeroAttackController : MonoBehaviour {


	public bool canAttack = true;
	public int attackDamage = 1;

	[SerializeField]
	private float timeToHitCombo = 2f;
	private float comboTiming;

	private int comboCount = 1;

	public static int CURRENT_COMBO_COUNT;

	[SerializeField]
	private float moveToAttackRange = 1f;



	//Delegate
	public delegate void OnAttack(int attackType);
	public event  OnAttack onAttack;

	public delegate void OnCombo(int comboCount);
	public event  OnCombo onCombo;

	public delegate void OnComboFinished(int comboCount);
	public event  OnComboFinished onComboFinished;

	public delegate void OnSpecialCharge(float percentage);
	public event  OnSpecialCharge onSpecialCharge;

	public delegate void OnSpecialChargeComplete();
	public event  OnSpecialChargeComplete onSpecialChargeComplete;

	public delegate void OnSpecialInit();
	public event  OnSpecialInit onSpecialInit;

	public delegate void OnSpecialFinished();
	public event  OnSpecialFinished onSpecialFinished;

	private bool inCombo; 


	private int currentSpecialValue;
	[SerializeField]
	private int countToSpecial = 10;
	public bool specialChargeComplete;

	public HeroMovement heroMovement;


	private AudioSource audioSource;


	public bool inSpecialAttack = false;

	[SerializeField]
	private float fieldOfViewAngle = 110f;  


	public int GetCurrentSpecialCount() {
		return currentSpecialValue;
	}

	public void SetCurrentSpecialCount(int value) {
		this.currentSpecialValue = value;
	}


	public int GetSpecialCount() {
		return countToSpecial;
	}

	public void SetSpecialCount(int value) {
		this.countToSpecial = value;
	}





	private void Start () {
		


		audioSource = GetComponent<AudioSource> ();

	}
	

	private void Update () {

		ComboTiming ();
	
	}



	public void SpecialAttack() {
		StartCoroutine(Special ());
	}


	private IEnumerator Special() {

		inSpecialAttack = true;

		canAttack = false;

		if (onSpecialInit != null) {
			onSpecialInit ();
		}


		GameObject[] enemies = GameObject.FindGameObjectsWithTag (Constants.ENEMY_TAG);
		var nonDeadEnemies = enemies.Where (c => !c.gameObject.GetComponent<EnemyController> ().health.isDead).ToArray();

		for (int i = 0; i < nonDeadEnemies.Length; i++) {

			MoveToAttackTarget (nonDeadEnemies[i], 0.2f);
			ComboControl ();

			yield return new WaitForSeconds (0.3f);
		}

		canAttack = true;
		specialChargeComplete = false;
		inSpecialAttack = false;

		if (onSpecialFinished != null) {
			onSpecialFinished ();
		}
	}

	private void ComboTiming() {

		if (comboTiming > 0 && HeroController.CurrentHeroStatus != HeroController.HeroStatus.Dead) { 

			comboTiming -= Time.deltaTime;

		} else {

			if (inCombo) {

				if (onComboFinished != null) {
					onComboFinished (comboCount);
				}

				inCombo = false;
			}

			comboCount = 1;
			CURRENT_COMBO_COUNT = 1;
		}
	}


	public void AttackInit(int attackType) {

		heroMovement.canMove = false;
	
	}


	public void AttackEnd(int attackType) {

		heroMovement.canMove = true;

	}


	private void OnTriggerEnter(Collider collider) {

		if (collider.tag == Constants.ENEMY_TAG) {

			if (collider.gameObject.GetComponent<EnemyController> ().health.isDead) {
				return;

			} else if (IsInSight(collider.gameObject, gameObject) && canAttack) {

				MoveToAttackTarget (collider.gameObject, 0.1f);

				ComboControl ();
				SpecialCharge ();
			}
		}
	}





	private bool IsInSight(GameObject target,GameObject objectLooker) {

		Vector3 direction = target.transform.position - objectLooker.transform.position;
		float angle = Vector3.Angle(direction, transform.forward);

		if (angle < fieldOfViewAngle * 0.5f) {
			return true;
		}

		return false;
	}


	private void OnAttackType() {

		if (onAttack != null) {

			int randomRange = Random.Range (1, 8);
			onAttack (randomRange);
		}

	}


	private void ComboControl() {

		if (comboTiming > 0f) {

			inCombo = true;

			comboCount++;
			CURRENT_COMBO_COUNT++;

			if (onCombo != null) {
				onCombo (comboCount);
			}
		}

		comboTiming = timeToHitCombo;

	}

	private void MoveToAttackTarget(GameObject target, float timeToMove) {

		Vector3 direction = (gameObject.transform.position - target.transform.position).normalized;
		Vector3 pos = target.transform.position + direction * 1f;


		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("position", pos);
		hashtable.Add ("easetype" , iTween.EaseType.linear);
		hashtable.Add ("time" , timeToMove);
		hashtable.Add ("ignoretimescale" , true);

		hashtable.Add ("onupdatetarget", gameObject);
		hashtable.Add ("onupdateparams", pos);
		hashtable.Add ("onupdate", "OnMoveToAttackUpdate");

		hashtable.Add ("oncompleteparams", target);
		hashtable.Add ("oncomplete", "MoveToTargetComplete");
		hashtable.Add ("oncompletetarget", gameObject);

		iTween.MoveTo (gameObject.transform.parent.gameObject, hashtable);


	
	}

	private void OnMoveToAttackUpdate(Vector3 pos) {

//		Debug.Log ("onmovetoattack pos = " + pos);
		gameObject.transform.parent.transform.LookAt (pos);

	}


	private void MoveToTargetComplete(GameObject target) {

		audioSource.pitch = Random.Range (1f, 3f);
		audioSource.Play ();

		OnAttackType ();
		target.GetComponent<Collider>().gameObject.GetComponent<EnemyController> ().TakeDamage (1);
	}



	private void SpecialCharge() {

		if (!specialChargeComplete) {
			
			currentSpecialValue += 1;

			if (onSpecialCharge != null) {

				if (countToSpecial != 0) {
					float percentage = (currentSpecialValue * 100) / countToSpecial;
					onSpecialCharge (percentage);
				}
			
			}


			if (currentSpecialValue == countToSpecial) {

				countToSpecial *= 2;
				currentSpecialValue = 0;

				specialChargeComplete = true;

				if (onSpecialChargeComplete != null) {
					onSpecialChargeComplete ();
				}
			}
		}

	}






}

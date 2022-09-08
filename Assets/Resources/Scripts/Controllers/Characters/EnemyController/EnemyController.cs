using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public TextMesh txtPoints;

	[SerializeField]
	private int pointsValue = 100;

	public EnemyAttack enemyAttack;
	public Health health;

	public EnemyAnimationController enemyAnimator;

	public GameObject deathParticles;

	private EnemyMovement enemyMovement;

    private Transform player;


    private void Awake() {
		
		enemyAttack = GetComponent<EnemyAttack> ();
		enemyMovement = GetComponent<EnemyMovement> ();

	}


	private void Start () {

		health.onDamageTaken += HealthOnDamageTaken;
		health.onDie += HealthOnDie;

		enemyAnimator.onAttackInit += EnemyAnimatorOnAttackInit;
		enemyAnimator.onAttackEnd += EnemyAnimatorOnAttackEnd;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        SetWeaponType();


	}

    private void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= 5f && enemyAttack.currentWeapon != EnemyAttack.Weapon.Katana) {
            enemyAnimator.AttackAnimation((int)enemyAttack.currentWeapon);
        }

else if (Vector3.Distance(player.position, transform.position) > 5f && enemyAttack.currentWeapon != EnemyAttack.Weapon.Katana)     {

            
                enemyAnimator.IdleAnimation();
        } 

    }

    private void SetWeaponType() {

		if (enemyAttack.currentWeapon >= EnemyAttack.Weapon.Katana) {

			GetComponent<SphereCollider> ().enabled = true;

			enemyAnimator.animator.applyRootMotion = false;
			enemyMovement.movementRange = 0f;
			enemyMovement.lookSpeed = 1f;

		}

		enemyAnimator.AttackAnimation ((int) enemyAttack.currentWeapon);
	
	}


	private void EnemyAnimatorOnAttackInit () {
		enemyAttack.Attack ();
	}

	private void EnemyAnimatorOnAttackEnd () {
		enemyAttack.AttackEnd ();
	}


	public void TakeDamage(int damage) {
		health.TakeDamage (damage);
	}


	private void HealthOnDamageTaken (int damage) {

	}


	private void HealthOnDie (Vector3 damagePosition) {

		enemyMovement.canMove = false;

		enemyAnimator.StopAnimator ();
		enemyAnimator.ActivateRagdoll (true);

		CameraController.CameraShake (0.1f);
		HudsController.RemoveIndicator (gameObject);

		ShowPoints ();

		GetComponent<DropController> ().Drop ();
		StartCoroutine (DeathCouroutine ());
	}


	private void ShowPoints () { 

		int points = Mathf.RoundToInt(pointsValue * Mathf.Clamp(TimeController.currentTimeScale, 0.5f, 1f) * HeroAttackController.CURRENT_COMBO_COUNT);
		txtPoints.text = "+" + points;

		Hashtable showScaleHashtable = new Hashtable ();
		showScaleHashtable.Add ("scale",new Vector3(0.1f,0.1f,0.1f));
		showScaleHashtable.Add ("time", 0.7f);


		Hashtable showMoveHashtable = new Hashtable ();
		showMoveHashtable.Add ("position", new Vector3(-1f,2f,0));
		showMoveHashtable.Add ("islocal", true);
		showMoveHashtable.Add ("time", 0.5f);
		showMoveHashtable.Add ("oncomplete", "DismissPoints");
		showMoveHashtable.Add ("oncompletetarget", gameObject);

		iTween.ScaleTo (txtPoints.gameObject, showScaleHashtable);
		iTween.MoveTo (txtPoints.gameObject, showMoveHashtable);

		GameController.AddPoints (points);
	}


	private void DismissPoints() {

		GameObject.Destroy (txtPoints.gameObject);
	}


	private IEnumerator DeathCouroutine() {

		deathParticles.SetActive (true);

		yield return new WaitForSeconds (2f);

		enemyAnimator.ActivateRagdoll (false);

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("y",-1f);
		hashtable.Add ("time", 2f);
		hashtable.Add ("easetype", iTween.EaseType.linear);
		hashtable.Add ("oncomplete", "CompositionAnimationComplete");

		iTween.MoveTo (gameObject, hashtable);

	}


	private void CompositionAnimationComplete() {

		GameObject.Destroy (gameObject);

	}


	private void OnTriggerEnter(Collider collider) {

		if (collider.tag == Constants.PLAYER_TAG) {

			if (enemyAttack.currentWeapon >= EnemyAttack.Weapon.Katana) {
				enemyAnimator.AttackMelee ();
			}
		}
	}


	public void DestroyEnemy() {

		HudsController.RemoveIndicator (gameObject);
		GameObject.Destroy (gameObject);
	}
}

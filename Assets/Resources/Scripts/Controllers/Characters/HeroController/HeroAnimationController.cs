using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationController : MonoBehaviour {

	public Animator animator;
	public Animator finalAnimator;
	public GameObject finalTargets;
	public Material modelMaterial;
	private Color initMaterialColor;

	public ParticleSystem damageParticle;

	//Delegate
	public delegate void OnAttackInit(int attackType);
	public event  OnAttackInit onAttackInit;

	public delegate void OnAttackEnd(int attackType);
	public event  OnAttackEnd onAttackEnd;

	public GameObject[] ragdollParts;




	public GameObject lookObj;


	private void Start () {

		ActivateRagdoll (false);
		initMaterialColor = modelMaterial.color;
finalAnimator.speed = 0;
	}


	public void AttackAnimation(int attackType) {

		animator.SetTrigger ("Attack");
		animator.SetInteger ("Attack_Type", attackType);
	        animator.speed = 1;

	}

public void FlyAnimation(){
	finalAnimator.speed = 1;
}

public void FinalTargetsMoveDown() {

		// Vector3 pos = new Vector3 (0, flyingTa.transform.position.y, 75f);
        	Hashtable hashtable = new Hashtable ();
		hashtable.Add ("y", -1f);
		hashtable.Add ("time",  1f);
		hashtable.Add ("easetype",  iTween.EaseType.easeOutQuart);
		hashtable.Add ("ignoretimescale", true);
            iTween.MoveTo(finalTargets, hashtable);


}
    public void IdleAnimation()
    {
        /*        animator.Play("Base Layer.idles", 0, 0.25f);
        */

        animator.speed = 0;


    }


	public void AttackInit(int attackType) {

		Debug.Log ("attackInit = " + attackType);

		if (onAttackInit != null) {
			onAttackInit (attackType);
		}

	}


	public void AttackEnd(int attackType) {

		Debug.Log ("AttackEnd = " + attackType);

		if (onAttackEnd != null) {
			onAttackEnd (attackType);
		}

	}

	public void DodgeBullet(int leftOrRight) {
	
		animator.SetTrigger ("Dodge_Bullet");
		animator.SetInteger ("Dodge_Direction", leftOrRight);

	}



	public void DamageTaken(Vector3 position) {

		StartCoroutine (DamageParticle(position));
		StartCoroutine (BlinkDamage());
	}

	private IEnumerator DamageParticle(Vector3 position) {

		damageParticle.transform.position = position;
		damageParticle.Play ();

		yield return new WaitForSeconds (0.5f);
	
	}


	private IEnumerator BlinkDamage() {

		modelMaterial.color = Color.gray;
		yield return new WaitForSeconds (0.1f);
		modelMaterial.color = initMaterialColor;

	}

	public void ActivateRagdoll(bool activate) {

		for (int i = 0; i < ragdollParts.Length; i++) {

			ragdollParts [i].GetComponent<Rigidbody> ().isKinematic = !activate;
			ragdollParts [i].GetComponent<Collider> ().enabled = activate;
		}

	}

	public void StopAnimator(bool enable) {
		animator.enabled = !enable;
	}


	public void Dead() {

		StopAnimator (true);
		ActivateRagdoll (true);

	}

	public void Revive() {

		StopAnimator (false);
		ActivateRagdoll (false);
		//TODO Blink Effect

	}


	private void OnAnimatorIK() {

//		if(lookObj != null) {
			
//			animator.SetLookAtWeight (1);
//			animator.SetLookAtPosition (lookObj.transform.position);
//
//			animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
//			animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
//
//			animator.SetIKPosition(AvatarIKGoal.RightHand,lookObj.transform.position);
//			animator.SetIKRotation(AvatarIKGoal.RightHand,lookObj.transform.rotation);

//		}  

	}    

}

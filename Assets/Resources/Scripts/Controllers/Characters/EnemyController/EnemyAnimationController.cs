using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour {

	public Animator animator;

	//Delegate
	public delegate void OnAttackInit();
	public event  OnAttackInit onAttackInit;

	public delegate void OnAttackEnd();
	public event  OnAttackEnd onAttackEnd;


	public GameObject[] ragdollParts;

    public float gunSpeed;


	private void Start () {

		ActivateRagdoll (false);
        gunSpeed = animator.speed;
        Debug.Log("speed" + gunSpeed);
	
	}


	public void ActivateRagdoll(bool activate) {

		for (int i = 0; i < ragdollParts.Length; i++) {

			ragdollParts [i].GetComponent<Rigidbody> ().isKinematic = !activate;
			ragdollParts [i].GetComponent<Collider> ().enabled = activate;
		}

	}
	


    public void IdleAnimation()
    {
        /*        animator.Play("Base Layer.idles", 0, 0.25f);
        */
        gunSpeed = animator.speed;
        animator.speed = 0;

    }

    public void AttackAnimation(int gunType) {

        animator.SetInteger("Gun_Type", gunType);
        animator.speed = 1;




    }

    public void AttackMelee() {
		animator.SetTrigger ("Melee_Attack");
	}


	public void AttackInit() {

		if (onAttackInit != null) {
			onAttackInit ();
		}

	}


	public void AttackEnd() {

		if (onAttackEnd != null) {
			onAttackEnd ();
		}
	}

	public void StopAnimator() {
        /*		animator.Stop ();
        */
        animator.enabled = false;
    }
}

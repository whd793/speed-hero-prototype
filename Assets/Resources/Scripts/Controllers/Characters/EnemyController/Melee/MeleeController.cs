using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeController : MonoBehaviour {

	public int damage;

	public  abstract void Attack ();
	public  abstract void InitAttack ();
	public  abstract void EndAttack ();

	public virtual void Start () {

		Physics.IgnoreLayerCollision (8, 9);
	}

}

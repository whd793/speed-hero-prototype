using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	public enum Weapon {
		Pistol = 0,
		Rifle = 1,
		Shotgun = 2,
		Machinegun = 3,
		Sniper = 4,
		Katana = 5,
	}

	public Weapon _currentWeapon;
	public Weapon  currentWeapon {

		get { return _currentWeapon; }

		set {

			if (_currentWeapon != value) {

				_currentWeapon = value;

				if (onWeaponChanged != null) {
					onWeaponChanged (_currentWeapon);
				}
			}
		}
	} 


	//Delegate
	public delegate void OnWeaponChanged(Weapon weapon);
	public event  OnWeaponChanged onWeaponChanged;


	public bool canAttack;


	public GameObject weaponsContainer;
	private GameObject currentWeaponObj;

	private GunController gunController;
	private MeleeController meleeController;

    //	public float attackSpeed = 1f;


    private Transform target;


    private void Start () {
		ActivateWeapon ();

        GameObject targetObj = GameObject.FindGameObjectWithTag("Player");

        if (targetObj != null)
        {
            target = targetObj.transform;
        }
    }

    private void Update ()
    {
        if (Vector3.Distance(target.position, transform.position) <= 7.1f)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

    }

    private void ActivateWeapon() {

		for (int i = 0; i < weaponsContainer.transform.childCount; i++) {

			if (i == (int)currentWeapon) {
				
				currentWeaponObj = weaponsContainer.transform.GetChild (i).gameObject;
				currentWeaponObj.SetActive (true);

				gunController = currentWeaponObj.GetComponent<GunController> ();
				meleeController = currentWeaponObj.GetComponent<MeleeController> ();

			}

		}

	}




	public void Attack() {

        if (canAttack)
        {
            if (currentWeapon < Weapon.Katana)
            {
                gunController.Shoot();

            }
            else if (currentWeapon == Weapon.Katana)
            {
                meleeController.Attack();

            }
        }

	}

	public void AttackEnd() {

		if (currentWeapon < Weapon.Katana) {
			

		} else if (currentWeapon == Weapon.Katana) {
			meleeController.EndAttack ();

		}

	}

}

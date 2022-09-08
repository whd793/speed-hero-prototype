using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public int maxHealth;
	public int currentHealth;

	public bool canTakeDamage = true;
	public bool isDead = false;



	//Delegate
	public delegate void OnDamageTaken(int damage);
	public event  OnDamageTaken onDamageTaken;

	public delegate void OnDamageTakenPosition(int damage, Vector3 damagePosition);
	public event  OnDamageTakenPosition onDamageTakenPosition;

	public delegate void OnHealthRecover(int recovery);
	public event  OnHealthRecover onHealthRecover;

	public delegate void OnDie(Vector3 damagePosition);
	public event  OnDie onDie;



	private void Start () {

		currentHealth = maxHealth;

	}


	public void TakeDamage(int damage) {

		if (canTakeDamage) {

			currentHealth -= damage;

			if (currentHealth <= 0) {

				currentHealth = 0;

				isDead = true;

				if (onDie != null) {
					onDie (Vector3.zero);
				}

			} else {

				if (onDamageTaken != null) {
					onDamageTaken (damage);
				}

			}
		}
	}


	public void TakeDamage(int damage, Vector3 damagePosition) {

		if (canTakeDamage) {

			currentHealth -= damage;

			if (currentHealth <= 0) {

				currentHealth = 0;

				isDead = true;

				if (onDie != null) {
					onDie (damagePosition);
				}

			} else {

				if (onDamageTakenPosition != null) {
					onDamageTakenPosition (damage,damagePosition);
				}

			}
		}
	}


	public void RecoverHealth(int recover) {

		currentHealth = Mathf.Clamp (currentHealth += recover, 0, maxHealth);

		if (onHealthRecover != null) {
			onHealthRecover (recover);
		}

	}
	
	public float GetCurrentHealthPercentage() {
		return (currentHealth * 100) / maxHealth;
	}

}

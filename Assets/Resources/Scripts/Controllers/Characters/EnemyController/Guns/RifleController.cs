using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleController : GunController {




	public override void InitShoot() {


	}


	public override void Shoot() {

		GameObject bullet = GameObject.Instantiate (bulletPrefab);
		bullet.transform.position = shootingPoint.position;

		Vector3 pathDirection = (gameObject.transform.forward - gameObject.transform.position).normalized;

		Quaternion facingDirection = Quaternion.FromToRotation(gameObject.transform.forward, pathDirection) * gameObject.transform.rotation;
		bullet.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, facingDirection, 1 * Time.deltaTime);
bullet.transform.rotation = Quaternion.Euler(new Vector3(0f, bullet.transform.eulerAngles.y, 0f));

		bullet.GetComponent<BulletController> ().damage = bulletDamage;
		bullet.GetComponent<BulletController> ().direction = transform.forward;
		bullet.GetComponent<BulletController> ().speed = bulletSpeed;



		StartCoroutine (ShootEffect());
	}

	private IEnumerator ShootEffect() {

		shootEffectObj.SetActive (true);

		yield return new WaitForSeconds (0.5f);

		shootEffectObj.SetActive (false);
	}

	public override void EndShoot() {

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : GunController {

	[Range(3,4)]
	public int bulletsCount;






	public override void InitShoot() {


	}


	public override void Shoot() {


		if (bulletsCount == 3) {
			Shoot3Bullets ();

		} else {
			Shoot4Bullets();
		}
	


	}


	private void Shoot3Bullets() {

		Ray ray0 = CreateBulletDirection (15f);
		Ray ray1 = CreateBulletDirection (1f);
		Ray ray2 = CreateBulletDirection (-15f);


		Ray[] rays = { ray0, ray1, ray2};

		BulletCore (rays);
	}


	private void Shoot4Bullets() {

		Ray ray0 = CreateBulletDirection (15f);
		Ray ray1 = CreateBulletDirection (5f);
		Ray ray2 = CreateBulletDirection (-5f);
		Ray ray3 = CreateBulletDirection (-15f);

		Ray[] rays = { ray0, ray1, ray2, ray3};

		BulletCore (rays);
	}



	private Ray CreateBulletDirection(float angle) {

		Ray ray = new Ray ();
		ray.origin = shootingPoint.position;
		ray.direction = transform.forward;
		ray.direction = Quaternion.AngleAxis (angle, transform.up) * ray.direction;


		return ray;
	}


	private void BulletCore(Ray[] rays) {

		for (int i = 0; i < rays.Length; i++) {

			GameObject bullet = GameObject.Instantiate (bulletPrefab);
			bullet.transform.position = shootingPoint.position;

			Vector3 pathDirection = rays [i].direction;

			Vector3 rotationDirection = (gameObject.transform.forward - gameObject.transform.position).normalized;

			Quaternion facingDirection = Quaternion.FromToRotation(gameObject.transform.forward, rotationDirection) * gameObject.transform.rotation;
			bullet.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, facingDirection, 1 * Time.deltaTime);
bullet.transform.rotation = Quaternion.Euler(new Vector3(0f, bullet.transform.eulerAngles.y, 0f));

			bullet.GetComponent<BulletController> ().damage = bulletDamage;
			bullet.GetComponent<BulletController> ().direction = pathDirection;
			bullet.GetComponent<BulletController> ().speed = bulletSpeed;
		}


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

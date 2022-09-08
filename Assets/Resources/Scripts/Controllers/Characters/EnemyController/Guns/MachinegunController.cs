using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinegunController : GunController {

	private SoundFX soundFX;


	private void Start () {
		soundFX = GetComponent<SoundFX> ();
	}





	public override void InitShoot() {


	}

	private Ray CreateBulletDirection(float angle) {

		Ray ray = new Ray ();
		ray.origin = shootingPoint.position;
		ray.direction = transform.forward;
		ray.direction = Quaternion.AngleAxis (angle, transform.up) * ray.direction;


		return ray;
	}

	public override void Shoot() {

		GameObject bullet = GameObject.Instantiate (bulletPrefab);
		bullet.transform.position = shootingPoint.position;

		Ray ray = CreateBulletDirection (Random.Range(-10f,10f));

		Vector3 pathDirection = ray.direction;


		Vector3 rotationDirection = (gameObject.transform.forward - gameObject.transform.position).normalized;

		Quaternion facingDirection = Quaternion.FromToRotation(gameObject.transform.forward, rotationDirection) * gameObject.transform.rotation;
		bullet.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, facingDirection, 1 * Time.deltaTime);

		bullet.GetComponent<BulletController> ().damage = bulletDamage;
		bullet.GetComponent<BulletController> ().direction = pathDirection;
		bullet.GetComponent<BulletController> ().speed = bulletSpeed;

		soundFX.PlaySound ();

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

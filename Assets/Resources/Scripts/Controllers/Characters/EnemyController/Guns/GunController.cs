using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunController : MonoBehaviour {



	public int bulletDamage;
	public float bulletSpeed;
//	public float bulletIntervalTime;

	public GameObject shootEffectObj;

	public Transform shootingPoint;

	public GameObject bulletPrefab;


	public  abstract void Shoot ();

	public  abstract void InitShoot ();

	public  abstract void EndShoot ();



}

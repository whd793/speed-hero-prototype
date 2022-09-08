	using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}



public class EnemySpawnner : MonoBehaviour {

	public bool canSpawn = true;
	public GameObject enemyPrefab;

	public float minTimeInterval = 1f;
	private float initMinTimeInterval;

	public float maxTimeInterval = 4f;
	private float initMaxTimeInterval;

	public float spawnTimeInterval = 4f;
	private float initSpawnTimeInterval;

	public Boundary boundary;
	public List<EnemyAttack.Weapon> canSpawnWeapons;


	//Delegate
	public delegate void OnSpawnEnemy(GameObject enemy);
	public event  OnSpawnEnemy onSpawnEnemy;

	// Use this for initialization
	private void Start () {

		canSpawnWeapons = new List<EnemyAttack.Weapon> ();

		GameController.onGameStatusChanged += GameControllerOnGameStatusChanged;
        GameController.onPlayerZoneChanged += GameControllerOnPlayerZoneChanged;

        GameController.onLevelUp += GameControllerOnLevelUp;

		canSpawnWeapons.Add(EnemyAttack.Weapon.Pistol);

		initMinTimeInterval = minTimeInterval;
		initMaxTimeInterval = maxTimeInterval;
		initSpawnTimeInterval = spawnTimeInterval;
	}


	private void GameControllerOnLevelUp (int level) {


//		Debug.Log ("Spawnner LevelUp = " + level);

		switch (level) {

			case 2:

				if (!canSpawnWeapons.Contains (EnemyAttack.Weapon.Rifle)) {
					canSpawnWeapons.Add (EnemyAttack.Weapon.Rifle);
				}

				break;


			case 3:

				if (!canSpawnWeapons.Contains (EnemyAttack.Weapon.Katana)) {
					canSpawnWeapons.Add (EnemyAttack.Weapon.Katana);
				}

				break;

			case 4:

				if (!canSpawnWeapons.Contains (EnemyAttack.Weapon.Machinegun)) {
					canSpawnWeapons.Add (EnemyAttack.Weapon.Machinegun);
				}

				break;

			case 6:
				break;

			case 8:

				if (!canSpawnWeapons.Contains (EnemyAttack.Weapon.Shotgun)) {
					canSpawnWeapons.Add (EnemyAttack.Weapon.Shotgun);
				}

				break;

			case 10:

				break;


		default:
			break;
		}

	}

	private void GameControllerOnGameStatusChanged (GameController.GameStatus gameStatus) {

		if (gameStatus == GameController.GameStatus.Playing) {

			DestroyAllEnemies ();

			minTimeInterval = initMinTimeInterval;
			maxTimeInterval = initMaxTimeInterval;
			spawnTimeInterval = initSpawnTimeInterval;

			canSpawnWeapons.Clear ();
			canSpawnWeapons.Add (EnemyAttack.Weapon.Pistol);

		} else if (gameStatus == GameController.GameStatus.Dead) {

			StopCoroutine(SpawnEnemy ());

		}


	}

    private void GameControllerOnPlayerZoneChanged(GameController.PlayerZone playerZone)
    {

        if (playerZone == GameController.PlayerZone.ZoneTwo)
        {

          /*  StartCoroutine(SpawnEnemyStartTwo());

*/
        }



    }

    public void DestroyAllEnemies() {

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject[] bullets = GameObject.FindGameObjectsWithTag ("Bullet");

		for (int i = 0; i < enemies.Length; i++) {
			enemies [i].GetComponent<EnemyController> ().DestroyEnemy ();
		}

		for (int i = 0; i < bullets.Length; i++) {
			GameObject.Destroy (bullets [i]);
		}
	}


    public void StartSpawnEnemyStart()
    {

        if (canSpawn)
        {

            StartCoroutine(SpawnEnemyStart());
        }

    }

    public void StartSpawnEnemyStartTwo()
    {

        if (canSpawn)
        {

            StartCoroutine(SpawnEnemyStartTwo());
        }

    }

    public void StartSpawnEnemy() {

		if (canSpawn) {

			StartCoroutine (SpawnEnemy ());
		}

	}



    int countz = 5;
	public IEnumerator SpawnEnemy() {

            while (GameController.CurrentGameStatus == GameController.GameStatus.Playing || GameController.CurrentGameStatus == GameController.GameStatus.Restart || GameController.CurrentPlayerZone == GameController.PlayerZone.ZoneOne) {

                yield return new WaitForSeconds(spawnTimeInterval);

                int randomRange = Random.Range(0, canSpawnWeapons.Count);
                EnemyAttack.Weapon weapon = canSpawnWeapons[randomRange];

                GameObject enemyInstace = GameObject.Instantiate(enemyPrefab);
                enemyInstace.GetComponent<EnemyController>().enemyAttack.currentWeapon = weapon;

                Vector3 positionToSpawn = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, Random.Range(boundary.zMin, boundary.zMax));
                enemyInstace.transform.position = positionToSpawn;
                countz++;

                if (onSpawnEnemy != null) {
                    onSpawnEnemy(enemyInstace);
                }
            
        }


	}

    int numEnemies = 10;
    


    public IEnumerator SpawnEnemyStart()
    {
        if (GameController.CurrentGameStatus == GameController.GameStatus.Playing || GameController.CurrentGameStatus == GameController.GameStatus.Restart)
        {
            yield return null;

            for (int i = 0; i < numEnemies; i++)
            {
                int randomRange = Random.Range(0, canSpawnWeapons.Count);
                EnemyAttack.Weapon weapon = canSpawnWeapons[randomRange];

                GameObject enemyInstace = GameObject.Instantiate(enemyPrefab);
                enemyInstace.GetComponent<EnemyController>().enemyAttack.currentWeapon = weapon;

                Vector3 positionToSpawn = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, Random.Range(boundary.zMin, boundary.zMax));
                enemyInstace.transform.position = positionToSpawn;

                if (onSpawnEnemy != null)
                {
                    onSpawnEnemy(enemyInstace);
                }

            }


            /*for (int i = 0; i < numEnemies; i++)
            {
                int randomRange = Random.Range(0, canSpawnWeapons.Count);
                EnemyAttack.Weapon weapon = canSpawnWeapons[randomRange];

                GameObject enemyInstace = GameObject.Instantiate(enemyPrefab);
                enemyInstace.GetComponent<EnemyController>().enemyAttack.currentWeapon = weapon;

                Vector3 positionToSpawn = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, Random.Range(boundary.zMin + 15, boundary.zMax + 15));
                enemyInstace.transform.position = positionToSpawn;

                if (onSpawnEnemy != null)
                {
                    onSpawnEnemy(enemyInstace);
                }

            }

            for (int i = 0; i < numEnemies; i++)
            {
                int randomRange = Random.Range(0, canSpawnWeapons.Count);
                EnemyAttack.Weapon weapon = canSpawnWeapons[randomRange];

                GameObject enemyInstace = GameObject.Instantiate(enemyPrefab);
                enemyInstace.GetComponent<EnemyController>().enemyAttack.currentWeapon = weapon;

                Vector3 positionToSpawn = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, Random.Range(boundary.zMin + 30, boundary.zMax + 30));
                enemyInstace.transform.position = positionToSpawn;

                if (onSpawnEnemy != null)
                {
                    onSpawnEnemy(enemyInstace);
                }

            }*/
        }
    }



    public IEnumerator SpawnEnemyStartTwo()
    {
        if (GameController.CurrentGameStatus == GameController.GameStatus.Playing || GameController.CurrentGameStatus == GameController.GameStatus.Restart)
        {
            yield return null;

            for (int i = 0; i < numEnemies; i++)
            {
                int randomRange = Random.Range(0, canSpawnWeapons.Count);
                EnemyAttack.Weapon weapon = canSpawnWeapons[randomRange];

                GameObject enemyInstace = GameObject.Instantiate(enemyPrefab);
                enemyInstace.GetComponent<EnemyController>().enemyAttack.currentWeapon = weapon;

                Vector3 positionToSpawn = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, Random.Range(boundary.zMin + 25, boundary.zMax + 25));
                enemyInstace.transform.position = positionToSpawn;

                if (onSpawnEnemy != null)
                {
                    onSpawnEnemy(enemyInstace);
                }

            }
        }
    }




        }

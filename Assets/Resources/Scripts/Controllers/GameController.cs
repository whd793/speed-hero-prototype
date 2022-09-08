using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public enum GameStatus {
		Menu,
		Tutorial,
		Playing,
		Advertising,
		Dead,
		Restart,
	}

    public enum PlayerZone
    {
        ZoneOne,
        ZoneTwo,
        ZoneThree,
        ZoneFour,
        ZoneFive
    }

/*
 
*/
    private static GameStatus _CurrentGameStatus;
	public static GameStatus CurrentGameStatus {

		get {return _CurrentGameStatus;}

		set {

			if (_CurrentGameStatus != value) {

				_CurrentGameStatus = value;

				if (onGameStatusChanged != null) {
					onGameStatusChanged (value);
				}
			}
		}
	}

    private static PlayerZone _CurrentPlayerZone;
    public static PlayerZone CurrentPlayerZone
    {

        get { return _CurrentPlayerZone; }

        set
        {

            if (_CurrentPlayerZone != value)
            {

                _CurrentPlayerZone = value;

                if (onPlayerZoneChanged != null)
                {
                    onPlayerZoneChanged(value);
                }
            }
        }
    }
    private static EnemySpawnner enemySpawnner;


	public static int totalPoints;

	public static int currentPointsToLevelUp;
	private static int pointsToLevelUp = 50;

	public static int currentLevel = 1;
	public static int maxLevel = 15;

	public static int HighScore {
		get { return PlayerPrefs.GetInt (Constants.PLAYER_HIGHSCORE, 0); }
		set  { PlayerPrefs.SetInt (Constants.PLAYER_HIGHSCORE, value); }
	}



	//Delegate
	public delegate void OnGameStatusChanged(GameStatus gameStatus);
	public static event  OnGameStatusChanged onGameStatusChanged;

    public delegate void OnPlayerZoneChanged(PlayerZone playerZone);
    public static event OnPlayerZoneChanged onPlayerZoneChanged;

    public delegate void OnLevelUp(int level);
	public static event  OnLevelUp onLevelUp;

	public delegate void OnPointsAdded(int points);
	public static event  OnPointsAdded onPointsAdded;

	public delegate void OnBatteryGained(int value);
	public static event  OnBatteryGained onBatteryGained;

	private static bool levelSameContext = false;

	private HeroController heroController;

    private HudsController hudsController;

	public static int totalBatteryCount; 

	private BackgroundSoundController backgroundSoundController;


	private void Awake() {

		heroController = GameObject.FindObjectOfType<HeroController> ();
		hudsController = GameObject.FindObjectOfType<HudsController> ();
		enemySpawnner = GameObject.FindObjectOfType<EnemySpawnner> ();

		backgroundSoundController = GetComponent<BackgroundSoundController> ();

	}

	// Use this for initialization
	private void Start () {


		if (heroController != null) {
			
			heroController.heroAttack.onCombo += HeroControllerHeroAttackOnCombo;
			heroController.heroAttack.onComboFinished += HeroControllerHeroAttackOnComboFinished;

			heroController.heroAttack.onSpecialCharge += HeroControllerHeroAttackOnSpecialCharge;
			heroController.heroAttack.onSpecialChargeComplete += HeroControllerHeroAttackOnSpecialChargeComplete;
			heroController.heroAttack.onSpecialFinished += HeroControllerHeroAttackOnSpecialFinished;
		}

		onGameStatusChanged += GameControllerOnGameStatusChanged;
        onPlayerZoneChanged += GameControllerOnPlayerZoneChanged;


        ADSController.onAdStatusChanged += ADSControllerOnAdStatusChanged;
	}

	private void ADSControllerOnAdStatusChanged (ADSController.ADStatus adStatus) {

		if (adStatus == ADSController.ADStatus.REWARDAPPROVEDINFO) {
			
			HudsController.CanShowBtnContinue = false;

			heroController.Revive ();
			enemySpawnner.DestroyAllEnemies ();


			GameController.CurrentGameStatus = GameStatus.Restart;
            GameController.CurrentPlayerZone = PlayerZone.ZoneOne;


        }

    }


	public static void StartGame() {
		GameController.CurrentGameStatus = GameStatus.Playing;
        GameController.CurrentPlayerZone = PlayerZone.ZoneOne;

    }



    private void GameControllerOnGameStatusChanged (GameStatus gameStatus) {

		if (gameStatus == GameStatus.Playing) {
			
			enemySpawnner.StartSpawnEnemy (); 


		} else if (gameStatus == GameStatus.Dead) {

			ShowAdControl ();

		} else if (gameStatus == GameStatus.Restart) {
			
		} else if (gameStatus == GameStatus.Menu) {

		} else if (gameStatus == GameStatus.Restart) {

			enemySpawnner.StartSpawnEnemy (); 
		}

	}

    private void GameControllerOnPlayerZoneChanged(PlayerZone playerZone)
    {

        if (playerZone == PlayerZone.ZoneTwo)
        {

            // enemySpawnner.StartSpawnEnemyStartTwo();

        }
       

    }

    private void HeroControllerHeroAttackOnComboFinished (int comboCount) {

		hudsController.comboIndicator.SetActive (false); 

	}

	private void HeroControllerHeroAttackOnCombo (int comboCount) {

		hudsController.comboIndicator.SetActive (true);
		hudsController.comboIndicator.transform.GetChild (0).GetComponent<TextMesh> ().text = "" + comboCount;
	}

	private void HeroControllerHeroAttackOnSpecialCharge (float percentage) {
		hudsController.SpecialCharge (percentage);
	}

	private void HeroControllerHeroAttackOnSpecialChargeComplete () {
		hudsController.ShowSpecialButton (true);
	}

	private void HeroControllerHeroAttackOnSpecialFinished () {
		hudsController.ShowSpecialButton (false);
		hudsController.SpecialCharge (0f);
	}


	public static void AddPoints(int points) {

		totalPoints += points;
		currentPointsToLevelUp += points;

		if (totalPoints > HighScore) {
			HighScore = totalPoints;
		}


		if (onPointsAdded != null) {
			onPointsAdded (points);
		}

		if (currentPointsToLevelUp >= pointsToLevelUp) {

			LevelUp ();
			AdjustSpawnTime ();
		}
	
	}


	private static void LevelUp() {

		currentPointsToLevelUp = 0;

		if (levelSameContext) {
			currentLevel -= 1;

		} else {

			currentLevel += 2;

			if (currentLevel >= maxLevel) {
				currentLevel = maxLevel;
			}
		}

		if (onLevelUp != null) {
			onLevelUp (currentLevel);
		}	

		levelSameContext = !levelSameContext;


/*		Debug.Log ("LevelUp = " + currentLevel);
*/	}

	private static void AdjustSpawnTime() {

		float levelPercentage = (currentLevel * 100f) / maxLevel;

		float max = enemySpawnner.maxTimeInterval;

		float interval = (max * levelPercentage) / 100f;

		float finalInterval = max - interval;


		if (finalInterval <= enemySpawnner.minTimeInterval) {
			finalInterval = enemySpawnner.minTimeInterval;
		}


		enemySpawnner.spawnTimeInterval = finalInterval;
	}


	public static void AddBattery(int value) {

		totalBatteryCount += value;


		if (onBatteryGained != null) {
			onBatteryGained (value);
		}

	}

	public static void RestartGame() {

		totalPoints = 0;
		currentPointsToLevelUp = 0;
		pointsToLevelUp = 50;
		currentLevel = 1;
		totalBatteryCount = 0;

		levelSameContext = false;

		GameController.CurrentGameStatus = GameStatus.Playing;
        GameController.CurrentPlayerZone = PlayerZone.ZoneOne;


        HudsController.CanShowBtnContinue = true;

	}


	public static void ContinueGame() {

		ADSController.ShowRewardedVideo ();
	}


	private void ShowAdControl() {

		PlayerPrefs.SetInt (Constants.PLAYER_DEATH_COUNT, PlayerPrefs.GetInt (Constants.PLAYER_DEATH_COUNT) + 1);

		if (PlayerPrefs.GetInt (Constants.PLAYER_DEATH_COUNT) > 1) {

			ADSController.ShowInterstitial ();
			PlayerPrefs.SetInt (Constants.PLAYER_DEATH_COUNT, 0);
		}

	}
}


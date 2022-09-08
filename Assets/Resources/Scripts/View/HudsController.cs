using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryCount {

	public static Vector3 batteryCountPosition;

}

public class HudsController : MonoBehaviour {

	private EnemySpawnner enemySpawnner;
	private static Greyman.OffScreenIndicator offScreenIndicator;
			
	public GameObject comboIndicator;

	public Text txtPoints;
	public Text txtHighscore;

	public Image imgSpecial;
	public Button btnSpecial;

	private HeroController heroController;

	public Image imgDamage;

	public Image imgLifeBar;

	public Text txtHealth;
	public Text txtSpecial;

	public Text txtBatteryCount;


	public GameObject txtPointsPrefab;


	public Button btnRetry;
	public Button btnStart;
	public Button btnContinue;
	public static bool CanShowBtnContinue = true;


	public GameObject lifeContainerObj;
	public GameObject specialBarContainerObj;
	public GameObject pointsContainerObj;
	public GameObject batteryContainerObj;



//power bar
    public GameObject powerBarGO;
    public Image PowerBarMask;
    public float barChangeSpeed = 2.5f;
    float maxPowerBarValue = 100;
    float currentPowerBarValue;
    bool powerIsIncreasing;
    bool PowerBarON;



	private void Awake() {
		
		enemySpawnner = GameObject.FindObjectOfType<EnemySpawnner> ();
		offScreenIndicator = GetComponent<Greyman.OffScreenIndicator> ();
		heroController = GameObject.FindObjectOfType<HeroController> ();
	}

	private void Start () {
//power bar
 currentPowerBarValue = 0;
        powerIsIncreasing = false;
        PowerBarON = false;
        // StartCoroutine(UpdatePowerBar());

//power bar end


		txtHealth.text = heroController.GetHealth().currentHealth + " / " + heroController.GetHealth().maxHealth;
		txtSpecial.text = heroController.heroAttack.GetCurrentSpecialCount() + " / " + heroController.heroAttack.GetSpecialCount();

		txtHighscore.text = "" + GameController.HighScore;

		GameController.onGameStatusChanged += GameControllerOnGameStatusChanged;
		GameController.onPointsAdded += GameControllerOnPointsAdded;
		GameController.onBatteryGained += GameControllerOnBatteryGained;

		enemySpawnner.onSpawnEnemy += EnemySpawnnerOnSpawnEnemy;

		OnMenu ();

	}

	private void OnMenu() {

		btnStart.gameObject.SetActive (true);
		btnRetry.gameObject.SetActive (false);

		Text btnStartText = btnStart.GetComponentInChildren<Text> ();

		StartCoroutine (StartTextFade (btnStartText));

	}


	private IEnumerator StartTextFade(Text btnStartText) {

		while (GameController.CurrentGameStatus == GameController.GameStatus.Menu) {

			btnStartText.CrossFadeAlpha (0, 1f, true);

			yield return new WaitForSecondsRealtime (1f);

			btnStartText.CrossFadeAlpha (1f, 1f, true);

			yield return new WaitForSecondsRealtime (1f);
		}
	}


	private void GameControllerOnGameStatusChanged (GameController.GameStatus gameStatus) {

		if (gameStatus == GameController.GameStatus.Menu) {

			OnMenu ();

		} else if (gameStatus == GameController.GameStatus.Playing) {

			btnStart.gameObject.SetActive (false);
			btnRetry.gameObject.SetActive (false);
			btnContinue.gameObject.SetActive (false);

			lifeContainerObj.SetActive (true);
			specialBarContainerObj.SetActive (true);
			pointsContainerObj.SetActive (true);

//			batteryContainerObj.SetActive (true); //TODO



			txtHighscore.text = "" + GameController.HighScore;
			txtPoints.text = "" + 0;
			txtBatteryCount.text = "" + 0;

		} else if (gameStatus == GameController.GameStatus.Dead) {

			OnDeadMenu ();

		} else if (gameStatus == GameController.GameStatus.Restart) {

			btnRetry.gameObject.SetActive (false);
			btnContinue.gameObject.SetActive (false);
		
		}

	}


	private void OnDeadMenu() {

		btnSpecial.gameObject.SetActive (false); 

		if (CanShowBtnContinue) {

			StartCoroutine (ShowContinueButton ());

		} else {
			btnRetry.gameObject.SetActive (true);
		}


	}

	private IEnumerator ShowContinueButton() {

		btnContinue.gameObject.SetActive (true);

		yield return new WaitForSeconds (1f);

		btnRetry.gameObject.SetActive (true);
	}


	private void GameControllerOnBatteryGained (int value) {

		txtBatteryCount.text = "" + GameController.totalBatteryCount;

		Hashtable hashtableScale = new Hashtable ();
		hashtableScale.Add ("amount", new Vector3(1f,1f,1f));
		hashtableScale.Add ("time", 0.4f);
		hashtableScale.Add ("ignoretimescale", true);

		iTween.ShakeScale (txtBatteryCount.gameObject, hashtableScale);
	}

	private void GameControllerOnPointsAdded (int points) {
		Points (GameController.totalPoints);	
	}


	private void Points(int points) {

		txtHighscore.text = ""+ GameController.HighScore;

		int fromPoints = int.Parse (txtPoints.text);

//		Debug.Log ("fromPoints = " + fromPoints);
//		Debug.Log ("toPoints = " + points);

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("from", fromPoints);
		hashtable.Add ("to", points);
		hashtable.Add ("onupdate", "UpdatePoints");
		hashtable.Add ("time", 0.4f);
		hashtable.Add ("ignoretimescale", true);

		iTween.ValueTo (gameObject, hashtable);


		Hashtable hashtableScale = new Hashtable ();
		hashtableScale.Add ("amount", new Vector3(1f,1f,1f));
		hashtableScale.Add ("time", 0.4f);
		hashtableScale.Add ("ignoretimescale", true);

		iTween.ShakeScale (txtPoints.gameObject, hashtableScale);

	}

	private void UpdatePoints(int points) {

		txtPoints.text = "" + points;
	}


	public void HealthBar(float healthPercentage) {

		txtHealth.text = heroController.GetHealth().currentHealth + " / " + heroController.GetHealth().maxHealth;

		float fromWidth = imgLifeBar.GetComponent<RectTransform> ().rect.width;
		float imgWidthMax = 125f;

		float widthPercentage = (imgWidthMax * healthPercentage) / 100f;

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("from", fromWidth);
		hashtable.Add ("to", widthPercentage);
		hashtable.Add ("onupdate", "HealthBarUpdate");
		hashtable.Add ("time", 0.7f);
		hashtable.Add ("ignoretimescale", true);

		iTween.ValueTo (gameObject, hashtable);
	}

	private void HealthBarUpdate(float percentage) {
		imgLifeBar.GetComponent<RectTransform> ().sizeDelta =  new Vector2 (percentage, imgLifeBar.GetComponent<RectTransform> ().rect.height); 
	}


	public void SpecialCharge(float specialPercentage) {

		txtSpecial.text = heroController.heroAttack.GetCurrentSpecialCount() + " / " + heroController.heroAttack.GetSpecialCount();

		float fromWidth = imgSpecial.GetComponent<RectTransform> ().rect.width;
		float imgWidthMax = 125f;

		float widthPercentage = (imgWidthMax * specialPercentage) / 100f;

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("from", fromWidth);
		hashtable.Add ("to", widthPercentage);
		hashtable.Add ("onupdate", "SpecialChargeUpdate");
		hashtable.Add ("time", 0.7f);
		hashtable.Add ("ignoretimescale", true);

		iTween.ValueTo (gameObject, hashtable);
	}


	private void SpecialChargeUpdate(float percentage) {
		
		imgSpecial.GetComponent<RectTransform> ().sizeDelta =  new Vector2 (percentage, imgSpecial.GetComponent<RectTransform> ().rect.height); 
	}


	public void DamageImageFadeIn() {

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("from", imgDamage.color.a);
		hashtable.Add ("to", 0.6f);
		hashtable.Add ("onupdate", "DamageFade");
		hashtable.Add ("time", 0.1f);
		hashtable.Add ("ignoretimescale", true);

		hashtable.Add ("oncomplete", "DamageImageFadeOut");

		iTween.ValueTo (gameObject, hashtable);
	}


	private void DamageImageFadeOut() {

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("from", imgDamage.color.a);
		hashtable.Add ("to", 0f);
		hashtable.Add ("onupdate", "DamageFade");
		hashtable.Add ("time", 0.5f);
		hashtable.Add ("ignoretimescale", true);

		iTween.ValueTo (gameObject, hashtable);

	}

	private void DamageFade(float alpha) {
		imgDamage.color = new Color (1,1,1, alpha);
	}


	public void SpawnTxtPoints(string text ,Vector3 position) {

		GameObject txtPoints = GameObject.Instantiate (txtPointsPrefab);
		txtPoints.transform.position = position;
		txtPoints.GetComponent<TextPoints> ().SetText (text);

	}



	private static void EnemySpawnnerOnSpawnEnemy (GameObject enemy) {
		offScreenIndicator.AddIndicator (enemy.transform, 0);
	}
	

	public static void RemoveIndicator(GameObject enemy) {
		offScreenIndicator.RemoveIndicator (enemy.transform);
	}


	public void ShowSpecialButton(bool show) {
		btnSpecial.gameObject.SetActive (show);
	}

	public void BtnSpecialClick() {

		heroController.SpecialAttack ();
		btnSpecial.gameObject.SetActive (false); 

	}


	public void BtnStart() {

		GameController.StartGame ();

	}


	public void BtnRetryClick() {

		GameController.RestartGame ();


	}


	public void BtnContinue() {

		GameController.ContinueGame ();

	}


public void UpdatePowerz() 
{
	        powerIsIncreasing = true;

	        PowerBarON = true;
        StartCoroutine(UpdatePowerBar());
        powerBarGO.SetActive(true);

}
	
//power bar
 IEnumerator UpdatePowerBar()
    {
        while (PowerBarON)
        {
            if (!powerIsIncreasing)
            {
                currentPowerBarValue -= barChangeSpeed;
                if (currentPowerBarValue <= 0)
                {
                    powerIsIncreasing = true;
                }
            }
            if (powerIsIncreasing)
            {
                currentPowerBarValue += barChangeSpeed;
                if (currentPowerBarValue >= maxPowerBarValue)
                {
                    powerIsIncreasing = false;
                }
            }
            
            float fill = currentPowerBarValue / maxPowerBarValue;
            PowerBarMask.fillAmount = fill;
            yield return new WaitForSeconds(0.02f);

        if (Input.GetMouseButtonDown(0))
            {
                PowerBarON = false;
        powerBarGO.SetActive(false);

                LaunchRocket();

            }
        }
        yield return null;
    }
    // IEnumerator TurnOffPowerBar()
    // {
    //     yield return new WaitForSeconds(0.3f);
    //     powerBarGO.SetActive(false);
    // }
    
    public void LaunchRocket()
    {
heroController.ShootSling(currentPowerBarValue);    

// Debug.Log("test");
}

	//power bar end
}

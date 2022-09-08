using UnityEngine;
using System.Collections;


public class CameraController : MonoBehaviour {

public GameObject flyingTa;

	public GameObject player;
	public float followVelocity = 5f;

	private Vector3 offset;        
	private Quaternion initRotation;

//	private PlayerInput playerInput;
	private TimeController timeController;
	private HeroController heroController;
	private PlayerController playerController;


    //Delegate
    public delegate void OnCinemaEffectInit();
	public  event  OnCinemaEffectInit onCinemaEffectInit;

	public delegate void OnCinemaEffectEnd();
	public  event  OnCinemaEffectEnd onCinemaEffectEnd;

	public Transform menuTransform;
	public Transform gameplayTransform;

/*    public GameObject flyingEnemy;
*/	private bool startPlaying;
public bool canRotatez = false;
	private AudioSource audioSource;


//   public bool endingPunch = false;
    
    //new end
    private void Start () {

		timeController = GameObject.FindObjectOfType<TimeController> ();
		heroController = GameObject.FindObjectOfType<HeroController>();
		playerController = GameObject.FindObjectOfType<PlayerController>();

        audioSource = GetComponent<AudioSource> ();

	

		GameController.onGameStatusChanged += GameControllerOnGameStatusChanged;
        //new
     
    //new end
    }


    private void GameControllerOnGameStatusChanged (GameController.GameStatus gameStatus) {

		if (gameStatus == GameController.GameStatus.Menu) {

			CameraMoveTo (menuTransform);

		} else if (gameStatus == GameController.GameStatus.Playing) {

			CameraMoveTo (gameplayTransform);
		}

	}


	private void CameraMoveTo(Transform transform) {

		Hashtable hashtableMove = new Hashtable ();
		hashtableMove.Add ("position", transform.position);
		hashtableMove.Add ("time",1f);

		hashtableMove.Add ("onupdate", "CinemaEffectUpdate");



		hashtableMove.Add ("easetype",iTween.EaseType.linear);
		hashtableMove.Add ("ignoretimescale",true);
		hashtableMove.Add ("oncomplete", "CameraMoveToComplete");


		iTween.MoveTo (gameObject, hashtableMove);

	}

	private void CameraMoveToComplete() {

		offset = transform.position - player.transform.position;
		initRotation = gameObject.transform.rotation;

		startPlaying = true;
	}



	private void Update() {


		if ((playerController.canSling == false && GameController.CurrentGameStatus == GameController.GameStatus.Playing || GameController.CurrentGameStatus == GameController.GameStatus.Restart) && startPlaying ) {
			Vector3 nextPosition = Vector3.Lerp (transform.position, player.transform.position + offset, Time.unscaledDeltaTime * followVelocity);
			transform.position = new Vector3 (nextPosition.x,nextPosition.y, nextPosition.z);
		} 

if (playerController.canSling && canRotatez == false) {
	        transform.position = new Vector3(0, flyingTa.transform.position.y + 9f, flyingTa.transform.position.z - 13f);

}

if (canRotatez) {
	        transform.RotateAround(flyingTa.transform.position, Vector3.up, -Time.deltaTime * 5f);

}
        // if (playerController.canSling)
        // {
            /*Vector3 nextPosition = Vector3.Lerp(transform.position, flyingTarget.transform.position + offset, Time.unscaledDeltaTime * followVelocity);
            transform.position = new Vector3(nextPosition.x, nextPosition.y, nextPosition.z);*/
/*            gameObject.SetActive(false);
*/


        // }
        /* if (PlayerController.CurrentPlayerState == PlayerController.PlayerState.Sling)
         {
             *//*   Vector3 nextPosition = Vector3.Lerp(transform.position, flyingEnemy.transform.position + offset, Time.unscaledDeltaTime * followVelocity);
                timeController.canTimeSlow = false;

                transform.position = new Vector3(nextPosition.x, nextPosition.y, nextPosition.z);*//*
             timeController.canTimeSlow = false;

         }*/


    }

	public  void CinemaEffect() {

		audioSource.Play ();

		heroController.heroMovement.canMove = false;
		timeController.canTimeSlow = false;
		timeController.timeScale = 0.01f;


		gameObject.transform.rotation = Quaternion.identity;

		Vector3 forward = player.transform.position + player.transform.up + player.transform.forward * 5;
		Vector3 right = player.transform.position+ player.transform.up  + player.transform.right * 5;
		Vector3 back = player.transform.position + player.transform.up + player.transform.forward *-1 * 5;
		Vector3 left = player.transform.position+ player.transform.up  + player.transform.right *-1 * 5;


		Vector3[] path = new Vector3[4];
		path [0] = forward;
		path [1] = right;
		path [2] = back;
		path [3] = left;
//		path [4] = forward;


		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("path", path);
		hashtable.Add ("time",  3f);
		hashtable.Add ("easetype",  iTween.EaseType.linear);
		hashtable.Add ("ignoretimescale", true);
		hashtable.Add ("onupdate", "CinemaEffectUpdate");
		hashtable.Add ("oncomplete", "CinemaEffectComplete");

		iTween.MoveTo (gameObject,hashtable);


		if (onCinemaEffectInit != null) {
			onCinemaEffectInit ();
		}
	
	}

	private void CinemaEffectUpdate() {

		gameObject.transform.LookAt (player.transform.position);

	}


	private void CinemaEffectComplete() {

		timeController.canTimeSlow = true;
		gameObject.transform.rotation = initRotation;
		heroController.heroMovement.canMove = true;


		if (onCinemaEffectEnd != null) {
			onCinemaEffectEnd ();
		}
	}


public  void CinemaEffectTwo() {

		audioSource.Play ();

		heroController.heroMovement.canMove = false;
		timeController.canTimeSlow = false;
		timeController.timeScale = 0.01f;


		gameObject.transform.rotation = Quaternion.identity;

		Vector3 forward = player.transform.position + player.transform.up + player.transform.forward * 5;
		Vector3 right = player.transform.position+ player.transform.up  + player.transform.right * 5;
		Vector3 back = player.transform.position + player.transform.up + player.transform.forward *-1 * 5;
		Vector3 left = player.transform.position+ player.transform.up  + player.transform.right *-1 * 5;


		Vector3[] path = new Vector3[4];
		path [0] = forward;
		path [1] = right;
		path [2] = back;
		path [3] = left;
//		path [4] = forward;


		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("path", path);
		hashtable.Add ("time",  3f);
		hashtable.Add ("easetype",  iTween.EaseType.linear);
		hashtable.Add ("ignoretimescale", true);
		hashtable.Add ("onupdate", "CinemaEffectUpdate");
		hashtable.Add ("oncomplete", "CinemaEffectCompleteTwo");

		iTween.MoveTo (gameObject,hashtable);


		if (onCinemaEffectInit != null) {
			onCinemaEffectInit ();
		}
	
	}
//	void OnDrawGizmosSelected() {
//
//		Vector3 forward = player.transform.position + player.transform.up + player.transform.forward * 5;
//		Vector3 right = player.transform.position+ player.transform.up  + player.transform.right * 5;
//		Vector3 back = player.transform.position + player.transform.up + player.transform.forward *-1 * 5;
//		Vector3 left = player.transform.position+ player.transform.up  + player.transform.right *-1 * 5;
//
//
//		Vector3[] path = new Vector3[5];
//		path [0] = forward;
//		path [1] = right;
//		path [2] = back;
//		path [3] = left;
//		path [4] = forward;
//
//		for (int i = 0; i < path.Length; i++) {
//
//			Gizmos.color = Color.yellow;
//			Gizmos.DrawSphere(path[i], 0.2f);
//		}
//
//
//	}

private void CinemaEffectCompleteTwo() {

		// timeController.canTimeSlow = true;
		gameObject.transform.rotation = initRotation;
		// heroController.heroMovement.canMove = true;


		if (onCinemaEffectEnd != null) {
			onCinemaEffectEnd ();
		}

				timeController.timeScale = 1f;

	}

	public static void CameraShake(float shakeValue) {

		Hashtable hashtable = new Hashtable ();
		hashtable.Add ("amount", new Vector3(shakeValue,shakeValue,shakeValue));
		hashtable.Add ("time", 0.1f);
		hashtable.Add ("ignoretimescale", true);

		iTween.ShakePosition (Camera.main.gameObject, hashtable);

	}







// public  void CinemaEffectzzz() {
// 	Vector3 pos = new Vector3 (0, flyingTa.transform.position.y, 35f);
//         	Hashtable hashtable = new Hashtable ();
// 		hashtable.Add ("position", pos);
// 		hashtable.Add ("time",  2f);
// 		hashtable.Add ("easetype",  iTween.EaseType.easeOutQuart);
// 		hashtable.Add ("ignoretimescale", true);
// 		hashtable.Add ("oncomplete", "CinemaEffectCompletezzz");
//             iTween.MoveTo(flyingTa, hashtable);

		
		
		

// 		if (onCinemaEffectInit != null) {
// 			onCinemaEffectInit ();
// 		}
	
// 	}


// 	public void CinemaEffectCompletezzz() {

// 		if (onCinemaEffectEnd != null) {
// 			onCinemaEffectEnd ();
// 		}
// 	}


    //new



}


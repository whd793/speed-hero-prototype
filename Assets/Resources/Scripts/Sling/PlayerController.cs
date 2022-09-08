using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    Sling,
    Flying,
    Landing,
}
public class PlayerController : MonoBehaviour
{
    // [SerializeField] Animator animator;
    // [SerializeField] CamerController cameraController;
    // [SerializeField] FlagController flagController;
 
    PlayerState playerState;
    float landingTimer;
    float proportion;

public float powerz;
    public bool canSling = false;
  

    void Start()
    {
       
        playerState = PlayerState.Sling;

      
    }


    void Update()
    {

}

    

 

    void Sling()
    {



    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Playerz")) return;
        if (other.CompareTag("Obstacle")) return;
        // ragdollController.RefrectFloor(Vector3.up);
    }

    public void LandCheck()
    {
        // if (!ragdollController.IsStop())
        // {
        //     landingTimer = 0;
        //     return;
        // }

        // landingTimer += Time.deltaTime;
        // if (landingTimer < 1f) return;

        // flagController.Show(transform.position);

/*        if (Variables.screenState != ScreenState.Game) return;
*//*        Variables.screenState = ScreenState.Result;
*/    }


}

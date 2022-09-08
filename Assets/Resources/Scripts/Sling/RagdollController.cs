using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RagdollController : MonoBehaviour
{
    public GameObject flyingBall;
    Rigidbody ragdollRigidbodies;
    Collider ragdollColliders;
    // [SerializeField] Animator animator;
    // [SerializeField] float downForce;
   
    [SerializeField] PlayerController playerController;

    // public Rigidbody RootRb => ragdollRigidbodies[0];
    void Awake()
    {
        ragdollRigidbodies = flyingBall.GetComponent<Rigidbody>();
        ragdollColliders = flyingBall.GetComponent<Collider>();
    }
    void Start()
    {
    }

    // void Update()
    // {
    //     foreach (var rb in ragdollRigidbodies)
    //     {
    //         rb.AddForce(-Vector3.up * downForce);
    //     }
    // }

    // public void EnableRagdoll(bool enabled)
    // {
    //     foreach (var rb in ragdollRigidbodies)
    //     {
    //         rb.isKinematic = !enabled;
    //     }
    //     foreach (var col in ragdollColliders)
    //     {
    //         col.enabled = enabled;
    //     }
    //     animator.enabled = !enabled;
    // }


    // public void AddForce(Vector3 force)
    // {
    //     foreach (var rb in ragdollRigidbodies)
    //     {
    //         rb.AddForce(force, ForceMode.Impulse);
    //     }
    // }


    // public void RefrectFloor(Vector3 normal)
    // {
    //     Vector3 inVelocity = ragdollRigidbodies.velocity;
    //     float boundForce = Mathf.Abs(inVelocity.y) * 3f;
    //     foreach (var rb in ragdollRigidbodies)
    //     {
    //         rb.AddForce(Vector3.up * boundForce, ForceMode.Impulse);
    //     }
    // }

    public void SetVelocity(Vector3 vel)
    {
      
            ragdollRigidbodies.velocity = vel;
        
    }

 
    public bool IsStop()
    {
        float velocityAverage = ragdollRigidbodies.velocity.magnitude;
        return velocityAverage < 0.5f;
    }
}

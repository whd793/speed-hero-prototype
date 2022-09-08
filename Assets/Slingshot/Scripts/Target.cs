using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public GameObject score;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Sphere")
        {
            Debug.Log("Red");
            StartCoroutine(HitPointCenter());
        }
    }

    IEnumerator HitPointCenter()
    {
        score.SetActive(true);
        yield return new WaitForSeconds(2);
        score.SetActive(false);
    }
}
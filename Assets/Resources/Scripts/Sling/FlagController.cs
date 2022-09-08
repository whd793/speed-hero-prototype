using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;
public class FlagController : MonoBehaviour
{
    [SerializeField] ParticleSystem confettiPrefab;
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(Vector3 playerPos)
    {
        gameObject.SetActive(true);
        playerPos.x -= 1f;
        transform.position = playerPos;
        transform.localScale = new Vector3(1, 0, 1);
        // transform.DOScaleY(1, 1f).SetEase(Ease.OutElastic);
        // iTween.scaleTo(gameObject,{"scale":Vector3(2,2,2)});


             iTween.ScaleTo(gameObject, iTween.Hash(
                 "y", 1f,
                 "easetype", iTween.EaseType.easeInOutElastic,
                 "time", 0.2f
                 ));     

        var confetti = Instantiate(confettiPrefab, transform.position, Quaternion.identity);
        confetti.transform.eulerAngles = new Vector3(-90, 0, 0);
    }

}

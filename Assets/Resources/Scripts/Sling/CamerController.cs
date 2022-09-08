using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unityで解像度に合わせて画面のサイズを自動調整する
/// http://www.project-unknown.jp/entry/2017/01/05/212837
/// </summary>
public class CamerController : MonoBehaviour
{
    public GameObject flyingTarget;


    public void RotateOnFly(Vector3 targetPos)
    {

        transform.position = new Vector3(0, flyingTarget.transform.position.y + 9f, flyingTarget.transform.position.z - 13f);
    //    transform.rotation = Quaternion.Euler(new Vector3(30f,0f,0));

        }

    

    public void RotateOnLanding(Vector3 targetPos)
    {
        transform.RotateAround(targetPos, Vector3.up, -Time.deltaTime * 5f);
    }

}

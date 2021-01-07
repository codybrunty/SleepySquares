using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMovement : MonoBehaviour
{
    public float angle = 25f;
    public float rotateDuration = .5f;


    public void StartSwitchNextSquareMovement()
    {
        iTween.RotateTo(gameObject, iTween.Hash(
             "z", gameObject.transform.eulerAngles.z + angle,
             "time", rotateDuration/2f,
             "easetype", "easeInOutSine",
             "oncomplete", "PingPongAnimation"
         ));
    }

    public void PingPongAnimation()
    {
        iTween.RotateTo(gameObject, iTween.Hash(
             "z", gameObject.transform.eulerAngles.z - (angle*2f),
             "time", rotateDuration,
             "easetype", "easeInOutSine",
             "looptype", "pingpong"
         ));
    }


    public void StopSwitchNextSquareMovement()
    {
        iTween.Stop(gameObject);
        gameObject.transform.rotation = new Quaternion(0f,0f,0f,0f);

    }


}

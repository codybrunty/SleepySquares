using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnFacialAnimation : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<FacialAnimation>().StartFacialAnimation();
    }


}

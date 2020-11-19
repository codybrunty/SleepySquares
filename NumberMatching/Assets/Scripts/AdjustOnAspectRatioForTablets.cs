using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustOnAspectRatioForTablets : MonoBehaviour
{


    void Start()
    {
        float testAspect = ((float)Camera.main.pixelHeight / Camera.main.pixelWidth);

        if (testAspect < 1.45)
        {
            Vector3 pos = gameObject.transform.position;
            gameObject.transform.position = new Vector3(pos.x,pos.y-.225f,pos.z);
        }

    }


}

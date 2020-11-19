using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTutorialSetup : MonoBehaviour
{
    void Start()
    {
        float testAspect = ((float)Camera.main.pixelHeight / Camera.main.pixelWidth);
        if (testAspect > 1.8) {
            Camera.main.orthographicSize = 6.98f;
        }
    }

}

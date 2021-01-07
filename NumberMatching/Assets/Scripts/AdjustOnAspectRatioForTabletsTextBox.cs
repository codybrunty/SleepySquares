using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustOnAspectRatioForTabletsTextBox : MonoBehaviour
{
    void Start()
    {
        float testAspect = ((float)Camera.main.pixelHeight / Camera.main.pixelWidth);

        if (testAspect < 1.45)
        {
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            Vector3 pos = rt.anchoredPosition;
            rt.anchoredPosition = new Vector3(pos.x, pos.y - 48f, pos.z);
            gameObject.transform.localScale = new Vector3(.9f,.9f,1f);
        }

    }
}

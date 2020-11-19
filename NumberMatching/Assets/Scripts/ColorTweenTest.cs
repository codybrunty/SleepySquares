using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTweenTest : MonoBehaviour
{
    public bool isAnimated = false;
    public Color colorA;
    public Color colorB;
    public float duration = 1f;

    void Update()
    {
        if (isAnimated)
        {
            Color lerpedColor = Color.Lerp(colorA, colorB, Mathf.PingPong(Time.time, duration));
            gameObject.GetComponent<SpriteRenderer>().color = lerpedColor;

        }
    }

}

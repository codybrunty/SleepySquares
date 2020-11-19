using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Dissapear : MonoBehaviour
{

    public void MakeDissapear(float duration)
    {
        StartCoroutine(DissapearOvertime(duration));
    }

    IEnumerator DissapearOvertime(float duration)
    {
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        Color startColor = sprite.color;
        Color endColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);

        for (float t = 0f; t < duration; t+=Time.deltaTime)
        {
            sprite.color = Color.Lerp(startColor, endColor, t / duration);
            yield return null;
        }
        sprite.color = endColor;
    }
}

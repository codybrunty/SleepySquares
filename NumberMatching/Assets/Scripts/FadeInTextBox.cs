using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInTextBox : MonoBehaviour
{

    public float delayTime = 0f;
    public float fadeDuration = .25f;
    private Image img;
    private Color newColor;
    private Color oldColor;
    public bool pingpong = false;

    void OnEnable()
    {
        img = gameObject.GetComponent<Image>();
        oldColor = new Color(img.color.r, img.color.g, img.color.b, 0.15f);
        img.color = oldColor;
        newColor = new Color(img.color.r, img.color.g, img.color.b,1f);
        StartCoroutine(FadeInImageOverTime());   
    }

    IEnumerator FadeInImageOverTime()
    {
        img.color = oldColor;
        for (float t = 0f; t < fadeDuration; t+=Time.deltaTime)
        {
            img.color = Color.Lerp(oldColor,newColor,t/fadeDuration);
            yield return null;
        }
        img.color = newColor;
        if (pingpong)
        {
            StartCoroutine(FadeOutImageOverTime());
        }
    }

    IEnumerator FadeOutImageOverTime()
    {
        img.color = newColor;
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            img.color = Color.Lerp(newColor, oldColor, t / fadeDuration);
            yield return null;
        }
        img.color = oldColor;
        if (pingpong)
        {
            StartCoroutine(FadeInImageOverTime());
        }
    }
}

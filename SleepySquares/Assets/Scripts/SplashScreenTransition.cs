using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SplashScreenTransition : MonoBehaviour
{
    private Image[] imgs;
    private TextMeshProUGUI[] texts;
    public float fadeDuration = .25f;

    private void Awake()
    {
        GetArrays();
        TurnOnGameObjects();
    }

    private void TurnOnGameObjects()
    {
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        FadeOutSplash();
    }

    private void FadeOutSplash()
    {
        for (int i = 0; i < imgs.Length; i++)
        {
            StartCoroutine(FadeOutIMG(imgs[i]));
        }
        for (int i = 0; i < texts.Length; i++)
        {
            StartCoroutine(FadeOutTexts(texts[i]));
        }
    }

    public void FadeInSplash()
    {
        for (int i = 0; i < imgs.Length; i++)
        {
            StartCoroutine(FadeInIMG(imgs[i]));
        }
        for (int i = 0; i < texts.Length; i++)
        {
            StartCoroutine(FadeInTexts(texts[i]));
        }
    }

    private void GetArrays()
    {
        imgs = gameObject.transform.GetComponentsInChildren<Image>(true);
        texts = gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
    }

    IEnumerator FadeOutTexts(TextMeshProUGUI text)
    {
        Color oldColor = text.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.g, 0f);

        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            text.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        text.color = newColor;
        text.gameObject.SetActive(false);
    }

    IEnumerator FadeInTexts(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(true);
        Color oldColor = text.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.g, 1f);

        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            text.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        text.color = newColor;
    }

    IEnumerator FadeOutIMG(Image img)
    {
        Color oldColor = img.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.g, 0f);

        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            img.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        img.color = newColor;
        img.gameObject.SetActive(false);
    }

    IEnumerator FadeInIMG(Image img)
    {
        img.gameObject.SetActive(true);
        Color oldColor = img.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.g, 1f);

        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            img.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        img.color = newColor;
    }
}

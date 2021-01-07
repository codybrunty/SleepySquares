using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FloatingText : MonoBehaviour{

    Vector3 currentPos;
    RectTransform rt;

    public float delayTime = 0.15f;
    public float moveTime = 0.15f;
    public Vector3 moveAmmount = new Vector3(195f, 0f, 0f);


    public float fadeInTime = 0.25f;
    public float holdTime = 0.25f;
    public float fadeOutTime = 0.25f;

    private Vector3 squarePos;
    private Vector3 currentScale;
    private Vector3 smallScale = new Vector3(.01f,.01f,.01f);

    private Coroutine moveCo = null;
    private Coroutine scaleCo = null;
    private Coroutine fadeInCo = null;
    private Coroutine fadeOutCo = null;

    private TextMeshProUGUI mainText;

    private void Awake() {
        mainText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Start() {
        rt = gameObject.GetComponent<RectTransform>();
        currentPos = rt.anchoredPosition;
        currentScale = rt.localScale;
    }

    public void FlashText()
    {
        rt.anchoredPosition = currentPos;
        rt.localScale = smallScale;

        Color newColor = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 1f);
        Color oldColor = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 0f);

        TurnOffText();

        if (moveCo == null) {
            moveCo = StartCoroutine(TweenMove(squarePos + moveAmmount, moveTime));
        }
        else
        {
            StopCoroutine(moveCo);
            moveCo = StartCoroutine(TweenMove(squarePos + moveAmmount, moveTime));
        }

        if (scaleCo == null)
        {
            scaleCo = StartCoroutine(TweenScale());
        }
        else
        {
            StopCoroutine(scaleCo);
            scaleCo = StartCoroutine(TweenScale());
        }

        if (fadeInCo == null)
        {
            fadeInCo = StartCoroutine(FadeInAlpha(newColor));
        }
        else
        {
            StopCoroutine(fadeInCo);
            fadeInCo = StartCoroutine(FadeInAlpha(newColor));
        }

        if (fadeOutCo == null)
        {
            fadeOutCo = StartCoroutine(FadeOutAlpha(newColor));
        }
        else
        {
            StopCoroutine(fadeOutCo);
            fadeOutCo = StartCoroutine(FadeOutAlpha(newColor));
        }


    }

    IEnumerator TweenScale()
    {
        for (float t = 0f; t < fadeInTime; t += Time.deltaTime)
        {
            rt.localScale = Vector3.Lerp(smallScale, currentScale, t / fadeInTime);
            yield return null;
        }
        rt.localScale = currentScale;
    }

    IEnumerator TweenMove(Vector3 targetPos,float duration) {
        yield return new WaitForSeconds(delayTime);
        for (float time = 0f; time < duration; time += Time.deltaTime) {
            rt.anchoredPosition = Vector3.Lerp(currentPos, targetPos, time / duration);
            yield return null;
        }
        rt.anchoredPosition = targetPos;
    }

    IEnumerator FadeInAlpha(Color targetColor)
    {
        Color orginalColor = mainText.color;
        for (float time = 0f; time < fadeInTime; time += Time.deltaTime)
        {
            mainText.color = Color.Lerp(orginalColor, targetColor, time / fadeInTime);
            yield return null;
        }

        mainText.color = targetColor;
    }

    IEnumerator FadeOutAlpha(Color targetColor)
    {
        Color orginalColor = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 0f);

        yield return new WaitForSeconds(holdTime);
        for (float t = 0f; t < fadeOutTime; t += Time.deltaTime)
        {
            mainText.color = Color.Lerp(targetColor, orginalColor, t / fadeOutTime);
            yield return null;
        }

        mainText.color = orginalColor;
        ResetPosition();
        TurnOffText();
    }

    private void ResetPosition()
    {
        rt.anchoredPosition = currentPos;
    }

    private void TurnOffText()
    {
        Color newColor = new Color(mainText.color.r, mainText.color.g, mainText.color.b,0f);
        mainText.color = newColor;
    }


}

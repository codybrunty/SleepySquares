using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FloatingText : MonoBehaviour{

    Vector3 currentPos;
    RectTransform rt;

    public float moveTime = 0.15f;
    public Vector3 moveAmmount = new Vector3(195f, 0f, 0f);

    void Start() {
        rt = gameObject.GetComponent<RectTransform>();
        currentPos = rt.anchoredPosition;
    }

    public void FlashText() {
        Color newColor = new Color(gameObject.GetComponent<TextMeshProUGUI>().color.r, gameObject.GetComponent<TextMeshProUGUI>().color.g, gameObject.GetComponent<TextMeshProUGUI>().color.b, 1f);

        StartCoroutine(TwenMove(currentPos + moveAmmount, moveTime));
        StartCoroutine(TweenAlpha(newColor, moveTime, true));
    }

    IEnumerator TwenMove(Vector3 targetPos,float duration) {
        for (float time = 0f; time < duration; time += Time.deltaTime) {
            rt.anchoredPosition = Vector3.Lerp(currentPos, targetPos, time / duration);
            yield return null;
        }
        rt.anchoredPosition = targetPos;
    }

    IEnumerator TweenAlpha(Color targetColor, float duration,bool textOn) {
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        Color orginalColor = text.color;

        for (float time = 0f; time < duration; time += Time.deltaTime) {
            text.color = Color.Lerp(orginalColor, targetColor, time / duration);
            yield return null;
        }

        text.color = targetColor;

        if (textOn) {
            yield return new WaitForSeconds(0.5f);

            Color newColor = new Color(gameObject.GetComponent<TextMeshProUGUI>().color.r, gameObject.GetComponent<TextMeshProUGUI>().color.g, gameObject.GetComponent<TextMeshProUGUI>().color.b, 0f);
            StartCoroutine(TweenAlpha(newColor, 0.25f,false));
        }
        else {
            rt.anchoredPosition = currentPos;
        }
    }




}

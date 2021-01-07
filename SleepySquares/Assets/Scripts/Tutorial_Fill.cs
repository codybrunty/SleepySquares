using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Fill : MonoBehaviour
{
    public AnimationCurve easeCurve;
    private Coroutine co = null;

    public void FillEffect(int score, int total)
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        float newFillNumber = (float)score / (total + 50);
        co = StartCoroutine(FillOverTime(1f-newFillNumber));
    }

    IEnumerator FillOverTime(float targetFillNumber)
    {
        //Debug.LogWarning(targetFillNumber);

        float currentFillNumber = gameObject.GetComponent<Image>().fillAmount;
        float fillDuration = 0.5f;

        for (float t = 0f; t < fillDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fillDuration;
            gameObject.GetComponent<Image>().fillAmount = Mathf.Lerp(currentFillNumber, targetFillNumber, easeCurve.Evaluate(normalizedTime));
            yield return null;
        }

        gameObject.GetComponent<Image>().fillAmount = targetFillNumber;


    }
}

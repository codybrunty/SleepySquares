using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearButtonFill : MonoBehaviour{

    public float fillDuration = 0.25f;
    public AnimationCurve easeCurve;
    bool fillAtStart = false;

    public void UpdateFillDisplay(float percentageFill) {
        if (!fillAtStart) {
            fillAtStart = true;
            gameObject.GetComponent<Image>().fillAmount = percentageFill;
        }
        else {
            if (percentageFill == 0f && gameObject.GetComponent<Image>().fillAmount == 0f) {

            }
            else {
                StartCoroutine(FillOverTime(percentageFill));
            }
        }
    }

    IEnumerator FillOverTime(float targetFillNumber) {

        float currentFillNumber = gameObject.GetComponent<Image>().fillAmount;

        for (float t = 0f; t < fillDuration; t += Time.deltaTime) {
            float normalizedTime = t / fillDuration;
            gameObject.GetComponent<Image>().fillAmount = Mathf.Lerp(currentFillNumber, targetFillNumber, easeCurve.Evaluate(normalizedTime));
            yield return null;
        }

        gameObject.GetComponent<Image>().fillAmount = targetFillNumber;


    }

}

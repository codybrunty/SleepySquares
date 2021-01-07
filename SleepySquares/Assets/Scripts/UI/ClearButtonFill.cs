using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearButtonFill : MonoBehaviour{

    public float fillDuration = 0.25f;
    public AnimationCurve easeCurve;
    bool fillAtStart = false;
    private Image mainImage;

    private void Awake() {
        mainImage = gameObject.GetComponent<Image>();
    }
    public void UpdateFillDisplay(float percentageFill) {
        if (!fillAtStart) {
            fillAtStart = true;
            mainImage.fillAmount = percentageFill;
        }
        else {
            if (percentageFill == 0f && mainImage.fillAmount == 0f) {

            }
            else {
                StartCoroutine(FillOverTime(percentageFill));
            }
        }
    }

    IEnumerator FillOverTime(float targetFillNumber) {

        float currentFillNumber = mainImage.fillAmount;

        for (float t = 0f; t < fillDuration; t += Time.deltaTime) {
            float normalizedTime = t / fillDuration;
            mainImage.fillAmount = Mathf.Lerp(currentFillNumber, targetFillNumber, easeCurve.Evaluate(normalizedTime));
            yield return null;
        }

        mainImage.fillAmount = targetFillNumber;


    }

}

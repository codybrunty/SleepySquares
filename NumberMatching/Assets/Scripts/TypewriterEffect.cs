using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour{

    private string instructions;
    private TextMeshProUGUI txt;
    public float delayTime = 0f;

    private void OnEnable() {
        txt = gameObject.GetComponent<TextMeshProUGUI>();
        txt.maxVisibleCharacters = 0;
        StartCoroutine(TypewriterEffectOnText());
    }

    IEnumerator TypewriterEffectOnText() {
        yield return new WaitForSeconds(delayTime);
        instructions = txt.text.Trim();
        int totalVisibleCharacters = instructions.Length+1;

        int counter = 0;
        while (counter < totalVisibleCharacters) {
            txt.maxVisibleCharacters = counter;
            counter++;
            yield return new WaitForSeconds(0.05f);
        }

    }

}

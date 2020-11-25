using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Scoreboard : MonoBehaviour{

    [SerializeField] TextMeshProUGUI floatingText = default;
    private Coroutine co = null;
    private FloatingText floatTextGRP;

    private void Awake() {
        floatTextGRP = floatingText.GetComponent<FloatingText>();
    }

    public void ScoreboardAdd(int number) {

        if (co != null) {
            StopCoroutine(co);
        }
        co = StartCoroutine(PopAnim(number));

    }

    IEnumerator PopAnim(int number)
    {

        yield return new WaitForSeconds(0.1f);

        floatingText.text = "+" + number.ToString();
        floatTextGRP.FlashText();

        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2, 2f, 0f));
        hash.Add("time", 1.5f);
        iTween.PunchScale(gameObject.transform.parent.parent.gameObject, hash);
    }

}

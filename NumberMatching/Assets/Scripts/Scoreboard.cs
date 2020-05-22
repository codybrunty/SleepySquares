using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Scoreboard : MonoBehaviour{

    [SerializeField] GameObject floatingText = default;

    void Start() {
        gameObject.GetComponent<TextMeshProUGUI>().text = "0";
    }

    public void ScoreboardAdd(int number) {
        PopAnim();
        GameObject floatingText_GO = Instantiate(floatingText, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);
        floatingText_GO.GetComponent<TextMeshProUGUI>().text = "+" + number;
    }

    private void PopAnim() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(gameObject, hash);
    }
}

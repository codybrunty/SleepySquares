using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Scoreboard : MonoBehaviour{

    [SerializeField] TextMeshProUGUI floatingText = default;

    public void ScoreboardAdd(int number) {
        PopAnim();
        floatingText.text = "+" + number;
        floatingText.gameObject.GetComponent<FloatingText>().FlashText();
    }

    private void PopAnim() {
        
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(0.5f, 0.5f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(gameObject.transform.parent.gameObject, hash);
        
    }
}

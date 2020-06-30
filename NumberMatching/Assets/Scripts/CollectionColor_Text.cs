using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectionColor_Text : MonoBehaviour{

    public string key;

    private void Start() {
        GetColor();
    }

    public void GetColor() {
        Color TextColor = CollectionManager.CM.GetUIColor(key);
        //take objects alpha
        gameObject.GetComponent<TextMeshProUGUI>().color = new Color(TextColor.r, TextColor.g, TextColor.b, gameObject.GetComponent<TextMeshProUGUI>().color.a);
    }
}

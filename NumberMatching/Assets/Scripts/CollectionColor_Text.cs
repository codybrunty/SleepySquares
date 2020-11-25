using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectionColor_Text : MonoBehaviour{

    public string key;
    private TextMeshProUGUI mainText;

    private void Awake() {
        SetMainText();
    }

    private void Start() {
        GetColor();
    }

    public void GetColor() {
        SetMainText();
        Color TextColor = CollectionManager.CM.GetUIColor(key);
        mainText.color = new Color(TextColor.r, TextColor.g, TextColor.b, mainText.color.a);
    }

    private void SetMainText() {
        if (mainText == null) {
            mainText = gameObject.GetComponent<TextMeshProUGUI>();
        }
    }
}

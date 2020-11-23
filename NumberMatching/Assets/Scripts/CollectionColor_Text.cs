using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectionColor_Text : MonoBehaviour{

    public string key;
    private TextMeshProUGUI mainText;

    private void Awake() {
        mainText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        GetColor();
    }

    public void GetColor() {
        Color TextColor = CollectionManager.CM.GetUIColor(key);
        mainText.color = new Color(TextColor.r, TextColor.g, TextColor.b, mainText.color.a);
    }
}

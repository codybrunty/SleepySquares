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
        gameObject.GetComponent<TextMeshProUGUI>().color = CollectionManager.CM.GetUIColor(key);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionColor_Image : MonoBehaviour{

    public string key;

    private void Start() {
        GetColor();
    }

    public void GetColor() {
        Color TextColor = CollectionManager.CM.GetUIColor(key);
        //take objects alpha
        gameObject.GetComponent<Image>().color = new Color(TextColor.r, TextColor.g, TextColor.b, gameObject.GetComponent<Image>().color.a);
    }

}

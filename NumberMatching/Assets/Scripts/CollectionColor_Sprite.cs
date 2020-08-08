using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionColor_Sprite : MonoBehaviour{

    public string key;

    public void GetColor() {
        Color TextColor = CollectionManager.CM.GetUIColor(key);
        //take objects alpha
        gameObject.GetComponent<SpriteRenderer>().color = new Color(TextColor.r, TextColor.g, TextColor.b, gameObject.GetComponent<SpriteRenderer>().color.a);
    }
}

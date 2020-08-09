using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionColor_Sprite : MonoBehaviour{

    public string key;

    public void GetColor() {
        Color spriteColor = CollectionManager.CM.GetUIColor(key);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, gameObject.GetComponent<SpriteRenderer>().color.a);
    }
}

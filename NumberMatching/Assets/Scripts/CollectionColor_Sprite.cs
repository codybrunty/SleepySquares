using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionColor_Sprite : MonoBehaviour{

    public string key;
    private SpriteRenderer mainSprite;

    private void Awake() {
        mainSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void GetColor() {
        Color spriteColor = CollectionManager.CM.GetUIColor(key);
        mainSprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, mainSprite.color.a);
    }
}

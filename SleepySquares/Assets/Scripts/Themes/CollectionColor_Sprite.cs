using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionColor_Sprite : MonoBehaviour{

    public string key;
    private SpriteRenderer mainSprite;

    private void Awake() {
        SetMainSprite();
    }

    public void GetColor() {
        SetMainSprite();
        Color spriteColor = CollectionManager.CM.GetUIColor(key);
        mainSprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, mainSprite.color.a);
    }

    private void SetMainSprite() {
        if (mainSprite == null) {
            mainSprite = gameObject.GetComponent<SpriteRenderer>();
        }
    }
}

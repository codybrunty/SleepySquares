﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionColor_Sprite : MonoBehaviour{

    public string key;

    private void Start() {
        GetColor();
    }

    public void GetColor() {
        gameObject.GetComponent<SpriteRenderer>().color = CollectionManager.CM.GetUIColor(key);
    }
}
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
        gameObject.GetComponent<Image>().color = CollectionManager.CM.GetUIColor(key);
    }

}

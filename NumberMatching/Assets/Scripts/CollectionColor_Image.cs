using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionColor_Image : MonoBehaviour{

    public string key;
    private Image mainImage;

    private void Awake() {
        mainImage = gameObject.GetComponent<Image>();
    }
    private void Start() {
        GetColor();
    }

    public void GetColor() {
        Color ImageColor = CollectionManager.CM.GetUIColor(key);
        mainImage.color = new Color(ImageColor.r, ImageColor.g, ImageColor.b, gameObject.GetComponent<Image>().color.a);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionColor_Image : MonoBehaviour{

    public string key;
    private Image mainImage;

    private void Awake() {
       SetMainImage();
    }
    private void Start() {
        GetColor();
    }

    public void GetColor() {
        SetMainImage();
        Color ImageColor = CollectionManager.CM.GetUIColor(key);
        mainImage.color = new Color(ImageColor.r, ImageColor.g, ImageColor.b, mainImage.color.a);
    }

    private void SetMainImage() {
        if (mainImage == null) {
            mainImage = gameObject.GetComponent<Image>();
        }
    }
}

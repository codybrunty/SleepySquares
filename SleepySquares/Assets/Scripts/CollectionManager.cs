using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionManager : MonoBehaviour{

    public static CollectionManager CM;

    public int assetIndex = 0;
    public CollectionAsset[] assets;
    
    private void Awake() {
        if (CM == null) {
            CM = this;
            assetIndex = PlayerPrefs.GetInt("CollectionAssetIndex", 0);
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    public void NextAssetIndex() {
        assetIndex++;

        if (assetIndex == assets.Length) {
            assetIndex = 0;
        }

        PlayerPrefs.SetInt("CollectionAssetIndex", assetIndex);
        UpdateAllCollectionAssets();
    }

    private void UpdateAllCollectionAssets() {
        GameObject[] collectionAssets = GameObject.FindGameObjectsWithTag("CollectionAsset");

        List<CollectionColor_Image> images = new List<CollectionColor_Image>();
        List<CollectionColor_Sprite> sprites = new List<CollectionColor_Sprite>();
        List<CollectionColor_Text> texts = new List<CollectionColor_Text>();

        foreach (GameObject obj in collectionAssets) {
            if(obj.GetComponent<CollectionColor_Image>() != null){
                images.Add(obj.GetComponent<CollectionColor_Image>());
            }
            else if (obj.GetComponent<CollectionColor_Sprite>() != null) {
                sprites.Add(obj.GetComponent<CollectionColor_Sprite>());
            }
            else if (obj.GetComponent<CollectionColor_Text>() != null) {
                texts.Add(obj.GetComponent<CollectionColor_Text>());
            }
        }

        foreach (CollectionColor_Image img in images) {
            img.GetColor();
        }
        foreach (CollectionColor_Sprite sp in sprites) {
            sp.GetColor();
        }
        foreach (CollectionColor_Text tex in texts) {
            tex.GetColor();
        }

    }

    public Color GetUIColor(string name) {
        switch (name) {
            case "First":
                return assets[assetIndex].baseColor;
            case "Second":
                return assets[assetIndex].secondaryColor;
            case "Third":
                return assets[assetIndex].scoreTextColor;
            case "Trophy1":
                return assets[assetIndex].trophy1Color;
            case "Trophy2":
                return assets[assetIndex].trophy2Color;
            case "Trophy3":
                return assets[assetIndex].trophy3Color;
            case "Button1":
                return assets[assetIndex].button1;
            case "Button2":
                return assets[assetIndex].button2;
            case "Button3":
                return assets[assetIndex].button3;
            case "Button4":
                return assets[assetIndex].button4;
            case "Button5":
                return assets[assetIndex].button5;
            default:
                return Color.red;

        }
    }

}

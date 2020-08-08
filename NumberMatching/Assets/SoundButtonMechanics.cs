using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundButtonMechanics : MonoBehaviour {
    
    [SerializeField] Sprite soundOnImage = default;
    [SerializeField] Sprite soundOffImage = default;

    private void Start() {
        SetSoundImages();
    }

    public void SoundButtonOnClick() {
        if (FindObjectOfType<SoundManager>().soundOn == 1) {
            FindObjectOfType<SoundManager>().TurnOffSound();
        }
        else {
            FindObjectOfType<SoundManager>().TurnOnSound();
        }
        SetSoundImages();
    }

    private void SetSoundImages() {
        if (FindObjectOfType<SoundManager>().soundOn == 1) {
            gameObject.GetComponent<Image>().sprite = soundOnImage;
            Color originalColor = gameObject.GetComponent<Image>().color;
            gameObject.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        }
        else {
            gameObject.GetComponent<Image>().sprite = soundOffImage;
            Color originalColor = gameObject.GetComponent<Image>().color;
            gameObject.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b,0.5f);
        }
    }
}
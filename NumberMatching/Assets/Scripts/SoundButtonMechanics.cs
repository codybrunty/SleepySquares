using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundButtonMechanics : MonoBehaviour {

    [SerializeField] Image soundImage = default;
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
            soundImage.sprite = soundOnImage;
            Color newColor = soundImage.color;
            soundImage.color = new Color(newColor.r, newColor.g, newColor.b,1f);
        }
        else {
            soundImage.sprite = soundOffImage;
            Color newColor = soundImage.color;
            soundImage.color = new Color(newColor.r, newColor.g, newColor.b, .5f);
        }
    }
}
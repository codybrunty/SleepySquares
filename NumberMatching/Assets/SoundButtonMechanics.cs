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
        }
        else {
            soundImage.sprite = soundOffImage;
        }
    }
}
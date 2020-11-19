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
    public float fadeDuration = .5f;

    private void OnEnable()
    {
        SetSoundImages();
    }

    public void TurnOffMusicButton()
    {
        StartCoroutine(FadeOut());
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
            Color newColor = new Color(soundImage.color.r, soundImage.color.g, soundImage.color.b, 1f);
            StartCoroutine(FadeIn(newColor));
        }
        else {
            soundImage.sprite = soundOffImage;
            Color newColor = new Color(soundImage.color.r, soundImage.color.g, soundImage.color.b, .5f);
            StartCoroutine(FadeIn(newColor));
        }
    }

    IEnumerator FadeOut()
    {
        Color zeroAlphaColor = new Color(soundImage.color.r, soundImage.color.g, soundImage.color.b, 0f);
        Color currentColor = new Color(soundImage.color.r, soundImage.color.g, soundImage.color.b, soundImage.color.a);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            soundImage.color = Color.Lerp(currentColor, zeroAlphaColor, t / fadeDuration);
            yield return null;
        }
        soundImage.color = zeroAlphaColor;
        gameObject.SetActive(false);
    }

    IEnumerator FadeIn(Color color)
    {
        Color zeroAlphaColor = new Color(color.r, color.g, color.b, 0f);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            soundImage.color = Color.Lerp(zeroAlphaColor, color, t / fadeDuration);
            yield return null;
        }
        soundImage.color = color;
    }
}
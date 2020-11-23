using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButtonMechanics : MonoBehaviour{

    [SerializeField] Image musicImage = default;
    [SerializeField] Sprite musicOnSprite = default;
    [SerializeField] Sprite musicOffSprite = default;
    public float fadeDuration = .5f;

    private void OnEnable()
    {
        SetMusicToggleImage();
    }

    public void TurnOffMusicButton()
    {
        StartCoroutine(FadeOut());
    }

    public void MusicButtonOnClick() {
        if (MusicManager.MM.musicOn == 1) {
            MusicManager.MM.StopMusic();
        }
        else {
            MusicManager.MM.StartMusic();
        }
        SetMusicToggleImage();
    }

    private void SetMusicToggleImage() {
        if (MusicManager.MM.musicOn == 1) {
            musicImage.sprite = musicOnSprite;
            Color newColor = new Color(musicImage.color.r, musicImage.color.g, musicImage.color.b, 1f);
            StartCoroutine(FadeIn(newColor));
        }
        else {
            musicImage.sprite = musicOffSprite;
            Color newColor = new Color(musicImage.color.r, musicImage.color.g, musicImage.color.b, .5f);
            StartCoroutine(FadeIn(newColor));
        }
    }

    IEnumerator FadeOut()
    {
        Color zeroAlphaColor = new Color(musicImage.color.r, musicImage.color.g, musicImage.color.b, 0f);
        Color currentColor = new Color(musicImage.color.r, musicImage.color.g, musicImage.color.b, musicImage.color.a);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicImage.color = Color.Lerp(currentColor, zeroAlphaColor, t / fadeDuration);
            yield return null;
        }
        musicImage.color = zeroAlphaColor;
        gameObject.SetActive(false);
    }

    IEnumerator FadeIn(Color color)
    {
        Color zeroAlphaColor = new Color(color.r,color.g,color.b,0f);

        for(float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicImage.color = Color.Lerp(zeroAlphaColor,color,t/fadeDuration);
            yield return null;
        }
        musicImage.color = color;
    }

}

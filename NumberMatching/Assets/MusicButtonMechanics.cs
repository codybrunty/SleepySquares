using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButtonMechanics : MonoBehaviour{

    [SerializeField] Image musicImage = default;
    [SerializeField] Sprite musicOnSprite = default;
    [SerializeField] Sprite musicOffSprite = default;

    private void Start() {
        SetMusicToggleImage();
    }

    public void MusicButtonOnClick() {
        if (FindObjectOfType<MusicManager>().musicOn == 1) {
            FindObjectOfType<MusicManager>().StopMusic();
        }
        else {
            FindObjectOfType<MusicManager>().StartMusic();
        }
        SetMusicToggleImage();
    }

    private void SetMusicToggleImage() {
        if (FindObjectOfType<MusicManager>().musicOn == 1) {
            musicImage.sprite = musicOnSprite;
            Color newColor = musicImage.color;
            musicImage.color = new Color(newColor.r, newColor.g, newColor.b, 1f);
        }
        else {
            musicImage.sprite = musicOffSprite;
            Color newColor = musicImage.color;
            musicImage.color = new Color(newColor.r, newColor.g, newColor.b, .5f);
        }
    }

}

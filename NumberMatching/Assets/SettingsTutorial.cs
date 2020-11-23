using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsTutorial : MonoBehaviour
{
    [SerializeField] GameObject MainPanel = default;
    [SerializeField] GameObject Exit = default;
    [SerializeField] GameObject Music = default;
    [SerializeField] GameObject SFX = default;
    [SerializeField] GameObject bg = default;
    public float fadeDuration = .5f;


    public void ShowMainPanel() {
        FadeInPanel(MainPanel);
        FadeInBG();
        FadeInExit();
        Music.SetActive(true);
        SFX.SetActive(true);
        PlayPositiveSFX();
    }

    public void ExitSettings() {
        StartCoroutine(FadeOutPanel(MainPanel));
        StartCoroutine(FadeOutExit());
        StartCoroutine(FadeOutBG());
        TurnOffSoundButtons();
        PlayNegativeSFX();
    }

    private void TurnOffSoundButtons() {
        if (Music.activeSelf == true) {
            Music.GetComponent<MusicButtonMechanics>().TurnOffMusicButton();
            SFX.GetComponent<SoundButtonMechanics>().TurnOffMusicButton();
        }
    }

    private void PlayNegativeSFX() {
        SoundManager.SM.PlayOneShotSound("deselect1");
    }

    private void PlayPositiveSFX() {
        SoundManager.SM.PlayOneShotSound("select1");
    }

    private void FadeInPanel(GameObject panel) {

        Image[] images = panel.GetComponentsInChildren<Image>(true);
        for (int i = 0; i < images.Length; i++) {
            Color newColor = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 0f);
            images[i].color = newColor;
        }

        TextMeshProUGUI[] texts = panel.GetComponentsInChildren<TextMeshProUGUI>(true);
        for (int i = 0; i < texts.Length; i++) {
            Color newColor = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, 0f);
            texts[i].color = newColor;
        }

        panel.SetActive(true);

        for (int i = 0; i < images.Length; i++) {
            if (images[i].name != "SCROLL") {
                StartCoroutine(FadeInImgs(images[i]));
            }
        }

        for (int i = 0; i < texts.Length; i++) {
            StartCoroutine(FadeInTexts(texts[i]));
        }

    }

    IEnumerator FadeOutPanel(GameObject panel) {
        Image[] images = panel.GetComponentsInChildren<Image>(true);

        for (int i = 0; i < images.Length; i++) {
            StartCoroutine(FadeOutImgs(images[i]));
        }

        TextMeshProUGUI[] texts = panel.GetComponentsInChildren<TextMeshProUGUI>(true);
        for (int i = 0; i < texts.Length; i++) {
            StartCoroutine(FadeOutTexts(texts[i]));
        }

        yield return new WaitForSeconds(fadeDuration);

        panel.SetActive(false);
    }

    private void FadeInBG() {
        if (bg.activeSelf == false) {
            bg.GetComponent<Image>().color = new Color(bg.GetComponent<Image>().color.r, bg.GetComponent<Image>().color.g, bg.GetComponent<Image>().color.b, 0f);
            bg.SetActive(true);
            StartCoroutine(FadeInImgs(bg.GetComponent<Image>()));
        }
    }

    IEnumerator FadeOutBG() {
        StartCoroutine(FadeOutImgs(bg.GetComponent<Image>()));
        yield return new WaitForSeconds(fadeDuration);
        bg.SetActive(false);
    }

    private void FadeInExit() {
        if (Exit.activeSelf == false) {
            Exit.GetComponent<Image>().color = new Color(Exit.GetComponent<Image>().color.r, Exit.GetComponent<Image>().color.g, Exit.GetComponent<Image>().color.b, 0f);
            Exit.SetActive(true);
            StartCoroutine(FadeInImgs(Exit.GetComponent<Image>()));
        }
    }

    IEnumerator FadeOutExit() {
        StartCoroutine(FadeOutImgs(Exit.GetComponent<Image>()));
        yield return new WaitForSeconds(fadeDuration);
        Exit.SetActive(false);
    }

    IEnumerator FadeInImgs(Image img) {
        //Debug.Log(img.gameObject.name);
        Color oldColor = img.color;
        Color newColor = new Color(img.color.r, img.color.g, img.color.b, img.GetComponent<Settings_Image>().originalAlpha);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime) {
            img.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        img.color = newColor;
    }

    IEnumerator FadeInTexts(TextMeshProUGUI txt) {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime) {
            txt.color = Color.Lerp(new Color(txt.color.r, txt.color.g, txt.color.b, 0f), new Color(txt.color.r, txt.color.g, txt.color.b, 1f), t / fadeDuration);
            yield return null;
        }
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1f);
    }

    IEnumerator FadeOutImgs(Image img) {
        Color oldColor = img.color;
        Color newColor = new Color(img.color.r, img.color.g, img.color.b, 0f);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime) {
            img.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        img.color = newColor;
    }

    IEnumerator FadeOutTexts(TextMeshProUGUI txt) {
        Color oldColor = txt.color;
        Color newColor = new Color(txt.color.r, txt.color.g, txt.color.b, 0f);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime) {
            txt.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        txt.color = newColor;
    }
}

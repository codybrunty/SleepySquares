using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour{

    [SerializeField] GameObject MainPanel = default;
    [SerializeField] GameObject TrophyPanel = default;
    [SerializeField] GameObject StorePanel = default;
    [SerializeField] GameObject LanguagePanel = default;
    [SerializeField] GameObject CreditsPanel = default;
    [SerializeField] Image Back = default;
    [SerializeField] Image Exit = default;
    [SerializeField] MusicButtonMechanics Music = default;
    [SerializeField] SoundButtonMechanics SFX = default;
    [SerializeField] Image bg = default;
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] NotificationSystem notificationSystem = default;
    [SerializeField] GameObject internetreachable = default;
    public float fadeDuration = .5f;
    public DailyManager m_oDailyManager;

    public void ShowMainPanel() {
        FadeInPanel(MainPanel);

        FadeInBG();
        FadeInExit();
        Music.gameObject.SetActive(true);
        SFX.gameObject.SetActive(true);

        StartCoroutine(FadeOutBack());
        StartCoroutine(FadeOutPanel(StorePanel));
        StartCoroutine(FadeOutPanel(LanguagePanel));
        StartCoroutine(FadeOutPanel(CreditsPanel));
        StartCoroutine(FadeOutPanel(TrophyPanel));

        DisableGameboardTouch();
        PlayPositiveSFX();
        internetreachable.SetActive(false);
    }

    public void ShowMainPanel(bool playNegative)
    {
        FadeInPanel(MainPanel);

        FadeInBG();
        FadeInExit();
        Music.gameObject.SetActive(true);
        SFX.gameObject.SetActive(true);

        StartCoroutine(FadeOutBack());
        StartCoroutine(FadeOutPanel(StorePanel));
        StartCoroutine(FadeOutPanel(LanguagePanel));
        StartCoroutine(FadeOutPanel(CreditsPanel));
        StartCoroutine(FadeOutPanel(TrophyPanel));

        DisableGameboardTouch();
        if (playNegative)
        {
            PlayNegativeSFX();
        }
        else
        {
            PlayPositiveSFX();
        }

        internetreachable.SetActive(false);
    }

    public void ShowTrophyPanel()
    {
        FadeInPanel(TrophyPanel);

        StartCoroutine(FadeOutPanel(MainPanel));
        StartCoroutine(FadeOutPanel(StorePanel));
        StartCoroutine(FadeOutPanel(LanguagePanel));
        StartCoroutine(FadeOutPanel(CreditsPanel));
        FadeInBG();
        FadeInExit();
        FadeInBack();

        TurnOffSoundButtons();
        DisableGameboardTouch();
        PlayPositiveSFX();

        internetreachable.SetActive(false);
    }

    public void ShowStorePanel()
    {
        internetreachable.SetActive(true);
        FadeInPanel(StorePanel);

        StartCoroutine(FadeOutPanel(MainPanel));
        StartCoroutine(FadeOutPanel(LanguagePanel));
        StartCoroutine(FadeOutPanel(CreditsPanel));
        StartCoroutine(FadeOutPanel(TrophyPanel));
        FadeInBG();
        FadeInExit();
        FadeInBack();

        TurnOffSoundButtons();
        DisableGameboardTouch();
        PlayPositiveSFX();
    }

    public void ShowLanguagePanel() {
        FadeInPanel(LanguagePanel);

        StartCoroutine(FadeOutPanel(MainPanel));
        StartCoroutine(FadeOutPanel(StorePanel));
        StartCoroutine(FadeOutPanel(CreditsPanel));
        StartCoroutine(FadeOutPanel(TrophyPanel));

        FadeInBG();
        FadeInExit();
        FadeInBack();

        TurnOffSoundButtons();
        DisableGameboardTouch();
        PlayPositiveSFX();

        internetreachable.SetActive(false);
    }

    public void ShowCreditsPanel()
    {
        FadeInPanel(CreditsPanel);

        StartCoroutine(FadeOutPanel(MainPanel));
        StartCoroutine(FadeOutPanel(StorePanel));
        StartCoroutine(FadeOutPanel(LanguagePanel));
        StartCoroutine(FadeOutPanel(TrophyPanel));

        FadeInBG();
        FadeInExit();
        FadeInBack();

        TurnOffSoundButtons();
        DisableGameboardTouch();
        PlayPositiveSFX();

        internetreachable.SetActive(false);
    }



    public void ExitSettings()
    {
        StartCoroutine(FadeOutPanel(MainPanel));
        StartCoroutine(FadeOutPanel(StorePanel));
        StartCoroutine(FadeOutPanel(LanguagePanel));
        StartCoroutine(FadeOutPanel(CreditsPanel));
        StartCoroutine(FadeOutPanel(TrophyPanel));
        StartCoroutine(FadeOutExit());
        StartCoroutine(FadeOutBack());
        StartCoroutine(FadeOutBG());

        TurnOffSoundButtons();
        StartCoroutine(EnableGameboardTouch());
        PlayNegativeSFX();
        internetreachable.SetActive(false);
    }

    public void ExitSettings(bool playSound)
    {
        StartCoroutine(FadeOutPanel(MainPanel));
        StartCoroutine(FadeOutPanel(StorePanel));
        StartCoroutine(FadeOutPanel(LanguagePanel));
        StartCoroutine(FadeOutPanel(CreditsPanel));
        StartCoroutine(FadeOutPanel(TrophyPanel));
        StartCoroutine(FadeOutExit());
        StartCoroutine(FadeOutBack());
        StartCoroutine(FadeOutBG());

        TurnOffSoundButtons();
        StartCoroutine(EnableGameboardTouch());

        if (playSound)
        {
            PlayNegativeSFX();
        }

        internetreachable.SetActive(false);
    }

    private void TurnOffSoundButtons()
    {
        if (Music.gameObject.activeSelf == true)
        {
            Music.TurnOffMusicButton();
            SFX.TurnOffMusicButton();
        }
    }

    private void PlayNegativeSFX() {
        SoundManager.SM.PlayOneShotSound("deselect1");
    }

    private void PlayPositiveSFX() {
        SoundManager.SM.PlayOneShotSound("select1");
    }

    IEnumerator EnableGameboardTouch() {
        yield return new WaitForSeconds(.25f);
        if (gameboard.gameOver != true) {
            
            gameboard.touchEnabled = true;

            //if we dont have any hearts and daily mode is on
            if (m_oDailyManager.hasHearts==false && gameboard.DailyModeOn == true) {
                gameboard.touchEnabled = false;
            }
        }
    }

    private void DisableGameboardTouch() {
        if (gameboard.gameOver != true) {
            gameboard.touchEnabled = false;
        }
    }

    private void FadeInPanel(GameObject panel)
    {

        Image[] images = panel.GetComponentsInChildren<Image>(true);
        for (int i = 0; i < images.Length; i++)
        {
            Color newColor = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 0f);
            images[i].color = newColor;
        }

        TextMeshProUGUI[] texts = panel.GetComponentsInChildren<TextMeshProUGUI>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            Color newColor = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, 0f);
            texts[i].color = newColor;
        }

        panel.SetActive(true);

        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].name != "SCROLL")
            {
                StartCoroutine(FadeInImgs(images[i]));
            }
        }

        for (int i = 0; i < texts.Length; i++)
        {
            StartCoroutine(FadeInTexts(texts[i]));
        } 

    }

    IEnumerator FadeOutPanel(GameObject panel)
    {
        Image[] images = panel.GetComponentsInChildren<Image>(true);

        for (int i = 0; i < images.Length; i++)
        {
            StartCoroutine(FadeOutImgs(images[i]));
        }

        TextMeshProUGUI[] texts = panel.GetComponentsInChildren<TextMeshProUGUI>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            StartCoroutine(FadeOutTexts(texts[i]));
        }

        yield return new WaitForSeconds(fadeDuration);

        panel.SetActive(false);
    }

    private void FadeInBG()
    {
        if (bg.gameObject.activeSelf == false)
        {
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0f);
            bg.gameObject.SetActive(true);
            StartCoroutine(FadeInImgs(bg));
        }
    }

    private void FadeInExit()
    {
        if (Exit.gameObject.activeSelf == false)
        {
            Exit.color = new Color(Exit.color.r, Exit.color.g, Exit.color.b, 0f);
            Exit.gameObject.SetActive(true);
            StartCoroutine(FadeInImgs(Exit));
        }
    }

    IEnumerator FadeOutExit()
    {
        StartCoroutine(FadeOutImgs(Exit));
        yield return new WaitForSeconds(fadeDuration);
        Exit.gameObject.SetActive(false);
    }

    IEnumerator FadeOutBG()
    {
        StartCoroutine(FadeOutImgs(bg));
        yield return new WaitForSeconds(fadeDuration);
        bg.gameObject.SetActive(false);
    }

    IEnumerator FadeOutBack()
    {
        StartCoroutine(FadeOutImgs(Back));
        yield return new WaitForSeconds(fadeDuration);
        Back.gameObject.SetActive(false);
    }

    private void FadeInBack()
    {
        if (Back.gameObject.activeSelf == false)
        {
            Back.color = new Color(Back.color.r, Back.color.g, Back.color.b, 0f);
            Back.gameObject.SetActive(true);
            StartCoroutine(FadeInImgs(Back));
        }
    }

    IEnumerator FadeInImgs(Image img)
    {
        //Debug.Log(img.gameObject.name);
        Color oldColor = img.color;
        Color newColor = new Color(img.color.r, img.color.g, img.color.b, img.GetComponent<Settings_Image>().originalAlpha);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            img.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        img.color = newColor;
    }

    IEnumerator FadeInTexts(TextMeshProUGUI txt)
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            txt.color = Color.Lerp(new Color(txt.color.r, txt.color.g, txt.color.b, 0f), new Color(txt.color.r, txt.color.g, txt.color.b, 1f), t / fadeDuration);
            yield return null;
        }
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1f);
    }

    IEnumerator FadeOutImgs(Image img)
    {
        Color oldColor = img.color;
        Color newColor = new Color(img.color.r, img.color.g, img.color.b, 0f);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            img.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        img.color = newColor;
    }

    IEnumerator FadeOutTexts(TextMeshProUGUI txt)
    {
        Color oldColor = txt.color;
        Color newColor = new Color(txt.color.r, txt.color.g, txt.color.b, 0f);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            txt.color = Color.Lerp(oldColor, newColor, t / fadeDuration);
            yield return null;
        }
        txt.color = newColor;
    }
}

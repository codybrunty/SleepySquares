﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LanguageButton : MonoBehaviour
{
    public string languageKey = default;
    private string currentLanguage = default;

    [SerializeField] TextMeshProUGUI text = default;
    [SerializeField] Color activeTextColor = default;

    [SerializeField] SplashScreenTransition splash = default;

    private Button mainButton;
    private Image mainImage;

    private void Awake() {
        mainButton = gameObject.GetComponent<Button>();
        mainImage = gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        currentLanguage = PlayerPrefs.GetString("Language", "English");
        if (languageKey == currentLanguage)
        {
            mainButton.interactable = false;
            mainImage.raycastTarget = false;
            text.color = activeTextColor;
        }
    }

    //for buttons inside the drop down
    public void LoadSpecificLanguage()
    {
        if (languageKey != "")
        {
            SoundManager.SM.PlayOneShotSound("select1");
            PlayerPrefs.SetString("Language", languageKey);
            splash.FadeInSplash();
            StartCoroutine(ReloadCurrentScene());
        }
    }

    IEnumerator ReloadCurrentScene()
    {
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
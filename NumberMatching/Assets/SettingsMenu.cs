using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour{

    [SerializeField] GameObject MainPanel = default;
    [SerializeField] GameObject StorePanel = default;
    [SerializeField] GameObject LanguagePanel = default;
    [SerializeField] GameObject CreditsPanel = default;
    [SerializeField] GameObject Exit = default;
    [SerializeField] GameObject bg = default;
    [SerializeField] GameBoardMechanics gameboard = default;


    public void ShowMainPanel() {
        MainPanel.SetActive(true);
        StorePanel.SetActive(false);
        LanguagePanel.SetActive(false);
        CreditsPanel.SetActive(false);
        Exit.SetActive(true);
        bg.SetActive(true);
        DisableGameboardTouch();
        PlayPositiveSFX();
    }

    public void ShowStorePanel() {
        MainPanel.SetActive(false);
        StorePanel.SetActive(true);
        LanguagePanel.SetActive(false);
        CreditsPanel.SetActive(false);
        Exit.SetActive(true);
        bg.SetActive(true);
        DisableGameboardTouch();
        PlayPositiveSFX();
    }

    public void ShowLanguagePanel() {
        MainPanel.SetActive(false);
        StorePanel.SetActive(false);
        LanguagePanel.SetActive(true);
        CreditsPanel.SetActive(false);
        Exit.SetActive(true);
        bg.SetActive(true);
        DisableGameboardTouch();
        PlayPositiveSFX();
    }

    public void ShowCreditsPanel() {
        MainPanel.SetActive(false);
        StorePanel.SetActive(false);
        LanguagePanel.SetActive(false);
        CreditsPanel.SetActive(true);
        Exit.SetActive(true);
        bg.SetActive(true);
        DisableGameboardTouch();
        PlayPositiveSFX();
    }

    public void ExitSettings() {
        MainPanel.SetActive(false);
        StorePanel.SetActive(false);
        LanguagePanel.SetActive(false);
        CreditsPanel.SetActive(false);
        Exit.SetActive(false);
        bg.SetActive(false);
        EnableGameboardTouch();
        PlayNegativeSFX();
    }

    private void PlayNegativeSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("deselect1");
    }

    private void PlayPositiveSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
    }

    private void EnableGameboardTouch() {
        if (gameboard.gameOver != true) {
            gameboard.touchEnabled = true;
        }
    }

    private void DisableGameboardTouch() {
        if (gameboard.gameOver != true) {
            gameboard.touchEnabled = false;
        }
    }


}

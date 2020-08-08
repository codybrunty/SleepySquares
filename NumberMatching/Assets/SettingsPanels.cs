using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanels : MonoBehaviour{

    [SerializeField] GameObject mainPanel = default;
    [SerializeField] GameObject storePanel = default;
    [SerializeField] GameObject languagePanel = default;

    [SerializeField] Image bg = default;
    [SerializeField] GameObject exit = default;

    [SerializeField] GameBoardMechanics gameboard = default;

    public void ShowMain() {
        mainPanel.SetActive(true);
        storePanel.SetActive(false);
        languagePanel.SetActive(false);


        bg.enabled=true;
        exit.SetActive(true);

        DisableGameboardTouch();

        PlayPositiveClickSFX();
    }

    public void ShowStore() {
        mainPanel.SetActive(false);
        storePanel.SetActive(true);
        languagePanel.SetActive(false);

        bg.enabled = true;
        exit.SetActive(true);

        DisableGameboardTouch();

        PlayPositiveClickSFX();
    }

    public void ShowLanguages() {
        mainPanel.SetActive(false);
        storePanel.SetActive(false);
        languagePanel.SetActive(true);

        bg.enabled = true;
        exit.SetActive(true);

        DisableGameboardTouch();

        PlayPositiveClickSFX();
    }

    public void ExitSettings() {
        mainPanel.SetActive(false);
        storePanel.SetActive(false);
        languagePanel.SetActive(false);

        bg.enabled = false;
        exit.SetActive(false);

        EnableGameboardTouch();

        PlayNegativeClickSFX();
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

    private void PlayNegativeClickSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("deselect1");
    }

    private void PlayPositiveClickSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
    }
}

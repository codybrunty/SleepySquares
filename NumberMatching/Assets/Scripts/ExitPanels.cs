using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanels : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] TimerCountdown timer = default;
    [SerializeField] GameObject pausePanel = default;
    [SerializeField] GameObject collectionsPanel = default;
    [SerializeField] GameObject profilePanel = default;
    [SerializeField] GameObject settingsPanel = default;

    public void ExitOnClick() {
        gameboard.touchEnabled = true;
        timer.isPaused = false;
        pausePanel.SetActive(false);
        collectionsPanel.SetActive(false);
        profilePanel.SetActive(false);
        settingsPanel.SetActive(false);
        gameObject.SetActive(false);

    }


}

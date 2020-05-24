using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPanels : MonoBehaviour{

    [SerializeField] TimerCountdown timer = default;
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] GameObject panelOn = default;
    [SerializeField] GameObject exitButton = default;

    public void EnterOnClick() {
        gameboard.touchEnabled = false;
        timer.isPaused = true;
        panelOn.SetActive(true);
        exitButton.SetActive(true);
    }

}

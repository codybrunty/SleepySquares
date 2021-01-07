using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HardModeButtonMechanics : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] GameObject hard = default;
    [SerializeField] GameObject normal = default;
    [SerializeField] GameObject daily = default;

    private void OnEnable()
    {
        SetModeDisplay();
    }

    public void HardModeButtonOnClick() {
        if (gameBoard.hardModeOn == 1) {
            gameBoard.TurnOffHardMode();
        }
        else {
            gameBoard.TurnOnHardMode();
        }
        SetModeDisplay();
    }

    public void HardModeButtonOnClick(int num) {
        gameBoard.DailyModeOn = false;
        if (num == 0) {
            gameBoard.TurnOffHardMode();
        }
        else {
            gameBoard.TurnOnHardMode();
        }
        SetModeDisplay();
    }

    private void SetModeDisplay() {
        if (gameBoard.DailyModeOn) {
            hard.SetActive(false);
            normal.SetActive(false);
            daily.SetActive(true);
        }
        else {
            if (gameBoard.hardModeOn == 1) {
                hard.SetActive(true);
                normal.SetActive(false);
                daily.SetActive(false);
            }
            else {
                hard.SetActive(false);
                normal.SetActive(true);
                daily.SetActive(false);
            }
        }
        
    }

}

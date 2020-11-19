using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HardModeButtonMechanics : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] GameObject hard = default;
    [SerializeField] GameObject normal = default;

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

    private void SetModeDisplay() {
        if (gameBoard.hardModeOn == 1)
        {
            hard.SetActive(true);
            normal.SetActive(false);
        }
        else
        {
            hard.SetActive(false);
            normal.SetActive(true);
        }
    }

}

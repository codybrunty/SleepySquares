using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HardModeButtonMechanics : MonoBehaviour{

    [SerializeField] TextMeshProUGUI hardModeTextDisplay = default;
    [SerializeField] GameBoardMechanics gameBoard = default;

    private void Start() {
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
        if (gameBoard.hardModeOn == 1) {
            hardModeTextDisplay.text = "E";
        }
        else {
            hardModeTextDisplay.text = "H";
        }
    }

}

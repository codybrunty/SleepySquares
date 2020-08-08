using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HardModeButtonMechanics : MonoBehaviour{

    [SerializeField] TextMeshProUGUI hardModeTextDisplay = default;
    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] GameObject hardSquare = default;
    [SerializeField] GameObject easySquare = default;

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
            hardModeTextDisplay.text = "NormalMode";
            hardSquare.SetActive(false);
            easySquare.SetActive(true);
        }
        else {
            hardModeTextDisplay.text = "Hard Mode";
            hardSquare.SetActive(true);
            easySquare.SetActive(false);
        }
    }

}

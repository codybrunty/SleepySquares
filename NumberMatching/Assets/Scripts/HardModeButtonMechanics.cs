using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HardModeButtonMechanics : MonoBehaviour{

    [SerializeField] TextMeshProUGUI hardModeTextDisplay = default;
    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] GameObject square_4 = default;
    [SerializeField] GameObject square_1 = default;

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
            hardModeTextDisplay.text = "Normal Mode";
            square_1.SetActive(true);
            square_4.SetActive(false);
        }
        else {
            hardModeTextDisplay.text = "Hard Mode";
            square_1.SetActive(false);
            square_4.SetActive(true);
        }
    }

}

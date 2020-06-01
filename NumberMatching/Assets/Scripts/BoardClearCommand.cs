using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardClearCommand : MonoBehaviour{

    public int clearsTotal = 0;
    [SerializeField] TextMeshProUGUI clearText = default;
    [SerializeField] GameBoardMechanics gameboard=default;

    public Color blinkColor1;


    public void BoardClearOnClick() {

        if (clearsTotal > 0) {
            Debug.Log("Clearing Board");
            clearsTotal--;
            UpdateClearTextDisplay();
            gameboard.ClearBlockers();
        }
        else {
            Debug.Log("No Clears Left");
        }

    }


    public void UpdateClearsTotal(int number) {
        clearsTotal += number;
        UpdateClearTextDisplay();
    }

    public void UpdateClearTextDisplay() {
        clearText.text = clearsTotal.ToString();
    }

}

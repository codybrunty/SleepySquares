using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardClearCommand : MonoBehaviour{

    public int clearsTotal = 0;
    public int clearsUsed = 0;
    public int clearsEveryPts = 100;
    [SerializeField] TextMeshProUGUI clearText;
    [SerializeField] GameBoardMechanics gameboard;


    private void Start() {
        UpdateClearTextDisplay();
    }

    public void BoardClearOnClick() {

        if (clearsTotal > clearsUsed) {
            Debug.Log("Clearing Board");
            clearsUsed++;
            gameboard.ClearBlockers();
            UpdateClearTextDisplay();
        }
        else {
            Debug.Log("No Clears Left");
        }

    }


    public void UpdateClearsTotal(int score) {
        clearsTotal = score / clearsEveryPts;
        UpdateClearTextDisplay();
    }

    private void UpdateClearTextDisplay() {
        int clearsLeft = clearsTotal - clearsUsed;
        clearText.text = clearsLeft.ToString();
    }

}

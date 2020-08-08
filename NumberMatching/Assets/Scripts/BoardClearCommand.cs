using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardClearCommand : MonoBehaviour{

    public int clearsTotal = 0;
    [SerializeField] TextMeshProUGUI clearText = default;
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] ClearButtonFill clearButtonFill = default;

    public Color blinkColor1;


    public void BoardClearOnClick() {

        if (clearsTotal > 0) {
            Debug.Log("Clearing Board");
            clearsTotal--;
            UpdateClearTextDisplay();
            UpdateClearFill();
            gameboard.ClearBlockers();
        }
        else {
            Debug.Log("No Clears Left");
        }

    }


    public void UpdateClearsTotal(int number) {
        clearsTotal += number;
        UpdateClearTextDisplay();
        UpdateClearFill();
    }

    public void UpdateClearTextDisplay() {
        clearText.text = clearsTotal.ToString();
    }

    public void UpdateClearFill() {
        int clearScore = gameboard.GetClearScore();

        //Debug.LogWarning(clearScore + " "+gameboard.score + " "+gameboard.clearsEveryPts);
        //Debug.LogWarning(gameboard.score % gameboard.clearsEveryPts);

        if (clearsTotal == 0) {
            clearButtonFill.UpdateFillDisplay((1f - ((float)(gameboard.score % gameboard.clearsEveryPts) / (float)gameboard.clearsEveryPts)));
        }
        else {
            clearButtonFill.UpdateFillDisplay(0f);
        }


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardClearCommand : MonoBehaviour{

    public int clearsTotal = 0;
    public int clearsUsed = 0;
    public int clearsEveryPts = 100;
    [SerializeField] TextMeshProUGUI clearText = default;
    [SerializeField] GameBoardMechanics gameboard=default;

    public Color blinkColor1;


    private void Start() {
        UpdateClearTextDisplay();

        Hashtable hash = new Hashtable();
        hash.Add("color", blinkColor1);
        hash.Add("time", 2f);
        iTween.ColorTo(gameObject, hash);
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

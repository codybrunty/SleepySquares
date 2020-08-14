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
        PopAnim();
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearReady1");
    }

    private void PopAnim() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(0.5f, 0.5f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(gameObject, hash);
    }

    public void UpdateClearTextDisplay() {
        clearText.text = clearsTotal.ToString();
    }

    public void UpdateClearFill() {
        int clearScore = gameboard.GetClearScore();

        if (clearsTotal == 0) {
            clearButtonFill.UpdateFillDisplay((1f - ((float)(gameboard.score % gameboard.clearsEveryPts) / (float)gameboard.clearsEveryPts)));

        }
        else {
            clearButtonFill.UpdateFillDisplay(0f);
        }


    }

}

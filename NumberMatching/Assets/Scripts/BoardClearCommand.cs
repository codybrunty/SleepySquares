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

    [SerializeField] Image clearButtonImage = default;
    [SerializeField] Image clearButtonIconImage = default;
    [SerializeField] Image clearButtonBGImage = default;

    [SerializeField] GameObject starEffects = default;

    public void BoardClearOnClick() {

        if (clearsTotal > 0) {
            Debug.Log("Clearing Board");
            clearsTotal--;
            UpdateClearDisplay();
            UpdateClearFill();
            gameboard.ClearBlockers();
        }
        else {
            Debug.Log("No Clears Left");
        }

    }


    public void UpdateClearsTotal(int number) {
        clearsTotal += number;
        GameDataManager.GDM.currentClears = clearsTotal;
        UpdateClearDisplay();
        UpdateClearFill();
        PopAnim();
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearReady1");
        GameDataManager.GDM.SaveGameData();
    }

    private void PopAnim() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2f, 2f, 0f));
        hash.Add("time", 1.5f);
        iTween.PunchScale(gameObject.transform.parent.gameObject, hash);
    }

    public void UpdateClearDisplay() {
        clearText.text = clearsTotal.ToString();

        if (clearsTotal > 0){
            EnabledClearButton();
        }
        else{
            DisabledClearButton();
        }
    }

    public void UpdateClearFill() {
        if(GameDataManager.GDM.hardModeOn == 1)
        {
            clearButtonFill.UpdateFillDisplay(1f);
        }
        else
        {
            if (clearsTotal == 0)
            {

                if (gameboard.firstClear == false) {
                    //clearButtonFill.UpdateFillDisplay((1f - ((float)(gameboard.score) / (float)gameboard.firstClearPts)));
                    clearButtonFill.UpdateFillDisplay(1f - ((float)(gameboard.score - gameboard.GetLastClearScore()) / (float)(gameboard.GetClearScore() - gameboard.GetLastClearScore())));
                }
                else {
                    //clearButtonFill.UpdateFillDisplay(1f - ((float)((gameboard.score - gameboard.firstClearPts) % gameboard.GetClearsEveryPoints()) / (float)(gameboard.GetClearsEveryPoints())));
                    //Debug.LogWarning( (float)(gameboard.score - gameboard.GetLastClearScore()) / (float)(gameboard.GetClearScore()) );
                    clearButtonFill.UpdateFillDisplay(1f - ((float)(gameboard.score - gameboard.GetLastClearScore()) / (float)(gameboard.GetClearScore() - gameboard.GetLastClearScore())));

                }
            }
            else
            {
                clearButtonFill.UpdateFillDisplay(0f);
            }
        }
    }


    private void EnabledClearButton(){
        Debug.Log("clear button enabled");
        gameObject.GetComponent<Button>().interactable = true;
        gameObject.GetComponent<Image>().raycastTarget = true;
        clearButtonImage.GetComponent<CollectionColor_Image>().key = "Button1";
        clearButtonImage.GetComponent<CollectionColor_Image>().GetColor();
        clearButtonIconImage.GetComponent<CollectionColor_Image>().key = "Second";
        clearButtonIconImage.GetComponent<CollectionColor_Image>().GetColor();
        clearButtonBGImage.GetComponent<CollectionColor_Image>().key = "Button2";
        clearButtonBGImage.GetComponent<CollectionColor_Image>().GetColor();
        starEffects.SetActive(true);
    }

    private void DisabledClearButton(){
        Debug.Log("clear button disabled");
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Image>().raycastTarget = false;
        clearButtonImage.GetComponent<CollectionColor_Image>().key = "Button3";
        clearButtonImage.GetComponent<CollectionColor_Image>().GetColor();
        clearButtonIconImage.GetComponent<CollectionColor_Image>().key = "Button4";
        clearButtonIconImage.GetComponent<CollectionColor_Image>().GetColor();
        clearButtonBGImage.GetComponent<CollectionColor_Image>().key = "Button5";
        clearButtonBGImage.GetComponent<CollectionColor_Image>().GetColor();
        starEffects.SetActive(false);
    }

}

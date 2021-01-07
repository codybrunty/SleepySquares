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
    private bool available = true;
    [SerializeField] List<Color> colorLevels = new List<Color>();
    [SerializeField] Image fillImage = default;
    [SerializeField] ParticleSystem bubbles = default;

    //script components 
    private Button mainButton;
    private Image mainImage;
    private CollectionColor_Image clearButtonImageCollection;
    private CollectionColor_Image clearButtonIconImageCollection;
    private CollectionColor_Image clearButtonBGImageCollection;

    private void Awake() {
        GetScriptComponents();
    }

    private void GetScriptComponents() {
        mainButton = gameObject.GetComponent<Button>();
        mainImage = gameObject.GetComponent<Image>();

        clearButtonImageCollection = clearButtonImage.GetComponent<CollectionColor_Image>();
        clearButtonIconImageCollection = clearButtonIconImage.GetComponent<CollectionColor_Image>();
        clearButtonBGImageCollection = clearButtonBGImage.GetComponent<CollectionColor_Image>();
    }

    public void BoardClearOnClick() {
        if (available) {
            available = false;
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

        StartCoroutine(DelayAvailable());
    }

    private void SetColors() {
        Debug.Log("Set Colors");
        int colorIndex = GetColor();
        fillImage.color = colorLevels[colorIndex];
        bubbles.startColor = colorLevels[colorIndex];
    }

    private int GetColor() {
        int clears = gameboard.clearCounter;
        int maxIncrementals  = gameboard.clearIncrementMultiplierMax + gameboard.incrementAfterClears;

        int halfwaypoint = (maxIncrementals) / 2;

        if (clears < halfwaypoint) {
            return 0;
        }
        else {
            if (clears >= (maxIncrementals)) {
                return 2;
            }
            else {
                return 1;
            }
        }
    }

    IEnumerator DelayAvailable() {
        yield return new WaitForSeconds(1f);
        available = true;
    }

    public void UpdateClearsTotal(int number) {
        clearsTotal += number;
        GameDataManager.GDM.currentClears = clearsTotal;
        UpdateClearDisplay();
        UpdateClearFill();
        PopAnim();
        SoundManager.SM.PlayOneShotSound("clearReady1");
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
        SetColors();
        if (GameDataManager.GDM.hardModeOn == 1)
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
        //Debug.Log("clear button enabled");
        mainButton.interactable = true;
        mainImage.raycastTarget = true;
        clearButtonImageCollection.key = "Button1";
        clearButtonImageCollection.GetColor();
        clearButtonIconImageCollection.key = "Second";
        clearButtonIconImageCollection.GetColor();
        clearButtonBGImageCollection.key = "Button2";
        clearButtonBGImageCollection.GetColor();
        starEffects.SetActive(true);
    }

    private void DisabledClearButton(){
        //Debug.Log("clear button disabled");
        mainButton.interactable = false;
        mainImage.raycastTarget = false;
        clearButtonImageCollection.key = "Button3";
        clearButtonImageCollection.GetColor();
        clearButtonIconImageCollection.key = "Button4";
        clearButtonIconImageCollection.GetColor();
        clearButtonBGImageCollection.key = "Button5";
        clearButtonBGImageCollection.GetColor();
        starEffects.SetActive(false);
    }

}

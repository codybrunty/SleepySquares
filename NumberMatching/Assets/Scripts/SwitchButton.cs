using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchButton : MonoBehaviour{

    public int switchAmmount = 3;
    [SerializeField] TextMeshProUGUI switchText = default;
    public bool activated = false;

    [SerializeField] GameObject switchBG = default;

    [SerializeField] CollectionColor_Image switchButton = default;
    [SerializeField] CollectionColor_Image switchIcon = default;
    [SerializeField] CollectionColor_Image switchNumberBG = default;
    [SerializeField] CollectionColor_Text switchNumberNum = default;

    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] SquareMechanics_Next nextSquare = default;

    [SerializeField] Button settingsButton = default;
    [SerializeField] Button resetButton = default;
    [SerializeField] Button clearButton = default;

    //[SerializeField] TextMeshProUGUI floatingText = default;
    [SerializeField] SettingsMenu settings = default;

    [SerializeField] TweenMovement nextSquareTween = default;

    [SerializeField] GameObject repair_UI_Dim = default;
    [SerializeField] GameObject score_UI_Dim = default;
    [SerializeField] GameObject best_UI_Dim = default;
    [SerializeField] GameObject best_crown_dim = default;
    
    [SerializeField] GameObject switchEffect = default;
    [SerializeField] ParticleSystem repairEffect = default;
    [SerializeField] Color orgColor = default;
    [SerializeField] Color dimColor = default;
    public bool ready = true;
    private bool addSwitches = true;


    private void Start() {
        switchAmmount = GameDataManager.GDM.currentSwitches;
        UpdateSwitchAmmountDisplay();
    }

    public void UpdateSwitchAmmountDisplay() {
        switchText.text = switchAmmount.ToString();
    }

    public void SwitchButtonOnClick() {
        ready = false;
        if (!activated) {
            TurnOnSwitchMode();
        }
        else {
            TurnOffSwitchMode(true);
        }
        StartCoroutine(SwapReady());
    }

    IEnumerator SwapReady()
    {
        yield return new WaitForSeconds(0.25f);
        ready = true;
    }

    public void TurnOnSwitchMode() {
        if (switchAmmount > 0) {
            PlayPositiveSFX();
            activated = true;
            ActiveColors();
            switchBG.SetActive(true);
            HighlightSwitchableSquares();
            DisableUIButtons();
            Disable_UI_Colors();
            nextSquareTween.StartSwitchNextSquareMovement();
            switchEffect.SetActive(true);
        }
        else {
            settings.ShowStorePanel();
        }

    }

    public void TurnOffEffect()
    {
        switchEffect.SetActive(false);
    }
    private void PlayPositiveSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("switchModeOn");
    }
    private void PlayNegativeSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("switchModeNegative");
    }

    public void TurnOffSwitchMode(bool playSFX) {
        Debug.Log("Turn Off Switch Mode");

        InactiveColors();
        switchBG.SetActive(false);
        DisableHighlightOnSwitchableSquares();
        activated = false;
        EnableUIButtons();
        Enable_UI_Colors();
        nextSquareTween.StopSwitchNextSquareMovement();
        TurnOffEffect();

        if (playSFX) {
            PlayNegativeSFX();
        }
    }

    private void EnableUIButtons() {
        resetButton.interactable = true;
        settingsButton.interactable = true;
        clearButton.interactable = true;
        gameObject.GetComponent<Button>().interactable = true;
        StartCoroutine(EnableRaycastOnButton(resetButton));
        StartCoroutine(EnableRaycastOnButton(settingsButton));
        StartCoroutine(EnableRaycastOnButton(clearButton));
        StartCoroutine(EnableRaycastOnButton(gameObject.GetComponent<Button>()));
    }

    IEnumerator EnableRaycastOnButton(Button button) {
        yield return new WaitForSeconds(.5f);
        button.GetComponent<Image>().raycastTarget = true;
    }
    
    private void DisableUIButtons() {
        resetButton.interactable = false;
        settingsButton.interactable = false;
        clearButton.interactable = false;
        gameObject.GetComponent<Button>().interactable = false;

        gameObject.GetComponent<Image>().raycastTarget = false;
        clearButton.GetComponent<Image>().raycastTarget = false;
        resetButton.GetComponent<Image>().raycastTarget = false;
        settingsButton.GetComponent<Image>().raycastTarget = false;
    }

    public void ReduceSwitchAmmount() {
        switchAmmount--;
        UpdateSwitchAmmountDisplay();

        //for playfab tracking
        int counter = PlayerPrefs.GetInt("Swaps_Used", 0);
        counter++;
        PlayerPrefs.SetInt("Swaps_Used", counter);
    }

    public void AddSwitches(int ammount)
    {
        if (addSwitches)
        {
            addSwitches = false;
            GameDataManager.GDM.currentSwitches += ammount;
            GameDataManager.GDM.SaveGameData();
            switchAmmount = GameDataManager.GDM.currentSwitches;
            AddSwitchAnimation(ammount);
            UpdateSwitchAmmountDisplay();
        }
        //weird bug was happening where add switches was getting triggered twice.
        StartCoroutine(StopDoublePayouts());
    }

    public void AddSwitchesNoAnimation(int ammount)
    {
        if (addSwitches)
        {
            addSwitches = false;
            GameDataManager.GDM.currentSwitches += ammount;
            GameDataManager.GDM.SaveGameData();
            switchAmmount = GameDataManager.GDM.currentSwitches;
            //AddSwitchAnimation(ammount);
            UpdateSwitchAmmountDisplay();
        }
        //weird bug was happening where add switches was getting triggered twice.
        StartCoroutine(StopDoublePayouts());
    }

    IEnumerator StopDoublePayouts()
    {
        yield return new WaitForSeconds(.25f);
        addSwitches = true;
    }

    private void AddSwitchAnimation(int number) {
        //floatingText.text = ("+" + number.ToString());
        //floatingText.gameObject.GetComponent<FloatingText>().FlashText();
        PopAnim();
    }

    private void PopAnim() {

        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2f, 2f, 0f));
        hash.Add("time", 1.5f);
        iTween.PunchScale(gameObject.transform.parent.gameObject, hash);

    }

    private void ActiveColors() {
        switchButton.key = "Second";
        switchIcon.key = "Button1";
        switchNumberBG.key = "Button1";
        switchNumberNum.key = "Second";
        switchButton.GetColor();
        switchIcon.GetColor();
        switchNumberBG.GetColor();
        switchNumberNum.GetColor();
    }

    private void InactiveColors() {
        switchButton.key = "Button1";
        switchIcon.key = "Second";
        switchNumberBG.key = "Second";
        switchNumberNum.key = "First";
        switchButton.GetColor();
        switchIcon.GetColor();
        switchNumberBG.GetColor();
        switchNumberNum.GetColor();
    }



    private void HighlightSwitchableSquares() {
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++) {
            GameObject square = gameBoard.gameBoardSquares[i];
            SquareMechanics_Gameboard squareInfo = square.GetComponent<SquareMechanics_Gameboard>();
            if (squareInfo.blocker != true && squareInfo.completed != true && squareInfo.number != 0 && squareInfo.number != nextSquare.number) {
                HighlightSquare(square);
            }
        }
    }


    private void DisableHighlightOnSwitchableSquares() {
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++) {
            GameObject square = gameBoard.gameBoardSquares[i];
            SquareMechanics_Gameboard squareInfo = square.GetComponent<SquareMechanics_Gameboard>();
            UnHilightSquare(square);
        }
    }

    private void HighlightSquare(GameObject square) {
        square.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 11;
        HighlightGreenFace(square);
        HighlightRedFace(square);
        HighlightPurpleFace(square);
        HighlightOrangeFace(square);
    }



    private void UnHilightSquare(GameObject square) {
        square.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 0;
        UnHighlightGreenFace(square);
        UnHighlightRedFace(square);
        UnHighlightPurpleFace(square);
        UnHighlightOrangeFace(square);
    }

    private void HighlightOrangeFace(GameObject square) {
        //Face Orange LeftEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Orange LeftEye white
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Orange LeftEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Orange RightEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Orange RightEye white
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(2).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Orange RightEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Orange LeftTopEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(4).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Orange LeftTopEye white
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(4).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Orange LeftTopEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(5).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Orange RightTopEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Orange RightTopEye white
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Orange RightTopEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(7).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Orange Mouth mouth
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(8).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
    }

    private void UnHighlightOrangeFace(GameObject square) {
        //Face Orange LeftEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Orange LeftEye white
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Orange LeftEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Orange RightEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Orange RightEye white
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(2).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Orange RightEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Orange LeftTopEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(4).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Orange LeftTopEye white
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(4).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Orange LeftTopEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(5).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Orange RightTopEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Orange RightTopEye white
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Orange RightTopEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(7).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Orange Mouth mouth
        square.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(8).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
    }

    private void HighlightPurpleFace(GameObject square) {
        //Face Purple LeftEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Purple LeftEye white
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Purple LeftEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Purple CenterEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Purple CenterEye white
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Purple CenterEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Purple RightEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(4).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Purple RightEye white
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(4).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Purple RightEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(5).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Purple Mouth mouth
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(6).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
    }

    private void UnHighlightPurpleFace(GameObject square) {
        //Face Purple LeftEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Purple LeftEye white
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Purple LeftEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Purple CenterEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Purple CenterEye white
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Purple CenterEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Purple RightEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(4).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Purple RightEye white
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(4).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Purple RightEyeLid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(5).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Purple Mouth mouth
        square.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(6).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
    }

    private void HighlightRedFace(GameObject square) {
        //Face Red LeftEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Red LeftEye white
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Red LeftEyeLid number_2_eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Red RightEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Red RightEye white
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Red RightEyeLid number_2_eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Red Mouth mouth
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(4).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
    }

    private void UnHighlightRedFace(GameObject square) {
        //Face Red LeftEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Red LeftEye white
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Red LeftEyeLid number_2_eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Red RightEye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Red RightEye white
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Red RightEyeLid number_2_eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Red Mouth mouth
        square.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(4).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
    }

    private void HighlightGreenFace(GameObject square) {
        //Face Green Eye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Green Eye white
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Green Eyelid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Green Mouth eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
    }

    private void UnHighlightGreenFace(GameObject square) {
        //Face Green Eye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Green Eye white
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Green Eyelid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Green Mouth eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
    }


    private void Disable_UI_Colors()
    {
        Disable_Score();
        Disable_Repair();
        Disable_Hard();
    }

    private void Enable_UI_Colors()
    {
        Enable_Score();
        Enable_Repair();
        Enable_Hard();
    }

    private void Enable_Repair() {
        repair_UI_Dim.SetActive(false);
        RepairEffectOriginalColor();
    }

    private void RepairEffectOriginalColor() {
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, .25f), new GradientAlphaKey(1.0f, .75f), new GradientAlphaKey(0.0f, 1.0f) });

        var col = repairEffect.colorOverLifetime;
        col.color = grad;
    }

    private void RepairEffectDimColor() {
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(140f / 255f, 140f / 255f, 140f / 255f), 0.0f), new GradientColorKey(new Color(140f / 255f, 140f / 255f, 140f / 255f), 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, .25f), new GradientAlphaKey(1.0f, .75f), new GradientAlphaKey(0.0f, 1.0f) });
        var col = repairEffect.colorOverLifetime;
        col.color = grad;
    }

    private void Disable_Repair()
    {
        RepairEffectDimColor();
        repair_UI_Dim.SetActive(true);
    }

    private void Enable_Score()
    {
        score_UI_Dim.SetActive(false);
        best_UI_Dim.SetActive(false);
        best_crown_dim.SetActive(false);
    }

    private void Disable_Score()
    {
        score_UI_Dim.SetActive(true);
        best_UI_Dim.SetActive(true);
        best_crown_dim.SetActive(true);
    }

    private void Enable_Hard()
    {
        //.SetActive(false);
    }

    private void Disable_Hard()
    {
        //.SetActive(true);
    }
}

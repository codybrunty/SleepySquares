using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EyePickingMechanics : MonoBehaviour
{
    private bool activated = false;
    [SerializeField] GameObject eyePickingBG = default;
    [SerializeField] GameBoardMechanics gameBoard = default;

    [SerializeField] Button settingsButton = default;
    [SerializeField] Button resetButton = default;
    [SerializeField] Button clearButton = default;
    [SerializeField] Button switchButton = default;
    [SerializeField] GameObject scoreboard = default;
    [SerializeField] RaycastMouse ray = default;

    private List<GameObject> squares_eyeMode = new List<GameObject>();
    private SquareMechanics_Gameboard eyepicker;

    [SerializeField] GameObject switch_UI_Dim = default;
    [SerializeField] GameObject repair_UI_Dim = default;
    [SerializeField] GameObject next_UI_Dim = default;
    [SerializeField] GameObject next_UI_Dim2 = default;
    [SerializeField] GameObject next_UI_Dim3 = default;
    [SerializeField] GameObject score_UI_Dim = default;
    [SerializeField] GameObject best_UI_Dim = default;
    [SerializeField] GameObject best_crown_dim = default;
    
    [SerializeField] ParticleSystem repairEffect = default;
    public Color orgColor;
    public Color dimColor;

    private Image clearButtonImage;
    private Image switchButtonImage;
    private Image resetButtonImage;
    private Image settingsButtonImage;

    private void Awake() {
        clearButtonImage = clearButton.GetComponent<Image>();
        switchButtonImage = switchButton.GetComponent<Image>();
        resetButtonImage = resetButton.GetComponent<Image>();
        settingsButtonImage = settingsButton.GetComponent<Image>();
    }

    public void TurnOn()
    {
        GetEyePicker();
        activated = true;
        eyePickingBG.SetActive(true);
        HighlightEyePickingSquares();
        ArrowHighlightedSquares();
        DisableUIButtons();
        Disable_UI_Colors();
        SoundManager.SM.PlayOneShotSound("EyeModeOn");
    }

    public void TurnOff()
    {
        activated = false;
        eyePickingBG.SetActive(false);
        DisableHighlightEyepickingSquares();
        UnArrowAllSquares();
        EnableUIButtons();
        Enable_UI_Colors();
        TurnOffAllEyePickerAndEyeModeStatuses();
        ray.TurnComplete();
        SoundManager.SM.PlayOneShotSound("EyeModeOff");
    }

    private void GetEyePicker()
    {
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            SquareMechanics_Gameboard square = gameBoard.gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>();
            if (square.eyePicker == true)
            {
                eyepicker = square;
                break;
            }
        }
    }

    private void TurnOffAllEyePickerAndEyeModeStatuses()
    {
        gameBoard.eyePickingMode = false;

        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            SquareMechanics_Gameboard square = gameBoard.gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>();
            square.eyeMode = false;
            square.eyePicker = false;
            square.eyePickerCountdown = 0;
        }
    }

    public void UnArrowSquare(GameObject square)
    {
        for (int i = 0; i < eyepicker.adjescentSquares.Count; i++)
        {
            if (eyepicker.adjescentSquares[i] == square.GetComponent<SquareMechanics_Gameboard>())
            {
                eyepicker.arrows[i].SetActive(false);
            }
        }
    }

    private void UnArrowAllSquares()
    {
        for (int i = 0; i < eyepicker.arrows.Count; i++)
        {
            eyepicker.arrows[i].SetActive(false);
        }
    }

    public void ArrowHighlightedSquares()
    {
        for (int i = 0; i < squares_eyeMode.Count; i++)
        {
            //Debug.Log(squares_eyeMode[i]);
            for (int j = 0; j < eyepicker.adjescentSquares.Count; j++)
            {
                //Debug.Log(eyepicker.adjescentSquares[j]);
                if (eyepicker.adjescentSquares[j] == squares_eyeMode[i].GetComponent<SquareMechanics_Gameboard>())
                {
                    //Debug.Log("bingo");
                    eyepicker.arrows[j].SetActive(true);
                }
            }
        }
    }

    private void HighlightEyePickingSquares()
    {
        squares_eyeMode.Clear();
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            if (gameBoard.gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().eyeMode)
            {
                HighlightSquare(gameBoard.gameBoardSquares[i]);
                squares_eyeMode.Add(gameBoard.gameBoardSquares[i]);
            }
        }
    }

    private void DisableHighlightEyepickingSquares()
    {
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            if (gameBoard.gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().eyeMode)
            {
                UnHilightSquare(gameBoard.gameBoardSquares[i]);
            }
        }
    }

    public void HighlightSquare(GameObject square)
    {
        square.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 11;
        HighlightGreenFace(square);
        HighlightRedFace(square);
        HighlightPurpleFace(square);
        HighlightOrangeFace(square);
    }

    public void UnHilightSquare(GameObject square)
    {
        square.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 0;
        UnHighlightGreenFace(square);
        UnHighlightRedFace(square);
        UnHighlightPurpleFace(square);
        UnHighlightOrangeFace(square);
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

    private void HighlightOrangeFace(GameObject square)
    {
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

    private void UnHighlightOrangeFace(GameObject square)
    {
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

    private void HighlightPurpleFace(GameObject square)
    {
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

    private void UnHighlightPurpleFace(GameObject square)
    {
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

    private void HighlightRedFace(GameObject square)
    {
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

    private void UnHighlightRedFace(GameObject square)
    {
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

    private void HighlightGreenFace(GameObject square)
    {
        //Face Green Eye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 16;
        //Face Green Eye white
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Green Eyelid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
        //Face Green Mouth eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 15;
    }

    private void UnHighlightGreenFace(GameObject square)
    {
        //Face Green Eye Iris
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 6;
        //Face Green Eye white
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Green Eyelid eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        //Face Green Mouth eyelid
        square.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
    }

    private void DisableUIButtons()
    {
        resetButton.interactable = false;
        settingsButton.interactable = false;
        clearButton.interactable = false;
        switchButton.interactable = false;

        clearButtonImage.raycastTarget = false;
        switchButtonImage.raycastTarget = false;
        resetButtonImage.raycastTarget = false;
        settingsButtonImage.raycastTarget = false;
    }

    private void EnableUIButtons()
    {
        resetButton.interactable = true;
        settingsButton.interactable = true;
        clearButton.interactable = true;
        switchButton.interactable = true;
        StartCoroutine(EnableRaycastOnButton(resetButton));
        StartCoroutine(EnableRaycastOnButton(settingsButton));
        StartCoroutine(EnableRaycastOnButton(clearButton));
        StartCoroutine(EnableRaycastOnButton(switchButton));
    }

    IEnumerator EnableRaycastOnButton(Button button)
    {
        yield return new WaitForSeconds(.5f);
        button.GetComponent<Image>().raycastTarget = true;
    }

    
    private void Disable_UI_Colors()
    {
        Disable_Score();
        Disable_Repair();
        Disable_Swap();
        Disable_Next();
    }

    private void Enable_UI_Colors()
    {
        Enable_Score();
        Enable_Repair();
        Enable_Swap();
        Enable_Next();
    }

    private void Enable_Swap()
    {
        switch_UI_Dim.SetActive(false);
    }

    private void Disable_Swap()
    {
        switch_UI_Dim.SetActive(true);
    }

    private void Enable_Repair()
    {
        repair_UI_Dim.SetActive(false);
        RepairEffectOriginalColor();
    }

    private void Disable_Repair()
    {
        repair_UI_Dim.SetActive(true);
        RepairEffectDimColor();
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

    private void Enable_Next()
    {
        next_UI_Dim.SetActive(false);
        next_UI_Dim2.SetActive(false);
        next_UI_Dim3.SetActive(false);
    }

    private void Disable_Next()
    {
        next_UI_Dim.SetActive(true);
        next_UI_Dim2.SetActive(true);
        next_UI_Dim3.SetActive(true);
    }

}

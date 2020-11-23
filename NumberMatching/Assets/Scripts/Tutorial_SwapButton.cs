using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial_SwapButton : MonoBehaviour
{
    public bool activated = false;
    [SerializeField] GameObject switchBG = default;

    [SerializeField] CollectionColor_Image switchButton = default;
    [SerializeField] CollectionColor_Image switchIcon = default;
    [SerializeField] CollectionColor_Image switchNumberBG = default;
    [SerializeField] CollectionColor_Text switchNumberNum = default;

    [SerializeField] TutorialGameboard gameBoard = default;
    [SerializeField] TutorialNextBoard nextBoard = default;
    [SerializeField] TextMeshProUGUI count = default;
    public int countNumber = 16;

    [SerializeField] GameObject instructions = default;
    [SerializeField] GameObject instructions_swapmode = default;
    public bool disabledSwap = false;
    public TweenMovement square_rotationGroup = default;

    [SerializeField] GameObject switchArrow = default;
    [SerializeField] GameObject purpleArrow = default;
    public bool tutorialBeginingMode = true;

    [SerializeField] GameObject scoreboard_dim = default;
    [SerializeField] GameObject scoreboardText_dim = default;
    [SerializeField] GameObject repair_dim = default;

    [SerializeField] GameObject glowEffects = default;

    private void Start()
    {
        count.text = countNumber.ToString();
    }

    public void SwapButtonOnClick(bool playSound)
    {

        if (!activated)
        {
            TurnOnSwitchMode();
        }
        else
        {
            TurnOffSwitchMode(playSound);
        }

    }

    private void DimUI()
    {
        scoreboard_dim.SetActive(true);
        scoreboardText_dim.SetActive(true);
        repair_dim.SetActive(true);
    }

    private void UnDimUI()
    {
        scoreboard_dim.SetActive(false);
        scoreboardText_dim.SetActive(false);
        repair_dim.SetActive(false);
    }

    public void ReduceSwitchAmmount()
    {
        countNumber--;
        count.text = countNumber.ToString();
    }

    public void TurnOnSwitchMode()
    {
        DimUI();
        activated = true;
        switchBG.SetActive(true);
        glowEffects.SetActive(true);

        DisableSwapButton();
        PlayPositiveSFX();


        ActiveColors();
        HighlightSwitchableSquares();

        instructions.SetActive(false);

        if (gameBoard.endingSeq == true)
        {
            gameBoard.instructions_ending1.SetActive(false);
        }

        instructions_swapmode.GetComponent<TypewriterEffect>().index = 0;
        instructions_swapmode.SetActive(true);

        square_rotationGroup.StartSwitchNextSquareMovement();

        if (tutorialBeginingMode)
        {
            switchArrow.SetActive(false);
            purpleArrow.SetActive(true);
        }
        else
        {
            switchArrow.SetActive(false);
            TurnOffPurpleArrow();
        }

        //DisableUIButtons();
        //DisableUIColors();
    }

    public void TurnOffGlow()
    {
        glowEffects.SetActive(false);
    }

    public void TurnOffPurpleArrow()
    {
        purpleArrow.SetActive(false);
    }

    public void TurnOffSwitchMode(bool playSound)
    {
        UnDimUI();
        activated = false;
        switchBG.SetActive(false);
        TurnOffGlow();

        if (disabledSwap == false)
        {
            EnableSwapButton();

            if (playSound) { 
                PlayNegativeSFX();
            }
        }

        if (gameBoard.endingSeq == true)
        {
            gameBoard.instructions_ending1.GetComponent<TypewriterEffect>().index = 0;
            gameBoard.instructions_ending1.SetActive(true);
        }

        InactiveColors();
        DisableHighlightOnSwitchableSquares();

        instructions_swapmode.SetActive(false);

        square_rotationGroup.StopSwitchNextSquareMovement();


        if (tutorialBeginingMode)
        {
            switchArrow.SetActive(true);
            TurnOffPurpleArrow();
        }
        else
        {
            switchArrow.SetActive(false);
            TurnOffPurpleArrow();
        }

        //EnableUIButtons();
        //EnableUIColors();
    }

    public void EnableSwapButton()
    {
        //Debug.LogWarning("Enabled");
        gameObject.GetComponent<Button>().interactable = true;
        gameObject.GetComponent<Image>().raycastTarget = true;
    }

    public void DisableSwapButton()
    {
        //Debug.LogWarning("Disable");
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Image>().raycastTarget = false;
    }

    private void ActiveColors()
    {
        switchButton.key = "Second";
        switchIcon.key = "Button1";
        switchNumberBG.key = "Button1";
        switchNumberNum.key = "Second";
        switchButton.GetColor();
        switchIcon.GetColor();
        switchNumberBG.GetColor();
        switchNumberNum.GetColor();
    }

    private void InactiveColors()
    {
        switchButton.key = "Button1";
        switchIcon.key = "Second";
        switchNumberBG.key = "Second";
        switchNumberNum.key = "First";
        switchButton.GetColor();
        switchIcon.GetColor();
        switchNumberBG.GetColor();
        switchNumberNum.GetColor();
    }

    private void PlayPositiveSFX()
    {
        SoundManager.SM.PlayOneShotSound("switchModeOn");
    }

    private void PlayNegativeSFX()
    {
        SoundManager.SM.PlayOneShotSound("switchModeNegative");
    }

    private void HighlightSwitchableSquares()
    {
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            GameObject square = gameBoard.gameBoardSquares[i];
            Tutorial_Square squareInfo = square.GetComponent<Tutorial_Square>();
            if (squareInfo.blocker != true && squareInfo.completed != true && squareInfo.number != 0 && squareInfo.number != nextBoard.mainNumber + 1)
            {
                HighlightSquare(square);
            }
        }
    }


    private void DisableHighlightOnSwitchableSquares()
    {
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            GameObject square = gameBoard.gameBoardSquares[i];
            Tutorial_Square squareInfo = square.GetComponent<Tutorial_Square>();
            UnHilightSquare(square);
        }
    }

    private void HighlightSquare(GameObject square)
    {
        square.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 11;
        HighlightGreenFace(square);
        HighlightRedFace(square);
        HighlightPurpleFace(square);
        HighlightOrangeFace(square);
    }



    private void UnHilightSquare(GameObject square)
    {
        square.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 0;
        UnHighlightGreenFace(square);
        UnHighlightRedFace(square);
        UnHighlightPurpleFace(square);
        UnHighlightOrangeFace(square);
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
}

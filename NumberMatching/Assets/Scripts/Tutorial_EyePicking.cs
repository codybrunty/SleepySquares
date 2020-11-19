using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_EyePicking : MonoBehaviour
{
    public bool activated = false;
    [SerializeField] GameObject eyePickingBG = default;
    [SerializeField] TutorialGameboard gameBoard = default;
    [SerializeField] Tutorial_RaycastForMouse ray = default;

    [SerializeField] GameObject instructions = default;
    [SerializeField] GameObject instructions_eye = default;

    private List<Tutorial_Square> eyeModeSquares = new List<Tutorial_Square>();

    public Tutorial_Square eyepicker;

    [SerializeField] GameObject nextboard_dim = default;
    [SerializeField] GameObject scoreboard_dim = default;
    [SerializeField] GameObject scoreboardText_dim = default;
    [SerializeField] GameObject repair_dim = default;
    [SerializeField] GameObject swap_dim = default;


    public void TurnOn()
    {
        activated = true;
        eyePickingBG.SetActive(true);
        GetEyePicker();
        HighlightEyePickingSquares();
        TurnOnAllArrows();
        TurnOnEyeBallPickingText();
        DimUI();
        SoundManager.SM.PlayOneShotSound("EyeModeOn");
    }

    public void TurnOff()
    {
        activated = false;
        eyePickingBG.SetActive(false);
        DisableHighlightEyepickingSquares();
        TurnOffEyeBallPickingText();
        TurnOffAllArrows();
        TurnOffAllEyePickerAndEyeModeStatuses();
        ray.TurnComplete();
        UnDimUI();
        SoundManager.SM.PlayOneShotSound("EyeModeOff");
    }

    private void DimUI()
    {
        nextboard_dim.SetActive(true);
        scoreboard_dim.SetActive(true);
        scoreboardText_dim.SetActive(true);
        repair_dim.SetActive(true);
        swap_dim.SetActive(true);
    }

    private void UnDimUI()
    {
        nextboard_dim.SetActive(false);
        scoreboard_dim.SetActive(false);
        scoreboardText_dim.SetActive(false);
        repair_dim.SetActive(false);
        swap_dim.SetActive(false);
    }

    private void TurnOnAllArrows()
    {
        for (int i = 0; i < eyeModeSquares.Count; i++)
        {
            for (int j = 0; j < eyepicker.adjescentSquares.Count; j++)
            {
                if (eyeModeSquares[i] == eyepicker.adjescentSquares[j])
                {
                    eyepicker.arrows[j].SetActive(true);
                }
            }
        }
    }

    private void TurnOffAllArrows()
    {
        for (int i = 0; i < eyepicker.arrows.Count; i++)
        {
            eyepicker.arrows[i].SetActive(false);
        }
    }

    private void GetEyePicker()
    {
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            if(gameBoard.gameBoardSquares[i].GetComponent<Tutorial_Square>().eyePicker == true){
                eyepicker = gameBoard.gameBoardSquares[i].GetComponent<Tutorial_Square>();
                break;
            }
        }
    }

    private void TurnOnEyeBallPickingText()
    {
        instructions.GetComponent<TypewriterEffect>().index = 0;
        instructions.SetActive(false);

        if (gameBoard.endingSeq == true)
        {
            gameBoard.instructions_ending1.SetActive(false);
        }

        instructions_eye.SetActive(true);

    }

    private void TurnOffEyeBallPickingText()
    {
        instructions_eye.GetComponent<TypewriterEffect>().index = 0;
        instructions_eye.SetActive(false);

        if (gameBoard.endingSeq == true)
        {
            gameBoard.instructions_ending1.GetComponent<TypewriterEffect>().index = 0;
            gameBoard.instructions_ending1.SetActive(true);
        }
        else
        {
            instructions.SetActive(true);
        }
    }

    private void TurnOffAllEyePickerAndEyeModeStatuses()
    {
        gameBoard.eyePickingMode = false;

        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            gameBoard.gameBoardSquares[i].GetComponent<Tutorial_Square>().eyeMode = false;
            gameBoard.gameBoardSquares[i].GetComponent<Tutorial_Square>().eyePicker = false;
            gameBoard.gameBoardSquares[i].GetComponent<Tutorial_Square>().eyePickerCountdown = 0;
        }
    }

    private void HighlightEyePickingSquares()
    {
        eyeModeSquares.Clear();
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            if (gameBoard.gameBoardSquares[i].GetComponent<Tutorial_Square>().eyeMode)
            {
                HighlightSquare(gameBoard.gameBoardSquares[i]);
                eyeModeSquares.Add(gameBoard.gameBoardSquares[i].GetComponent<Tutorial_Square>());
            }
        }
    }

    private void DisableHighlightEyepickingSquares()
    {
        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++)
        {
            if (gameBoard.gameBoardSquares[i].GetComponent<Tutorial_Square>().eyeMode)
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

        UnArrowSquare(square);
    }

    private void UnArrowSquare(GameObject square)
    {

        for (int i = 0; i < eyepicker.adjescentSquares.Count; i++)
        {
            if (eyepicker.adjescentSquares[i]==square.GetComponent<Tutorial_Square>())
            {
                eyepicker.arrows[i].SetActive(false);
            }
        }

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

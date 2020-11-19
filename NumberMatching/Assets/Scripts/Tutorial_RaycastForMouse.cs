using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_RaycastForMouse : MonoBehaviour
{
    [SerializeField] TutorialGameboard gameboard = default;
    [SerializeField] TutorialNextBoard nextBoard = default;
    public bool resetMode = false;
    [SerializeField] Tutorial_SwapButton switchButton = default;
    public float swapDuration = 1f;
    private bool squareHit = false;
    [SerializeField] Tutorial_Next_Square next = default;

    private void Update()
    {
        RayCastForTuteSquare();
    }

    private void RayCastForTuteSquare()
    {
        if (gameboard.touchEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                int layerMask_square = LayerMask.NameToLayer(layerName: "Square_Gameboard");
                RaycastHit2D hit_onSquare = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_square);
                if (hit_onSquare.collider != null)
                {
                    TuteSquareHit(hit_onSquare.collider.gameObject);
                    squareHit = true;
                }

                int layerMask_switchBG = LayerMask.NameToLayer(layerName: "SwitchBG");
                RaycastHit2D hit_onSwitchBG = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_switchBG);
                if (hit_onSwitchBG.collider != null)
                {
                    if (switchButton.activated && squareHit == false)
                    {
                        switchButton.SwapButtonOnClick(true);
                    }

                }
                squareHit = false;
            }
        }
    }

    private void TuteSquareHit(GameObject square)
    {
        if (!gameboard.eyePickingMode)
        {
            if (square.GetComponent<Tutorial_Square>().number == 0)
            {
                if (!switchButton.activated)
                {
                    EmptyTuteSquareClicked(square);
                }
                else
                {
                    switchButton.SwapButtonOnClick(true);
                }
            }

            //blocker square
            else if (square.GetComponent<Tutorial_Square>().number == 5 && square.GetComponent<Tutorial_Square>().blocker == true)
            {
                //check if in switch mode
                if (!switchButton.activated)
                {
                    //BlockerSquareClicked();
                }
                else
                {
                    switchButton.SwapButtonOnClick(true);
                }
            }

            //filled square
            else
            {
                if (switchButton.activated)
                {
                    OccupiedSquareClicked(square);
                }
            }
        }
        else
        {
            if (square.GetComponent<Tutorial_Square>().eyeMode)
            {
                square.GetComponent<Tutorial_Square>().EyeModeUpdate();
            }
        }
    }

    private void OccupiedSquareClicked(GameObject square)
    {
        if (square.GetComponent<Tutorial_Square>().completed == false)
        {

            int number = nextBoard.mainNumber+1;
            int clickedNumber = square.GetComponent<Tutorial_Square>().number;

            //blockers squares cant switch in from next board
            if (number != 0 && number != 5)
            {
                //blockers squares cant switch out from gameboard
                if (clickedNumber != 5 && number != clickedNumber)
                {
                    //disable touch so user cant click out of swap while its animating
                    gameboard.touchEnabled = false;
                    StartCoroutine(SwapAnimation(clickedNumber, number, square));
                }
            }
        }
        else
        {
            switchButton.SwapButtonOnClick(true);
        }

    }

    IEnumerator SwapAnimation(int clickedNumber, int number, GameObject square)
    {
        square.GetComponent<Tutorial_Square>().MoveAlongRoute(swapDuration);
        square.GetComponent<Tutorial_Square>().HideConnections();
        square.GetComponent<Tutorial_Square>().WakeUpNeighbors();
        next.MoveNextSquareAlongRoute(swapDuration, square);
        switchButton.TurnOffGlow();

        yield return new WaitForSeconds(swapDuration);
        SwapSquares(clickedNumber, number, square);
        gameboard.touchEnabled = true;
    }

    private void SwapSquares(int clickedNumber, int number, GameObject square )
    {
        nextBoard.mainNumber = clickedNumber - 1;
        nextBoard.PopFirstSquare();
        square.GetComponent<Tutorial_Square>().ResetSquare_OnClick();
        square.GetComponent<Tutorial_Square>().number = number;
        square.GetComponent<Tutorial_Square>().SetSquareDisplay();
        square.GetComponent<Tutorial_Square>().CalculateConnections();
        square.GetComponent<Tutorial_Square>().RecalculateAdjescentSquares();
        switchButton.ReduceSwitchAmmount();
        nextBoard.ColorDisplay();
        switchButton.SwapButtonOnClick(false);
    }

    private void EmptyTuteSquareClicked(GameObject square)
    {
        int number = nextBoard.mainNumber+1;

        if (number != 0)
        {
            nextBoard.RotateNextBoard();
            square.GetComponent<Tutorial_Square>().number = number;
            square.GetComponent<Tutorial_Square>().SetSquareDisplay();
            square.GetComponent<Tutorial_Square>().CalculateConnections();
        }

        if (!gameboard.eyePickingMode)
        {
            TurnComplete();
        }
    }

    public void TurnComplete()
    {
        gameboard.CheckIfBoardFull();
    }
}

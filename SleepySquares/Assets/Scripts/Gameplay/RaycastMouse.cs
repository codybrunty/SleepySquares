using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMouse : MonoBehaviour {

    [SerializeField] NextBoardMechanics nextBoard = default;
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] SwitchButton switchButton = default;
    public bool resetMode = false;
    
    public bool gameStarted = false;
    public float swapDuration = 1f;
    //public bool switchModeIsActive = false;
    private bool squareHit = false;
    [SerializeField] SquareMechanics_Next next = default;
    private GameObject pressSquare = null;

    private void Update() {
        RayCastForSquare();
    }

    private void RayCastForSquare() {
        if (gameboard.touchEnabled) {
            //on release
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                int layerMask_square = LayerMask.NameToLayer(layerName: "Square_Gameboard");
                RaycastHit2D hit_onSquare = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_square);
                if (hit_onSquare.collider != null)
                {
                    SquareMechanics_Gameboard squareMechanics = hit_onSquare.collider.gameObject.GetComponent<SquareMechanics_Gameboard>();
                    //press down release
                    squareMechanics.PressRelease();
                    pressSquare = null;
                    GameSquareHit(squareMechanics);
                    squareHit = true;
                }


                int layerMask_switchBG = LayerMask.NameToLayer(layerName: "SwitchBG");
                RaycastHit2D hit_onSwitchBG = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_switchBG);
                if (hit_onSwitchBG.collider != null)
                {
                    if (switchButton.activated && squareHit == false && switchButton.ready)
                    {
                        switchButton.TurnOffSwitchMode(true);
                    }

                }
                squareHit = false;

                //incase we relase touch off gameboard
                if (pressSquare != null) {
                    pressSquare.GetComponent<SquareMechanics_Gameboard>().PressRelease();
                    pressSquare = null;
                }
            }

            //while hovering over
            if (Input.GetMouseButton(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMask_square = LayerMask.NameToLayer(layerName: "Square_Gameboard");
                RaycastHit2D hit_onSquare = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_square);
                if (hit_onSquare.collider != null) {
                    if (pressSquare != hit_onSquare.collider.gameObject) {
                        hit_onSquare.collider.gameObject.GetComponent<SquareMechanics_Gameboard>().PressDown();
                        if (pressSquare != null) {
                            pressSquare.GetComponent<SquareMechanics_Gameboard>().PressRelease();
                        }
                        pressSquare = hit_onSquare.collider.gameObject;
                    }

                }


            }
        }
    }

    public void GameSquareHit(SquareMechanics_Gameboard squareMechanics){

        if (!gameboard.eyePickingMode)
        {
            //empty square non blocker
            if (squareMechanics.number == 0)
            {

                //check if in switch mode
                if (!switchButton.activated)
                {
                    CheckIfEmptySquareLucky(squareMechanics);
                    EmptySquareClicked(squareMechanics);
                }
                else
                {
                    switchButton.TurnOffSwitchMode(true);
                }


            }

            //blocker square
            else if (squareMechanics.number == 5 && squareMechanics.blocker == true)
            {
                //check if in switch mode
                if (!switchButton.activated)
                {
                    BlockerSquareClicked();
                }
                else
                {
                    switchButton.TurnOffSwitchMode(true);
                }
            }

            //filled square
            else
            {
                if (switchButton.activated)
                {
                    OccupiedSquareClicked(squareMechanics);
                }
            }
        }
        // in eye pick mode
        else
        {
            if (squareMechanics.eyeMode)
            {
                //Debug.LogWarning("good pick");
                squareMechanics.EyeModeUpdate();
            }
        }
    }

    private void CheckIfEmptySquareLucky(SquareMechanics_Gameboard squareMechanics) {
        if (squareMechanics.luckyCoin == true) {
            squareMechanics.luckyCoin = false;
            
            squareMechanics.LuckyCoinFound(next.number);
            Debug.Log("found lucky coin");
            gameboard.AddLuckyCoinToBoard();
        }
    }

    private void BlockerSquareClicked() {
        Debug.Log("Blocker Square Clicked");
    }

    private void OccupiedSquareClicked(SquareMechanics_Gameboard squareMechanics) {
        

        if (squareMechanics.completed == false) {

            int number = nextBoard.GetFirstNumber();
            int clickedNumber = squareMechanics.number;
            

            //blockers squares cant switch in from next board
            if (number != 0 && number != 5) {
                //blockers squares cant switch out from gameboard
                if (clickedNumber != 5 && number != clickedNumber) {

                    //disable touch so user cant click out of swap while its animating
                    gameboard.touchEnabled = false;
                    switchButton.TurnOffEffect();
                    StartCoroutine(SwapAnimation(clickedNumber, number, squareMechanics));
                }
            }

            
        }
        else
        {
            bool playSFX = true;
            switchButton.TurnOffSwitchMode(playSFX);
        }


    }

    IEnumerator SwapAnimation(int clickedNumber, int number, SquareMechanics_Gameboard squareMechanics)
    {
        
        squareMechanics.MoveAlongRoute(swapDuration);
        squareMechanics.HideConnections();
        squareMechanics.WakeUpNeighbors();
        next.MoveNextSquareAlongRoute(swapDuration, squareMechanics);

        SoundManager.SM.PlayOneShotSound("swoosh");
        yield return new WaitForSeconds(swapDuration);
        SwapSquares(clickedNumber, number, squareMechanics);
        gameboard.touchEnabled = true;
    }

    private void SwapSquares(int clickedNumber, int number, SquareMechanics_Gameboard squareMechanics)
    {
        nextBoard.SetFirstNumber(clickedNumber);
        squareMechanics.ResetSquare_OnClick();
        squareMechanics.number = number;
        squareMechanics.SetSquareDisplay();
        squareMechanics.CalculateConnections();
        squareMechanics.RecalculateAdjescentSquares();
        switchButton.ReduceSwitchAmmount();
        gameboard.SaveBoardState();
        bool playSFX = false;
        switchButton.TurnOffSwitchMode(playSFX);
    }

    private void EmptySquareClicked(SquareMechanics_Gameboard squareMechanics)
    {
        if (!gameStarted)
        {
            StartGame();
        }


        int number = nextBoard.GetFirstNumber();
        if (number != 0)
        {
            nextBoard.RotateNextBoard();
            squareMechanics.number = number;
            squareMechanics.SetSquareDisplay();
            squareMechanics.CalculateConnections();
        }

        if(!gameboard.eyePickingMode){
            TurnComplete();
        }
    }

    public void TurnComplete()
    {
        gameboard.AddToMoveCounter();
        gameboard.CheckIfBoardFull();
        gameboard.SaveBoardState();
    }

    private void StartGame() {
        gameboard.StartGame();
        gameStarted = true;
    }
    
}

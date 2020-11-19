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
                    //press down release
                    hit_onSquare.collider.gameObject.GetComponent<SquareMechanics_Gameboard>().PressRelease();
                    pressSquare = null;

                    GameSquareHit(hit_onSquare.collider.gameObject);
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

    public void GameSquareHit(GameObject square){

        if (!gameboard.eyePickingMode)
        {
            //empty square non blocker
            if (square.GetComponent<SquareMechanics_Gameboard>().number == 0)
            {

                //check if in switch mode
                if (!switchButton.activated)
                {
                    CheckIfEmptySquareLucky(square);
                    EmptySquareClicked(square);
                }
                else
                {
                    switchButton.TurnOffSwitchMode(true);
                }


            }

            //blocker square
            else if (square.GetComponent<SquareMechanics_Gameboard>().number == 5 && square.GetComponent<SquareMechanics_Gameboard>().blocker == true)
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
                    OccupiedSquareClicked(square);
                }
            }
        }
        // in eye pick mode
        else
        {
            if (square.GetComponent<SquareMechanics_Gameboard>().eyeMode)
            {
                //Debug.LogWarning("good pick");
                square.GetComponent<SquareMechanics_Gameboard>().EyeModeUpdate();
            }
        }
    }

    private void CheckIfEmptySquareLucky(GameObject square) {
        if (square.GetComponent<SquareMechanics_Gameboard>().luckyCoin == true) {
            square.GetComponent<SquareMechanics_Gameboard>().luckyCoin = false;


            square.GetComponent<SquareMechanics_Gameboard>().LuckyCoinFound(next.number);
            Debug.Log("found lucky coin");
            gameboard.AddLuckyCoinToBoard();
        }
    }

    private void BlockerSquareClicked() {
        Debug.Log("Blocker Square Clicked");
    }

    private void OccupiedSquareClicked(GameObject square) {
        

        if (square.GetComponent<SquareMechanics_Gameboard>().completed == false) {

            int number = nextBoard.GetFirstNumber();
            int clickedNumber = square.GetComponent<SquareMechanics_Gameboard>().number;
            

            //blockers squares cant switch in from next board
            if (number != 0 && number != 5) {
                //blockers squares cant switch out from gameboard
                if (clickedNumber != 5 && number != clickedNumber) {

                    //disable touch so user cant click out of swap while its animating
                    gameboard.touchEnabled = false;
                    switchButton.TurnOffEffect();
                    StartCoroutine(SwapAnimation(clickedNumber, number, square));
                }
            }

            
        }
        else
        {
            bool playSFX = true;
            switchButton.TurnOffSwitchMode(playSFX);
        }


    }

    IEnumerator SwapAnimation(int clickedNumber, int number, GameObject square)
    {


        square.GetComponent<SquareMechanics_Gameboard>().MoveAlongRoute(swapDuration);
        square.GetComponent<SquareMechanics_Gameboard>().HideConnections();
        square.GetComponent<SquareMechanics_Gameboard>().WakeUpNeighbors();
        next.MoveNextSquareAlongRoute(swapDuration,square);

        SoundManager.SM.PlayOneShotSound("swoosh");
        yield return new WaitForSeconds(swapDuration);
        SwapSquares(clickedNumber, number, square);
        gameboard.touchEnabled = true;
    }

    private void SwapSquares(int clickedNumber, int number, GameObject square)
    {
        nextBoard.SetFirstNumber(clickedNumber);
        square.GetComponent<SquareMechanics_Gameboard>().ResetSquare_OnClick();
        square.GetComponent<SquareMechanics_Gameboard>().number = number;
        square.GetComponent<SquareMechanics_Gameboard>().SetSquareDisplay();
        square.GetComponent<SquareMechanics_Gameboard>().CalculateConnections();
        square.GetComponent<SquareMechanics_Gameboard>().RecalculateAdjescentSquares();
        switchButton.ReduceSwitchAmmount();
        gameboard.SaveBoardState();
        bool playSFX = false;
        switchButton.TurnOffSwitchMode(playSFX);
    }

    private void EmptySquareClicked(GameObject square)
    {
        if (!gameStarted)
        {
            StartGame();
        }


        int number = nextBoard.GetFirstNumber();
        if (number != 0)
        {
            nextBoard.RotateNextBoard();
            square.GetComponent<SquareMechanics_Gameboard>().number = number;
            square.GetComponent<SquareMechanics_Gameboard>().SetSquareDisplay();
            square.GetComponent<SquareMechanics_Gameboard>().CalculateConnections();
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

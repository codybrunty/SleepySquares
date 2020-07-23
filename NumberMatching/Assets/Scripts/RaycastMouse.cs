using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMouse : MonoBehaviour {

    [SerializeField] NextBoardMechanics nextBoard = default;
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] SwitchButton switchButton = default;
    
    private bool gameStarted = false;
    //public bool switchModeIsActive = false;

    private void Update() {
        RayCastForSquare();
    }

    private void RayCastForSquare() {
        if (gameboard.touchEnabled) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);



                int layerMask_square = LayerMask.NameToLayer(layerName: "Square_Gameboard");
                RaycastHit2D hit_onSquare = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_square);
                if (hit_onSquare.collider != null) {
                    GameSquareHit(hit_onSquare.collider.gameObject);
                }



                int layerMask_square_next = LayerMask.NameToLayer(layerName: "Square_Next");
                RaycastHit2D hit_onSquare_next = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_square_next);
                if (hit_onSquare_next.collider != null) {
                    NextSquareHit(hit_onSquare_next.collider.gameObject);
                }


                
                int layerMask_switchBG = LayerMask.NameToLayer(layerName: "SwitchBG");
                RaycastHit2D hit_onSwitchBG = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_switchBG);
                if (hit_onSwitchBG.collider != null) {
                    if (switchButton.activated) {
                        switchButton.TurnOffSwitchMode(true);
                    }
                }



            }
        }
    }

    private void NextSquareHit(GameObject square) {
        if (square.GetComponent<SquareMechanics_Next>().number > 0) {
            NextSquareClicked();
        }
        else {
            EmptyNextSquareClicked();
        }

    }

    private void NextSquareClicked() {
        Debug.Log("next square clicked");
    }

    private void EmptyNextSquareClicked() {
        Debug.Log("empty next square clicked");
    }

    private void GameSquareHit(GameObject square){

        //empty square non blocker
        if (square.GetComponent<SquareMechanics_Gameboard>().number == 0) {

            //check if in switch mode
            if (!switchButton.activated) {
                CheckIfEmptySquareLucky(square);
                EmptySquareClicked(square);
                gameboard.SaveBoardState();//save board state on empty click
            }
            else {
                switchButton.TurnOffSwitchMode(true);
            }


        }

        //blocker square
        else if(square.GetComponent<SquareMechanics_Gameboard>().number == 5 && square.GetComponent<SquareMechanics_Gameboard>().blocker == true) {
            //check if in switch mode
            if (!switchButton.activated) {
                BlockerSquareClicked();
            }
            else {
                switchButton.TurnOffSwitchMode(true);
            }
        }

        //filled square
        else {
            if (switchButton.activated) {
                OccupiedSquareClicked(square);
            }
        }

    }

    private void CheckIfEmptySquareLucky(GameObject square) {
        if (square.GetComponent<SquareMechanics_Gameboard>().luckyCoin == true) {
            square.GetComponent<SquareMechanics_Gameboard>().luckyCoin = false;
            FindObjectOfType<SoundManager>().PlayOneShotSound("yahoo");
            Debug.Log("found lucky coin");
            StartCoroutine(gameboard.SetLuckyCoin());
        }
    }

    private void BlockerSquareClicked() {
        Debug.Log("Blocker Square Clicked");
    }

    private void OccupiedSquareClicked(GameObject square) {
        bool playSFX = true;

        if (square.GetComponent<SquareMechanics_Gameboard>().completed == false) {

            int number = nextBoard.GetFirstNumber();
            int clickedNumber = square.GetComponent<SquareMechanics_Gameboard>().number;
            

            //blockers squares cant switch in from next board
            if (number != 0 && number != 5) {
                //blockers squares cant switch out from gameboard
                if (clickedNumber != 5 && number != clickedNumber) {
                    nextBoard.SetFirstNumber(clickedNumber);
                    square.GetComponent<SquareMechanics_Gameboard>().ResetSquare_OnClick();
                    square.GetComponent<SquareMechanics_Gameboard>().number = number;
                    square.GetComponent<SquareMechanics_Gameboard>().SetSquareDisplay();
                    square.GetComponent<SquareMechanics_Gameboard>().CalculateConnections();
                    square.GetComponent<SquareMechanics_Gameboard>().RecalculateAdjescentSquares();

                    //gameboard.AddToMoveCounter();
                    //gameboard.CheckIfBoardFull();
                    playSFX = false;
                    switchButton.ReduceSwitchAmmount();
                    gameboard.SaveBoardState();//save board state on switch click
                }
            }

            
        }


        switchButton.TurnOffSwitchMode(playSFX);
    }

    private void EmptySquareClicked(GameObject square) {
        if (!gameStarted) {
            StartGame();
        }
        

        int number = nextBoard.GetFirstNumber();
        if (number != 0) {
            nextBoard.RotateNextBoard();
            square.GetComponent<SquareMechanics_Gameboard>().number = number;
            square.GetComponent<SquareMechanics_Gameboard>().SetSquareDisplay();
            square.GetComponent<SquareMechanics_Gameboard>().CalculateConnections();
        }


        gameboard.AddToMoveCounter();
        gameboard.CheckIfBoardFull();
    }

    private void StartGame() {
        gameboard.StartGame();
        gameStarted = true;
    }
    
}

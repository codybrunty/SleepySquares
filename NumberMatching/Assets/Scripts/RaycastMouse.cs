using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMouse : MonoBehaviour {

    [SerializeField] NextBoardMechanics nextBoard = default;
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] SwitchButton switchButton = default;

    public bool switchSquares = false;
    private bool gameStarted = false;

    public bool blockerMode = false;

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

                    gameboard.SaveBoardState();
                }

                int layerMask_square_next = LayerMask.NameToLayer(layerName: "Square_Next");
                RaycastHit2D hit_onSquare_next = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_square_next);
                if (hit_onSquare_next.collider != null) {

                    NextSquareHit(hit_onSquare_next.collider.gameObject);

                    gameboard.SaveBoardState();
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
            CheckIfEmptySquareLucky(square);
            EmptySquareClicked(square);
        }
        //blocker square
        else if(square.GetComponent<SquareMechanics_Gameboard>().number == 5 && square.GetComponent<SquareMechanics_Gameboard>().blocker == true) {
            BlockerSquareClicked();
        }

        //filled square
        else {
            if (switchSquares) {
                OccupiedSquareClicked(square);
                switchButton.ReduceSwitchAmmount();
                switchButton.TurnOffSwitchMode();
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

        if (blockerMode) {
            blockerMode = false;
        }
        else {
            blockerMode = true;
        }
    }

    private void OccupiedSquareClicked(GameObject square) {

        if (square.GetComponent<SquareMechanics_Gameboard>().completed == false) {

            int number = nextBoard.GetFirstNumber();
            int clickedNumber = square.GetComponent<SquareMechanics_Gameboard>().number;

            //blockers squares cant switch in from next board
            if (number != 0 && number != 5) {
                //blockers squares cant switch out from gameboard
                if (clickedNumber != 5) {
                    nextBoard.SetFirstNumber(clickedNumber);
                    square.GetComponent<SquareMechanics_Gameboard>().ResetSquare_OnClick();
                    square.GetComponent<SquareMechanics_Gameboard>().number = number;
                    square.GetComponent<SquareMechanics_Gameboard>().SetSquareDisplay();
                    square.GetComponent<SquareMechanics_Gameboard>().CalculateConnections();
                    square.GetComponent<SquareMechanics_Gameboard>().RecalculateAdjescentSquares();

                    gameboard.AddToMoveCounter();
                    gameboard.CheckIfBoardFull();
                }
            }

            
        }
        

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

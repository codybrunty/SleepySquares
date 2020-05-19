using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMouse : MonoBehaviour {

    [SerializeField] NextBoardMechanics nextBoard = default;
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] TimerCountdown timer = default;

    private bool gameStarted = false;


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
                    //Debug.Log(hit_onSquare.collider.name);
                    GameSquareHit(hit_onSquare.collider.gameObject);
                }

                int layerMask_square_next = LayerMask.NameToLayer(layerName: "Square_Next");
                RaycastHit2D hit_onSquare_next = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_square_next);
                if (hit_onSquare_next.collider != null) {
                    //Debug.Log(hit_onSquare.collider.name);
                    NextSquareHit(hit_onSquare_next.collider.gameObject);
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
            EmptySquareClicked(square);
        }
        //blocker square
        else if(square.GetComponent<SquareMechanics_Gameboard>().number == 5 && square.GetComponent<SquareMechanics_Gameboard>().blocker == true) {
            BlockerSquareClicked();
        }
        //filled square
        else {
            OccupiedSquareClicked(square);
        }

    }

    private void BlockerSquareClicked() {
        Debug.Log("Blocker Square Clicked");
        gameboard.GameOver();
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
                }
            }
        }
        

    }

    private void EmptySquareClicked(GameObject square) {
        if (!gameStarted) {
            CheckIfGameStart();
        }

        int number = nextBoard.GetFirstNumber();
        if (number != 0) {
            nextBoard.RotateNextBoard();
            square.GetComponent<SquareMechanics_Gameboard>().number = number;
            square.GetComponent<SquareMechanics_Gameboard>().SetSquareDisplay();
            square.GetComponent<SquareMechanics_Gameboard>().CalculateConnections();
        }
    }

    private void CheckIfGameStart() {
        if (!timer.started) {
            timer.StartTimerCountdown();
            gameStarted = true;
        }
    }
}

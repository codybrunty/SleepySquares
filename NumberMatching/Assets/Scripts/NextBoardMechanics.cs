using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBoardMechanics : MonoBehaviour{


    [Header("NextBoard Attributes")]
    public int ActiveBalls = 0;
    public int numberOfNextSquares = 0;

    [Header("Game Objects")]
    public List<GameObject> nextSquares = new List<GameObject>();
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] GameObject square_next = default;


    private void Start() {
        CreateNextBoard();
        FillNextBoard();
    }

    private void FillNextBoard() {
        for (int i = 0; i < nextSquares.Count; i++) {
            nextSquares[i].GetComponent<SquareMechanics_Next>().SetRandomNumberAndDisplay();
        }
    }

    private void CreateNextBoard() {
        for (int i = 0; i < numberOfNextSquares; i++) {
            Generate_NextSquare(i, gameboard.gameBoardHeight + 0.25f);
        }
    }

    private void Generate_NextSquare(float x, float y) {
        Vector3 squareSpawnPoint = new Vector3(x, y, 0);
        GameObject newSquare = Instantiate(square_next, squareSpawnPoint, Quaternion.identity, gameObject.transform);
        newSquare.name = "NextBall_" + x;
        newSquare.GetComponent<SquareMechanics_Next>().SetNextBoard();
        nextSquares.Add(newSquare);
    }

    public void RotateNextBoard() {
        ClearFirstNextSquare();
        bool allZeros = CheckIfAllZeros();

        if (allZeros) {
            Debug.Log("Empty Next Board. New Number added.");
            nextSquares[0].GetComponent<SquareMechanics_Next>().SetRandomNumberAndDisplay();
        }
        else {
            RotateNextSquaresDown();
        }
    }

    private void RotateNextSquaresDown() {
        for (int i = 0; i < nextSquares.Count; i++) {
            //all but last
            if (i != nextSquares.Count - 1) {
                nextSquares[i].GetComponent<SquareMechanics_Next>().number = nextSquares[i + 1].GetComponent<SquareMechanics_Next>().number;
                nextSquares[i].GetComponent<SquareMechanics_Next>().SetNumberDisplay();
            }
            else {
                //last square
                nextSquares[i].GetComponent<SquareMechanics_Next>().SetRandomNumberAndDisplay();
                //nextSquares[i].GetComponent<SquareMechanics_Next>().number = 0;
                //nextSquares[i].GetComponent<SquareMechanics_Next>().SetNumberDisplay();
            }
        }
    }

    private bool CheckIfAllZeros() {
        bool allZeros = true;

        for(int i = 0; i < nextSquares.Count; i++) {
            if (nextSquares[i].GetComponent<SquareMechanics_Next>().number != 0) {
                allZeros = false;
            }
        }

        return allZeros;
    }

    public void AddOneNextSquareNumber() {
        bool squareAdded = false;
        for (int i = 0; i < nextSquares.Count; i++) {
            if (nextSquares[i].GetComponent<SquareMechanics_Next>().number == 0) {
                if (!squareAdded) {
                    nextSquares[i].GetComponent<SquareMechanics_Next>().SetRandomNumberAndDisplay();
                    squareAdded = true;
                }
            }
        }

        if (!squareAdded) {
            Debug.Log("No Square Added Next Board is Full");
            NextBoardFull();
        }
    }

    private void NextBoardFull() {
        gameboard.GameOver();
    }

    private void ClearFirstNextSquare() {
        nextSquares[0].GetComponent<SquareMechanics_Next>().number = 0;
        nextSquares[0].GetComponent<SquareMechanics_Next>().SetNumberDisplay();
    }

    public int GetFirstNumber() {
        int number = nextSquares[0].GetComponent<SquareMechanics_Next>().number;
        return number;
    }

    public void SetFirstNumber(int newNumber) {
        nextSquares[0].GetComponent<SquareMechanics_Next>().number = newNumber;
        nextSquares[0].GetComponent<SquareMechanics_Next>().SetNumberDisplay();
    }
}

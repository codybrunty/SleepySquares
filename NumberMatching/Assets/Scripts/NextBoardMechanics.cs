using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBoardMechanics : MonoBehaviour{
    
    public List<SquareMechanics_Next> nextSquares = new List<SquareMechanics_Next>();
    [SerializeField] GameBoardMechanics gameboard = default;

    private void Start() {
        ResetNextBoard();
    }

    public void ResetNextBoard() {
        Set_NextSquare();
        FillNextBoard();
    }

    private void FillNextBoard() {
        for (int i = 0; i < nextSquares.Count; i++) {
            nextSquares[i].GetComponent<SquareMechanics_Next>().SetRandomNumberAndDisplay();
        }
    }

    private void Set_NextSquare() {
        for (int i = 0; i<nextSquares.Count;i++) {
            nextSquares[i].SetNextBoard();
        }
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
                nextSquares[i].GetComponent<SquareMechanics_Next>().SetRandomNumberAndDisplay();
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

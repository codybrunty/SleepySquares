using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBoardMechanics : MonoBehaviour{
    
    public List<SquareMechanics_Next> nextSquares = new List<SquareMechanics_Next>();

    public void SetNextBoard() {
        SetNextBoardOnNextSquare();
        FillNextBoard();
    }

    public void ResetNextBoard() {
        SetNextBoardOnNextSquare();
        FillNextBoardWithRandom();
    }

    public void SaveNextSquaresInGameData() {
        List<int> savedNextBoardNums = new List<int> { nextSquares[0].number, nextSquares[1].number, nextSquares[2].number };
        GameDataManager.GDM.savedNextSquares = savedNextBoardNums;
    }

    private void FillNextBoard() {
        List<int> savedNextBoardNums = GameDataManager.GDM.savedNextSquares;
        for (int i = 0; i < nextSquares.Count; i++) {
            nextSquares[i].GetComponent<SquareMechanics_Next>().SetNumberAndDisplay(savedNextBoardNums[i]);
        }
    }

    private void FillNextBoardWithRandom() {
        for (int i = 0; i < nextSquares.Count; i++) {
            nextSquares[i].GetComponent<SquareMechanics_Next>().SetRandomNumberAndDisplay();
        }
    }

    private void SetNextBoardOnNextSquare() {
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

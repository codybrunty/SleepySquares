using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBoardMechanics : MonoBehaviour{

    public List<SquareMechanics_Next> nextSquares = new List<SquareMechanics_Next>();
    public List<Coroutine> cors = new List<Coroutine>();
    public float moveDuration = .5f;
    public List<Vector3> pos = new List<Vector3>();

    private void Start()
    {
        GetPositions();
    }

    private void GetPositions()
    {
        for (int i = 0; i < nextSquares.Count; i++)
        {
            pos.Add(nextSquares[i].transform.localPosition);
        }
    }

    public void SetNextBoard() {
        FillNextBoard();
    }

    public void ResetNextBoard() {
        FillNextBoardWithRandom();
    }

    public void SaveNextSquaresInGameData() {
        List<int> savedNextBoardNums = new List<int> { nextSquares[0].number, nextSquares[1].number, nextSquares[2].number };

        int hardModeOn = GameDataManager.GDM.hardModeOn;
        if (hardModeOn == 1)
        {
            GameDataManager.GDM.HM_savedNextSquares = savedNextBoardNums;
        }
        else
        {
            GameDataManager.GDM.savedNextSquares = savedNextBoardNums;
        }
    }

    private void FillNextBoard() {
        int hardModeOn = GameDataManager.GDM.hardModeOn;

        if (hardModeOn == 1)
        {
            List<int> savedNextBoardNums = GameDataManager.GDM.HM_savedNextSquares;

            for (int i = 0; i < nextSquares.Count; i++)
            {
                nextSquares[i].SetNumberAndDisplay(savedNextBoardNums[i]);
            }
        }

        else
        {
            List<int> savedNextBoardNums = GameDataManager.GDM.savedNextSquares;

            for (int i = 0; i < nextSquares.Count; i++)
            {
                nextSquares[i].SetNumberAndDisplay(savedNextBoardNums[i]);
            }
        }
        


    }

    private void FillNextBoardWithRandom() {
        for (int i = 0; i < nextSquares.Count; i++)
        {
            nextSquares[i].SetRandomNumber();
            nextSquares[i].SetNumberDisplay();
        }
    }

    public void RotateNextBoard() {
        ClearFirstNextSquare();
        bool allZeros = CheckIfAllZeros();

        if (allZeros) {
            Debug.Log("Empty Next Board. New Number added.");
            nextSquares[0].SetRandomNumber();
            nextSquares[0].SetNumberDisplay();
        }
        else {
            RotateNextSquaresDown();
            NextboardSquareAnimationsAndDisplays();

        }
    }

    private void NextboardSquareAnimationsAndDisplays()
    {
        FixPositions();

        nextSquares[0].gameObject.SetActive(false);
        iTween.Stop(nextSquares[0].gameObject);

        for (int i = 1; i < nextSquares.Count; i++)
        {
            Coroutine c =StartCoroutine(MoveOverTime(nextSquares[i], pos[i], pos[i-1]));
            cors.Add(c);
        }
        Coroutine c2 = StartCoroutine(TurnOnFirstSquare());
        cors.Add(c2);
    }

    private void FixPositions()
    {
        for (int i = 0; i < cors.Count; i++)
        {
            Debug.LogWarning("stopping corotines");
            StopCoroutine(cors[i]);

            if (i != 0)
            {
                nextSquares[i].SetFakeDisplay(nextSquares[i-1].number);
            }
            
        }
        cors.Clear();

        for (int i = 0; i < nextSquares.Count; i++)
        {
            nextSquares[i].transform.localPosition = pos[i];
        }
    }

    IEnumerator TurnOnFirstSquare()
    {
        yield return new WaitForSeconds(moveDuration);
        nextSquares[0].gameObject.SetActive(true);
        nextSquares[0].SetNumberDisplay();
        cors.Clear();
    }

    IEnumerator MoveOverTime(SquareMechanics_Next squareMechanics, Vector3 oldPos, Vector3 newPos)
    {
        for (float t = 0; t < moveDuration; t+=Time.deltaTime)
        {
            squareMechanics.gameObject.transform.localPosition = Vector3.Lerp(oldPos,newPos,t/moveDuration);
            yield return null;
        }

        squareMechanics.gameObject.transform.localPosition = oldPos;
        squareMechanics.SetNumberDisplay();
    }

    private void RotateNextSquaresDown() {

        for (int i = 0; i < nextSquares.Count; i++) {
            //all but last
            if (i != nextSquares.Count - 1) {
                nextSquares[i].number = nextSquares[i + 1].number;
            }
            else {
                nextSquares[i].SetRandomNumber();
            }
        }

    }

    private bool CheckIfAllZeros() {
        bool allZeros = true;

        for(int i = 0; i < nextSquares.Count; i++) {
            if (nextSquares[i].number != 0) {
                allZeros = false;
            }
        }

        return allZeros;
    }

    private void ClearFirstNextSquare() {
        nextSquares[0].number = 0;
        nextSquares[0].SetNumberDisplay();
    }

    public int GetFirstNumber() {
        int number = nextSquares[0].number;
        return number;
    }

    public void SetFirstNumber(int newNumber) {
        nextSquares[0].number = newNumber;
        nextSquares[0].SetNumberDisplay();
    }
}

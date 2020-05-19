using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameBoardMechanics : MonoBehaviour{
    
    [Header("GameBoard Attributes")]
    public int score = 0;
    public int blockersOnField = 0;
    public int speedUpBlockerSpawnEveryPts = 50;
    private int speedUpBlockercounter = 1;

    [Header("GameBoard Settings")]
    public int gameBoardWidth;
    public int gameBoardHeight;
    public float borderSize = 1.0f;
    public bool touchEnabled = true;

    [Header("Game Objects")]
    public List<GameObject> gameBoardSquares = new List<GameObject>();
    public List<GameObject> blockerSquares = new List<GameObject>();
    [SerializeField] GameObject cameraHolder = default;
    [SerializeField] GameObject square_gameboard = default;
    [SerializeField] TextMeshProUGUI score_text = default;
    [SerializeField] TimerCountdown timer = default;
    private List<GameObject> completeList = new List<GameObject>();
    private bool completedListPass = true;
    [SerializeField] GameObject gameOver_text = default;
    [SerializeField] Button clearBlockerButton = default;
    [SerializeField] GameObject postLeaderboardButton = default;

    //blockers
    private List<GameObject> emptySquares = new List<GameObject>();

    private void Start() {
        SetUpCamera();
        CreateGameBoardSquares();
        SetGameBoardSquaresAdjescents();
    }

    public void GameOver() {
        Debug.Log("Game Over");
        gameOver_text.SetActive(true);
        touchEnabled = false;
        timer.StopTimer();
        postLeaderboardButton.SetActive(true);
    }

    public void CheckForCompleteLink(GameObject square) {
        completeList.Clear();
        completedListPass = true;
        AddSquareToCompletedList(square);
        if (completedListPass) {
            Debug.Log("Completed Link");
            PrintCompletedLink();
            ResetLinkFromBoard();
        }
        else {
            Debug.Log("Link Not Complete");
        }
    }
    
    public void ClearBlockers() {
        for (int i = 0; i < blockerSquares.Count; i++) {
            blockerSquares[i].GetComponent<SquareMechanics_Gameboard>().ResetSquare_BlockerClear();
        }
        blockerSquares.Clear();
    }

    private void ResetLinkFromBoard() {
        for (int i = 0; i < completeList.Count; i++) {
            AddToScore(completeList[i]);
            ResetSquare(completeList[i]);
        }
        UpdateScoreDiplay();
        UpdateClearsTotal();
        ReduceBlockerCountdownDuration();
    }

    private void ReduceBlockerCountdownDuration() {
        int blockerScore = speedUpBlockercounter * speedUpBlockerSpawnEveryPts;
        if (score > blockerScore) {
            speedUpBlockercounter++;
            timer.ReduceBlockerCountdownDuration();
        }
    }

    private void UpdateClearsTotal() {
        clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearsTotal(score);
    }

    private void UpdateScoreDiplay() {
        score_text.text = score.ToString();
    }

    public void AddBlockerToField() {
        blockersOnField++;
        AddBlockerToBoard();
    }

    private void AddBlockerToBoard() {
        emptySquares.Clear();
        for (int i = 0; i < gameBoardSquares.Count; i++) {
            if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number == 0) {
                emptySquares.Add(gameBoardSquares[i]);
            }
        }
        List<GameObject> emptySquares_randomOrder = RandomizeEmptySquares(emptySquares);
        if (emptySquares.Count != 0) {
            CreateBlockerAt(emptySquares_randomOrder[0]);
        }
        else {
            GameOver();
        }
    }

    private void CreateBlockerAt(GameObject square) {

        blockerSquares.Add(square);
        square.GetComponent<SquareMechanics_Gameboard>().number = 5;
        square.GetComponent<SquareMechanics_Gameboard>().SetSquareDisplay();


    }

    private List<GameObject> RandomizeEmptySquares(List<GameObject> squares) {
        for (int i = 0; i < squares.Count; i++) {
            GameObject tempGameObject = squares[i];
            int randomIndex = UnityEngine.Random.Range(0, squares.Count);
            squares[i] = squares[randomIndex];
            squares[randomIndex] = tempGameObject;
        }
        return squares;
    }

    private void ResetSquare(GameObject square) {
        square.GetComponent<SquareMechanics_Gameboard>().ResetSquare_OnCompletion();
    }

    private void AddToScore(GameObject square) {
        score = score + square.GetComponent<SquareMechanics_Gameboard>().number;
    }

    private void AddSquareToCompletedList(GameObject square) {
        completeList.Add(square);

        if (completedListPass) {
            for (int i = 0; i < square.GetComponent<SquareMechanics_Gameboard>().adjescentConnections.Count; i++) {
                if (square.GetComponent<SquareMechanics_Gameboard>().adjescentConnections[i] == true) {
                    if (square.GetComponent<SquareMechanics_Gameboard>().adjescentSquares[i].completed == true) {
                        if (!completeList.Contains(square.GetComponent<SquareMechanics_Gameboard>().adjescentSquares[i].gameObject)) {
                            AddSquareToCompletedList(square.GetComponent<SquareMechanics_Gameboard>().adjescentSquares[i].gameObject);
                        }
                    }
                    else {
                        completedListPass = false;
                    }
                }
            }
        }

    }

    private void SetGameBoardSquaresAdjescents() {
        for(int i = 0; i < gameBoardSquares.Count; i++) {
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().SetAdjescentSquares(gameObject.GetComponent<GameBoardMechanics>());
        }
    }

    private void SetUpCamera() {
        cameraHolder.transform.position = new Vector3((float)(gameBoardWidth - 1) / 2f, (float)(gameBoardHeight - 1) / 2f, -10f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float verticalSize = (float)gameBoardHeight / 2f + (borderSize);
        float horizontalSize = ((float)gameBoardWidth / 2f + (borderSize)) / aspectRatio;

        if (verticalSize > horizontalSize) {
            Camera.main.orthographicSize = verticalSize;
        }
        else {
            Camera.main.orthographicSize = horizontalSize;
        }

        cameraHolder.transform.localPosition = cameraHolder.transform.localPosition + new Vector3(0f, 1f, 0f);
    }

    private void CreateGameBoardSquares() {
        for (int x = 0; x < gameBoardWidth; x++) {
            for (int y = 0; y < gameBoardHeight; y++) {
                Generate_GameboardSquare(x, y);
            }
        }
    }

    private void Generate_GameboardSquare(float x, float y) {
        Vector3 squareSpawnPoint = new Vector3(x, y, 0);
        GameObject newSquare = Instantiate(square_gameboard, squareSpawnPoint, Quaternion.identity, gameObject.transform);
        newSquare.name = "Square_" + (Convert.ToInt32(x)).ToString() + "," + (Convert.ToInt32(y)).ToString();
        newSquare.GetComponent<SquareMechanics_Gameboard>().gamePositionX = Convert.ToInt32(x);
        newSquare.GetComponent<SquareMechanics_Gameboard>().gamePositionY = Convert.ToInt32(y);
        newSquare.GetComponent<SquareMechanics_Gameboard>().gamePositionIndex = gameBoardSquares.Count;
        gameBoardSquares.Add(newSquare);
    }

    private void PrintCompletedLink() {
        for (int i = 0; i < completeList.Count; i++) {
            Debug.Log(completeList[i].name + ": "+completeList[i].GetComponent<SquareMechanics_Gameboard>().number);
        }
    }

}

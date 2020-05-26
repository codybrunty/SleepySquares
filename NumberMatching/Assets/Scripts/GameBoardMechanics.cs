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
    public int luckyCoins = 1;

    [Header("Timer On Settings")]
    public int reduceBlockerTimerEveryPts = 50;
    private int reduceBlockerTimerCounter = 1;

    [Header("Timer Off Settings")]
    public int moveLimit = 10;
    public int moveLimitMin = 5;
    public int reduceMoveLimitEveryPts = 50;
    public int moveCounter = 0;
    private int reduceMoveLimitCounter = 1;


    [Header("GameBoard Settings")]
    public int gameBoardWidth;
    public int gameBoardHeight;
    public float borderSize = 1.0f;
    public bool touchEnabled = true;
    public bool gameOver = false;

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
    [SerializeField] Scoreboard scoreboard = default;

    //blockers
    private List<GameObject> emptySquares = new List<GameObject>();
    public SpriteRenderer pic;
    

    private void Start() {
        //SetUpCamera();
        CreateGameBoardSquares();
        SetGameBoardSquaresAdjescents();
    }

    private void Update() {
        //SetUpCamera();
        NewSetUp();
    }

    private void NewSetUp() {
        float screenRatio = (float)Screen.width / ((float)Screen.height);
        //float targetRatio = ((float)gameBoardWidth / (float)gameBoardHeight);
        float targetRatio = (float)pic.bounds.size.x / (float)pic.bounds.size.y;



        if (screenRatio > targetRatio) {
            //Camera.main.orthographicSize = (float)gameBoardHeight / 2f;
            Camera.main.orthographicSize = (float)pic.bounds.size.y / 2f;
        }
        else {
            float differenceInSize = targetRatio / screenRatio;
            //Camera.main.orthographicSize = (float)gameBoardHeight / 2f*differenceInSize;
            Camera.main.orthographicSize = (float)pic.bounds.size.y / 2f*differenceInSize;
        }

        //cameraHolder.transform.position = new Vector3((float)pic.bounds.size.x / 2f, pic.bounds.size.y / 2f, -10f);
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

        //cameraHolder.transform.localPosition = cameraHolder.transform.localPosition + new Vector3(0f, -0.75f, 0f);
    }

    public IEnumerator SetLuckyCoin() {
        Debug.Log("lucky coroutine started");
        yield return new WaitForSeconds(60);
        Debug.Log("lucky coin set");
        gameBoardSquares[UnityEngine.Random.Range(0, gameBoardSquares.Count)].GetComponent<SquareMechanics_Gameboard>().luckyCoin = true;
    }

    public void StartGame() {
        Debug.Log("Game Started");
        //StartCoroutine(SetLuckyCoin());
    }

    public void AddToMoveCounter() {
        moveCounter++;

        //if timer is off blockers apear by movelimit;
        if(GameSettings.GS.timerStatus == 0) {
            CheckMoveLimit();
        }

    }

    private void CheckMoveLimit() {
        if (moveCounter % moveLimit == 0) {
            AddBlockerToBoard();
        }
    }

    public void CheckIfBoardFull() {
        int emptyCounter = 0;
        for (int i = 0; i < gameBoardSquares.Count; i++) {
            if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number == 0) {
                emptyCounter++;
                Debug.Log("notgame ober");
                break;
            }
        }
        if (emptyCounter == 0) {
            GameOver();
        }
    }

    public void GameOver() {
        if (!gameOver) {
            gameOver = true;
            Debug.Log("Game Over");
            gameOver_text.SetActive(true);
            touchEnabled = false;

            if (GameSettings.GS.timerStatus == 1) {
                timer.StopTimer();
            }

            postLeaderboardButton.SetActive(true);
        }
    }

    public void CheckForCompleteLink(GameObject square) {
        completeList.Clear();
        completedListPass = true;
        AddSquareToCompletedList(square);
        if (completedListPass) {
            Debug.Log("Completed Link");
            //PrintCompletedLink();
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
        int floatingTextNumber = 0;
        for (int i = 0; i < completeList.Count; i++) {
            floatingTextNumber += completeList[i].GetComponent<SquareMechanics_Gameboard>().number;
            AddToScore(completeList[i]);
            ResetSquare(completeList[i]);
        }

        ClearSFX();
        UpdateScoreDiplay();
        ScoreDisplayFloatingText(floatingTextNumber);
        UpdateClearsTotal();

        //if timer on/off reduce by timer duration/move limit
        if (GameSettings.GS.timerStatus == 0) {
            ReduceMoveLimit();
        }
        else {
            ReduceBlockerCountdownDuration();
        }
    }

    private void ClearSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard");
    }

    private void ReduceBlockerCountdownDuration() {
        int blockerScore = reduceBlockerTimerCounter * reduceBlockerTimerEveryPts;
        if (score > blockerScore) {
            reduceBlockerTimerCounter++;
            timer.ReduceBlockerCountdownDuration();
        }
    }

    private void ReduceMoveLimit() {
        int moveLimitScore = reduceMoveLimitCounter * reduceMoveLimitEveryPts;
        if (score > moveLimitScore) {
            reduceMoveLimitCounter++;
            moveLimit--;
            if (moveLimit < moveLimitMin) {
                moveLimit = moveLimitMin;
            }
        }
    }

    private void UpdateClearsTotal() {
        clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearsTotal(score);
    }

    private void UpdateScoreDiplay() {
        score_text.text = score.ToString();

    }

    public void AddBlockerToBoard() {
        blockersOnField++;
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

    private void ScoreDisplayFloatingText(int number) {
        scoreboard.ScoreboardAdd(number);
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

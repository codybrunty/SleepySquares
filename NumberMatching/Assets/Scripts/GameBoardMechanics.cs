using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CloudOnce;

public class GameBoardMechanics : MonoBehaviour{

    [Header("GameBoard Attributes")]
    public int totalPoints = 0;
    public int highScore = 0;
    public int score = 0;
    public int luckyCoins = 1;
    public int hardModeOn = 0;

    [Header("Clear Settings")]
    public int clearsEveryPts = 100;
    private int clearCounter = 1;

    [Header("Move Settings")]
    public int moveLimitStart = 10;
    public int moveLimit = 10;
    public int moveLimitMin = 5;
    public int reduceMoveLimitEveryPts = 50;
    public int moveCounter = 0;


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
    [SerializeField] TextMeshProUGUI highScore_text = default;
    [SerializeField] TextMeshProUGUI totalPoints_text = default;
    private List<GameObject> completeList = new List<GameObject>();
    private bool completedListPass = true;
    [SerializeField] Button clearBlockerButton = default;
    [SerializeField] GameOverPanel gameOverPanel = default;
    [SerializeField] Scoreboard scoreboard = default;
    [SerializeField] NextBoardMechanics nextBoard = default;
    [SerializeField] SwitchButton switchButton = default;
    [SerializeField] TrophySystem trophySystem = default;
    [SerializeField] ResetGameScene resetButton = default;
    [SerializeField] HardModeTextMechanics hardText = default;

    //blockers
    private List<GameObject> emptySquares = new List<GameObject>();
    public SpriteRenderer pic;

    private void Awake() {
        SetGameBoardSquareInfo();
    }

    private void Start() {
        SetBoardBaseColor();
        SetBoardState();
    }
    
    private void SetBoardBaseColor() {
        CollectionColor_Sprite[] sprites = gameObject.GetComponentsInChildren<CollectionColor_Sprite>(true);

        foreach(CollectionColor_Sprite sprite in sprites) {
            sprite.GetColor();
        }

    }

    private void Update() {
        CameraSetup();
    }

    public void ResetBoardState() {
        Debug.Log("reset board state");
        for (int i = 0; i < gameBoardSquares.Count; i++) {
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().ZerOutSquareInfo();
        }

        blockerSquares.Clear();
        clearBlockerButton.GetComponent<BoardClearCommand>().clearsTotal = 0;
        clearCounter = 1;
        clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearTextDisplay();

        switchButton.switchAmmount = GameDataManager.GDM.currentSwitches;
        switchButton.UpdateSwitchAmmountDisplay();

        hardModeOn = GameDataManager.GDM.hardModeOn;
        score = 0;
        UpdateScoreDiplay();
        GetHighScore();
        UpdateHighScoreDisplay();

        totalPoints = GameDataManager.GDM.TotalPoints_AllTime;
        UpdateTotalPointsTrophyDisplay();

        ReduceMoveLimit();
        moveCounter = 0;

        nextBoard.ResetNextBoard();

        touchEnabled = true;
        gameOver = false;

        SaveBoardState();
    }

    private void GetHighScore() {
        if (hardModeOn == 1) {
            highScore = GameDataManager.GDM.HardModeHighScore_AllTime;
        }
        else {
            highScore = GameDataManager.GDM.HighScore_AllTime;
        }
    }

    private void SaveHighScore() {
        if (hardModeOn == 1) {
            GameDataManager.GDM.HardModeHighScore_AllTime=highScore;
            Debug.LogWarning("HardMode HighScore Updated");
        }
        else {
            GameDataManager.GDM.HighScore_AllTime=highScore;
            Debug.LogWarning("Regular HighScore Updated");
        }
        GameDataManager.GDM.SaveGameData();
    }
    private void SetBoardState() {
        Debug.Log("set board state");
        if (GameDataManager.GDM.gameOver) {
            Debug.Log("previous session ended in game over ");
            ResetBoardState();
        }
        else {
            Debug.Log("continue previous session");
            for (int i = 0; i < gameBoardSquares.Count; i++) {
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number = GameDataManager.GDM.squares[i].number;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().completed = GameDataManager.GDM.squares[i].completed;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().blocker = GameDataManager.GDM.squares[i].blocker;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().adjescentConnections = GameDataManager.GDM.squares[i].adjescentConnections;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().luckyCoin = GameDataManager.GDM.squares[i].luckyCoin;

                if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number != 0) {
                    gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().SilentSquareDisplay();
                }
            }

            SetBlockerSquaresList();

            clearBlockerButton.GetComponent<BoardClearCommand>().clearsTotal = GameDataManager.GDM.currentClears;
            clearCounter = GameDataManager.GDM.currentClearCounter;
            clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearTextDisplay();

            switchButton.switchAmmount = GameDataManager.GDM.currentSwitches;
            switchButton.UpdateSwitchAmmountDisplay();

            hardModeOn = GameDataManager.GDM.hardModeOn;

            GetHighScore();
            UpdateHighScoreDisplay();

            totalPoints = GameDataManager.GDM.TotalPoints_AllTime;
            UpdateTotalPointsTrophyDisplay();

            score = GameDataManager.GDM.currentPoints;
            UpdateScoreDiplay();

            moveCounter = GameDataManager.GDM.moveCounter;
            ReduceMoveLimit();

            nextBoard.SetNextBoard();
        }


    }

    public void SaveBoardState() {
        Debug.Log("save board state");
        for (int i = 0; i < gameBoardSquares.Count; i++) {
            GameDataManager.GDM.squares[i].number = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number;
            GameDataManager.GDM.squares[i].completed = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().completed;
            GameDataManager.GDM.squares[i].blocker = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().blocker;
            GameDataManager.GDM.squares[i].adjescentConnections = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().adjescentConnections;
            GameDataManager.GDM.squares[i].luckyCoin = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().luckyCoin;
        }
        GameDataManager.GDM.currentPoints = score;

        GameDataManager.GDM.TotalPoints_AllTime = totalPoints;
        UpdateTotalPointsTrophyDisplay();


        GameDataManager.GDM.hardModeOn = hardModeOn;
        GetHighScore();
        GameDataManager.GDM.currentClears = clearBlockerButton.GetComponent<BoardClearCommand>().clearsTotal;
        GameDataManager.GDM.currentClearCounter = clearCounter;
        GameDataManager.GDM.currentSwitches = switchButton.switchAmmount;
        GameDataManager.GDM.moveCounter = moveCounter;

        nextBoard.SaveNextSquaresInGameData();

        GameDataManager.GDM.gameOver = gameOver;

        GameDataManager.GDM.SaveGameData();
    }


    private void SetBlockerSquaresList() {
        for (int i = 0; i < gameBoardSquares.Count; i++) {
            if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().blocker == true) {
                blockerSquares.Add(gameBoardSquares[i]);
            }
        }
    }

    private void CameraSetup() {
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
        CheckMoveLimit();
    }

    private void CheckMoveLimit() {
        //movecounter hits movelimit and hardmode is off
        if (moveCounter % moveLimit == 0 && hardModeOn == 0) {
            AddBlockerToBoard();
        }
    }

    public void CheckIfBoardFull() {
        int emptyCounter = 0;
        for (int i = 0; i < gameBoardSquares.Count; i++) {
            if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number == 0) {
                emptyCounter++;
                //Debug.Log("not game over");
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
            touchEnabled = false;
            PostToLeaderboard();
            RevealGameOverPanel();
            SaveBoardState();
        }
    }

    private void RevealGameOverPanel() {
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.GameOverPanelAnimation();
        FindObjectOfType<SoundManager>().PlayOneShotSound("GameOver");
        gameOverPanel.score = score;
        gameOverPanel.highscore = highScore;
        gameOverPanel.UpdateGameOverPanel();
    }

    private void PostToLeaderboard() {
        Debug.Log("Post To Leaderboard");
        long scoreToPost = score;
        if (hardModeOn == 1) {
            Leaderboards.HardModeHighScore.SubmitScore(scoreToPost, callbackCheck);
        }
        else {
            Leaderboards.HighScore.SubmitScore(scoreToPost, callbackCheck);
        }
       
    }

    private void callbackCheck(CloudRequestResult<bool> result) {
        if (result.Result == false) {
            Debug.Log(result.Error);
        }
    }

    public void CheckForCompleteLink(GameObject square) {
        completeList.Clear();
        completedListPass = true;
        AddSquareToCompletedList(square);
        if (completedListPass) {
            Debug.Log("Completed Link");
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
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearBlockers");
        SaveBoardState();
    }

    private void ResetLinkFromBoard() {
        int floatingTextNumber = 0;
        for (int i = 0; i < completeList.Count; i++) {
            floatingTextNumber += completeList[i].GetComponent<SquareMechanics_Gameboard>().number;
            AddToScore(completeList[i]);
            AddToTotalPoints(completeList[i]);
            ResetSquare(completeList[i]);
        }

        ClearSFX(floatingTextNumber);
        UpdateScoreDiplay();
        HighScoreCheck();
        //UpdateTotalPointsTrophyDisplay();
        ScoreDisplayFloatingText(floatingTextNumber);
        UpdateClearsTotal();

        ReduceMoveLimit();
    }

    private void HighScoreCheck() {
        if (score > highScore) {
            highScore = score;
            UpdateHighScoreDisplay();
            SaveHighScore();
        }
    }

    private void ClearSFX(int clearNumber) { 
        if(clearNumber <= 15) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard1");
        }
        else if (clearNumber > 15 && clearNumber <= 35) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard2");
        }
        else {
            FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard3");
        }

    }


    private void ReduceMoveLimit() {
        moveLimit = moveLimitStart - ( (int)(score / reduceMoveLimitEveryPts));

        if (moveLimit < moveLimitMin) {
            moveLimit = moveLimitMin;
        }
    }

    private void UpdateClearsTotal() {

        int clearScore = GetClearScore();
        if (score > clearScore) {
            clearCounter++;
            clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearsTotal(1);
            UpdateClearsTotal();
        }

    }

    public int GetClearScore() {
        return clearCounter * clearsEveryPts;
    }

    private void UpdateScoreDiplay() {
        Debug.Log("score updated to " + score);
        score_text.text = score.ToString();
        clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearFill();
    }

    private void UpdateHighScoreDisplay() {
        highScore_text.text = highScore.ToString();
    }

    private void UpdateTotalPointsTrophyDisplay() {
        totalPoints_text.text = totalPoints.ToString();
        trophySystem.UpdateTrophyPanel();
    }

    public void AddBlockerToBoard() {
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
    private void AddToTotalPoints(GameObject square) {
        totalPoints = totalPoints + square.GetComponent<SquareMechanics_Gameboard>().number;
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

    
    private void SetGameBoardSquareInfo() {

        for (int i = 0; i < gameBoardSquares.Count; i++) {

            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().gamePositionIndex = i;
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().gamePositionX = i / gameBoardHeight;
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().gamePositionY = i % gameBoardHeight;

        }

        for (int i = 0; i < gameBoardSquares.Count; i++) {

            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().SetAdjescentSquares(gameObject.GetComponent<GameBoardMechanics>());

        }
    }

    /*
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
    */

    private void PrintCompletedLink() {
        for (int i = 0; i < completeList.Count; i++) {
            Debug.Log(completeList[i].name + ": "+completeList[i].GetComponent<SquareMechanics_Gameboard>().number);
        }
    }

    public void TurnOnHardMode() {
        Debug.Log("Turn Hard Mode On");
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
        hardModeOn = 1;
        GameDataManager.GDM.hardModeOn = hardModeOn;
        GameDataManager.GDM.SaveGameData();
        hardText.UpdateHardText();
        resetButton.ResetHardModeSwitch(0);
    }

    public void TurnOffHardMode() {
        Debug.Log("Turn Hard Mode Off");
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
        hardModeOn = 0;
        GameDataManager.GDM.hardModeOn = hardModeOn;
        GameDataManager.GDM.SaveGameData();
        hardText.UpdateHardText();
        resetButton.ResetHardModeSwitch(1);

    }

}

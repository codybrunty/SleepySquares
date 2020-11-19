using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CloudOnce;

public class GameBoardMechanics : MonoBehaviour
{

    [Header("GameBoard Attributes")]
    public int totalPoints = 0;
    public int highScore = 0;
    public int score = 0;
    public int luckyCoins = 1;
    public int hardModeOn = 0;

    [Header("Clear Settings")]
    public bool firstClear = false;
    public int firstClearPts = 50;
    public int clearsEveryPts = 100;
    public int incrementClearEveryPtsBy = 5;
    public int incrementClearEveryPtsAfter = 375;
    public int clearIncrementMultiplier = 0;
    public int clearCounter = 1;

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
    [SerializeField] TextMeshProUGUI score_text = default;
    [SerializeField] TextMeshProUGUI highScore_text = default;
    private List<GameObject> completeList = new List<GameObject>();
    private bool completedListPass = true;
    [SerializeField] Button clearBlockerButton = default;
    [SerializeField] GameOverPanel gameOverPanel = default;
    [SerializeField] Scoreboard scoreboard = default;
    [SerializeField] NextBoardMechanics nextBoard = default;
    [SerializeField] SwitchButton switchButton = default;
    [SerializeField] SettingsButtonMovement settingsButton = default;
    [SerializeField] IconsPanelMovement iconsPanel = default;
    [SerializeField] NextBoardMovement nextBoardPanel = default;
    [SerializeField] NextBoardBGMovement nextbgPanel = default;
    [SerializeField] ScoreboardMovement scoreboardPanel = default;
    [SerializeField] ScoreboardMovement bestScoreboardPanel = default;
    [SerializeField] ResetGameScene resetButton = default;
    [SerializeField] HardModeTextMechanics hardText = default;
    //[SerializeField] GameObject gameOverBorders = default;
    [SerializeField] CameraShake cameraHolder = default;
    [SerializeField] GameObject gameboardScaleGroup = default;
    public float shakeTime = 1f;
    public float shakeMaxMagnitude = .175f;
    private Vector3 gameboardOrgScale;
    [SerializeField] GameObject gameOverEffect = default;
    [SerializeField] GameObject gameOverEffect2 = default;

    [SerializeField] GameObject gameOverEffectPosition1GO = default;
    [SerializeField] GameObject gameOverEffectPosition2GO = default;
    [SerializeField] GameObject gameOverEffectPosition3GO = default;
    [SerializeField] GameObject gameOverEffectPosition4GO = default;
    [SerializeField] GameObject gameOverEffectPosition5GO = default;
    [SerializeField] GameObject gameOverEffectPosition6GO = default;
    [SerializeField] GameObject gameOverEffectPosition7GO = default;
    [SerializeField] GameObject gameOverEffectPosition8GO = default;

    private Vector3 gameOverEffectPosition1 = default;
    private Vector3 gameOverEffectPosition2 = default;
    private Vector3 gameOverEffectPosition3 = default;
    private Vector3 gameOverEffectPosition4 = default;
    private Vector3 gameOverEffectPosition5 = default;
    private Vector3 gameOverEffectPosition6 = default;
    private Vector3 gameOverEffectPosition7 = default;
    private Vector3 gameOverEffectPosition8 = default;

    [SerializeField] GameObject gameboard_bg = default;
    [SerializeField] GameObject smokeEffect = default;

    //blockers
    private List<GameObject> emptySquares = new List<GameObject>();
    //public SpriteRenderer pic;
    public bool bubblesOn = true;
    public bool eyePickingMode = false;

    private Coroutine luckyCor = null;
    [SerializeField] RaycastMouse raycastForMouse = default;

    [SerializeField] WordArtPopUps popArt = default;
    private bool newHighScore = false;
    [SerializeField] GameObject confetti = default;
    [SerializeField] NotificationSystem notificationSystem = default;
    private bool  playHighScoreAnimation = false;
    public float boardScoreSquareClearDuration = 0.5f;
    public bool playSquareClearAnimation = false;
    public bool gameStarted = false;
    public float luckyWaitMin = 10f;
    public float luckyWaitMax = 60f;

    #region Start
    private void Awake()
    {
        SetGameBoardSquareInfo();
        CameraSetup();
    }

    private void Start()
    {
        GetUIStartPositions();
        GetGameOverEffectPositions();
        SetBoardBaseColor();
        SetBoardState();
        gameboardOrgScale = gameboardScaleGroup.transform.localScale;
        //PrintSquarePositions();
    }
    #endregion

    #region Reset Board State
    public void ResetBoardState()
    {
        Debug.Log("Reset Board State");
        SetUIOnScreen();
        gameboard_bg.GetComponent<GameBoardBGMovement>().TrueSize();
        TurnOffAllSquareZzz();
        ZeroOutAllSquares();

        blockerSquares.Clear();
        clearBlockerButton.GetComponent<BoardClearCommand>().clearsTotal = 0;
        clearCounter = 1;
        clearIncrementMultiplier = 0;
        firstClear = false;
        clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearDisplay();
        clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearFill();

        switchButton.switchAmmount = GameDataManager.GDM.currentSwitches;
        switchButton.UpdateSwitchAmmountDisplay();

        hardModeOn = GameDataManager.GDM.hardModeOn;
        score = 0;
        UpdateScoreDiplay();
        GetHighScore();
        UpdateHighScoreDisplay();
        NewHighScoreCheck();

        totalPoints = GameDataManager.GDM.TotalPoints_AllTime;
        ReduceMoveLimit();
        moveCounter = 0;

        nextBoard.ResetNextBoard();
        StartCoroutine(EnableTouch(.1f));
        gameOver = false;

        DimSleepingSquares();

        SaveBoardState();
    }

    IEnumerator EnableTouch(float num)
    {
        yield return new WaitForSeconds(num);
        touchEnabled = true;
    }
    #endregion

    #region Set Board State

    private void SetNormalBoard()
    {
        Debug.Log("Normal Mode Board Setup");
        if (GameDataManager.GDM.gameOver)
        {
            Debug.Log("previous session ended in game over ");
            ResetBoardState();
        }
        else
        {
            Debug.Log("continue previous session");
            //clear board first
            for (int i = 0; i < gameBoardSquares.Count; i++)
            {
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().ZeroOutSquareInfo();
            }


            for (int i = 0; i < gameBoardSquares.Count; i++)
            {
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number = GameDataManager.GDM.squares[i].number;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().completed = GameDataManager.GDM.squares[i].completed;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().blocker = GameDataManager.GDM.squares[i].blocker;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().adjescentConnections = GameDataManager.GDM.squares[i].adjescentConnections;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().luckyCoin = false;

                if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number != 0)
                {
                    gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().SilentSquareDisplay();
                }
            }

            SetBlockerSquaresList();

            firstClear = GameDataManager.GDM.firstClear;
            clearBlockerButton.GetComponent<BoardClearCommand>().clearsTotal = GameDataManager.GDM.currentClears;
            clearCounter = GameDataManager.GDM.currentClearCounter;
            clearIncrementMultiplier = GameDataManager.GDM.clearIncrementMultiplier;
            clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearDisplay();
            clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearFill();

            switchButton.switchAmmount = GameDataManager.GDM.currentSwitches;
            switchButton.UpdateSwitchAmmountDisplay();

            GetHighScore();
            UpdateHighScoreDisplay();
            totalPoints = GameDataManager.GDM.TotalPoints_AllTime;

            score = GameDataManager.GDM.currentPoints;
            UpdateScoreDiplay();

            newHighScore = false;
            if (score == highScore)
            {
                newHighScore = true;
            }

            moveCounter = GameDataManager.GDM.moveCounter;
            ReduceMoveLimit();
            nextBoard.SetNextBoard();
            UpdateSquareConnections();
            DimSleepingSquares();
        }
    }

    private void SetHardBoard()
    {
        Debug.Log("Hard Mode Board Setup");
        if (GameDataManager.GDM.HM_gameOver)
        {
            Debug.Log("previous session ended in game over ");
            ResetBoardState();
        }
        else
        {
            Debug.Log("continue previous session");
            //clear board first
            for (int i = 0; i < gameBoardSquares.Count; i++)
            {
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().ZeroOutSquareInfo();
            }


            for (int i = 0; i < gameBoardSquares.Count; i++)
            {
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number = GameDataManager.GDM.HM_squares[i].number;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().completed = GameDataManager.GDM.HM_squares[i].completed;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().blocker = GameDataManager.GDM.HM_squares[i].blocker;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().adjescentConnections = GameDataManager.GDM.HM_squares[i].adjescentConnections;
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().luckyCoin = false;

                if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number != 0)
                {
                    gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().SilentSquareDisplay();
                }
            }

            SetBlockerSquaresList();

            firstClear = false;
            clearBlockerButton.GetComponent<BoardClearCommand>().clearsTotal = 0;
            clearCounter = 0;
            clearIncrementMultiplier = 0;
            clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearDisplay();
            clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearFill();

            switchButton.switchAmmount = GameDataManager.GDM.currentSwitches;
            switchButton.UpdateSwitchAmmountDisplay();


            GetHighScore();
            UpdateHighScoreDisplay();

            totalPoints = GameDataManager.GDM.TotalPoints_AllTime;
            score = GameDataManager.GDM.HM_currentPoints;
            UpdateScoreDiplay();

            newHighScore = false;
            if (score == highScore)
            {
                newHighScore = true;
            }

            moveCounter = 0;
            nextBoard.SetNextBoard();
            UpdateSquareConnections();
            DimSleepingSquares();
        }
    }

    public void SetBoardState()
    {
        Debug.Log("setup board state");
        hardModeOn = GameDataManager.GDM.hardModeOn;
        if (hardModeOn == 1)
        {
            SetHardBoard();
        }
        else
        {
            SetNormalBoard();
        }
    }
    #endregion

    #region Save Board State
    private void SaveNormalBoard()
    {
        Debug.Log("save normal board state");
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            GameDataManager.GDM.squares[i].number = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number;
            GameDataManager.GDM.squares[i].completed = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().completed;
            GameDataManager.GDM.squares[i].blocker = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().blocker;
            GameDataManager.GDM.squares[i].adjescentConnections = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().adjescentConnections;
            GameDataManager.GDM.squares[i].luckyCoin = false;
        }
        GameDataManager.GDM.currentPoints = score;


        if (totalPoints > GameDataManager.GDM.TotalPoints_AllTime)
        {
            GameDataManager.GDM.TotalPoints_AllTime = totalPoints;
        }
        else
        {
            if (totalPoints < GameDataManager.GDM.TotalPoints_AllTime)
            {
                Debug.LogError("TotalPoints < GameData Total Points");
                Debug.LogError(totalPoints.ToString() + " < " + GameDataManager.GDM.TotalPoints_AllTime.ToString());
            }
        }


        GameDataManager.GDM.hardModeOn = hardModeOn;
        GetHighScore();

        GameDataManager.GDM.firstClear = firstClear;
        GameDataManager.GDM.currentClears = clearBlockerButton.GetComponent<BoardClearCommand>().clearsTotal;
        GameDataManager.GDM.currentClearCounter = clearCounter;
        GameDataManager.GDM.clearIncrementMultiplier = clearIncrementMultiplier;
        GameDataManager.GDM.currentSwitches = switchButton.switchAmmount;
        GameDataManager.GDM.moveCounter = moveCounter;

        nextBoard.SaveNextSquaresInGameData();

        GameDataManager.GDM.gameOver = gameOver;
        GameDataManager.GDM.SaveGameData();
    }

    private void SaveHardBoard()
    {
        Debug.Log("save hard board state");
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            GameDataManager.GDM.HM_squares[i].number = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number;
            GameDataManager.GDM.HM_squares[i].completed = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().completed;
            GameDataManager.GDM.HM_squares[i].blocker = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().blocker;
            GameDataManager.GDM.HM_squares[i].adjescentConnections = gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().adjescentConnections;
            GameDataManager.GDM.HM_squares[i].luckyCoin = false;
        }
        GameDataManager.GDM.HM_currentPoints = score;

        if (totalPoints > GameDataManager.GDM.TotalPoints_AllTime)
        {
            GameDataManager.GDM.TotalPoints_AllTime = totalPoints;
        }
        else
        {
            if (totalPoints < GameDataManager.GDM.TotalPoints_AllTime)
            {
                Debug.LogError("TotalPoints < GameData Total Points");
                Debug.LogError(totalPoints.ToString() + " < " + GameDataManager.GDM.TotalPoints_AllTime.ToString());
            }
        }

        GameDataManager.GDM.hardModeOn = hardModeOn;
        GetHighScore();
        GameDataManager.GDM.currentSwitches = switchButton.switchAmmount;

        nextBoard.SaveNextSquaresInGameData();
        GameDataManager.GDM.HM_gameOver = gameOver;
        GameDataManager.GDM.SaveGameData();
    }

    public void SaveBoardState()
    {
        Debug.Log("save board state");
        hardModeOn = GameDataManager.GDM.hardModeOn;
        if (hardModeOn == 1)
        {
            SaveHardBoard();
        }
        else
        {
            SaveNormalBoard();
        }
    }
    #endregion

    #region Game Over
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            TimeManager.TM.StopTimer();
            Debug.Log("Game Over");
            touchEnabled = false;
            PostToLeaderboard();

            StartCoroutine(AnimationsThenRevealGameOverPanel());
            raycastForMouse.gameStarted = false;

            GameOverCounter();
            SaveBoardState();
        }
    }

    private void GameOverCounter()
    {
        //hardmode
        if (hardModeOn == 1)
        {
            int counter = PlayerPrefs.GetInt("GameOver_HM",0);
            counter++;
            PlayerPrefs.SetInt("GameOver_HM", counter);
        }
        //normalMode
        else
        {
            int counter = PlayerPrefs.GetInt("GameOver", 0);
            counter++;
            PlayerPrefs.SetInt("GameOver", counter);
        }
    }

    IEnumerator AnimationsThenRevealGameOverPanel()
    {
        //StartCoroutine(cameraHolder.Shake(shakeTime, shakeMaxMagnitude));
        ShakeBoard();
        SoundManager.SM.PlayOneShotSound("pop");
        ScaleSquares();
        StartCoroutine(ExplosionEffects());
        yield return new WaitForSeconds(shakeTime - .3f);
        SwitchSquaresToFakeMaterial();
        yield return new WaitForSeconds(.2f);
        //SoundManager.SM.PlayOneShotSound("pop");

        ShowGameOverPanel();
    }

    private void ShowGameOverPanel()
    {
        MoveUIOffScreen();
        ScaleSquaresBG();
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.score = score;
        gameOverPanel.highscore = highScore;
        gameOverPanel.UpdateGameOverPanel();
        MakeSquaresFloat();


        gameOverPanel.GameOverPanelAnimation();
    }

    #endregion

    #region Leaderboard

    public void PostToLeaderboard()
    {
        Debug.Log("Post To Leaderboard");
        long scoreToPost = score;
        if (hardModeOn == 1)
        {
            Leaderboards.HardModeHighScore.SubmitScore(scoreToPost, callbackCheck);
        }
        else
        {
           Leaderboards.HighScore.SubmitScore(scoreToPost, callbackCheck);
        }

    }
    
    private void callbackCheck(CloudRequestResult<bool> result)
    {
        if (result.Result == false)
        {
            Debug.Log(result.Error);
        }
    }
    
    #endregion

    #region Scoring
    public void CheckForCompleteLink(GameObject square)
    {
        completeList.Clear();
        completedListPass = true;
        AddSquareToCompletedList(square);
        if (completedListPass)
        {
            Debug.Log("Completed Link");
            ResetLinkFromBoard();
        }
        else {
            Debug.Log("Link Not Complete");
        }
    }

    private void HighScoreCheck()
    {
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
            if (!newHighScore)
            {
                newHighScore = true;
                playHighScoreAnimation = true;
            }
        }
    }

    IEnumerator NewHighScoreAnimation()
    {
        SoundManager.SM.PlayOneShotSound("kazoo");
        confetti.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2, 2f, 0f));
        hash.Add("time", 1.5f);
        iTween.PunchScale(bestScoreboardPanel.gameObject, hash);
        yield return new WaitForSeconds(3f);
        confetti.SetActive(false);
    }

    private void UpdateScoreDiplay()
    {
        Debug.Log("score updated to " + score);
        score_text.text = score.ToString();
        if (hardModeOn != 1)
        {
            clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearFill();
        }
    }

    private void UpdateHighScoreDisplay()
    {
        highScore_text.text = highScore.ToString();
    }



    private void ClearSFX(int clearNumber) {
        if (clearNumber <= 15) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard1");
        }
        else if (clearNumber > 15 && clearNumber <= 35) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard2");
        }
        else {
            FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard3");
            popArt.WordArtAnimation(clearNumber);
        }

    }

    private void ClearSFX(int clearNumber, bool dontPlayArt) {
        if (clearNumber <= 15) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard1");
        }
        else if (clearNumber > 15 && clearNumber <= 35) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard2");
        }
        else {
            FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard3");
        }

    }

    #endregion

    #region Hard Mode
    public void TurnOnHardMode()
    {
        Debug.Log("Turn Hard Mode On");
        TimeManager.TM.StopTimer();
        CheckIfFirstTimeEverHardMode();
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
        GameDataManager.GDM.SaveGameData();
        hardModeOn = 1;
        GameDataManager.GDM.hardModeOn = hardModeOn;
        hardText.UpdateHardText();
        resetButton.ResetHardModeSwitch();
        clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearFill();
        RemoveAnyLuckyCoinsFromBoard();
        GameDataManager.GDM.SaveGameData();
    }

    private void CheckIfFirstTimeEverHardMode()
    {
        int playedHardMode = PlayerPrefs.GetInt("PlayedHardMode", 0);
        if(playedHardMode == 0)
        {
            PlayerPrefs.SetInt("PlayedHardMode", 1);
            notificationSystem.CheckAlertStatus();
        }
    }

    public void TurnOffHardMode()
    {
        Debug.Log("Turn Hard Mode Off");
        TimeManager.TM.StopTimer();
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
        GameDataManager.GDM.SaveGameData();
        hardModeOn = 0;
        GameDataManager.GDM.hardModeOn = hardModeOn;
        hardText.UpdateHardText();
        resetButton.ResetHardModeSwitch();
        clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearFill();
        RemoveAnyLuckyCoinsFromBoard();
        GameDataManager.GDM.SaveGameData();
    }
    #endregion

    #region Miscellaneous
    private void DimSleepingSquares()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().CheckDimIfCompleted();
        }
    }

    private void NewHighScoreCheck()
    {
        newHighScore = false;
        if (score == highScore)
        {
            newHighScore = true;
        }
    }

    private void GetGameOverEffectPositions()
    {
        gameOverEffectPosition1 = gameOverEffectPosition1GO.transform.position;
        gameOverEffectPosition2 = gameOverEffectPosition2GO.transform.position;
        gameOverEffectPosition3 = gameOverEffectPosition3GO.transform.position;
        gameOverEffectPosition4 = gameOverEffectPosition4GO.transform.position;
        gameOverEffectPosition5 = gameOverEffectPosition5GO.transform.position;
        gameOverEffectPosition6 = gameOverEffectPosition6GO.transform.position;
        gameOverEffectPosition7 = gameOverEffectPosition7GO.transform.position;
        gameOverEffectPosition8 = gameOverEffectPosition8GO.transform.position;
    }

    private void SetBoardBaseColor()
    {
        CollectionColor_Sprite[] sprites = gameObject.GetComponentsInChildren<CollectionColor_Sprite>(true);

        foreach (CollectionColor_Sprite sprite in sprites)
        {
            sprite.GetColor();
        }

    }

    private void ZeroOutAllSquares()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().ZeroOutSquareInfo();
        }
    }

    private void TurnOffAllSquareZzz()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().StopZzz();
        }
    }

    private void GetHighScore()
    {
        if (hardModeOn == 1)
        {
            highScore = GameDataManager.GDM.HardModeHighScore_AllTime;
        }
        else
        {
            highScore = GameDataManager.GDM.HighScore_AllTime;
        }
    }

    private void SaveHighScore()
    {
        if (hardModeOn == 1)
        {
            GameDataManager.GDM.HardModeHighScore_AllTime = highScore;
            Debug.Log("HardMode HighScore Updated");
        }
        else
        {
            GameDataManager.GDM.HighScore_AllTime = highScore;
            Debug.Log("Regular HighScore Updated");
        }
        GameDataManager.GDM.SaveGameData();
    }

    private void SetBlockerSquaresList()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().blocker == true)
            {
                blockerSquares.Add(gameBoardSquares[i]);
            }
        }
    }

    private void CameraSetup()
    {
        float normalAspect = 1152f / 2048f;
        float currentAspect = ((float)Camera.main.pixelWidth / Camera.main.pixelHeight);
        float newOrtho = 16.77f * (normalAspect / currentAspect);


        float testAspect = ((float)Camera.main.pixelHeight / Camera.main.pixelWidth);
        if (testAspect > 1.5)
        {
            Camera.main.orthographicSize = newOrtho;
        }


        else
        {
            Camera.main.orthographicSize = 17.5f;

        }
    }


    public void AllSquaresDown(GameObject square)
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            if (gameBoardSquares[i] != square)
            {
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().SquareDown();
            }
        }
    }

    public void AddLuckyCoinToBoard()
    {
        RemoveAnyLuckyCoinsFromBoard();
        luckyCor = StartCoroutine(SetLuckyCoin());
    }

    public IEnumerator SetLuckyCoin()
    {
        //Debug.Log("Lucky Coin Coroutine Started");
        float luckyCoinWaitTime = UnityEngine.Random.Range(luckyWaitMin, luckyWaitMax);
        //float luckyCoinWaitTime = UnityEngine.Random.Range(5f, 10f);

        int moveCounter_tmp = moveCounter;
        yield return new WaitForSeconds(luckyCoinWaitTime);
        Debug.Log("Lucky Coin Placed On The Board After " + luckyCoinWaitTime.ToString() + " Seconds");

        if (moveCounter - moveCounter_tmp > (int)luckyCoinWaitTime/4)
        {
            ReloadEmptySquares();
            if (emptySquares.Count > 0) {
                int randomSquareIndex = UnityEngine.Random.Range(0, emptySquares.Count);
                emptySquares[randomSquareIndex].GetComponent<SquareMechanics_Gameboard>().luckyCoin = true;
                emptySquares[randomSquareIndex].GetComponent<SquareMechanics_Gameboard>().SetLuckyColor();
            }
            /*
            else {
                Debug.Log("no empty squares");
            }
            */
        }
        else
        {
            Debug.Log("Player only made " + (moveCounter - moveCounter_tmp) + " moves not enough for a lucky coin needed " + (int)luckyCoinWaitTime / 4);
            AddLuckyCoinToBoard();
        }

    }

    public void AddLuckyPoints(int num) {

        AddToScore(num);
        AddToTotalPoints(num);
        HighScoreCheck();
        UpdateClearsTotal();
        UpdateScoreDiplay();
        UpdateHighScoreDisplay();
        ScoreDisplayFloatingText(num);

        ClearSFX(num, true);
        //SoundManager.SM.PlayOneShotSound("LuckyPoints");

        if (playHighScoreAnimation) {
            playHighScoreAnimation = false;
            StartCoroutine(NewHighScoreAnimation());
        }

    }

    private void ReloadEmptySquares()
    {
        emptySquares.Clear();
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number == 0)
            {
                emptySquares.Add(gameBoardSquares[i]);
            }
        }
    }

    public void StartGame()
    {
        if (hardModeOn == 1)
        {
            TimeManager.TM.StartHardTimer();
        }
        else
        {
            TimeManager.TM.StartNormalTimer();
        }
        AddLuckyCoinToBoard();
    }

    public void RemoveAnyLuckyCoinsFromBoard()
    {
        //Debug.Log("Removing All Lucky Coins On The Board");

        if (luckyCor != null)
        {
            //Debug.Log("Lucky Coin Coroutine Stopped");
            StopCoroutine(luckyCor);
        }

        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().luckyCoin = false;
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().SetLuckyColor();
        }
    }

    public void AddToMoveCounter()
    {
        moveCounter++;
        CheckMoveLimit();
    }

    private void CheckMoveLimit()
    {
        //movecounter hits movelimit and hardmode is off
        if (moveCounter % moveLimit == 0 && hardModeOn == 0)
        {
            AddBlockerToBoard();
        }
    }

    public void CheckIfBoardFull()
    {
        int emptyCounter = 0;
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number == 0)
            {
                emptyCounter++;
                //Debug.Log("not game over");
                break;
            }
        }
        if (emptyCounter == 0)
        {
            GameOver();
        }
    }

    private void ScaleSquaresBG()
    {
        gameboard_bg.GetComponent<GameBoardBGMovement>().Shrink();
        //gameboard_bg.SetActive(false);
    }

    private void ScaleSquares()
    {
        Hashtable hash = new Hashtable();
        hash.Add("scale", new Vector3(2.4f, 2.4f, 2.615938f));
        hash.Add("easetype", "easeInQuad");
        hash.Add("oncomplete", "FixGameboardScale");
        hash.Add("oncompletetarget", gameObject);
        hash.Add("time", shakeTime);
        iTween.ScaleTo(gameboardScaleGroup, hash);
    }

    private void SwitchSquaresToFakeMaterial()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().SwitchToFakeMaterials();
        }
    }


    private void ShakeBoard()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().ShakeSquare();
        }
    }

    IEnumerator ExplosionEffects()
    {
        Instantiate(gameOverEffect, gameOverEffectPosition1 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect2, gameOverEffectPosition1 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        //.SM.PlayOneShotSound("fireworks");
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect2, gameOverEffectPosition2 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect, gameOverEffectPosition2 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        //SoundManager.SM.PlayOneShotSound("fireworks");
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect2, gameOverEffectPosition3 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect, gameOverEffectPosition3 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        //SoundManager.SM.PlayOneShotSound("fireworks");
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect2, gameOverEffectPosition4 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect, gameOverEffectPosition4 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        //SoundManager.SM.PlayOneShotSound("fireworks");
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect, gameOverEffectPosition5 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect2, gameOverEffectPosition5 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        //SoundManager.SM.PlayOneShotSound("fireworks");
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect2, gameOverEffectPosition6 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect, gameOverEffectPosition6 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        //SoundManager.SM.PlayOneShotSound("fireworks");
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect2, gameOverEffectPosition7 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect, gameOverEffectPosition7 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        //SoundManager.SM.PlayOneShotSound("fireworks");
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect2, gameOverEffectPosition8 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect, gameOverEffectPosition8 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform);
        //SoundManager.SM.PlayOneShotSound("fireworks");
        yield return new WaitForSeconds(.125f);

    }

    private void FixGameboardScale()
    {
        gameboardScaleGroup.transform.localScale = gameboardOrgScale;
    }

    private void GetUIStartPositions()
    {
        resetButton.GetStartPosition();
        settingsButton.GetStartPosition();
        iconsPanel.GetStartPosition();
        nextBoardPanel.GetStartPosition();
        nextbgPanel.GetStartPosition();
        scoreboardPanel.GetStartPosition();
        bestScoreboardPanel.GetStartPosition();
    }

    private void MoveUIOffScreen()
    {
        resetButton.MoveOffScreen();
        settingsButton.MoveOffScreen();
        iconsPanel.MoveOffScreen();
        nextBoardPanel.MoveOffScreen();
        nextbgPanel.MoveOffScreen();
        scoreboardPanel.MoveOffScreen();
        bestScoreboardPanel.MoveOffScreen();
    }

    public void AddBlockerToBoard()
    {
        ReloadEmptySquares();
        int randomSquareIndex = UnityEngine.Random.Range(0, emptySquares.Count);

        //List<GameObject> emptySquares_randomOrder = RandomizeEmptySquares(emptySquares);
        if (emptySquares.Count != 0)
        {
            //CreateBlockerAt(emptySquares_randomOrder[0]);
            //StartCoroutine(EjectSquare(emptySquares_randomOrder[0]));
            CreateBlockerAt(emptySquares[randomSquareIndex]);
            StartCoroutine(EjectSquare(emptySquares[randomSquareIndex]));
        }
        else
        {
            GameOver();
        }
    }

    IEnumerator EjectSquare(GameObject square)
    {
        float waitTime = 0f;
        if (playSquareClearAnimation)
        {
            waitTime += boardScoreSquareClearDuration;
        }
        yield return new WaitForSeconds(waitTime);
        square.GetComponent<SquareMechanics_Gameboard>().SetSquareDisplay();
        UpdateSquareConnections();
    }

    private void CreateBlockerAt(GameObject square)
    {
        blockerSquares.Add(square);
        square.GetComponent<SquareMechanics_Gameboard>().number = 5;
    }

    private List<GameObject> RandomizeEmptySquares(List<GameObject> squares)
    {
        for (int i = 0; i < squares.Count; i++)
        {
            GameObject tempGameObject = squares[i];
            int randomIndex = UnityEngine.Random.Range(0, squares.Count);
            squares[i] = squares[randomIndex];
            squares[randomIndex] = tempGameObject;
        }
        return squares;
    }

    private void ScoreDisplayFloatingText(int number)
    {
        scoreboard.ScoreboardAdd(number);
    }

    private void AddToScore(GameObject square) {
        score = score + square.GetComponent<SquareMechanics_Gameboard>().number;
    }
    private void AddToTotalPoints(GameObject square) {
        totalPoints = totalPoints + square.GetComponent<SquareMechanics_Gameboard>().number;
    }

    private void AddToScore(int number) {
        score = score + number;
    }
    private void AddToTotalPoints(int number) {
        totalPoints = totalPoints + number;
    }

    private void AddSquareToCompletedList(GameObject square)
    {
        completeList.Add(square);

        if (completedListPass)
        {
            for (int i = 0; i < square.GetComponent<SquareMechanics_Gameboard>().adjescentConnections.Count; i++)
            {
                if (square.GetComponent<SquareMechanics_Gameboard>().adjescentConnections[i] == true)
                {
                    if (square.GetComponent<SquareMechanics_Gameboard>().adjescentSquares[i].completed == true)
                    {
                        if (!completeList.Contains(square.GetComponent<SquareMechanics_Gameboard>().adjescentSquares[i].gameObject))
                        {
                            AddSquareToCompletedList(square.GetComponent<SquareMechanics_Gameboard>().adjescentSquares[i].gameObject);
                        }
                    }
                    else
                    {
                        completedListPass = false;
                    }
                }
            }
        }

    }


    private void SetGameBoardSquareInfo()
    {

        for (int i = 0; i < gameBoardSquares.Count; i++)
        {

            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().gamePositionIndex = i;
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().gamePositionX = i / gameBoardHeight;
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().gamePositionY = i % gameBoardHeight;

        }

        for (int i = 0; i < gameBoardSquares.Count; i++)
        {

            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().SetAdjescentSquares(gameObject.GetComponent<GameBoardMechanics>());

        }
    }

    private void SetUIOnScreen()
    {
        resetButton.MoveOnScreen();
        settingsButton.MoveOnScreen();
        iconsPanel.MoveOnScreen();
        nextBoardPanel.MoveOnScreen();
        nextbgPanel.MoveOnScreen();
        scoreboardPanel.MoveOnScreen();
        bestScoreboardPanel.MoveOnScreen();
    }

    private void MakeSquaresFloat()
    {
        int counter = 0;
        foreach (GameObject square in gameBoardSquares)
        {
            square.GetComponent<SquareMechanics_Gameboard>().connection_group.SetActive(false);
            square.GetComponent<SquareMechanics_Gameboard>().floatingGRP.GetComponent<FloatingSquare>().FloatBurst();

            if (counter % 2 == 0)
            {
                Instantiate(smokeEffect, square.transform.position, Quaternion.identity, gameObject.transform);
            }

        }
    }

    public void ClearBlockers()
    {
        for (int i = 0; i < blockerSquares.Count; i++)
        {
            blockerSquares[i].GetComponent<SquareMechanics_Gameboard>().ResetSquare_BlockerClear();
        }
        blockerSquares.Clear();
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearBlockers");

        UpdateSquareConnections();
        DimSleepingSquares();

        SaveBoardState();
    }

    public void UpdateSquareConnections()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            if (gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number != 0 && gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number != 5)
            {
                gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().ConnectionDisplay();
            }
        }
    }

    public void ResetLinkFromBoard()
    {
        List<GameObject> squares = new List<GameObject>(completeList);

        touchEnabled = false;
        int floatingTextNumber = 0;

        //string debugstring = "";
        for (int i = 0; i < squares.Count; i++)
        {
            //debugstring += squares[i].name+",";
            floatingTextNumber += squares[i].GetComponent<SquareMechanics_Gameboard>().number;
            AddToScore(squares[i]);
            AddToTotalPoints(squares[i]);
            squares[i].GetComponent<SquareMechanics_Gameboard>().ResetSquare_OnCompletion_Before();

        }
        //Debug.LogWarning(debugstring);

        //double score if over 34
        if(floatingTextNumber >= 36) {
            AddToScore(floatingTextNumber);
            AddToTotalPoints(floatingTextNumber);
            floatingTextNumber *= 2;
        }

        HighScoreCheck();
        ReduceMoveLimit();
        UpdateSquareConnections();

        StartCoroutine(ClearAnimation(floatingTextNumber, squares));
    }

    IEnumerator ClearAnimation(int number, List<GameObject> squares)
    {

        playSquareClearAnimation = true;
        
        for (int i = 0; i < squares.Count; i++)
        {
            Hashtable hash = new Hashtable();
            hash.Add("scale", new Vector3(1.08f, 1.08f, 1.08f));
            hash.Add("time", boardScoreSquareClearDuration);
            hash.Add("easetype", "easeOutCubic");
            iTween.ScaleTo(squares[i].GetComponent<SquareMechanics_Gameboard>().floatingGRP, hash);
        }
        //Debug.Log("test1"+squares[0].name);
        yield return new WaitForSeconds(boardScoreSquareClearDuration);
        //Debug.Log("test2" + squares[0].name);

        for (int i = 0; i < squares.Count; i++)
        {
            squares[i].GetComponent<SquareMechanics_Gameboard>().ResetSquare_OnCompletion_After();
        }

        UpdateClearsTotal();
        ClearSFX(number);
        UpdateScoreDiplay();
        UpdateHighScoreDisplay();
        ScoreDisplayFloatingText(number);

        if (playHighScoreAnimation)
        {
            playHighScoreAnimation = false;
            StartCoroutine(NewHighScoreAnimation());
        }
        touchEnabled = true;

        playSquareClearAnimation = false;

        //Debug.LogWarning("undimnow");
        DimSleepingSquares();
    }

    private void ReduceMoveLimit()
    {
        moveLimit = moveLimitStart - ((int)(score / reduceMoveLimitEveryPts));

        if (moveLimit < moveLimitMin)
        {
            moveLimit = moveLimitMin;
        }
    }

    private void UpdateClearsTotal()
    {
        if (hardModeOn != 1)
        {
            int clearScore = GetClearScore();
            if (score >= clearScore)
            {
                CalculateClearCounter();
                clearBlockerButton.GetComponent<BoardClearCommand>().UpdateClearsTotal(1);
                UpdateClearsTotal();
                SaveBoardState();
            }
        }
    }

    private void CalculateClearCounter()
    {
        if (!firstClear)
        {
            firstClear = true;
            GameDataManager.GDM.firstClear = true;
        }
        else
        {
            clearCounter++;
            if (score >= incrementClearEveryPtsAfter) {
                //Debug.LogWarning(score + " increase clear increment multiplier");
                clearIncrementMultiplier++;
            }
        }
    }

    public int GetClearScore()
    {
        if (!firstClear)
        {
            //Debug.LogWarning("Target: " + firstClearPts);
            return firstClearPts;
        }
        else
        {
            //Debug.LogWarning("Current: " + ((clearCounter * clearsEveryPts) + firstClearPts + (clearIncrementMultiplier * incrementClearEveryPtsBy)));
            //Debug.LogWarning("Target: " + ((clearCounter * clearsEveryPts) + firstClearPts + (clearIncrementMultiplier * incrementClearEveryPtsBy)));
            return (clearCounter * clearsEveryPts) + firstClearPts + (clearIncrementMultiplier * incrementClearEveryPtsBy);
        }
    }

    public int GetLastClearScore() {
        if (!firstClear) {
            return 0;
        }
        else {
            //Debug.LogWarning("Last: "+ (((clearCounter - 1) * clearsEveryPts) + firstClearPts + (GetLastIncrementMultiplier() * incrementClearEveryPtsBy)));
            return ((clearCounter-1) * clearsEveryPts) + firstClearPts + (GetLastIncrementMultiplier() * incrementClearEveryPtsBy);
        }
    }

    private int GetLastIncrementMultiplier() {
        if(clearIncrementMultiplier == 0) {
            return 0;
        }
        else {
            return clearIncrementMultiplier - 1;
        }
    }

    public int GetClearsEveryPoints() {
        return clearsEveryPts;
    }



    #endregion

    #region Printing
    private void PrintCompletedLink()
    {
        for (int i = 0; i < completeList.Count; i++)
        {
            Debug.Log(completeList[i].name + ": " + completeList[i].GetComponent<SquareMechanics_Gameboard>().number);
        }
    }

    private void PrintSquarePositions()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().PrintPosition();
        }
    }

    #endregion

}

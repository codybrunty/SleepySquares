using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using CloudOnce;

public class GameBoardMechanics : MonoBehaviour
{

    [Header("GameBoard Attributes")]
    public int totalPoints = 0;
    public int highScore = 0;
    public int score = 0;
    public int luckyCoins = 1;
    public int hardModeOn = 0;

    [Header("Clear Settings")]
    [SerializeField] private int clearScore;
    public bool firstClear = false;
    public int firstClearPts = 50;
    public int clearsEveryPts = 100;
    public int incrementClearEveryPtsBy = 5;
    public int incrementAfterClears= 3;
    public int clearIncrementMultiplier = 0;
    public int clearIncrementMultiplierMax = 10;
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
    public List<SquareMechanics_Gameboard> gameBoardSquaresMechanics = new List<SquareMechanics_Gameboard>();
    public List<FloatingSquare> gameBoardSquaresFloatGRPS = new List<FloatingSquare>();
    public List<SquareMechanics_Gameboard> blockerSquares = new List<SquareMechanics_Gameboard>();
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
    private List<SquareMechanics_Gameboard> emptySquaresMechanics = new List<SquareMechanics_Gameboard>();
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
    public int doublePointsThreshold_normal = 36;
    public int doublePointsThreshold_hard = 51;

    private BoardClearCommand boardClearCommand;
    private GameBoardBGMovement gbMovement;
    public bool DailyModeOn = false;

    public int dailyDesignIndex;
    public List<DailyDesign> dailyDesigns = new List<DailyDesign>();
    public DailyManager m_oDailyManager;
    private List<int> dailyGoalScores = new List<int> { 150, 175, 200, 225, 250, 275, 300 };
    //private List<int> dailyGoalScores = new List<int> { 50, 50, 50, 50, 50, 50, 50 };
    public int dailyGoalScoreIndex;
    public BestScoreIcon m_oBestScoreIcon;
    public TargetReward m_oTargetReward;

    #region Start
    private void Awake()
    {
        boardClearCommand = clearBlockerButton.GetComponent<BoardClearCommand>();
        gbMovement = gameboard_bg.GetComponent<GameBoardBGMovement>();
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
    }

    #endregion

    #region Reset Board State
    public void ResetBoardState()
    {
        Debug.Log("Reset Board State");
        SetUIOnScreen();
        gbMovement.TrueSize();
        TurnOffAllSquareZzz();
        ZeroOutAllSquares();

        blockerSquares.Clear();
        boardClearCommand.clearsTotal = 0;
        clearCounter = 1;
        clearIncrementMultiplier = 0;
        firstClear = false;
        boardClearCommand.UpdateClearDisplay();
        boardClearCommand.UpdateClearFill();

        switchButton.switchAmmount = GameDataManager.GDM.currentSwitches;
        switchButton.UpdateSwitchAmmountDisplay();

        hardModeOn = GameDataManager.GDM.hardModeOn;
        hardText.UpdateHardText();
        score = 0;
        UpdateScoreDiplay();
        GetHighScore();
        UpdateHighScoreDisplay();
        NewHighScoreCheck();

        totalPoints = GameDataManager.GDM.TotalPoints_AllTime;
        ReduceMoveLimit();
        moveCounter = 0;

        nextBoard.ResetNextBoard();

        gameOver = false;

        DimSleepingSquares();

        SaveBoardState();
    }

    public IEnumerator EnableTouch(float num)
    {
        Debug.Log("enable touch");
        yield return new WaitForSeconds(num);
        touchEnabled = true;
    }
    #endregion

    #region Set Board State

    public void SetDailyBoard() {
        DailyModeOn = true;
        PlayerPrefs.SetInt("DailyReminder", 1);
        GetPuzzleFromDailySeed();
        GameDataManager.GDM.hardModeOn = 1;
        GameDataManager.GDM.SaveGameData();
        ResetBoardState();

        if (m_oDailyManager.hasHearts) {
            StartCoroutine(EnableTouch(.1f));
        }
        else {
            Debug.Log("touch disabled no hearts");
            touchEnabled = false;
        }
        

        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++) {
            gameBoardSquaresMechanics[i].number = dailyDesigns[dailyDesignIndex].designNumbers[i];
            gameBoardSquaresMechanics[i].adjescentConnections = new List<bool> { false,false,false,false};
            gameBoardSquaresMechanics[i].luckyCoin = false;
            gameBoardSquaresMechanics[i].completed = false;
            gameBoardSquaresMechanics[i].blocker = false;

            if (dailyDesigns[dailyDesignIndex].designNumbers[i] == 5) {
                gameBoardSquaresMechanics[i].completed = true;
                gameBoardSquaresMechanics[i].blocker = true;
            }


            if (gameBoardSquaresMechanics[i].number != 0) {
                gameBoardSquaresMechanics[i].SilentSquareDisplay();
            }
        }

    }

    private void GetPuzzleFromDailySeed() {

        Debug.Log("Getting The Saved Daily Puzzle Seed");
        int savedSeed = PlayerPrefs.GetInt("DailySeedNumber", 0);
        Debug.Log("Getting The Current Daily Puzzle Seed");
        int currentSeed = TimeManager.TM.GetDateInt();

        if (savedSeed != currentSeed) {
            Debug.Log("Selecting New Puzzle From Current Seed");
            savedSeed = currentSeed;
            
            //new day reset player prefs
            PlayerPrefs.SetInt("DailySeedNumber", savedSeed);
            PlayerPrefs.SetInt("DailyHearts", 3);
            PlayerPrefs.SetInt("DailyTargetHit",0);
            PlayerPrefs.SetInt("DailyHighScore", 0);
            PlayerPrefs.SetInt("DailyReminder", 0);

            SetRandomDailyDesignIndex(savedSeed);
        }
        else {
            Debug.Log("Dailies From Saved Seed");
            dailyDesignIndex = PlayerPrefs.GetInt("DailyDesignIndex", 0);
            dailyGoalScoreIndex = PlayerPrefs.GetInt("DailyGoalScoresIndex", 0);
        }
        
        Debug.Log("Dailiy Design: " + dailyDesignIndex);
    }

    private void SetRandomDailyDesignIndex(int seed) {
        UnityEngine.Random.InitState(seed);

        dailyDesignIndex = UnityEngine.Random.Range(0, dailyDesigns.Count);
        PlayerPrefs.SetInt("DailyDesignIndex", dailyDesignIndex);

        dailyGoalScoreIndex = UnityEngine.Random.Range(0, dailyGoalScores.Count);
        PlayerPrefs.SetInt("DailyGoalScoresIndex", dailyGoalScoreIndex);

        UnityEngine.Random.InitState(System.Environment.TickCount);
    }

    private void SetNormalBoard()
    {
        m_oDailyManager.HideContinueOptions();
        StartCoroutine(EnableTouch(.1f));
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
            for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
            {
                gameBoardSquaresMechanics[i].ZeroOutSquareInfo();
            }


            for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
            {
                gameBoardSquaresMechanics[i].number = GameDataManager.GDM.squares[i].number;
                gameBoardSquaresMechanics[i].completed = GameDataManager.GDM.squares[i].completed;
                gameBoardSquaresMechanics[i].blocker = GameDataManager.GDM.squares[i].blocker;
                gameBoardSquaresMechanics[i].adjescentConnections = GameDataManager.GDM.squares[i].adjescentConnections;
                gameBoardSquaresMechanics[i].luckyCoin = false;

                if (gameBoardSquaresMechanics[i].number != 0)
                {
                    gameBoardSquaresMechanics[i].SilentSquareDisplay();
                }
            }

            SetBlockerSquaresList();

            firstClear = GameDataManager.GDM.firstClear;
            boardClearCommand.clearsTotal = GameDataManager.GDM.currentClears;
            clearCounter = GameDataManager.GDM.currentClearCounter;
            clearIncrementMultiplier = GameDataManager.GDM.clearIncrementMultiplier;
            boardClearCommand.UpdateClearDisplay();
            boardClearCommand.UpdateClearFill();

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
        m_oDailyManager.HideContinueOptions();
        StartCoroutine(EnableTouch(.1f));
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
            for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
            {
                gameBoardSquaresMechanics[i].ZeroOutSquareInfo();
            }


            for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
            {
                gameBoardSquaresMechanics[i].number = GameDataManager.GDM.HM_squares[i].number;
                gameBoardSquaresMechanics[i].completed = GameDataManager.GDM.HM_squares[i].completed;
                gameBoardSquaresMechanics[i].blocker = GameDataManager.GDM.HM_squares[i].blocker;
                gameBoardSquaresMechanics[i].adjescentConnections = GameDataManager.GDM.HM_squares[i].adjescentConnections;
                gameBoardSquaresMechanics[i].luckyCoin = false;

                if (gameBoardSquaresMechanics[i].number != 0)
                {
                    gameBoardSquaresMechanics[i].SilentSquareDisplay();
                }
            }

            SetBlockerSquaresList();

            firstClear = false;
            boardClearCommand.clearsTotal = 0;
            clearCounter = 0;
            clearIncrementMultiplier = 0;
            boardClearCommand.UpdateClearDisplay();
            boardClearCommand.UpdateClearFill();

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

    public void SetBoardState() {
        DailyModeOn = false;
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
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            GameDataManager.GDM.squares[i].number = gameBoardSquaresMechanics[i].number;
            GameDataManager.GDM.squares[i].completed = gameBoardSquaresMechanics[i].completed;
            GameDataManager.GDM.squares[i].blocker = gameBoardSquaresMechanics[i].blocker;
            GameDataManager.GDM.squares[i].adjescentConnections = gameBoardSquaresMechanics[i].adjescentConnections;
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
        GameDataManager.GDM.currentClears = boardClearCommand.clearsTotal;
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
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            GameDataManager.GDM.HM_squares[i].number = gameBoardSquaresMechanics[i].number;
            GameDataManager.GDM.HM_squares[i].completed = gameBoardSquaresMechanics[i].completed;
            GameDataManager.GDM.HM_squares[i].blocker = gameBoardSquaresMechanics[i].blocker;
            GameDataManager.GDM.HM_squares[i].adjescentConnections = gameBoardSquaresMechanics[i].adjescentConnections;
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
        if (DailyModeOn) {
            GameDataManager.GDM.currentSwitches = switchButton.switchAmmount;
            GameDataManager.GDM.SaveGameData();
        }
        else {
            Debug.Log("save board state");
            hardModeOn = GameDataManager.GDM.hardModeOn;
            if (hardModeOn == 1) {
                SaveHardBoard();
            }
            else {
                SaveNormalBoard();
            }
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

            //TestPrintClearEveryPts();

            GameOverCounter();
            SaveBoardState();
        }
    }

    private void TestPrintClearEveryPts() {
        Debug.LogWarning("Lost on clear amount: "+(clearsEveryPts + (incrementClearEveryPtsBy * clearIncrementMultiplier)));
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
        if (DailyModeOn) {
            yield return new WaitForSeconds(.1f);
            LoseHeart();
            yield return new WaitForSeconds(.5f);
        }

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

    private void LoseHeart() {
        m_oDailyManager.LoseOneHeart();
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

        if (DailyModeOn) {
            Debug.Log("Posted to Daily Leaderboard");
            //Leaderboards.DailyHighScore.SubmitScore(scoreToPost, callbackCheck);
        }
        else {
            if (hardModeOn == 1) {
                Debug.Log("Posted to 4 Eyed Leaderboard");
                //Leaderboards.HardModeHighScore.SubmitScore(scoreToPost, callbackCheck);
            }
            else {
                Debug.Log("Posted to 3 Eyed Leaderboard");
                //Leaderboards.HighScore.SubmitScore(scoreToPost, callbackCheck);
            }

        }

    }
    /*
    private void callbackCheck(CloudRequestResult<bool> result)
    {
        if (result.Result == false)
        {
            Debug.Log(result.Error);
        }
    }
    */
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
        if (DailyModeOn) {
            DailyModeNewHighScore();
        }

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

    private void DailyModeNewHighScore() {
        int targetHit = PlayerPrefs.GetInt("DailyTargetHit", 0);
        if (targetHit == 0) {
            PlayerPrefs.SetInt("DailyTargetHit", 1);
            m_oBestScoreIcon.EnableCrown();
            m_oTargetReward.TargetHitRevealAnimation();

            //for playfab tracking
            int counter = PlayerPrefs.GetInt("Daily_Complete", 0);
            counter++;
            PlayerPrefs.SetInt("Daily_Complete", counter);
        }
        PlayerPrefs.SetInt("DailyHighScore", score);
    }


    private void UpdateScoreDiplay()
    {
        Debug.Log("score updated to " + score);
        score_text.text = score.ToString();
        if (hardModeOn != 1)
        {
            boardClearCommand.UpdateClearFill();
        }
    }

    private void UpdateHighScoreDisplay()
    {
        highScore_text.text = highScore.ToString();

        if (DailyModeOn) {
            int targetHit = PlayerPrefs.GetInt("DailyTargetHit", 0);
            if (targetHit == 1) {
                //switch arrow to crown
                m_oBestScoreIcon.EnableCrown();
            }
        }
    }



    private void ClearSFX(int clearNumber) {
        if (clearNumber <= 15) {
            SoundManager.SM.PlayOneShotSound("clearboard1");
        }
        else if (clearNumber > 15 && clearNumber <= 35) {
            SoundManager.SM.PlayOneShotSound("clearboard2");
        }
        else {
            SoundManager.SM.PlayOneShotSound("clearboard3");
            popArt.WordArtAnimation(clearNumber);
        }

    }

    private void ClearSFX(int clearNumber, bool dontPlayArt) {
        if (clearNumber <= 15) {
            SoundManager.SM.PlayOneShotSound("clearboard1");
        }
        else if (clearNumber > 15 && clearNumber <= 35) {
            SoundManager.SM.PlayOneShotSound("clearboard2");
        }
        else {
            SoundManager.SM.PlayOneShotSound("clearboard3");
        }

    }

    #endregion

    #region Hard Mode
    public void TurnOnHardMode()
    {
        m_oDailyManager.CheckIfDailyStarted();
        DailyModeOn = false;
        Debug.Log("Turn Hard Mode On");
        TimeManager.TM.StopTimer();
        //SoundManager.SM.PlayOneShotSound("select1");
        GameDataManager.GDM.SaveGameData();
        hardModeOn = 1;
        GameDataManager.GDM.hardModeOn = hardModeOn;
        hardText.UpdateHardText();
        resetButton.ResetHardModeSwitch();
        boardClearCommand.UpdateClearFill();
        RemoveAnyLuckyCoinsFromBoard();
        GameDataManager.GDM.SaveGameData();
    }

    public void TurnOffHardMode() {
        m_oDailyManager.CheckIfDailyStarted();
        DailyModeOn = false;
        Debug.Log("Turn Hard Mode Off");
        TimeManager.TM.StopTimer();
        //SoundManager.SM.PlayOneShotSound("select1");
        GameDataManager.GDM.SaveGameData();
        hardModeOn = 0;
        GameDataManager.GDM.hardModeOn = hardModeOn;
        hardText.UpdateHardText();
        resetButton.ResetHardModeSwitch();
        boardClearCommand.UpdateClearFill();
        RemoveAnyLuckyCoinsFromBoard();
        GameDataManager.GDM.SaveGameData();
    }
    #endregion

    #region Miscellaneous
    private void DimSleepingSquares()
    {
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            gameBoardSquaresMechanics[i].CheckDimIfCompleted();
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
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            gameBoardSquaresMechanics[i].ZeroOutSquareInfo();
        }
    }

    private void TurnOffAllSquareZzz()
    {
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            gameBoardSquaresMechanics[i].StopZzz();
        }
    }

    private void GetHighScore()
    {
        if (DailyModeOn) {
            int dailyTargetHit = PlayerPrefs.GetInt("DailyTargetHit", 0);
            
            //daily already hit true
            if(dailyTargetHit == 1) {
                highScore = PlayerPrefs.GetInt("DailyHighScore", 0);
            }
            else {
                highScore = dailyGoalScores[dailyGoalScoreIndex];
            }
        }
        else { 
            if (hardModeOn == 1) {
                highScore = GameDataManager.GDM.HardModeHighScore_AllTime;
            }
            else {
                highScore = GameDataManager.GDM.HighScore_AllTime;
            }
        }
    }

    private void SaveHighScore()
    {
        if (DailyModeOn) {
            PlayerPrefs.SetInt("DailyHighScore", highScore);
        }
        else {
            if (hardModeOn == 1) {
                GameDataManager.GDM.HardModeHighScore_AllTime = highScore;
                Debug.Log("HardMode HighScore Updated");
            }
            else {
                GameDataManager.GDM.HighScore_AllTime = highScore;
                Debug.Log("Regular HighScore Updated");
            }
            GameDataManager.GDM.SaveGameData();
        }


    }

    private void SetBlockerSquaresList()
    {
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            if (gameBoardSquaresMechanics[i].blocker == true)
            {
                blockerSquares.Add(gameBoardSquaresMechanics[i]);
            }
        }
    }

    private void CameraSetup()
    {
        float normalAspect = 1152f / 2048f;
        float currentAspect = ((float)Camera.main.pixelWidth / Camera.main.pixelHeight);
        float newOrtho = 16.77f * (normalAspect / currentAspect);


        float testAspect = ((float)Camera.main.pixelHeight / Camera.main.pixelWidth);
        if (testAspect > 1.7)
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
                gameBoardSquaresMechanics[i].SquareDown();
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
            if (emptySquaresMechanics.Count > 0) {
                int randomSquareIndex = UnityEngine.Random.Range(0, emptySquaresMechanics.Count);
                emptySquaresMechanics[randomSquareIndex].luckyCoin = true;
                emptySquaresMechanics[randomSquareIndex].SetLuckyColor();
            }
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

    private void ReloadEmptySquares() {
        emptySquares.Clear();
        emptySquaresMechanics.Clear();

        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            if (gameBoardSquaresMechanics[i].number == 0) {
                emptySquares.Add(gameBoardSquares[i]);
                emptySquaresMechanics.Add(gameBoardSquaresMechanics[i]);
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

        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            gameBoardSquaresMechanics[i].luckyCoin = false;
            gameBoardSquaresMechanics[i].SetLuckyColor();
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
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            if (gameBoardSquaresMechanics[i].number == 0)
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
        gbMovement.Shrink();
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
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            gameBoardSquaresMechanics[i].SwitchToFakeMaterials();
        }
    }


    private void ShakeBoard()
    {
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            gameBoardSquaresMechanics[i].ShakeSquare();
        }
    }

    IEnumerator ExplosionEffects()
    {
        GameObject effect1 = Instantiate(gameOverEffect2, gameOverEffectPosition1 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect1.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        GameObject effect2 = Instantiate(gameOverEffect2, gameOverEffectPosition1 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect2.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return new WaitForSeconds(.125f);
        GameObject effect3 = Instantiate(gameOverEffect2, gameOverEffectPosition2 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect3.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        GameObject effect4 = Instantiate(gameOverEffect2, gameOverEffectPosition2 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect4.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return new WaitForSeconds(.125f);
        GameObject effect5 = Instantiate(gameOverEffect2, gameOverEffectPosition3 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect5.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        GameObject effect6 = Instantiate(gameOverEffect2, gameOverEffectPosition3 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect6.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return new WaitForSeconds(.125f);
        GameObject effect7 = Instantiate(gameOverEffect2, gameOverEffectPosition4 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect7.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        GameObject effect8 = Instantiate(gameOverEffect2, gameOverEffectPosition4 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect8.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return new WaitForSeconds(.125f);
        GameObject effect9 = Instantiate(gameOverEffect2, gameOverEffectPosition5 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect9.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        GameObject effect10 = Instantiate(gameOverEffect2, gameOverEffectPosition5 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect10.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return new WaitForSeconds(.125f);
        GameObject effect11 = Instantiate(gameOverEffect2, gameOverEffectPosition6 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect11.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        GameObject effect12 = Instantiate(gameOverEffect2, gameOverEffectPosition6 + new Vector3(1, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
        effect12.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
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
        int randomSquareIndex = UnityEngine.Random.Range(0, emptySquaresMechanics.Count);
        
        if (emptySquaresMechanics.Count != 0)
        {
            CreateBlockerAt(emptySquaresMechanics[randomSquareIndex]);
            StartCoroutine(EjectSquare(emptySquaresMechanics[randomSquareIndex]));
        }
        else
        {
            GameOver();
        }
    }

    IEnumerator EjectSquare(SquareMechanics_Gameboard squareMechanics)
    {
        float waitTime = 0f;
        if (playSquareClearAnimation)
        {
            waitTime += boardScoreSquareClearDuration;
        }
        yield return new WaitForSeconds(waitTime);
        squareMechanics.SetSquareDisplay();
        UpdateSquareConnections();
    }

    private void CreateBlockerAt(SquareMechanics_Gameboard squareMechanics)
    {
        blockerSquares.Add(squareMechanics);
        squareMechanics.number = 5;
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

    private void AddToScore(int number) {
        score = score + number;
    }
    private void AddToTotalPoints(int number) {
        totalPoints = totalPoints + number;
    }

    private void AddSquareToCompletedList(GameObject square)
    {
        SquareMechanics_Gameboard squareMechanics = square.GetComponent<SquareMechanics_Gameboard>();
        completeList.Add(square);

        if (completedListPass)
        {
            for (int i = 0; i < squareMechanics.adjescentConnections.Count; i++)
            {
                if (squareMechanics.adjescentConnections[i] == true)
                {
                    if (squareMechanics.adjescentSquares[i].completed == true)
                    {
                        if (!completeList.Contains(squareMechanics.adjescentSquares[i].gameObject))
                        {
                            AddSquareToCompletedList(squareMechanics.adjescentSquares[i].gameObject);
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

        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            gameBoardSquaresMechanics[i].gamePositionIndex = i;
            gameBoardSquaresMechanics[i].gamePositionX = i / gameBoardHeight;
            gameBoardSquaresMechanics[i].gamePositionY = i % gameBoardHeight;
        }

        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            gameBoardSquaresMechanics[i].SetAdjescentSquares();
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
        foreach (SquareMechanics_Gameboard squareMechanics in gameBoardSquaresMechanics) {
            squareMechanics.connection_group.SetActive(false);
        }
        foreach (FloatingSquare floatSquare in gameBoardSquaresFloatGRPS) {
            floatSquare.FloatBurst();
        }
        foreach (GameObject square in gameBoardSquares) {
            Instantiate(smokeEffect, square.transform.position, Quaternion.identity, gameObject.transform);
        }
    }

    public void ClearBlockers()
    {
        for (int i = 0; i < blockerSquares.Count; i++)
        {
            blockerSquares[i].ResetSquare_BlockerClear();
        }
        blockerSquares.Clear();
        SoundManager.SM.PlayOneShotSound("clearBlockers");

        UpdateSquareConnections();
        DimSleepingSquares();

        SaveBoardState();
    }

    public void UpdateSquareConnections()
    {
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            if (gameBoardSquaresMechanics[i].number != 0 && gameBoardSquaresMechanics[i].number != 5)
            {
                gameBoardSquaresMechanics[i].ConnectionDisplay();
            }
        }
    }

    public void ResetLinkFromBoard()
    {
        List<GameObject> completedSquares = new List<GameObject>(completeList);

        touchEnabled = false;
        int floatingTextNumber = 0;
        
        for (int i = 0; i < completedSquares.Count; i++)
        {
            SquareMechanics_Gameboard squareMechanics = completedSquares[i].GetComponent<SquareMechanics_Gameboard>();
            floatingTextNumber += squareMechanics.number;
            AddToScore(squareMechanics.number);
            AddToTotalPoints(squareMechanics.number);
            squareMechanics.ResetSquare_OnCompletion_Before();
        }


        if (hardModeOn == 0) {
            if (floatingTextNumber >= doublePointsThreshold_normal) {
                AddToScore(floatingTextNumber);
                AddToTotalPoints(floatingTextNumber);
                floatingTextNumber *= 2;
            }
        }
        else {
            if (floatingTextNumber >= doublePointsThreshold_hard) {
                AddToScore(floatingTextNumber);
                AddToTotalPoints(floatingTextNumber);
                floatingTextNumber *= 2;
            }
        }


        HighScoreCheck();
        ReduceMoveLimit();
        UpdateSquareConnections();

        StartCoroutine(ClearAnimation(floatingTextNumber, completedSquares));
    }

    IEnumerator ClearAnimation(int number, List<GameObject> squares) {
        playSquareClearAnimation = true;
        List<SquareMechanics_Gameboard> squaresMechanics = new List<SquareMechanics_Gameboard>();
        for (int i = 0; i < squares.Count; i++) {
            squaresMechanics.Add(squares[i].GetComponent<SquareMechanics_Gameboard>());
        }
        
        for (int i = 0; i < squaresMechanics.Count; i++)
        {
            Hashtable hash = new Hashtable();
            hash.Add("scale", new Vector3(1.08f, 1.08f, 1.08f));
            hash.Add("time", boardScoreSquareClearDuration);
            hash.Add("easetype", "easeOutCubic");
            iTween.ScaleTo(squaresMechanics[i].floatingGRP, hash);
        }
        //Debug.Log("test1"+squares[0].name);
        yield return new WaitForSeconds(boardScoreSquareClearDuration);
        //Debug.Log("test2" + squares[0].name);

        for (int i = 0; i < squaresMechanics.Count; i++)
        {
            squaresMechanics[i].ResetSquare_OnCompletion_After();
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
            clearScore = GetClearScore();
            if (score >= clearScore)
            {
                CalculateClearCounter();
                boardClearCommand.UpdateClearsTotal(1);
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
            if (clearCounter > incrementAfterClears) {

                clearIncrementMultiplier++;
                if(clearIncrementMultiplier > clearIncrementMultiplierMax) {
                    clearIncrementMultiplier = clearIncrementMultiplierMax;
                }
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
        else {
            //Debug.LogWarning("multiplier: " + clearIncrementMultiplier);
            //Debug.LogWarning("incremental points: " + incrementClearEveryPtsBy);
            //Debug.LogWarning("(m+i): " + (clearIncrementMultiplier * incrementClearEveryPtsBy));

            //Debug.LogWarning("Target: " + ((clearCounter * clearsEveryPts) + firstClearPts + (clearIncrementMultiplier * incrementClearEveryPtsBy)));

            //old
            //return (clearCounter * clearsEveryPts) + firstClearPts + ((clearIncrementMultiplier * incrementClearEveryPtsBy) + GetMultiplierFactorialAmmount(clearIncrementMultiplier));
            //new
            return (clearCounter * clearsEveryPts) + firstClearPts + (GetMultiplierFactorialAmmount(clearIncrementMultiplier)+ GetMultiplierAtMaxPoints());
        }
    }

    public int GetLastClearScore() {
        if (!firstClear) {
            return 0;
        }
        else {
            //Debug.LogWarning("Last: "+ (((clearCounter - 1) * clearsEveryPts) + firstClearPts + (GetLastIncrementMultiplier() * incrementClearEveryPtsBy)));
            //old    
            //return ((clearCounter-1) * clearsEveryPts) + firstClearPts + ((GetLastIncrementMultiplier() * incrementClearEveryPtsBy)+ GetMultiplierFactorialAmmount(GetLastIncrementMultiplier()));
            //new
            return ((clearCounter - 1) * clearsEveryPts) + firstClearPts + (GetMultiplierFactorialAmmount(GetLastIncrementMultiplier())+ GetPreviousMultiplierAtMaxPoints());
        }
    }

    private int GetMultiplierAtMaxPoints() {
        if (clearIncrementMultiplier == clearIncrementMultiplierMax) {
           return (clearIncrementMultiplierMax * incrementClearEveryPtsBy) * (clearCounter - (clearIncrementMultiplier + incrementAfterClears));
        }
        else {
            return 0;
        }
    }

    private int GetPreviousMultiplierAtMaxPoints() {
        if (clearIncrementMultiplier == clearIncrementMultiplierMax) {
            return (clearIncrementMultiplierMax * incrementClearEveryPtsBy) * (clearCounter - 1 - (GetLastIncrementMultiplier() + incrementAfterClears));
        }
        else {
            return 0;
        }
    }

    private int GetMultiplierFactorialAmmount(int num) {
        if (num == 0) {
            return 0;
        }
        else if (num == 1) {
            return incrementClearEveryPtsBy;
        }
        else {
            return ((num* incrementClearEveryPtsBy)+(GetMultiplierFactorialAmmount(num - 1)));
        }
    }

    private int GetLastIncrementMultiplier() {
        if(clearIncrementMultiplier == 0) {
            return 0;
        }
        else {
            if (clearIncrementMultiplier == clearIncrementMultiplierMax) {
                if (clearIncrementMultiplierMax+incrementAfterClears == clearCounter) {
                    return clearIncrementMultiplier - 1;
                }
                else {
                    return clearIncrementMultiplier;
                }
            }
            else {
                return clearIncrementMultiplier - 1;
            }
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
        for (int i = 0; i < gameBoardSquaresMechanics.Count; i++)
        {
            gameBoardSquaresMechanics[i].PrintPosition();
        }
    }

    #endregion

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameboard : MonoBehaviour
{
    public int gameBoardWidth;
    public int gameBoardHeight;
    public bool touchEnabled = true;
    public List<GameObject> gameBoardSquares = new List<GameObject>();
    public bool eyePickingMode = false;


    private List<GameObject> completeList = new List<GameObject>();
    private bool completedListPass = true;

    public int score = 0;
    [SerializeField] TutorialScoreboard scoreboard = default;
    public int scoreGameOverTotal = 42;
    public int endingGameOverTotal = 77;
    [SerializeField] TutorialGoNext goNext = default;
    [SerializeField] GameObject gameboardScaleGroup = default;

    [SerializeField] GameObject gameOverEffect=default;
    [SerializeField] GameObject gameOverEffect2 = default;
    [SerializeField] GameObject gameOverEffect3 = default;
    [SerializeField] GameObject gameOverEffectPosition1GO = default;
    [SerializeField] GameObject gameOverEffectPosition2GO = default;
    [SerializeField] GameObject gameOverEffectPosition3GO = default;
    [SerializeField] GameObject gameOverEffectPosition4GO = default;
    private Vector3 gameOverEffectPosition1;
    private Vector3 gameOverEffectPosition2;
    private Vector3 gameOverEffectPosition3;
    private Vector3 gameOverEffectPosition4;

    [SerializeField] GameObject repair = default;
    [SerializeField] GameObject repairButton = default;
    [SerializeField] GameObject swapButton = default;
    [SerializeField] GameObject swap = default;

    [SerializeField] GameObject instructions = default;
    [SerializeField] GameObject instructions_eye=default;
    [SerializeField] GameObject instructions_repair = default;
    [SerializeField] GameObject instructions_repairDone = default;
    [SerializeField] GameObject instructions_swap1 = default;
    [SerializeField] GameObject instructions_fixboard2 = default;
    public GameObject instructions_ending1 = default;
    public GameObject instructions_ending2 = default;

    [SerializeField] TutorialNextBoard nextboard = default;

    public float shakeTime = 1f;
    public float shakeMaxMagnitude = .175f;

    private bool update1 = false;
    private bool update2 = false;
    public bool endingSeq = false;

    [SerializeField] Tutorial_Fill fill = default;
    [SerializeField] GameObject repairArrow = default;
    [SerializeField] GameObject swapArrow = default;
    [SerializeField] GameObject nextArrow = default;
    public bool playSquareClearAnimation = false;
    public float boardScoreSquareClearDuration = 0.5f;
    private Coroutine arrowCo;

    private void Start()
    {
        SetGameBoardSquareInfo();
        GetGameOverEffectPositions();
    }

    private void Update()
    {
        if (instructions_repairDone.GetComponent<TypewriterEffect>().go && update1 == false)
        {
            update1 = true;
            instructions_repairDone.SetActive(false);
            nextArrow.SetActive(false);
            StartCoroutine(SwapSequence());
        }

        if (instructions_swap1.activeSelf==true && update2 == false && instructions_swap1.GetComponent<TypewriterEffect>().index == 3)
        {
            update2 = true;
            MoveSwapButtonIntoPlace();
        }
    }

    private void MoveSwapButtonIntoPlace()
    {
        Vector3 endPosition = swap.transform.position;
        Vector3 startPosition = new Vector3(swap.transform.position.x, swap.transform.position.y - 100f, swap.transform.position.z);
        swap.transform.position = startPosition;
        swap.SetActive(true);
        iTween.MoveTo(swap, endPosition, 0.5f);
    }


    IEnumerator SwapSequence()
    {
        yield return new WaitForSeconds(.25f);
        PlaceSquare(gameBoardSquares[1], 1);
        yield return new WaitForSeconds(.25f);
        PlaceSquare(gameBoardSquares[4], 3);
        yield return new WaitForSeconds(.25f);
        PlaceSquare(gameBoardSquares[7], 1);
        yield return new WaitForSeconds(.25f);

        /*
        gameBoardSquares[0].GetComponent<Tutorial_Square>().ShakeSquare();
        gameBoardSquares[2].GetComponent<Tutorial_Square>().ShakeSquare();
        gameBoardSquares[3].GetComponent<Tutorial_Square>().ShakeSquare();
        gameBoardSquares[5].GetComponent<Tutorial_Square>().ShakeSquare();
        gameBoardSquares[6].GetComponent<Tutorial_Square>().ShakeSquare();
        gameBoardSquares[8].GetComponent<Tutorial_Square>().ShakeSquare();
        */

        //yield return new WaitForSeconds(shakeTime - .4f);

        gameBoardSquares[0].GetComponent<Tutorial_Square>().cube.GetComponent<MeshRenderer>().material = gameBoardSquares[0].GetComponent<Tutorial_Square>().materialColors_fake[4];
        gameBoardSquares[2].GetComponent<Tutorial_Square>().cube.GetComponent<MeshRenderer>().material = gameBoardSquares[0].GetComponent<Tutorial_Square>().materialColors_fake[4];
        gameBoardSquares[3].GetComponent<Tutorial_Square>().cube.GetComponent<MeshRenderer>().material = gameBoardSquares[0].GetComponent<Tutorial_Square>().materialColors_fake[4];
        gameBoardSquares[5].GetComponent<Tutorial_Square>().cube.GetComponent<MeshRenderer>().material = gameBoardSquares[0].GetComponent<Tutorial_Square>().materialColors_fake[4];
        gameBoardSquares[6].GetComponent<Tutorial_Square>().cube.GetComponent<MeshRenderer>().material = gameBoardSquares[0].GetComponent<Tutorial_Square>().materialColors_fake[4];
        gameBoardSquares[8].GetComponent<Tutorial_Square>().cube.GetComponent<MeshRenderer>().material = gameBoardSquares[0].GetComponent<Tutorial_Square>().materialColors_fake[4];

        yield return new WaitForSeconds(.15f);

        
        SoundManager.SM.PlayOneShotSound("poof");
        //Instantiate(gameOverEffect, gameBoardSquares[0].transform.position, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect3, gameBoardSquares[0].transform.position, Quaternion.identity, gameObject.transform);
        gameBoardSquares[0].GetComponent<Tutorial_Square>().completed = true;
        gameBoardSquares[0].GetComponent<Tutorial_Square>().blocker = true;
        gameBoardSquares[0].GetComponent<Tutorial_Square>().number = 5;
        gameBoardSquares[0].GetComponent<Tutorial_Square>().floatingGRP.GetComponent<FloatingSquare>().QuickBurst();
        yield return new WaitForSeconds(.15f);

        SoundManager.SM.PlayOneShotSound("poof");
        //Instantiate(gameOverEffect, gameBoardSquares[2].transform.position, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect3, gameBoardSquares[2].transform.position, Quaternion.identity, gameObject.transform);
        gameBoardSquares[2].GetComponent<Tutorial_Square>().completed = true;
        gameBoardSquares[2].GetComponent<Tutorial_Square>().blocker = true;
        gameBoardSquares[2].GetComponent<Tutorial_Square>().number = 5;
        gameBoardSquares[2].GetComponent<Tutorial_Square>().floatingGRP.GetComponent<FloatingSquare>().QuickBurst();
        yield return new WaitForSeconds(.15f);

        SoundManager.SM.PlayOneShotSound("poof");
        //Instantiate(gameOverEffect, gameBoardSquares[3].transform.position, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect3, gameBoardSquares[3].transform.position, Quaternion.identity, gameObject.transform);
        gameBoardSquares[3].GetComponent<Tutorial_Square>().completed = true;
        gameBoardSquares[3].GetComponent<Tutorial_Square>().blocker = true;
        gameBoardSquares[3].GetComponent<Tutorial_Square>().number = 5;
        gameBoardSquares[3].GetComponent<Tutorial_Square>().floatingGRP.GetComponent<FloatingSquare>().QuickBurst();
        yield return new WaitForSeconds(.15f);

        SoundManager.SM.PlayOneShotSound("poof");
        //Instantiate(gameOverEffect, gameBoardSquares[5].transform.position, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect3, gameBoardSquares[5].transform.position, Quaternion.identity, gameObject.transform);
        gameBoardSquares[5].GetComponent<Tutorial_Square>().completed = true;
        gameBoardSquares[5].GetComponent<Tutorial_Square>().blocker = true;
        gameBoardSquares[5].GetComponent<Tutorial_Square>().number = 5;
        gameBoardSquares[5].GetComponent<Tutorial_Square>().floatingGRP.GetComponent<FloatingSquare>().QuickBurst();
        yield return new WaitForSeconds(.15f);

        SoundManager.SM.PlayOneShotSound("poof");
        //Instantiate(gameOverEffect, gameBoardSquares[6].transform.position, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect3, gameBoardSquares[6].transform.position, Quaternion.identity, gameObject.transform);
        gameBoardSquares[6].GetComponent<Tutorial_Square>().completed = true;
        gameBoardSquares[6].GetComponent<Tutorial_Square>().blocker = true;
        gameBoardSquares[6].GetComponent<Tutorial_Square>().number = 5;
        gameBoardSquares[6].GetComponent<Tutorial_Square>().floatingGRP.GetComponent<FloatingSquare>().QuickBurst();
        yield return new WaitForSeconds(.15f);

        SoundManager.SM.PlayOneShotSound("poof");
        //Instantiate(gameOverEffect, gameBoardSquares[8].transform.position, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect3, gameBoardSquares[8].transform.position, Quaternion.identity, gameObject.transform);
        gameBoardSquares[8].GetComponent<Tutorial_Square>().completed = true;
        gameBoardSquares[8].GetComponent<Tutorial_Square>().blocker = true;
        gameBoardSquares[8].GetComponent<Tutorial_Square>().number = 5;
        gameBoardSquares[8].GetComponent<Tutorial_Square>().floatingGRP.GetComponent<FloatingSquare>().QuickBurst();


        nextboard.mainNumber = 1;
        nextboard.ColorDisplay();
        instructions_swap1.SetActive(true);
        touchEnabled = true;
    }

    private void PlaceSquare(GameObject square, int number)
    {
        square.GetComponent<Tutorial_Square>().number = number;
        square.GetComponent<Tutorial_Square>().SetSquareDisplay();
        square.GetComponent<Tutorial_Square>().CalculateConnections();
    }

    private void GetGameOverEffectPositions()
    {
        gameOverEffectPosition1 = gameOverEffectPosition1GO.transform.position;
        gameOverEffectPosition2 = gameOverEffectPosition2GO.transform.position;
        gameOverEffectPosition3 = gameOverEffectPosition3GO.transform.position;
        gameOverEffectPosition4 = gameOverEffectPosition4GO.transform.position;
    }

    private void SetGameBoardSquareInfo()
    {

        for (int i = 0; i < gameBoardSquares.Count; i++)
        {

            gameBoardSquares[i].GetComponent<Tutorial_Square>().gamePositionIndex = i;
            gameBoardSquares[i].GetComponent<Tutorial_Square>().gamePositionX = i / gameBoardHeight;
            gameBoardSquares[i].GetComponent<Tutorial_Square>().gamePositionY = i % gameBoardHeight;

        }

        for (int i = 0; i < gameBoardSquares.Count; i++)
        {

            gameBoardSquares[i].GetComponent<Tutorial_Square>().SetAdjescentSquares(gameObject.GetComponent<TutorialGameboard>());

        }
    }

    public void CheckForCompleteLink(GameObject square)
    {
        completeList.Clear();
        completedListPass = true;
        AddSquareToCompletedList(square);
        if (completedListPass)
        {
            //Debug.Log("Completed Link");
            ResetLinkFromBoard();
        }
        else
        {
            //Debug.Log("Link Not Complete");
        }
    }

    private void ResetLinkFromBoard()
    {

        touchEnabled = false;
        int floatingTextNumber = 0;

        for (int i = 0; i < completeList.Count; i++)
        {
            floatingTextNumber += completeList[i].GetComponent<Tutorial_Square>().number;
            AddToScore(completeList[i]);
            completeList[i].GetComponent<Tutorial_Square>().ResetSquare_OnCompletion_Before();
        }

        //because arrow gets turned on when this is in the coroutine
        if (score >= scoreGameOverTotal && update2 == true)
        {
            swapButton.GetComponent<Tutorial_SwapButton>().tutorialBeginingMode = false;
        }

        UpdateSquareConnections();
        StartCoroutine(ClearAnimation(floatingTextNumber));
    }

    IEnumerator ClearAnimation(int number)
    {

        playSquareClearAnimation = true;

        for (int i = 0; i < completeList.Count; i++)
        {
            Hashtable hash = new Hashtable();
            hash.Add("scale", new Vector3(1.08f, 1.08f, 1.08f));
            hash.Add("time", boardScoreSquareClearDuration);
            hash.Add("easetype", "easeOutCubic");
            iTween.ScaleTo(completeList[i].GetComponent<Tutorial_Square>().floatingGRP, hash);
        }

        yield return new WaitForSeconds(boardScoreSquareClearDuration);

        for (int i = 0; i < completeList.Count; i++)
        {
            completeList[i].GetComponent<Tutorial_Square>().ResetSquare_OnCompletion_After();
        }
        touchEnabled = true;
        playSquareClearAnimation = false;

        ClearSFX(number);
        scoreboard.ScoreboardAdd(number);

        UpdateRepairFill();
        CheckGameBoardGameOverTotal();
        DimSleepingSquares();
    }

    private void DimSleepingSquares()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<Tutorial_Square>().CheckDimIfCompleted();
        }
    }

    private void UpdateRepairFill()
    {
        if (endingSeq == true)
        {
            fill.FillEffect(score, scoreGameOverTotal);
        }
    }

    private void CheckGameBoardGameOverTotal()
    {
        if (score >= scoreGameOverTotal && update1 == false)
        {
            touchEnabled = false;
            StartCoroutine(ShakeExplodeGameboard());
        }

        if (score >= scoreGameOverTotal && update2 == true)
        {
            touchEnabled = false;

            swapButton.GetComponent<Tutorial_SwapButton>().DisableSwapButton();
            swapButton.GetComponent<Tutorial_SwapButton>().disabledSwap = true;
            swapButton.GetComponent<Tutorial_SwapButton>().tutorialBeginingMode = false;

            repairButton.GetComponent<Tutorial_RepairButton>().EnabledClearButton();
            instructions_fixboard2.SetActive(true);
            arrowCo = StartCoroutine(TurnOnRepairArrow(1.5f));
            update2 = false;

        }
        if (score >= endingGameOverTotal)
        {
            touchEnabled = false;
            swapButton.GetComponent<Tutorial_SwapButton>().DisableSwapButton();
            StartCoroutine(ShakeEnding());
        }
    }

    public void AllSquaresDown(GameObject square)
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            if (gameBoardSquares[i] != square)
            {
                gameBoardSquares[i].GetComponent<Tutorial_Square>().SquareDown();
            }
        }
    }

    private void ShakeBoard()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<Tutorial_Square>().ShakeSquare();
        }
    }

    IEnumerator ShakeEnding()
    {
        ShakeBoard();
        ScaleSquares();
        StartCoroutine(ExplosionEffects());
        yield return new WaitForSeconds(shakeTime - .4f);
        SwitchSquaresToFakeMaterial();
        SoundManager.SM.PlayOneShotSound("Begin");
        yield return new WaitForSeconds(.15f);
        MakeSquaresFloat();
        instructions_ending1.SetActive(false);
        yield return new WaitForSeconds(.5f);
        instructions_ending2.SetActive(true);
        PlayerPrefs.SetInt("TutorialComplete", 1);
        yield return new WaitForSeconds(2f);
        goNext.animationDone=true;
    }

    IEnumerator ShakeExplodeGameboard()
    {
        ShakeBoard();
        ScaleSquares();
        StartCoroutine(ExplosionEffects());
        yield return new WaitForSeconds(shakeTime-.4f);
        SwitchSquaresToFakeMaterial();
        yield return new WaitForSeconds(.15f);
        SoundManager.SM.PlayOneShotSound("Begin");
        MakeSquaresFloat();

        yield return new WaitForSeconds(.75f);
        SwitchInstructions();
        ShowRepairButton();

    }

    private void SwitchInstructions()
    {
        instructions.SetActive(false);
        instructions_eye.SetActive(false);
        instructions_repair.SetActive(true);
    }

    private void ShowRepairButton()
    {
        MoveRepairButtonIntoPlace();
        repairButton.GetComponent<Tutorial_RepairButton>().EnabledClearButton();
    }

    private void MoveRepairButtonIntoPlace()
    {
        Vector3 endPosition = repair.transform.position;
        Vector3 startPosition = new Vector3(repair.transform.position.x, repair.transform.position.y-100f, repair.transform.position.z);
        repair.transform.position = startPosition;
        repair.SetActive(true);
        iTween.MoveTo(repair, endPosition, 0.5f);
        arrowCo = StartCoroutine(TurnOnRepairArrow(1.5f));
    }

    IEnumerator TurnOnRepairArrow(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        repairArrow.SetActive(true);
    }
    
    public void TurnOffRepairArrow()
    {
        if (arrowCo != null)
        {
            StopCoroutine(arrowCo);
        }

        repairArrow.SetActive(false);
    }

    public void TurnOffSwapArrow()
    {
        swapArrow.SetActive(false);
    }

    private void MakeSquaresFloat()
    {
        foreach (GameObject square in gameBoardSquares)
        {
            square.GetComponent<Tutorial_Square>().connection_group.SetActive(false);
            square.GetComponent<Tutorial_Square>().floatingGRP.GetComponent<FloatingSquare>().QuickBurst();
            Instantiate(gameOverEffect3, square.transform.position, Quaternion.identity, gameObject.transform);
        }

    }
    IEnumerator ExplosionEffects()
    {
        Instantiate(gameOverEffect, gameOverEffectPosition1, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect2, gameOverEffectPosition1, Quaternion.identity, gameObject.transform);
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect2, gameOverEffectPosition2, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect, gameOverEffectPosition2, Quaternion.identity, gameObject.transform);
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect2, gameOverEffectPosition3, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect, gameOverEffectPosition3, Quaternion.identity, gameObject.transform);
        yield return new WaitForSeconds(.125f);
        Instantiate(gameOverEffect2, gameOverEffectPosition4, Quaternion.identity, gameObject.transform);
        Instantiate(gameOverEffect, gameOverEffectPosition4, Quaternion.identity, gameObject.transform);
    }

    private void ScaleSquares()
    {
        Hashtable hash = new Hashtable();
        hash.Add("scale", new Vector3(0.9f, 0.9f, 1f));
        hash.Add("easetype", "easeInQuad");
        hash.Add("oncomplete", "FixGameboardScale");
        hash.Add("oncompletetarget", gameObject);
        hash.Add("time", 1f);
        iTween.ScaleTo(gameboardScaleGroup, hash);
    }

    private void SwitchSquaresToFakeMaterial()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            gameBoardSquares[i].GetComponent<Tutorial_Square>().SwitchToFakeMaterials();
        }
    }

    private void ClearSFX(int clearNumber)
    {
        if (clearNumber <= 4)
        {
            SoundManager.SM.PlayOneShotSound("clearboard1");
        }
        else if (clearNumber > 4 && clearNumber <= 10)
        {
            SoundManager.SM.PlayOneShotSound("clearboard2");
        }
        else
        {
            SoundManager.SM.PlayOneShotSound("clearboard3");
        }

    }

    private void AddToScore(GameObject square)
    {
        score = score + square.GetComponent<Tutorial_Square>().number;
    }

    public void UpdateSquareConnections()
    {
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            if (gameBoardSquares[i].GetComponent<Tutorial_Square>().number != 0 && gameBoardSquares[i].GetComponent<Tutorial_Square>().number != 5)
            {
                gameBoardSquares[i].GetComponent<Tutorial_Square>().ConnectionDisplay();
            }
        }
    }

    private void AddSquareToCompletedList(GameObject square)
    {
        completeList.Add(square);

        if (completedListPass)
        {
            for (int i = 0; i < square.GetComponent<Tutorial_Square>().adjescentConnections.Count; i++)
            {
                if (square.GetComponent<Tutorial_Square>().adjescentConnections[i] == true)
                {
                    if (square.GetComponent<Tutorial_Square>().adjescentSquares[i].completed == true)
                    {
                        if (!completeList.Contains(square.GetComponent<Tutorial_Square>().adjescentSquares[i].gameObject))
                        {
                            AddSquareToCompletedList(square.GetComponent<Tutorial_Square>().adjescentSquares[i].gameObject);
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

    public void CheckIfBoardFull()
    {
        int emptyCounter = 0;
        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            if (gameBoardSquares[i].GetComponent<Tutorial_Square>().number == 0)
            {
                emptyCounter++;
                //Debug.Log("not game over");
                break;
            }
        }
        if (emptyCounter == 0)
        {
            StartCoroutine(ResetTutorialGameBoard());
        }
    }

    IEnumerator ResetTutorialGameBoard()
    {
        yield return new WaitForSeconds(1f);
        RepairGameboard();
    }

    private void ResetSquare_Full(GameObject square)
    {
        square.GetComponent<Tutorial_Square>().ResetSquare_OnFull();
    }

    public void RepairGameboard()
    {

        gameboardScaleGroup.transform.localScale = new Vector3(1f, 1f, 1f);

        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            ResetSquare_Full(gameBoardSquares[i]);
        }

        SoundManager.SM.PlayOneShotSound("clearBlockers");
        UpdateSquareConnections();

        if (endingSeq == true)
        {
            PlaceSquare(gameBoardSquares[2], 3);
            PlaceSquare(gameBoardSquares[6], 3);
        }
    }

    public void RepairGameboard_DisableButton()
    {
        gameboardScaleGroup.transform.localScale = new Vector3(1f,1f,1f);
        repairArrow.SetActive(false);

        for (int i = 0; i < gameBoardSquares.Count; i++)
        {
            ResetSquare_Full(gameBoardSquares[i]);
        }

        SoundManager.SM.PlayOneShotSound("clearBlockers");
        UpdateSquareConnections();
        repairButton.GetComponent<Tutorial_RepairButton>().DisabledClearButton();
        instructions_repair.SetActive(false);

        if (instructions_repairDone.GetComponent<TypewriterEffect>().index < 1)
        {
            instructions_repairDone.SetActive(true);
        }
        else
        {
            instructions_fixboard2.SetActive(false);
            StartCoroutine(EndingSequence());
        }


    }

    IEnumerator EndingSequence()
    {
        endingSeq = true;
        yield return new WaitForSeconds(0.5f);
        instructions_ending1.SetActive(true);
        yield return new WaitForSeconds(1f);

        PlaceSquare(gameBoardSquares[2], 3);
        yield return new WaitForSeconds(.25f);

        PlaceSquare(gameBoardSquares[6], 3);
        nextboard.mainNumber = 1;
        nextboard.ColorDisplay();

        touchEnabled = true;

        swapButton.GetComponent<Tutorial_SwapButton>().disabledSwap = false;
        swapButton.GetComponent<Tutorial_SwapButton>().EnableSwapButton();

    }
}

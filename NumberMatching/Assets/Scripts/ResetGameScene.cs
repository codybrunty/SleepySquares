using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using CloudOnce;
using System;

public class ResetGameScene : MonoBehaviour{

    [SerializeField] SettingsMenu settings = default;
    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] GameObject GameOverPanel = default;
    private List<GameObject> emptySquares = new List<GameObject>();
    private List<Button> disabledButtons = new List<Button>();
    [SerializeField] RaycastMouse ray = default;
    [SerializeField] NotificationSystem notificationSystem = default;



    private Vector3 startPosition;
    public float moveDuration = 1f;
    public AnimationCurve ease;

    private Coroutine coroutine;

    public void GetStartPosition() {
        startPosition = gameObject.transform.position;
    }

    public void MoveOnScreen() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        gameObject.transform.position = startPosition;
    }

    public void MoveOffScreen() {
        coroutine = StartCoroutine(MoveOverTime());
    }

    IEnumerator MoveOverTime() {

        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + 500f, startPosition.z);

        for (float t = 0f; t < moveDuration; t += Time.deltaTime) {
            float normalizedTime = t / moveDuration;
            gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, ease.Evaluate(normalizedTime));
            yield return null;
        }

        gameObject.transform.position = endPosition;

    }




    public void ResetHardModeSwitch() {
        //OnResetPostToLeaderboard(hardModeOn);
        settings.ExitSettings();
        //gameBoard.ResetBoardState();
        gameBoard.SetBoardState();
        GameOverPanel.SetActive(false);
        ray.gameStarted = false;
    }

    public void InGameResetOnClick() {
        DisableButtonsBeforeFill();
        gameBoard.RemoveAnyLuckyCoinsFromBoard();
        //StartCoroutine(FillBoard(true));
        gameBoard.GameOver();
        StartCoroutine(EnableButtons());
    }

    IEnumerator EnableButtons()
    {
        yield return new WaitForSeconds(0.75f);
        foreach (Button b in disabledButtons)
        {
            b.interactable = true;
            b.GetComponent<Image>().raycastTarget = true;
        }
        ray.resetMode = false;
    }

    private void EnableButtonsAfterFill()
    {
        foreach (Button b in disabledButtons)
        {
            b.interactable = true;
            b.GetComponent<Image>().raycastTarget = true;
        }
        ray.resetMode = false;
    }

    private void DisableButtonsBeforeFill() {
        disabledButtons.Clear();
        Button[] buttons = gameObject.transform.parent.GetComponentsInChildren<Button>();
        foreach (Button b in buttons) {
            disabledButtons.Add(b);
            b.interactable = false;
            b.GetComponent<Image>().raycastTarget = false;
        }
        ray.resetMode = true;
    }

    IEnumerator FillBoard(bool noWait) {
        float waitTime = .1f;
        if (noWait) {
            waitTime = 0f;
        }

        yield return new WaitForSeconds(waitTime);
        emptySquares.Clear();


        for (int i = 0; i < gameBoard.gameBoardSquares.Count; i++) {
            if (gameBoard.gameBoardSquares[i].GetComponent<SquareMechanics_Gameboard>().number == 0) {
                emptySquares.Add(gameBoard.gameBoardSquares[i]);
            }
        }



        if (emptySquares.Count > 0) {
            int randomIndex = UnityEngine.Random.Range(0, emptySquares.Count);
            FindObjectOfType<RaycastMouse>().GameSquareHit(emptySquares[randomIndex]);
            StartCoroutine(FillBoard(false));
        }
        else {
            EnableButtonsAfterFill();
        }



    }

    public void ResetGameOverOnClick() {
        SoundManager.SM.PlayOneShotSound("ResetGame");
        settings.ExitSettings(false);
        gameBoard.ResetBoardState();
        GameOverPanel.GetComponent<GameOverPanel>().ResetGameOverPanelScale();
        GameOverPanel.SetActive(false);
        notificationSystem.CheckAlertStatus();
        //MusicManager.MM.FadeInNewMusic();
    }

    public void OnResetPostToLeaderboard(int hardModeOn) {
        if (hardModeOn == 1) {
            Debug.Log("Post To HardMode HighScore Leaderboard");
            long scoreToPost = gameBoard.score;
            Leaderboards.HardModeHighScore.SubmitScore(scoreToPost, callbackCheck);
        }
        else {
            Debug.Log("Post To HighScore Leaderboard");
            long scoreToPost = gameBoard.score;
            Leaderboards.HighScore.SubmitScore(scoreToPost, callbackCheck);
        }


    }
    
    private void callbackCheck(CloudRequestResult<bool> result) {
        if (result.Result == false) {
            Debug.Log(result.Error);
        }
    }
    
}

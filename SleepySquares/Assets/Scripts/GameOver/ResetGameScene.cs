using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ResetGameScene : MonoBehaviour{

    [SerializeField] SettingsMenu settings = default;
    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] GameOverPanel GameOverPanel = default;
    private List<SquareMechanics_Gameboard> emptySquaresMechanics = new List<SquareMechanics_Gameboard>();
    private List<Button> disabledButtons = new List<Button>();
    [SerializeField] RaycastMouse ray = default;
    [SerializeField] NotificationSystem notificationSystem = default;
    [SerializeField] RaycastMouse raycastMouse = default;



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
        GameOverPanel.gameObject.SetActive(false);
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
        yield return new WaitForSeconds(1f);
        if (gameBoard.DailyModeOn) {
            yield return new WaitForSeconds(1f);
        }

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
        emptySquaresMechanics.Clear();


        for (int i = 0; i < gameBoard.gameBoardSquaresMechanics.Count; i++) {
            if (gameBoard.gameBoardSquaresMechanics[i].number == 0) {
                emptySquaresMechanics.Add(gameBoard.gameBoardSquaresMechanics[i]);
            }
        }



        if (emptySquaresMechanics.Count > 0) {
            int randomIndex = UnityEngine.Random.Range(0, emptySquaresMechanics.Count);
            raycastMouse.GameSquareHit(emptySquaresMechanics[randomIndex]);
            StartCoroutine(FillBoard(false));
        }
        else {
            EnableButtonsAfterFill();
        }



    }

    public void ResetGameOverOnClick() {
        SoundManager.SM.PlayOneShotSound("ResetGame");
        settings.ExitSettings(false);
        GameOverPanel.ResetGameOverPanelScale();
        GameOverPanel.gameObject.SetActive(false);

        if (gameBoard.DailyModeOn) {
            gameBoard.SetDailyBoard();
        }
        else {
            gameBoard.ResetBoardState();
        }
    }

    public void OnResetPostToLeaderboard(int hardModeOn) {
        if (hardModeOn == 1) {
            Debug.Log("Post To HardMode HighScore Leaderboard");
            long scoreToPost = gameBoard.score;
            //Leaderboards.HardModeHighScore.SubmitScore(scoreToPost, callbackCheck);
        }
        else {
            Debug.Log("Post To HighScore Leaderboard");
            long scoreToPost = gameBoard.score;
            //Leaderboards.HighScore.SubmitScore(scoreToPost, callbackCheck);
        }


    }
    /*
    private void callbackCheck(CloudRequestResult<bool> result) {
        if (result.Result == false) {
            Debug.Log(result.Error);
        }
    }
    */
}

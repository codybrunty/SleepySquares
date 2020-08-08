using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using CloudOnce;

public class ResetGameScene : MonoBehaviour{

    [SerializeField] SettingsPanels settings = default;
    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] GameObject GameOverPanel = default;
    private List<GameObject> emptySquares = new List<GameObject>();
    private List<Button> disabledButtons = new List<Button>();

    public void ResetHardModeSwitch(int hardModeOn) {
        OnResetPostToLeaderboard(hardModeOn);
        settings.ExitSettings();
        gameBoard.ResetBoardState();
        GameOverPanel.SetActive(false);
    }

    public void InGameResetOnClick() {
        DisableAllButtonsBeforeFill();
        StartCoroutine(FillBoard(true));
    }

    private void DisableAllButtonsBeforeFill() {
        disabledButtons.Clear();
        Button[] buttons = gameObject.transform.parent.GetComponentsInChildren<Button>();
        foreach (Button b in buttons) {
            disabledButtons.Add(b);
            b.interactable = false;
            b.GetComponent<Image>().raycastTarget = false;
        }
    }

    private void EnableAllButtonsAfterFill() {
        foreach (Button b in disabledButtons) {
            b.interactable = true;
            b.GetComponent<Image>().raycastTarget = true;
        }
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
            EnableAllButtonsAfterFill();
        }
    }

    public void ResetGameOverOnClick() {
        settings.ExitSettings();
        gameBoard.ResetBoardState();
        GameOverPanel.GetComponent<GameOverPanel>().ResetGameOveerPanelScale();
        GameOverPanel.SetActive(false);
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

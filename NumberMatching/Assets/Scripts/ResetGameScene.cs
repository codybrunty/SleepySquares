using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using CloudOnce;

public class ResetGameScene : MonoBehaviour{

    [SerializeField] ExitPanels exit = default;
    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] GameObject GameOverPanel = default;
    private List<GameObject> emptySquares = new List<GameObject>();

    public void ResetHardModeSwitch(int hardModeOn) {
        OnResetPostToLeaderboard(hardModeOn);
        exit.ExitOnClick();
        gameBoard.ResetBoardState();
        GameOverPanel.SetActive(false);
    }

    public void InGameResetOnClick() {
        StartCoroutine(FillBoard(true));
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
    }

    public void ResetGameOverOnClick() {
        exit.ExitOnClick();
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

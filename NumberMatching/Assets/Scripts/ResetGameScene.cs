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
    
    public void ResetInGameOnClick() {
        OnResetPostToLeaderboard();
        exit.ExitOnClick();
        gameBoard.ResetBoardState();
        GameOverPanel.SetActive(false);
    }

    public void ResetGameOverOnClick() {
        exit.ExitOnClick();
        gameBoard.ResetBoardState();
        GameOverPanel.SetActive(false);
    }

    public void OnResetPostToLeaderboard() { 
        Debug.Log("Post To Leaderboard");
        long scoreToPost = gameBoard.score;
        Leaderboards.HighScore.SubmitScore(scoreToPost, callbackCheck);
    }

    private void callbackCheck(CloudRequestResult<bool> result) {
        if (result.Result == false) {
            Debug.Log(result.Error);
        }
    }

}

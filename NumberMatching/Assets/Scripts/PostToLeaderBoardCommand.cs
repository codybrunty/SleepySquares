using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CloudOnce;

public class PostToLeaderBoardCommand : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameboard = default;

    private void Start() {
        Debug.Log("Post To Leaderboard");
        long scoreToPost = gameboard.score;
        Leaderboards.HighScore.SubmitScore(scoreToPost, callbackCheck);
    }

    private void callbackCheck(CloudRequestResult<bool> result) {
        if (result.Result == false) {
            Debug.Log(result.Error);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CloudOnce;

public class PostToLeaderBoardCommand : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameboard;

    public void PostToLeaderBoardOnClick() {
        Debug.Log("Post To Leaderboard Clicked");
        long scoreToPost = gameboard.score;

        Leaderboards.HighScore.SubmitScore(scoreToPost, callbackCheck);
        //Cloud.Leaderboards.SubmitScore("CgkI1a-4sNcXEAIQAQ", scoreToPost);

        gameObject.GetComponent<Button>().interactable = false;
    }

    private void callbackCheck(CloudRequestResult<bool> result) {
        if (result.Result == false) {
            Debug.Log(result.Error);
        }
    }

}

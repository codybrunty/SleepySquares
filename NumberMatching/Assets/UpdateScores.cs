using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScores : MonoBehaviour{

    public GameBoardMechanics gameboard;

    public void SettingsButtonUpdateScoresOnClick() {
        gameboard.PostToLeaderboard();
    }

}

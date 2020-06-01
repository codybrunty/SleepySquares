using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGameScene : MonoBehaviour{

    [SerializeField] ExitPanels exit = default;
    [SerializeField] GameBoardMechanics gameBoard = default;

    public void ResetOnClick() {
        exit.ExitOnClick();
        gameBoard.ResetBoardState();
    }


}

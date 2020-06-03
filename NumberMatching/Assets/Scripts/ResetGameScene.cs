using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResetGameScene : MonoBehaviour{

    [SerializeField] ExitPanels exit = default;
    [SerializeField] GameBoardMechanics gameBoard = default;
    [SerializeField] TextMeshProUGUI gameOver_Text = default;
    [SerializeField] GameObject popUpButton = default;

    public void ResetOnClick() {
        exit.ExitOnClick();
        gameOver_Text.gameObject.SetActive(false);
        gameBoard.ResetBoardState();
        popUpButton.SetActive(false);
    }


}

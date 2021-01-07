using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockCurrentGameMode : MonoBehaviour
{
    public GameBoardMechanics gameboard;
    public enum GameMode { dailys,eyed3,eyed4}
    public GameMode buttonGameMode;
    private void OnEnable() {
        if (gameboard.DailyModeOn) {
            if (buttonGameMode == GameMode.dailys) {
                gameObject.GetComponent<Button>().interactable = false;
            }
            else {
                gameObject.GetComponent<Button>().interactable = true;
            }
        }
        else {
            if(gameboard.hardModeOn == 1) {
                if (buttonGameMode == GameMode.eyed4) {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else {
                    gameObject.GetComponent<Button>().interactable = true;
                }
            }
            else {
                if (buttonGameMode == GameMode.eyed3) {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else {
                    gameObject.GetComponent<Button>().interactable = true;
                }
            }
        }
    }
}

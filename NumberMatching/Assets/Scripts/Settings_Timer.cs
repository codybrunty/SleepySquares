using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Settings_Timer : MonoBehaviour{

    private void Start() {
        SetButtonState();
    }

    private void SetButtonState() {
        if (GameSettings.GS.timerStatus == 1) {
            ButtonStateTimerOn();
        }
        else {
            ButtonStateTimerOff();
        }
    }

    private void ButtonStateTimerOff() {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Timer Off";
    }

    private void ButtonStateTimerOn() {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Timer On"; ;
    }

    public void SettingsTimerOnClick() {
        if(GameSettings.GS.timerStatus == 0) {
            TimerTurnedOff();
        }
        else {
            TimerTurnedOn();
        }
    }



    private void TimerTurnedOff() {
        SetButtonState();
        GameSettings.GS.timerStatus = 1;
        PlayerPrefs.SetInt("TimerSettings", GameSettings.GS.timerStatus);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void TimerTurnedOn() {
        SetButtonState();
        GameSettings.GS.timerStatus = 0;
        PlayerPrefs.SetInt("TimerSettings", GameSettings.GS.timerStatus);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

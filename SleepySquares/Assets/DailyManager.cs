using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyManager : MonoBehaviour{

    [Header("Daily")]
    public int Day;
    public int RandomDailyDesignIndex;

    [Header("Hearts")]
    public GameObject hearts_group;
    public List<Image> hearts = new List<Image>();
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public GameObject heartParticle;

    [Header("Continue")]
    public GameObject continuePanel;
    public GameObject gameboardDim;
    public GameBoardMechanics gameboard;
    public bool hasHearts = true;

    private void Awake() {

        //increment daily failed incase they quit durring a daily challenge
        int counter = PlayerPrefs.GetInt("Daily_Failed_Increment", 0);
        int dailyfails = PlayerPrefs.GetInt("Daily_Failed", 0);
        PlayerPrefs.SetInt("Daily_Failed", dailyfails+counter);
        Debug.Log("Daily Fails at "+(dailyfails+counter).ToString() );
    }

    public void EnableHearts() {
        hearts_group.SetActive(true);
        SetHearts();
    }

    public void DisableHearts() {
        hearts_group.SetActive(false);
    }

    public void SetHearts() {
        int dailyHeartsLeft = PlayerPrefs.GetInt("DailyHearts",3);
        HeartsCheck();
        if (dailyHeartsLeft > 0) {
            for (int i = 0; i < hearts.Count; i++) {
                if (i <= dailyHeartsLeft - 1) {
                    hearts[i].sprite = fullHeart;
                }
                else {
                    hearts[i].sprite = emptyHeart;
                }
            }
            HideContinueOptions();
        }
        else {
            for (int i = 0; i < hearts.Count; i++) {
                hearts[i].sprite = emptyHeart;
            }
            ShowContinueOptions();
        }


    }

    private void ShowContinueOptions() {
        Debug.Log("Out of Hearts Continue?");
        HeartsCheck();
        gameboard.touchEnabled = false;
        gameboardDim.SetActive(true);
        continuePanel.SetActive(true);
    }

    public void HideContinueOptions() {
        HeartsCheck();
        StartCoroutine(gameboard.EnableTouch(.1f));
        gameboardDim.SetActive(false);
        continuePanel.SetActive(false);
    }

    public void HeartsCheck() {
        if(PlayerPrefs.GetInt("DailyHearts", 3) > 0){
            hasHearts = true;
        }
        else {
            hasHearts = false;
        }
    }

    public void ContinueDaily() {
        GainHearts();
        HideContinueOptions();
    }

    public void GainHearts() {
        PlayerPrefs.SetInt("DailyHearts", 3);
        SoundManager.SM.PlayOneShotSound("GainHearts");

        Hashtable hash1 = new Hashtable();
        hash1.Add("amount", new Vector3(2f, 2f, 0f));
        hash1.Add("time", 0.75f);
        iTween.PunchScale(hearts[0].gameObject, hash1);
        hearts[0].sprite = fullHeart;

        Hashtable hash2 = new Hashtable();
        hash2.Add("amount", new Vector3(2f, 2f, 0f));
        hash2.Add("time", 0.75f);
        iTween.PunchScale(hearts[1].gameObject, hash2);
        hearts[1].sprite = fullHeart;

        Hashtable hash3 = new Hashtable();
        hash3.Add("amount", new Vector3(2f, 2f, 0f));
        hash3.Add("time", 0.75f);
        iTween.PunchScale(hearts[2].gameObject, hash3);
        hearts[2].sprite = fullHeart;
    }

    public void LoseOneHeart() {
        int dailyHeartsLeft = PlayerPrefs.GetInt("DailyHearts", 3);

        SoundManager.SM.PlayOneShotSound("LoseHeart");

        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2f, 2f, 0f));
        hash.Add("time", 0.75f);
        iTween.PunchScale(hearts[dailyHeartsLeft - 1].gameObject, hash);

        Instantiate(heartParticle.gameObject, hearts[dailyHeartsLeft - 1].transform.position,Quaternion.identity, hearts[dailyHeartsLeft - 1].transform.parent);

        hearts[dailyHeartsLeft - 1].sprite = emptyHeart;

        PlayerPrefs.SetInt("DailyHearts", dailyHeartsLeft-1);

        //for playfab tracking
        int counter = PlayerPrefs.GetInt("Daily_Failed", 0);
        counter++;
        PlayerPrefs.SetInt("Daily_Failed", counter);
    }

    private void OnApplicationQuit() {
        CheckIfDailyStarted();
    }

    public void CheckIfDailyStarted() {
        if (gameboard.DailyModeOn && gameboard.score > 0) {
            int newHearts = PlayerPrefs.GetInt("DailyHearts", 3) - 1;
            if (newHearts < 0) {
                newHearts = 0;
            }
            PlayerPrefs.SetInt("DailyHearts", newHearts);

            //for playfab tracking
            PlayerPrefs.SetInt("Daily_Failed_Increment", 1);
        }
        gameboard.PostToLeaderboard();
    }
}

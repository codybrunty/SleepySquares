using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star2Mechanics : MonoBehaviour {

    [SerializeField] GameObject starEffect = default;
    public int starStatus; //0=off,1=on
    private CollectionColor_Image star;

    private void Start() {
        star = gameObject.GetComponent<CollectionColor_Image>();
        starStatus = PlayerPrefs.GetInt("Star2_GoldStatus", 0);
        UpdateStarDisplay();
    }

    private void UpdateStarDisplay() {
        if (starStatus == 1) {
            StarGold();
        }
        else {
            StarNormal();
        }
    }

    public void StarOn() {
        starStatus = PlayerPrefs.GetInt("Star2_GoldStatus", 0);

        if (starStatus == 0) {
            starStatus = 1;
            PlayerPrefs.SetInt("Star2_GoldStatus", starStatus);
            PlayStarEffect();
            StartCoroutine(TurnGoldDelay());
        }
    }

    public void StarOff() {
        starStatus = PlayerPrefs.GetInt("Star2_GoldStatus", 0);

        if (starStatus == 1) {
            starStatus = 0;
            PlayerPrefs.SetInt("Star2_GoldStatus", starStatus);
            StarNormal();
        }
    }

    private void StarGold() {
        star.key = "Trophy3";
        star.GetColor();
    }

    private void StarNormal() {
        star.key = "Second";
        star.GetColor();
    }

    private void PlayStarEffect() {
        starEffect.SetActive(true);
        StartCoroutine(TurnOffStartEffect());
    }


    IEnumerator TurnOffStartEffect() {
        yield return new WaitForSeconds(1f);
        starEffect.SetActive(false);
    }

    IEnumerator TurnGoldDelay() {
        yield return new WaitForSeconds(0.5f);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2f, 2f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(gameObject, hash);
        StarGold();
    }

}

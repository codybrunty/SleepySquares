using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star2Mechanics : MonoBehaviour {

    [SerializeField] GameObject starEffect = default;
    public int starStatus; //0=off,1=on
    [SerializeField] CollectionColor_Image star;
    [SerializeField] SwitchButton switchButton = default;
    [SerializeField] GameObject switchesText = default;

    public AnimationCurve ease = default;
    public AnimationCurve ease2 = default;

    private void Start() {
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
            StartCoroutine(TurnGoldAndRewardSwitchesDelay());
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
        StarInvisible();
    }

    private void StarInvisible() {
        Color invis = star.GetComponent<Image>().color;
        star.GetComponent<Image>().color = new Color(invis.r, invis.g, invis.b, 0f);
    }

    private void StarVisible() {
        Color invis = star.GetComponent<Image>().color;
        star.GetComponent<Image>().color = new Color(invis.r, invis.g, invis.b, 1f);
    }

    private void StarNormal() {
        star.key = "Second";
        star.GetColor();
        StarVisible();
    }

    private void PlayStarEffect() {
        starEffect.SetActive(true);
        StartCoroutine(TurnOffStartEffect());
    }


    IEnumerator TurnOffStartEffect() {
        yield return new WaitForSeconds(1.5f);
        starEffect.SetActive(false);
    }

    IEnumerator TurnGoldAndRewardSwitchesDelay() {
        yield return new WaitForSeconds(0.5f);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2f, 2f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(gameObject, hash);
        StarGold();
        StarVisible();
        PlayStarSFX();
        yield return new WaitForSeconds(0.5f);
        FlyToSwitches();
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<SoundManager>().PlayOneShotSound("yahoo");
        switchButton.AddSwitches(2);
    }

    private void PlayStarSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("star");
    }

    private void FlyToSwitches() {
        StartCoroutine(TwenMove(0.5f));
    }

    IEnumerator TwenMove(float duration) {

        Vector3 currentPos = gameObject.transform.position;
        Vector3 textPosition = switchesText.transform.position;

        float startRotation = gameObject.GetComponent<RectTransform>().eulerAngles.z;
        float endRotation = startRotation + 360.0f*3;

        for (float time = 0f; time < duration; time += Time.deltaTime) {
            gameObject.transform.position = Vector3.Lerp(currentPos, textPosition, ease.Evaluate(time / duration));
            
            float zRotation = Mathf.Lerp(startRotation, endRotation, ease2.Evaluate(time / duration)) % 360.0f;
            gameObject.GetComponent<RectTransform>().eulerAngles = new Vector3(gameObject.GetComponent<RectTransform>().eulerAngles.x, gameObject.GetComponent<RectTransform>().eulerAngles.y, zRotation);

            yield return null;
        }

        gameObject.transform.position = textPosition;
        StarInvisible();
        gameObject.transform.position = currentPos;
        gameObject.GetComponent<RectTransform>().eulerAngles = new Vector3(gameObject.GetComponent<RectTransform>().eulerAngles.x, gameObject.GetComponent<RectTransform>().eulerAngles.y, startRotation);
    }
}

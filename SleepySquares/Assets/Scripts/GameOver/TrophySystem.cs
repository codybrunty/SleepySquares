using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrophySystem : MonoBehaviour{

    [SerializeField] List<Sprite> trophies = new List<Sprite>();
    [SerializeField] public  Image trophy = default;
    [SerializeField] Image bar = default;
    [SerializeField] Star1Mechanics star1 = default;
    [SerializeField] Star2Mechanics star2 = default;
    [SerializeField] TextMeshProUGUI levelText = default;
    [SerializeField] GameObject TrophyEffect = default;
    public int trophyIndex = 0;
    public int trophyPanelMinScore = 0;
    public int trophyPanelMaxScore = 0;

    public int first = 300;
    public int second = 800; 
    public int addAfter = 1000;

    private int savedTotalPoints = -1;
    private int totalPoints;
    private int topBarMax;

    public AnimationCurve easeCurve;

    private bool isStart = false;

    private Vector3 startPosition;
    public float moveDuration = 1f;

    private Coroutine coroutine;
    public float fillNumber;

    private Coroutine fillCoroutine;

    public void GetStartPosition() {
        startPosition = gameObject.transform.position;
    }

    public void MoveOnScreen() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        gameObject.transform.position = startPosition;
    }

    public void MoveOffScreen() {
        coroutine = StartCoroutine(MoveOverTime());
    }

    IEnumerator MoveOverTime() {

        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y+500f, startPosition.z);

        for (float t = 0f; t < moveDuration; t += Time.deltaTime) {
            float normalizedTime = t / moveDuration;
            gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, easeCurve.Evaluate(normalizedTime));
            yield return null;
        }

        gameObject.transform.position = endPosition;

    }

    public void UpdateTrophyPanel() {
        Debug.Log("Updating Trophy Panel");
        totalPoints = GameDataManager.GDM.TotalPoints_AllTime;

        if (totalPoints != savedTotalPoints) {
            
            savedTotalPoints = totalPoints;
            CalculateTrophyPanelMinAndMaxScore();
            SetTrophyImage();

            GetFillNumber();
            FillBar();
            StarEffects();

        }
        
    }

    private void GetFillNumber(){
        fillNumber = (float)((float)(totalPoints - trophyPanelMinScore) / (float)(trophyPanelMaxScore - trophyPanelMinScore));
    }

    private void StarEffects(){

        if (fillNumber < .25f){
            star1.StarOff();
            star2.StarOff();
        }

        if (fillNumber > .25f && fillNumber < .75f){
            star1.StarOn();
            star2.StarOff();
        }

        if (fillNumber > .75f){
            star1.StarOn();
            star2.StarOn();
        }


    }

    private void FillBar(){

        if (!isStart){
            bar.fillAmount = fillNumber;
            isStart = true;
        }
        else{
            if (totalPoints < trophyPanelMaxScore){
                if (fillCoroutine != null){
                    StopCoroutine(fillCoroutine);
                }
                fillCoroutine = StartCoroutine(FillOverTime(fillNumber));
            }
            else{
                if (fillCoroutine != null){
                    StopCoroutine(fillCoroutine);
                }

                fillCoroutine = StartCoroutine(FillOverTime(1f));
                trophyIndex++;
                PlayerPrefs.SetInt("TrophyIndex", trophyIndex);
                StartCoroutine(TrophyChangeAnimation());
            }
        }


    }

    private void SetTrophyImage() {
        //Debug.LogWarning(trophyIndex);
        trophy.sprite = trophies[trophyIndex % trophies.Count];
        List<string> trophyColorList = new List<string> { "Trophy1", "Trophy2", "Trophy3" };
        trophy.GetComponent<CollectionColor_Image>().key = trophyColorList[trophyIndex % 3];
        trophy.GetComponent<CollectionColor_Image>().GetColor();
        levelText.text = (trophyIndex + 1).ToString();
    }

    private void CalculateTrophyPanelMinAndMaxScore() {
        trophyIndex = PlayerPrefs.GetInt("TrophyIndex", 0);
        
        if (trophyIndex == 0) {
            trophyPanelMinScore = 0;
            trophyPanelMaxScore = first;
        }
        else if (trophyIndex == 1) {
            trophyPanelMinScore = first;
            trophyPanelMaxScore = second;
        }
        else {
            trophyPanelMaxScore = ((trophyIndex - 1) * addAfter) + second;
            trophyPanelMinScore = trophyPanelMaxScore - (addAfter);
        }

    }

    IEnumerator FillOverTime(float targetFillNumber) {

        float fillDuration = 0.5f;
        float currentFillNumber = bar.fillAmount;

        for (float t = 0f; t < fillDuration; t += Time.deltaTime) {
            float normalizedTime = t / fillDuration;
            bar.fillAmount = Mathf.Lerp(currentFillNumber, targetFillNumber, easeCurve.Evaluate(normalizedTime));
            yield return null;
        }

        bar.fillAmount = targetFillNumber;

    }

    IEnumerator TrophyChangeAnimation() {
        yield return new WaitForSeconds(1f);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(40f, 25f, 0f));
        hash.Add("time", 1f);
        iTween.ShakePosition(gameObject, hash);
        yield return new WaitForSeconds(0.4f);
        TrophyEffect.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        SoundManager.SM.PlayOneShotSound("newTrophy");
        SwitchTrophyDisplay();
        Hashtable hash2 = new Hashtable();
        hash2.Add("amount", new Vector3(1.5f, 1.5f, 0f));
        hash2.Add("time", .75f);
        hash2.Add("oncomplete", "TurnStarEffectOff");
        hash2.Add("onCompleteTarget", gameObject);
        iTween.PunchScale(gameObject.transform.GetChild(0).gameObject, hash2);

    }

    public void TurnStarEffectOff() {
        TrophyEffect.SetActive(false);
    }

    private void SwitchTrophyDisplay() {
        SetTrophyImage();
        star1.StarOff();
        star2.StarOff();
        CalculateTrophyPanelMinAndMaxScore();
        bar.fillAmount = 0;
        //bottomBar.fillAmount = 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrophySystem : MonoBehaviour{

    [SerializeField] List<Sprite> trophies = new List<Sprite>();
    [SerializeField] Image trophy = default;
    [SerializeField] Image topBar = default;
    [SerializeField] Image bottomBar = default;
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


    public void UpdateTrophyPanel() {
        Debug.Log("Updating Trophy Panel");
        totalPoints = GameDataManager.GDM.TotalPoints_AllTime;//50

        if (totalPoints != savedTotalPoints) {
            //Debug.Log("saving total points");
            

            savedTotalPoints = totalPoints;
            CalculateTrophyPanelMinAndMaxScore();
            SetTrophyImage();

            topBarMax = trophyPanelMinScore + ((trophyPanelMaxScore - trophyPanelMinScore) / 2); //150

            if (totalPoints < topBarMax) {//50<150
                FillTopBar(false);
            }


            else {
                if (topBar.fillAmount != 1) {
                    FillTopBar(true);
                }
                else {
                    FillBottomBar();
                }
            }
        }
        
    }

    private void FillBottomBar() {
        topBar.fillAmount = 1f;

        if (isStart) {
            star2.StarOn();
        }

        if (totalPoints < trophyPanelMaxScore) {
            float fillNumber = (float)((float)(totalPoints - topBarMax) / (float)(trophyPanelMaxScore - topBarMax));


            //on first time just populate the bars
            if (!isStart) {
                isStart = true;
                bottomBar.fillAmount = fillNumber;
            }
            else {
                StartCoroutine(MoveOverTime(bottomBar, fillNumber,false));
            }
            star1.StarOff();



        }
        else {
            StartCoroutine(MoveOverTime(bottomBar, 1f, false));
            star1.StarOn();
            trophyIndex++;
            PlayerPrefs.SetInt("TrophyIndex", trophyIndex);
            StartCoroutine(TrophyChangeAnimation());
        }
    }

    private void FillTopBar(bool botNext) {

        float fillNumber = (float)((float)(totalPoints - trophyPanelMinScore) / (float)(topBarMax - trophyPanelMinScore));//0.33333f

        //on first time just populate the bars
        if (!isStart) {
            if (botNext == false) {
                isStart = true;
            }
            topBar.fillAmount = fillNumber;
        }
        else {
            StartCoroutine(MoveOverTime(topBar, fillNumber,botNext));
        }

        bottomBar.fillAmount = 0f;

        star1.StarOff();

        if (botNext==true && !isStart) {
            FillBottomBar();
        }
        else {
            star2.StarOff();
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


    IEnumerator MoveOverTime(Image bar, float targetFillNumber,bool botNext) {
       // Debug.Log(bar.name + " filling up");

        float fillDuration = bar.fillAmount - targetFillNumber;
        if (fillDuration < 0.1f) {
            fillDuration = 0.1f;
        }

        //Debug.LogWarning(fillDuration);



        float currentFillNumber = bar.fillAmount;
        if (currentFillNumber == 1) {
            currentFillNumber = 0;
        }

        for (float t = 0f; t < fillDuration; t += Time.deltaTime) {
            float normalizedTime = t / fillDuration;
            bar.fillAmount = Mathf.Lerp(currentFillNumber, targetFillNumber, easeCurve.Evaluate(normalizedTime));
            yield return null;
        }

        bar.fillAmount = targetFillNumber;

        if (botNext) {
            //Debug.Log("Fill Bottom After Top");
            FillBottomBar();
        }
    }

    IEnumerator TrophyChangeAnimation() {
        yield return new WaitForSeconds(2.5f);
        


        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(40f, 25f, 0f));
        hash.Add("time", 1f);
        iTween.ShakePosition(gameObject, hash);

        yield return new WaitForSeconds(0.4f);

        TrophyEffect.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        FindObjectOfType<SoundManager>().PlayOneShotSound("newTrophy");
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
        topBar.fillAmount = 0;
        bottomBar.fillAmount = 0;
    }
}

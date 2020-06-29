using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophySystem : MonoBehaviour{

    [SerializeField] List<Sprite> trophies = new List<Sprite>();
    [SerializeField] Image trophy = default;
    [SerializeField] Image topBar = default;
    [SerializeField] Image bottomBar = default;
    [SerializeField] Star1Mechanics star1 = default;
    [SerializeField] Star2Mechanics star2 = default;
    public int trophyIndex = 0;
    public int trophyPanelMinScore = 0;
    public int trophyPanelMaxScore = 0;

    public int first = 300;
    public int second = 800; 
    public int addAfter = 1000;

    private int savedTotalPoints;

    public AnimationCurve easeCurve;
    public float fillDuration=1f;

    private bool isStart = false;

    private void Start() {
        savedTotalPoints = 0;
    }
    
    public void UpdateTrophyPanel() {
        int totalPoints = GameDataManager.GDM.TotalPoints_AllTime;//50
        if  (totalPoints == 0) {
            CalculateTrophyPanelMinAndMaxScore();
            SetTrophyImage();
        }

        else if (totalPoints != savedTotalPoints) {
            savedTotalPoints = totalPoints;
            Debug.Log("Updated Trophy Panel");

            CalculateTrophyPanelMinAndMaxScore();
            SetTrophyImage();

            int topBarMax = trophyPanelMinScore + ((trophyPanelMaxScore - trophyPanelMinScore) / 2); //150
            if (totalPoints < topBarMax) {//50<150
                float fillNumber = (float)((float)(totalPoints - trophyPanelMinScore) / (float)(topBarMax - trophyPanelMinScore));//0.33333f

                //on first time just populate the bars
                if (!isStart) {
                    isStart = true;
                    topBar.fillAmount = fillNumber;
                }
                else {
                    StartCoroutine(MoveOverTime(topBar, fillNumber));
                }
                


                bottomBar.fillAmount = 0f;
                Debug.Log("bot 0");

                star1.StarOff();
                star2.StarOff();

            }
            else {
                topBar.fillAmount = 1f;
                Debug.Log("top 1");
                star2.StarOn();

                if (totalPoints < trophyPanelMaxScore) {
                    float fillNumber = (float)((float)(totalPoints - topBarMax) / (float)(trophyPanelMaxScore - topBarMax));


                    //on first time just populate the bars
                    if (!isStart) {
                        isStart = true;
                        bottomBar.fillAmount = fillNumber;
                    }
                    else {
                        StartCoroutine(MoveOverTime(bottomBar, fillNumber));
                    }
                    star1.StarOff();



                }
                else {
                    bottomBar.fillAmount = 1f;
                    star1.StarOn();
                    trophyIndex++;
                    PlayerPrefs.SetInt("TrophyIndex", trophyIndex);
                }
            }
        }
        
    }

    private void SetTrophyImage() {
        trophy.sprite = trophies[trophyIndex];
        List<string> trophyColorList = new List<string> { "Trophy1", "Trophy2", "Trophy3" };
        trophy.GetComponent<CollectionColor_Image>().key = trophyColorList[trophyIndex % 3];
        trophy.GetComponent<CollectionColor_Image>().GetColor();

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
            //trophyPanelMaxScore = (trophyIndex * 900) + ((trophyIndex - 2) * 100);
            trophyPanelMinScore = trophyPanelMaxScore - (addAfter);
        }

    }


    IEnumerator MoveOverTime(Image bar, float targetFillNumber) {
        Debug.Log(bar.name + " filling up");
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
    }
}

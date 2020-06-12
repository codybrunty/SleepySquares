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
    [SerializeField] CollectionColor_Image star1 = default;
    [SerializeField] CollectionColor_Image star2 = default;
    public int trophyIndex = 0;
    public int trophyPanelMinScore = 0;
    public int trophyPanelMaxScore = 0;

    public int first = 300;
    public int second = 800; 
    public int addAfter = 1000;

    private int savedTotalPoints;

    private void Start() {
        savedTotalPoints = 0;
    }

    public void UpdateTrophyPanel() {
        int totalPoints = GameDataManager.GDM.TotalPoints_AllTime;//50
        if (totalPoints != savedTotalPoints || totalPoints == 0) {
            savedTotalPoints = totalPoints;
            Debug.Log("Updated Trophy Panel");

            CalculateTrophyPanelMinAndMaxScore();
            SetTrophyImage();

            int topBarMax = trophyPanelMinScore + ((trophyPanelMaxScore - trophyPanelMinScore) / 2); //150
            if (totalPoints < topBarMax) {//50<150
                float fillNumber = (float)((float)(totalPoints - trophyPanelMinScore) / (float)(topBarMax - trophyPanelMinScore));//0.33333f
                topBar.fillAmount = fillNumber;
                bottomBar.fillAmount = 0f;
                star1.key = "Second";
                star1.GetColor();
                star2.key = "Second";
                star2.GetColor();
            }
            else {
                topBar.fillAmount = 1f;
                star2.key = "Trophy3";
                star2.GetColor();

                if (totalPoints < trophyPanelMaxScore) {
                    float fillNumber = (float)((float)(totalPoints - topBarMax) / (float)(trophyPanelMaxScore - topBarMax));
                    bottomBar.fillAmount = fillNumber;
                    star1.key = "Second";
                    star1.GetColor();

                }
                else {
                    bottomBar.fillAmount = 1f;
                    star1.key = "Trophy3";
                    star1.GetColor();
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
}

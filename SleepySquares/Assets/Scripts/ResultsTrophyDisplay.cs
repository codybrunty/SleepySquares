using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ResultsTrophyDisplay : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameboard = default;

    [SerializeField] TextMeshProUGUI profileLevel = default;
    [SerializeField] List<Sprite> trophySprites = new List<Sprite>();
    [SerializeField] Image trophy = default;
    [SerializeField] Image bar = default;
    [SerializeField] GameObject star1 = default;
    [SerializeField] GameObject star2 = default;
    [SerializeField] GameObject trophyEffect = default;
    [SerializeField] GameObject animationGRP = default;
    [SerializeField] TrophyRewardPopUp popup1 = default;
    [SerializeField] TrophyRewardPopUp popup2 = default;
    [SerializeField] SwitchButton switchButton = default;
    public Vector3 shakeMag;

    public float fillDuration = 1f;
    public AnimationCurve ease;

    private int trophyIndex;
    public int trophyPoints;

    private int barMin;
    public int barMax;
    private float star1_pos = .25f;
    private float star2_pos = .75f;

    private CollectionColor_Image star1_ccimage;
    private CollectionColor_Image star2_ccimage;
    private CollectionColor_Image trophy_ccimage;


    private List<int> trophyLevelScores = new List<int> {   0,      150,    500,    1500,   2500,
                                                            5000,   7500,   10000,  12500,  15000,
                                                            18000,  21000,  24000,  27000,  30000,
                                                            35000,  40000,  45000,  50000,  60000,
                                                            70000,  80000,  90000,  105000,  120000,
                                                            135000,  150000, 175000, 200000, 225000};

    
    /*
    //practice trophy system score
    private List<int> trophyLevelScores = new List<int> {   0,      10,    20,    30,   40,
                                                            50,   60,   70,  80,  90,
                                                            100,  110,  120,  130,  140,
                                                            150,  160,  170,  180,  190,
                                                            200,  210,  220,  230,  240,
                                                            250,  260, 270, 280, 290};
    
    */

    private Vector3 position;

    private void OnEnable()
    {
        SetCollection();
        position = gameObject.transform.position;
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);

        StartCoroutine(ScaleUp());

        trophyIndex = PlayerPrefs.GetInt("TrophyIndex", 0);
        trophyPoints = PlayerPrefs.GetInt("TrophyPoints", 0);
        //Debug.Log("Start Trophy Level = " + (trophyIndex + 1).ToString());
        //Debug.Log("Start Trophy Points = " + (trophyPoints).ToString());


        SetTrophyDisplay();
        AnimateTrophyDisplay();


        //Debug.Log("Final Trophy Level = " + (trophyIndex + 1).ToString());
        //Debug.Log("Final Trophy Points = " + (trophyPoints).ToString());
        PlayerPrefs.SetInt("TrophyIndex", trophyIndex);
        PlayerPrefs.SetInt("TrophyPoints", trophyPoints);

        StartCoroutine(ScaleDown());
    }

    private void SetCollection() {
        star1_ccimage= star1.GetComponent<CollectionColor_Image>();
        star2_ccimage= star2.GetComponent<CollectionColor_Image>();
        trophy_ccimage= trophy.GetComponent<CollectionColor_Image>();
    }

    IEnumerator ScaleUp()
    {
        yield return new WaitForSeconds(0f);
        Vector3 newScale = new Vector3(3f, 3f, 3f);
        iTween.ScaleTo(gameObject, newScale, .65f);
    }

    IEnumerator ScaleDown()
    {
        float waitTime = 2f;

        if (trophyPoints==barMax)
        {
            waitTime = 3.5f;
        }
        yield return new WaitForSeconds(waitTime);
        Vector3 newScale = new Vector3(1f, 1f, 1f);
        iTween.ScaleTo(gameObject, newScale, .5f);
        iTween.MoveTo(gameObject, position, .5f);
    }

    #region Animate Trophy Display
    private void AnimateTrophyDisplay()
    {
        float oldFillNumber = (float)(trophyPoints - barMin) / (float)(barMax - barMin);


        trophyPoints += gameboard.score;
        if (trophyPoints >= barMax)
        {
            trophyPoints = barMax;
            trophyIndex++;
            StartCoroutine(LevelUpTrophyDisplay());
        }

        float newFillNumber = (float)(trophyPoints - barMin) / (float)(barMax - barMin);

        StartCoroutine(MoveBarOverTime(oldFillNumber, newFillNumber));
        AnimateNewStars(oldFillNumber, newFillNumber);
    }

    IEnumerator LevelUpTrophyDisplay()
    {
        yield return new WaitForSeconds(fillDuration + .35f /*+ 0.75f*/);
        //ShakeTrophyGroup();
        //yield return new WaitForSeconds(0.4f);
        trophyEffect.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        SoundManager.SM.PlayOneShotSound("newTrophy");
        SetTrophyDisplay();
        PunchScaleGroup();
    }

    private void PunchScaleGroup()
    {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1.5f, 1.5f, 0f));
        hash.Add("time", .75f);
        hash.Add("oncomplete", "TurnStarEffectOff");
        hash.Add("onCompleteTarget", gameObject);
        iTween.PunchScale(animationGRP, hash);
    }

    public void TurnStarEffectOff()
    {
        StartCoroutine(TurnOffEffect());
    }

    IEnumerator TurnOffEffect()
    {
        yield return new WaitForSeconds(1f);
        trophyEffect.SetActive(false);
    }

    private void ShakeTrophyGroup()
    {
        Hashtable hash = new Hashtable();
        hash.Add("amount", shakeMag);
        hash.Add("time", 1f);
        iTween.ShakePosition(animationGRP, hash);
    }

    IEnumerator MoveBarOverTime(float oldFill, float newFill)
    {
        //time it takes the menu to open
        yield return new WaitForSeconds(0.5f);

        for (float t = 0f; t < fillDuration; t+=Time.deltaTime)
        {
            bar.fillAmount = Mathf.Lerp(oldFill, newFill, ease.Evaluate(t / fillDuration));
            yield return null;
        }
        bar.fillAmount = newFill;
    }


    IEnumerator PlayStar1(float oldFill, float newFill)
    {
        yield return new WaitForSeconds(0.5f);
        float ammount = 0f;
        for (float t = 0f; t < fillDuration; t += Time.deltaTime)
        {
            ammount = Mathf.Lerp(oldFill, newFill, ease.Evaluate(t / fillDuration));
            //Debug.Log("ammount: "+ammount);
            if (ammount > 0.25f)
            {
                break;
            }
            yield return null;
        }
        PopStar1();
        yield return new WaitForSeconds(0.1f);
        FlashText1();
    }

    IEnumerator PlayStar2(float oldFill, float newFill)
    {
        yield return new WaitForSeconds(0.5f);
        float ammount = 0f;
        for (float t = 0f; t < fillDuration; t += Time.deltaTime)
        {
            //Debug.Log("ammount: " + ammount);
            ammount = Mathf.Lerp(oldFill, newFill, ease.Evaluate(t / fillDuration));
            if (ammount > 0.75f)
            {
                break;
            }
            yield return null;
        }
        PopStar2();
        yield return new WaitForSeconds(0.1f);
        FlashText2();
    }

    private void FlashText1()
    {
        popup1.RewardPopUp();
        //SoundManager.SM.PlayOneShotSound("yahoo");
        switchButton.AddSwitchesNoAnimation(2);
    }

    private void FlashText2()
    {
        popup2.RewardPopUp();
        //SoundManager.SM.PlayOneShotSound("yahoo");
        switchButton.AddSwitchesNoAnimation(2);
    }

    private void PopStar1()
    {
        Debug.Log("pop1");
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2f, 2f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(star1, hash);
        SoundManager.SM.PlayOneShotSound("star");
        star1_ccimage.key = "Trophy3";
        star1_ccimage.GetColor();
    }

    private void PopStar2()
    {
        Debug.Log("pop2");
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2f, 2f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(star2, hash);
        SoundManager.SM.PlayOneShotSound("star");
        star2_ccimage.key = "Trophy3";
        star2_ccimage.GetColor();
    }

    private void AnimateNewStars(float oldFill, float newFill)
    {
        if (oldFill < .75f && newFill > .75f)
        {
            StartCoroutine(PlayStar2(oldFill,newFill));
        }
        if (oldFill < .25f && newFill > .25f)
        {
            StartCoroutine(PlayStar1(oldFill, newFill));
        }
    }

    #endregion
    #region Trophy Display

    private void SetTrophyDisplay()
    {
        SetProfileLevel();
        SetTrophyImage();
        SetTrophyColor();
        SetGoldBarPosition();
        SetStarColor();
    }

    private void SetStarColor()
    {
        if (bar.fillAmount > star1_pos)
        {
            star1_ccimage.key = "Trophy3";
            star1_ccimage.GetColor();
        }
        else
        {
            star1_ccimage.key = "Second";
            star1_ccimage.GetColor();
        }

        if (bar.fillAmount > star2_pos)
        {
            star2_ccimage.key = "Trophy3";
            star2_ccimage.GetColor();
        }
        else
        {
            star2_ccimage.key = "Second";
            star2_ccimage.GetColor();
        }
    }

    private void SetGoldBarPosition()
    {
        if (trophyIndex >= 29)
        {
            int barLength = (trophyLevelScores[trophyLevelScores.Count - 1] - trophyLevelScores[trophyLevelScores.Count - 2]) * 2;
            int multiplier = trophyIndex - 29;

            barMin = trophyLevelScores[trophyLevelScores.Count - 1] + barLength*multiplier;
            barMax = trophyLevelScores[trophyLevelScores.Count - 1] + barLength*(multiplier+1);

        }
        else
        {
            barMin = trophyLevelScores[trophyIndex];
            barMax = trophyLevelScores[trophyIndex + 1];
        }

        trophyPoints = PlayerPrefs.GetInt("TrophyPoints", 0);
        float fillNumber = (float)(trophyPoints - barMin) / (float)(barMax - barMin);
        bar.fillAmount = fillNumber;

        //Debug.LogWarning( "Trophy Index: " + trophyIndex + " BarMin: " + barMin + " BarMax: " + barMax);
    }

    private void SetProfileLevel()
    {
        profileLevel.text = (trophyIndex + 1).ToString();
    }

    private void SetTrophyImage()
    {
        if (trophyIndex >= 29)
        {
            trophy.sprite = trophySprites[29];
        }
        else
        {
            trophy.sprite = trophySprites[trophyIndex];
        }
    }

    private void SetTrophyColor()
    {
        List<string> trophyColorList = new List<string> { "Trophy1", "Trophy2", "Trophy3" };

        if (trophyIndex >= 29)
        {
            trophy_ccimage.key = trophyColorList[2];
        }
        else
        {
            trophy_ccimage.key = trophyColorList[trophyIndex % 3];
        }
        trophy_ccimage.GetColor();
    }

    #endregion
}

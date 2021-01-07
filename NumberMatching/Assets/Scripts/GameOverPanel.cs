using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameOverPanel : MonoBehaviour{

    public long score = default;
    public long highscore = default;
    [SerializeField] TextMeshProUGUI text_score = default;
    [SerializeField] TextMeshProUGUI text_highscore = default;
    [SerializeField] Image bg = default;
    [SerializeField] GameObject gameOverPanelGRP = default;
    [SerializeField] GameObject panel_bg = default;
    [SerializeField] ResultsTrophyDisplay trophyDisplay = default;
    [SerializeField] GameObject gameOverEffect = default;
    [SerializeField] Sprite bg_normal = default;
    [SerializeField] Sprite bg_hard = default;
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] Image resultsScren = default;

    private float fadeInDuration = .5f;
    public AnimationCurve ease;

    private RectTransform gameOverPanelGRP_rt;
    private RectTransform gameOverEffect_rt;

    private void Awake() {
        gameOverPanelGRP_rt = gameOverPanelGRP.GetComponent<RectTransform>();
        gameOverEffect_rt = gameOverEffect.GetComponent<RectTransform>();
    }

    private void OnEnable() {
        ResetImageAlphaToZero();
        StartCoroutine(FadeInAlpha());
        SetBGImage();
    }

    private void SetBGImage() {
        if (gameboard.hardModeOn == 1 && gameboard.DailyModeOn==false) {
            resultsScren.sprite = bg_hard;
        }
        else {
            resultsScren.sprite = bg_normal;
        }
    }

    IEnumerator FadeInAlpha() {
        float endAlpha = 152f/255f;
        Color color = bg.color;

        for (float t = 0f; t < fadeInDuration; t += Time.deltaTime) {
            float normalizedTime = t / fadeInDuration;
            float alpha = Mathf.Lerp(0f, endAlpha, ease.Evaluate(normalizedTime));
            bg.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        bg.color = new Color(color.r, color.g, color.b, endAlpha);

    }

    private void ResetImageAlphaToZero() {
        Color color = bg.color;
        bg.color = new Color(color.r, color.g, color.b, 0f);
    }

    public void UpdateGameOverPanel() {
        text_score.text = score.ToString();
        text_highscore.text = highscore.ToString();
    }

    public void GameOverPanelAnimation()
    {
        StartCoroutine(AnimationSequence());
    }

    IEnumerator AnimationSequence()
    {
        panel_bg.SetActive(true);


        yield return new WaitForSeconds(.25f);
        //add some time if trophy pops to next level
        float waitTime = 0f;
        if (trophyDisplay.trophyPoints == trophyDisplay.barMax)
        {
            waitTime = 1.5f;
        }



        yield return new WaitForSeconds(1.75f);
        yield return new WaitForSeconds(waitTime);

        ScalePanel();
        ScaleEffect();
        gameOverEffect.SetActive(true);
        SoundManager.SM.PlayOneShotSound("GameOver");

        //MusicManager.MM.FadeOutCurrentMusic();
    }

    private void ScalePanel()
    {
        Hashtable hash = new Hashtable();
        hash.Add("scale", new Vector3(1f, 1f, 1f));
        hash.Add("time", .5f);
        iTween.ScaleTo(gameOverPanelGRP, hash);
    }

    private void ScaleEffect()
    {
        Hashtable hash = new Hashtable();
        hash.Add("scale", new Vector3(800f, 800f, 800f));
        hash.Add("time", .5f);
        iTween.ScaleTo(gameOverEffect, hash);
    }

    public void ResetGameOverPanelScale() {
        panel_bg.SetActive(false);
        gameOverPanelGRP_rt.localScale = new Vector3(.01f, .01f, 1f);
        gameOverEffect_rt.localScale = new Vector3(1f, 1f, 1f);
        gameOverEffect.SetActive(false);
    }
}

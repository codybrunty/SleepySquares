using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Tutorial_Instructions_17 : MonoBehaviour{

    [SerializeField] GameObject square_GO = default;
    [SerializeField] SpriteRenderer square_body = default;
    [SerializeField] GameObject face = default;
    [SerializeField] Color greenColor = default;
    [SerializeField] GameObject clickNext = default;
    [SerializeField] GameObject nextSquare = default;

    [SerializeField] GameObject square1eyelid_closing = default;
    [SerializeField] GameObject square1eyelid = default;
    [SerializeField] GameObject closeEye = default;

    [SerializeField] GameObject square1eyelid_closing2 = default;
    [SerializeField] GameObject square1eyelid2 = default;
    [SerializeField] GameObject closeEye2 = default;

    [SerializeField] List<GameObject> clear_squares_GO = new List<GameObject>();
    [SerializeField] List<GameObject> clear_faces = new List<GameObject>();
    [SerializeField] List<SpriteRenderer> clear_body = new List<SpriteRenderer>();
    [SerializeField] Color bodyColor = default;
    [SerializeField] TextMeshProUGUI scoreboardText = default;
    [SerializeField] FloatingText scoreboardPlus = default;
    [SerializeField] GameObject scoreboard = default;

    [SerializeField] Image fill = default;
    [SerializeField] AnimationCurve ease = default;
    [SerializeField] TextMeshProUGUI clearsText = default;
    [SerializeField] GameObject flashyButton = default;
    [SerializeField] GameObject clearButton = default;


    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial17_Animations());
    }

    IEnumerator Tutorial17_Animations() {
        PunchGreen();
        PunchNextSquare();
        yield return new WaitForSeconds(1f);
        CloseEyelid1();
        yield return new WaitForSeconds(.1f);
        CloseEyelid2();
        yield return new WaitForSeconds(1f);
        ClearBoard();
        PunchScoreboard();
        UpdateScore();
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard2");
        yield return new WaitForSeconds(.75f);
        StartCoroutine(FillClearButton());
        yield return new WaitForSeconds(2.75f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        flashyButton.SetActive(true);
    }

    private void PopClearButton() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(0.5f, 0.5f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(clearButton, hash);
    }

    IEnumerator FillClearButton() {

        float currentFillNumber = fill.fillAmount;
        float fillDuration = 0.5f;

        for (float t = 0f; t < fillDuration; t += Time.deltaTime) {
            float normalizedTime = t / fillDuration;
            fill.fillAmount = Mathf.Lerp(currentFillNumber, 0f, ease.Evaluate(normalizedTime));
            yield return null;
        }

        fill.fillAmount = 0f;
        clearsText.text = "1";
        PopClearButton();
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearReady1");

    }

    private void PunchScoreboard() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(0.5f, 0.5f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(scoreboard, hash);
    }

    private void UpdateScore() {
        scoreboardPlus.GetComponent<TextMeshProUGUI>().text = "+4";
        scoreboardPlus.FlashText();
        scoreboardText.text = "18";
    }

    private void ClearBoard() {
        for (int i = 0; i < clear_squares_GO.Count; i++) {
            clear_faces[i].SetActive(false);
            PunchSquare(clear_squares_GO[i]);
            clear_body[i].color = bodyColor;
        }
    }

    private void PunchSquare(GameObject square) {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(square, hash);
    }

    private void CloseEyelid2() {
        square1eyelid_closing2.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(4.016245f, 4.219735f, 0f));
        hash.Add("time", 0.25f);
        hash.Add("oncomplete", "SwitchEyelidGraphics2");
        hash.Add("oncompletetarget", gameObject);

        Hashtable hash2 = new Hashtable();
        hash2.Add("scale", new Vector3(0.3897383f, .5148228f, 1f));
        hash2.Add("time", 0.25f);



        iTween.MoveTo(square1eyelid_closing2, hash);
        iTween.ScaleTo(square1eyelid_closing2, hash2);
    }

    private void SwitchEyelidGraphics2() {
        closeEye2.SetActive(false);
        square1eyelid_closing2.SetActive(false);
        square1eyelid2.SetActive(true);
    }

    private void CloseEyelid1() {
        square1eyelid_closing.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(3.773235f, 5.0209f, 0f));
        hash.Add("time", 0.25f);
        hash.Add("oncomplete", "SwitchEyelidGraphics");
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(square1eyelid_closing, hash);
    }

    private void SwitchEyelidGraphics() {
        closeEye.SetActive(false);
        square1eyelid_closing.SetActive(false);
        square1eyelid.SetActive(true);
    }

    private void PunchGreen() {
        square_body.color = greenColor;
        face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(square_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster1");
    }

    private void PunchNextSquare() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(nextSquare, hash);
    }

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(4.508f, -3.371f, 5f);
        clickNext.transform.localScale = new Vector3(1.904417f, 1.89173f, 3.987f);
    }
}

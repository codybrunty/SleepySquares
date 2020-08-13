using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial_Instructions_6 : MonoBehaviour{

    [SerializeField] GameObject square_GO = default;
    [SerializeField] SpriteRenderer square_body = default;
    [SerializeField] GameObject face = default;
    [SerializeField] Color greenColor = default;
    [SerializeField] GameObject clickNext = default;

    [SerializeField] GameObject square1eyelid_closing = default;
    [SerializeField] GameObject square1eyelid = default;
    [SerializeField] GameObject closeEye = default;

    [SerializeField] GameObject square1eyelid_closing2 = default;
    [SerializeField] GameObject square1eyelid2 = default;
    [SerializeField] GameObject closeEye2 = default;

    [SerializeField] GameObject scoreboard = default;

    [SerializeField] List<GameObject> clear_squares_GO = new List<GameObject>();
    [SerializeField] List<GameObject> clear_faces = new List<GameObject>();
    [SerializeField] List<SpriteRenderer> clear_body = new List<SpriteRenderer>();
    [SerializeField] Color bodyColor = default;
    [SerializeField] TextMeshProUGUI scoreboardText = default;
    [SerializeField] FloatingText scoreboardPlus = default;

    [SerializeField] GameObject nextSquare = default;
    [SerializeField] GameObject instruct1 = default;
    [SerializeField] GameObject instruct2 = default;
    [SerializeField] GameObject flashyButton = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial6_Animations());
    }

    IEnumerator Tutorial6_Animations() {
        PunchGreen();
        PunchNextSquare();
        yield return new WaitForSeconds(1f);
        CloseEyelid1();
        yield return new WaitForSeconds(.1f);
        CloseEyelid2();
        yield return new WaitForSeconds(.25f);
        MoveScoreBoard();
        yield return new WaitForSeconds(2.5f);
        ClearBoard();
        PunchScoreboard();
        UpdateScore();
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard1");
        yield return new WaitForSeconds(1.5f);
        ChangeInstructions();
        yield return new WaitForSeconds(2f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        flashyButton.SetActive(true);
    }

    private void ChangeInstructions() {
        instruct1.SetActive(false);
        instruct2.SetActive(true);
    }

    private void PunchNextSquare() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(nextSquare, hash);
    }

    private void UpdateScore() {
        scoreboardPlus.GetComponent<TextMeshProUGUI>().text = "+6";
        scoreboardPlus.FlashText();
        scoreboardText.text = "6";
    }

    private void ClearBoard() {
        for(int i = 0; i < clear_squares_GO.Count; i++) {
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

    private void PunchScoreboard() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(0.5f, 0.5f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(scoreboard, hash);
    }

    private void MoveScoreBoard() {
        Vector3 scorePosition = scoreboard.transform.position;
        Vector3 hidePosition = new Vector3(scorePosition.x + -500, scorePosition.y, scorePosition.z);
        scoreboard.transform.position = hidePosition;

        scoreboard.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("position", scorePosition);
        hash.Add("time", 1f);
        iTween.MoveTo(scoreboard, hash);
    }

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(0.1416f, 5.318f, 5f);
        clickNext.transform.localScale = new Vector3(.7113212f, .9581257f, 3.987f);
    }

    private void CloseEyelid2() {
        square1eyelid_closing2.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(0.1938766f, 3.809871f, 0f));
        hash.Add("time", 0.25f);
        hash.Add("oncomplete", "SwitchEyelidGraphics2");
        hash.Add("oncompletetarget", gameObject);

        Hashtable hash2 = new Hashtable();
        hash2.Add("scale", new Vector3(0.8332989f, 0.556218f, 1f));
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
        hash.Add("position", new Vector3(1.589539f, 3.449951f, 0f));
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
}

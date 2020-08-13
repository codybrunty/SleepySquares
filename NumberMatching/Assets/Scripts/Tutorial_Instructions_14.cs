using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial_Instructions_14 : MonoBehaviour{

    [SerializeField] GameObject clickNext = default;
    [SerializeField] GameObject nextSquare = default;
    [SerializeField] GameObject square_GO = default;
    [SerializeField] SpriteRenderer square_body = default;
    [SerializeField] GameObject face = default;
    [SerializeField] GameObject oldFace = default;
    [SerializeField] Color redColor = default;

    [SerializeField] GameObject square1eyelid_closing = default;
    [SerializeField] GameObject square1eyelid = default;
    [SerializeField] GameObject closeEye1 = default;

    [SerializeField] GameObject square2eyelid_closing = default;
    [SerializeField] GameObject square2eyelid = default;
    [SerializeField] GameObject closeEye2 = default;

    [SerializeField] GameObject square3eyelid_closing = default;
    [SerializeField] GameObject square3eyelid = default;
    [SerializeField] GameObject closeEye3 = default;

    [SerializeField] GameObject square4eyelid_closing = default;
    [SerializeField] GameObject square4eyelid = default;
    [SerializeField] GameObject closeEye4 = default;

    [SerializeField] List<GameObject> clear_squares_GO = new List<GameObject>();
    [SerializeField] List<GameObject> clear_faces = new List<GameObject>();
    [SerializeField] List<SpriteRenderer> clear_body = new List<SpriteRenderer>();
    [SerializeField] Color bodyColor = default;
    [SerializeField] TextMeshProUGUI scoreboardText = default;
    [SerializeField] FloatingText scoreboardPlus = default;
    [SerializeField] GameObject scoreboard = default;
    [SerializeField] GameObject flashyButton = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial14_Animations());
    }

    IEnumerator Tutorial14_Animations() {
        PunchRed();
        PunchNextSquare();
        yield return new WaitForSeconds(1f);
        CloseEyeLid1();
        CloseEyeLid2();
        yield return new WaitForSeconds(.1f);
        CloseEyeLid3();
        CloseEyeLid4();
        yield return new WaitForSeconds(4f);
        ClearBoard();
        PunchScoreboard();
        UpdateScore();
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearboard1");
        yield return new WaitForSeconds(2f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        yield return new WaitForSeconds(1f);
        flashyButton.SetActive(true);
    }

    private void PunchScoreboard() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(0.5f, 0.5f, 0f));
        hash.Add("time", .75f);
        iTween.PunchScale(scoreboard, hash);
    }

    private void UpdateScore() {
        scoreboardPlus.GetComponent<TextMeshProUGUI>().text = "+8";
        scoreboardPlus.FlashText();
        scoreboardText.text = "14";
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

    private void CloseEyeLid1() {
        square1eyelid_closing.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(-.2590159f, 5.298213f, 0f));
        hash.Add("time", 0.25f);
        hash.Add("oncomplete", "SwitchEyelidGraphics");
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(square1eyelid_closing, hash);
    }

    private void SwitchEyelidGraphics() {
        closeEye1.SetActive(false);
        square1eyelid_closing.SetActive(false);
        square1eyelid.SetActive(true);
    }

    private void CloseEyeLid2() {
        square2eyelid_closing.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(0.5734866f, 5.327753f, 0f));
        hash.Add("time", 0.25f);
        hash.Add("oncomplete", "SwitchEyelidGraphics2");
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(square2eyelid_closing, hash);
    }

    private void SwitchEyelidGraphics2() {
        closeEye2.SetActive(false);
        square2eyelid_closing.SetActive(false);
        square2eyelid.SetActive(true);
    }

    private void CloseEyeLid3() {
        square3eyelid_closing.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(1.561563f, 5.326015f, 0f));
        hash.Add("time", 0.25f);
        hash.Add("oncomplete", "SwitchEyelidGraphics3");
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(square3eyelid_closing, hash);
    }

    private void SwitchEyelidGraphics3() {
        closeEye3.SetActive(false);
        square3eyelid_closing.SetActive(false);
        square3eyelid.SetActive(true);
    }

    private void CloseEyeLid4() {
        square4eyelid_closing.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(-.2880346f, 3.484759f, 0f));
        hash.Add("time", 0.25f);
        hash.Add("oncomplete", "SwitchEyelidGraphics4");
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(square4eyelid_closing, hash);
    }

    private void SwitchEyelidGraphics4() {
        closeEye4.SetActive(false);
        square4eyelid_closing.SetActive(false);
        square4eyelid.SetActive(true);
    }

    private void PunchRed() {
        square_body.color = redColor;
        oldFace.SetActive(false);
        face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(square_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster2");
    }

    private void PunchNextSquare() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(nextSquare, hash);
    }

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(1.894f, 2.327f, 5f);
        clickNext.transform.localScale = new Vector3(3.081023f, 7.206347f, 3.987f);
    }
}

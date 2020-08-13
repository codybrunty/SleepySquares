using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Instructions_4 : MonoBehaviour{

    [SerializeField] GameObject square_GO = default;
    [SerializeField] SpriteRenderer square_body = default;
    [SerializeField] GameObject face = default;
    [SerializeField] Color redColor = default;

    [SerializeField] GameObject square1eyelid_closing = default;
    [SerializeField] GameObject square1eyelid = default;
    [SerializeField] GameObject closeEye = default;

    [SerializeField] GameObject square1eyelid_closing2 = default;
    [SerializeField] GameObject square1eyelid2 = default;

    [SerializeField] GameObject instruct1 = default;
    [SerializeField] GameObject instruct2 = default;
    [SerializeField] GameObject closeEye2 = default;

    [SerializeField] GameObject nextSquare = default;


    [SerializeField] GameObject clickNext = default;
    [SerializeField] GameObject flashyButton = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial4_Animations());
    }

    IEnumerator Tutorial4_Animations() {

        PunchRed();
        PunchNextSquare();
        yield return new WaitForSeconds(1f);
        CloseEyelid1();
        yield return new WaitForSeconds(.1f);
        CloseEyelid2();
        yield return new WaitForSeconds(5f);
        instruct1.SetActive(false);
        instruct2.SetActive(true);
        yield return new WaitForSeconds(3f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        flashyButton.SetActive(true);
    }

    private void PunchNextSquare() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(nextSquare, hash);
    }

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(3.816f, 1.634f, 5f);
        clickNext.transform.localScale = new Vector3(.6956572f, .9876481f, 3.987f);
    }

    private void CloseEyelid2() {
        square1eyelid_closing2.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(3.41116f, 3.439575f, 0f));
        hash.Add("time", 0.25f);
        hash.Add("oncomplete", "SwitchEyelidGraphics2");
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(square1eyelid_closing2, hash);
    }

    private void SwitchEyelidGraphics2() {
        closeEye2.SetActive(false);
        square1eyelid_closing2.SetActive(false);
        square1eyelid2.SetActive(true);
    }

    private void CloseEyelid1() {
        square1eyelid_closing.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(2.384856f, 3.439575f, 0f));
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

    private void PunchRed() {
        square_body.color = redColor;
        face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(square_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster2");
    }

}

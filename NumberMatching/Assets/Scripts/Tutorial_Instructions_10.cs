using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Instructions_10 : MonoBehaviour{

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
    [SerializeField] GameObject flashyButton = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial10_Animations());
    }

    IEnumerator Tutorial10_Animations() {
        PunchGreen();
        PunchNextSquare();
        yield return new WaitForSeconds(1f);
        CloseEyelid1();
        yield return new WaitForSeconds(.1f);
        CloseEyelid2();
        yield return new WaitForSeconds(.5f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        flashyButton.SetActive(true);
    }

    private void CloseEyelid2() {
        square1eyelid_closing2.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(3.802129f, 5.687098f, 0f));
        hash.Add("time", 0.25f);
        hash.Add("oncomplete", "SwitchEyelidGraphics2");
        hash.Add("oncompletetarget", gameObject);

        Hashtable hash2 = new Hashtable();
        hash2.Add("scale", new Vector3(.8636594f, .5181384f, 1f));
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
        hash.Add("position", new Vector3(2.402927f, 5.327753f, 0f));
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

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(.125f, 1.651f, 5f);
        clickNext.transform.localScale = new Vector3(.7418613f, .9647248f, 3.987f);
    }

    private void PunchNextSquare() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(nextSquare, hash);
    }
}

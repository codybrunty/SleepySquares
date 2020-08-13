using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Instructions_18 : MonoBehaviour{


    [SerializeField] GameObject instruct1 = default;
    [SerializeField] GameObject instruct2 = default;
    [SerializeField] GameObject instruct3 = default;
    [SerializeField] GameObject instruct4 = default;

    [SerializeField] Color bodyColor = default;
    [SerializeField] GameObject clickNext = default;
    [SerializeField] List<GameObject> clear_squares_GO = new List<GameObject>();
    [SerializeField] List<SpriteRenderer> clear_body = new List<SpriteRenderer>();
    [SerializeField] Image fill = default;
    [SerializeField] AnimationCurve ease = default;


    [SerializeField] Color purpleColor = default;
    [SerializeField] GameObject square_GO = default;
    [SerializeField] SpriteRenderer square_body = default;
    [SerializeField] GameObject face = default;

    [SerializeField] Image next1 = default;
    [SerializeField] Image next2 = default;
    [SerializeField] Image next3 = default;
    [SerializeField] Color next1Color = default;
    [SerializeField] Color next2Color = default;
    [SerializeField] Color next3Color = default;
    [SerializeField] GameObject flashyButton = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial8_Animations());
    }

    IEnumerator Tutorial8_Animations() {
        ClearBoard();
        StartCoroutine(EmptyClearButton());
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearBlockers");
        yield return new WaitForSeconds(3.5f);
        ChangeInstructions();
        yield return new WaitForSeconds(5f);
        PunchPurple();
        ChangeNextBoard();
        yield return new WaitForSeconds(1.5f);
        ChangeInstructions2();
        yield return new WaitForSeconds(6.5f);
        ChangeInstructions3();
        yield return new WaitForSeconds(3f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        flashyButton.SetActive(true);
   
}

    private void ChangeNextBoard() {
        PunchNextSquare();
        next1.color = next1Color;
        next2.color = next2Color;
        next3.color = next3Color;
    }

    private void PunchNextSquare() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(next1.gameObject, hash);
    }

    private void ChangeInstructions() {
        instruct1.SetActive(false);
        instruct2.SetActive(true);
    }
    private void ChangeInstructions2() {
        instruct2.SetActive(false);
        instruct3.SetActive(true);
    }
    private void ChangeInstructions3() {
        instruct3.SetActive(false);
        instruct4.SetActive(true);
    }

    private void PunchPurple() {
        square_body.color = purpleColor;
        face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(square_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster3");
    }

    IEnumerator EmptyClearButton() {

        float currentFillNumber = fill.fillAmount;
        float fillDuration = 0.5f;

        for (float t = 0f; t < fillDuration; t += Time.deltaTime) {
            float normalizedTime = t / fillDuration;
            fill.fillAmount = Mathf.Lerp(currentFillNumber, 1f, ease.Evaluate(normalizedTime));
            yield return null;
        }

        fill.fillAmount = 1f;

    }

    private void ClearBoard() {
        for (int i = 0; i < clear_squares_GO.Count; i++) {
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

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(0.9906f, 2.9985f, 5f);
        clickNext.transform.localScale = new Vector3(.3897632f, .5147362f, 3.987f);
    }

}

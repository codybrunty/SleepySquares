using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Instructions_7 : MonoBehaviour{

    [SerializeField] GameObject square_GO = default;
    [SerializeField] SpriteRenderer square_body = default;
    [SerializeField] GameObject face = default;
    [SerializeField] Color purpleColor = default;
    [SerializeField] GameObject clickNext = default;
    [SerializeField] GameObject nextSquare = default;
    [SerializeField] GameObject flashyButton = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial7_Animations());
    }

    IEnumerator Tutorial7_Animations() {
        PunchPurple();
        PunchNextSquare();
        yield return new WaitForSeconds(1f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        flashyButton.SetActive(true);
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

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(1.979f, 5.318f, 5f);
        clickNext.transform.localScale = new Vector3(0.7067277f, 0.9581257f, 3.987f);
    }

    private void PunchNextSquare() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(nextSquare, hash);
    }

}

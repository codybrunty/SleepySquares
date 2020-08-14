using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Instructions_2 : MonoBehaviour{

    [SerializeField] GameObject green = default;
    [SerializeField] GameObject red = default;
    [SerializeField] GameObject purple = default;
    [SerializeField] GameObject gameboard = default;
    [SerializeField] GameObject nextBoard = default;
    [SerializeField] GameObject clickNext = default;
    [SerializeField] GameObject flashyButton = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial2_Animations());
    }

    IEnumerator Tutorial2_Animations() {
        MoveSquaresOffScreen();
        yield return new WaitForSeconds(1f);
        ScaleGameboard();
        MoveNextBoard();
        yield return new WaitForSeconds(3.5f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        flashyButton.SetActive(true);
    }

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(1.984f,3.495f,5f);
        clickNext.transform.localScale = new Vector3(0.7101183f,0.9537292f,3.987f);
    }

    private void MoveNextBoard() {
        Vector3 nextPosition = nextBoard.transform.position;
        Vector3 hidePosition = new Vector3(nextPosition.x + 500, nextPosition.y, nextPosition.z);
        nextBoard.transform.position = hidePosition;

        nextBoard.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("position", nextPosition);
        hash.Add("time", 1f);
        iTween.MoveTo(nextBoard, hash);
    }

    private void ScaleGameboard() {
        gameboard.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("scale", new Vector3(1f, 1f, 1f));
        hash.Add("time", 1f);

        foreach (Transform child in gameboard.transform) {
            iTween.ScaleTo(child.gameObject, hash);
        }
    }

    private void MoveSquaresOffScreen() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("swoosh");
        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(15f, 4.12f, 5f));
        hash.Add("time", 1f);
        iTween.MoveTo(green, hash);
        iTween.MoveTo(red, hash);
        iTween.MoveTo(purple, hash);
    }
}

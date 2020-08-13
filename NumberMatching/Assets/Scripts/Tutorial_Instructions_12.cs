using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Instructions_12 : MonoBehaviour{

    [SerializeField] GameObject instruct1 = default;
    [SerializeField] GameObject instruct2 = default;
    [SerializeField] GameObject clickNext = default;
    [SerializeField] GameObject swapButton = default;
    [SerializeField] GameObject rings = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial12_Animations());
    }

    IEnumerator Tutorial12_Animations() {
        yield return new WaitForSeconds(1f);
        MoveSwapButton();
        yield return new WaitForSeconds(4.5f);
        ChangeInstructions();
        yield return new WaitForSeconds(2.75f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        rings.SetActive(true);
    }

    private void MoveSwapButton() {

        Vector3 scorePosition = swapButton.transform.position;
        Vector3 hidePosition = new Vector3(scorePosition.x, scorePosition.y-250f, scorePosition.z);
        swapButton.transform.position = hidePosition;

        swapButton.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("position", scorePosition);
        hash.Add("time", 1f);
        iTween.MoveTo(swapButton, hash);
        
    }

    private void ChangeInstructions() {
        instruct1.SetActive(false);
        instruct2.SetActive(true);
    }
    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(-1.578f, -4.179f, 5f);
        clickNext.transform.localScale = new Vector3(2.983074f, 2.646954f, 3.987f);
    }
}

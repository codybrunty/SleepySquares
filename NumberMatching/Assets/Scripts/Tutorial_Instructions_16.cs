using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Instructions_16 : MonoBehaviour{

    [SerializeField] GameObject instruct1 = default;
    [SerializeField] GameObject instruct2 = default;
    [SerializeField] GameObject clickNext = default;
    [SerializeField] GameObject repairButton = default;
    [SerializeField] GameObject flashyButton = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial16_Animations());
    }

    IEnumerator Tutorial16_Animations() {
        RepairButtonMove();
        yield return new WaitForSeconds(5f);
        ChangeInstructions();
        yield return new WaitForSeconds(2f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        yield return new WaitForSeconds(1f);
        flashyButton.SetActive(true);
    }

    private void RepairButtonMove() {
        Vector3 repairPosition = repairButton.transform.position;
        Vector3 hidePosition = new Vector3(repairPosition.x, repairPosition.y - 250f, repairPosition.z);
        repairButton.transform.position = hidePosition;

        repairButton.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("position", repairPosition);
        hash.Add("time", 1f);
        iTween.MoveTo(repairButton, hash);
    }

    private void ChangeInstructions() {
        instruct1.SetActive(false);
        instruct2.SetActive(true);
    }

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(4.0079f, 3.996f, 5f);
        clickNext.transform.localScale = new Vector3(.3913784f, .5269324f, 3.987f);
    }

}

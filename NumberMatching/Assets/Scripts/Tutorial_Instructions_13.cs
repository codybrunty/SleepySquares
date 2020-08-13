using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Instructions_13 : MonoBehaviour{

    [SerializeField] GameObject clickNext = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial13_Animations());
        FindObjectOfType<SoundManager>().PlayOneShotSound("switchModeOn");
    }

    IEnumerator Tutorial13_Animations() {
        yield return new WaitForSeconds(3f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
    }

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(0.135f, 5.322f, 5f);
        clickNext.transform.localScale = new Vector3(.7329057f, .9682886f, 3.987f);
    }

}

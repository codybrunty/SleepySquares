using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayChangeSortingLayer : MonoBehaviour{

    public float waitTime = 1f;
    public int sortOrderResult = 1;

    private void OnEnable() {
        StartCoroutine(DelayChange());
    }

    IEnumerator DelayChange() {

        yield return new WaitForSeconds(waitTime);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortOrderResult;

    }


}

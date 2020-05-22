using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FloatingText : MonoBehaviour{
    public float moveTime = 0.5f;

    void Start() {
        Hashtable hash = new Hashtable();
        hash.Add("position", gameObject.transform.position + new Vector3(0f, 200f, 0f));
        hash.Add("time", moveTime);
        hash.Add("oncomplete", "AfterMove");
        iTween.MoveTo(gameObject, hash);

        Color newColor = new Color(gameObject.GetComponent<TextMeshProUGUI>().color.r, gameObject.GetComponent<TextMeshProUGUI>().color.g, gameObject.GetComponent<TextMeshProUGUI>().color.b, 1f);
        StartCoroutine(TweenAlpha(newColor, moveTime/5f));

    }


    private void AfterMove() {
        Hashtable hash = new Hashtable();
        hash.Add("scale", new Vector3(.1f, .1f, .1f));
        hash.Add("time", moveTime/2f);
        hash.Add("oncomplete", "DestroyFloatingText");
        iTween.ScaleTo(gameObject, hash);
        
    }

    private void DestroyFloatingText() {
        Destroy(gameObject);
    }

    IEnumerator TweenAlpha(Color targetColor, float duration) {
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        Color orginalColor = text.color;
        for (float time = 0f; time < duration; time += Time.deltaTime) {
            text.color = Color.Lerp(orginalColor, targetColor, time / duration);
            yield return null;
        }
        text.color = targetColor;
    }

    //private void DestroyFloatingText() {
    //    Destroy(gameObject, destroyTime);
    //}
}

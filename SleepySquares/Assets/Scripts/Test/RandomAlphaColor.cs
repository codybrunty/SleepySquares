using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAlphaColor : MonoBehaviour{

    public float minAlpha;
    public float maxAlpha;

    void Start(){
        Color color = gameObject.GetComponent<SpriteRenderer>().color;
        color = new Color(color.r,color.g,color.b,UnityEngine.Random.Range(minAlpha, maxAlpha));
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareMechanics_Next : MonoBehaviour{
    
    public int number = 0;
    public List<Color> spriteColors = new List<Color>();
    private NextBoardMechanics nextBoard;
    public bool bounce = false;

    public void SetNextBoard() {
        nextBoard = gameObject.transform.parent.GetComponent<NextBoardMechanics>();
    }

    public void SetRandomNumberAndDisplay() {
        number = RandomSquareNumber();
        SetNumberDisplay();
    }

    private int RandomSquareNumber() {
        int randomMax = 4;
        
        return UnityEngine.Random.Range(1, randomMax);
    }

    public void SetNumberDisplay() {
        if (number != 0) {
            gameObject.GetComponent<Image>().color = spriteColors[number - 1];
            PopAnim();
        }
        
    }

    public void PopAnim() {
        if (bounce) {
            Hashtable hash = new Hashtable();
            hash.Add("amount", new Vector3(1f, 1f, 0f));
            hash.Add("time", 0.5f);
            iTween.PunchScale(gameObject, hash);
        }
    }

}

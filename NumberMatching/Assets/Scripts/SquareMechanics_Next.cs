using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareMechanics_Next : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameboard = default;
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

    public void SetNumberAndDisplay(int num) {
        number = num;
        SetNumberDisplay();
    }

    private int RandomSquareNumber() {
        int randomMax = 4;

        if (gameboard.hardModeOn == 1) {
            randomMax = 5;
        }

        //decreasee the frequency of 4 getting picked
        int resultNumbeer = UnityEngine.Random.Range(1, randomMax);
        if (resultNumbeer == 4) {
            int randomNumber = UnityEngine.Random.Range(1, 3);
            if (randomNumber != 1) {
                randomMax = 4;
                resultNumbeer = UnityEngine.Random.Range(1, randomMax);
            }
        }


        return resultNumbeer;
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

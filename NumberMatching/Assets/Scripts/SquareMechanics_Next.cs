using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMechanics_Next : MonoBehaviour{

    [Header("GameBoard Info")]
    public int number = 0;
    public bool blocker = false;

    public SpriteRenderer numberSpriteRenderer = default;
    public List<Sprite> numberSprites = new List<Sprite>();
    private NextBoardMechanics nextBoard;

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
        switch (number) {
            case 1:
                numberSpriteRenderer.enabled = true;
                numberSpriteRenderer.sprite = numberSprites[0];
                break;
            case 2:
                numberSpriteRenderer.enabled = true;
                numberSpriteRenderer.sprite = numberSprites[1];
                break;
            case 3:
                numberSpriteRenderer.enabled = true;
                numberSpriteRenderer.sprite = numberSprites[2];
                break;
            case 4:
                numberSpriteRenderer.enabled = true;
                numberSpriteRenderer.sprite = numberSprites[3];
                break;
            case 5:
                numberSpriteRenderer.enabled = true;
                numberSpriteRenderer.sprite = numberSprites[4];
                blocker = true;
                break;
            case 0:
                    numberSpriteRenderer.enabled = false;
                    break;
        }
    }
}

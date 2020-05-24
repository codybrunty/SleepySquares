using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMechanics_Gameboard : MonoBehaviour{

    [Header("Square Info")]
    public int number = 0;
    public bool completed = false;
    public bool blocker = false;
    public List<bool> adjescentConnections = new List<bool>() { false, false, false, false };
    public bool luckyCoin = false;
    [Header("GameBoard Info")]
    public int gamePositionX=0;
    public int gamePositionY=0;
    public int gamePositionIndex=0;
    public List<SquareMechanics_Gameboard> adjescentSquares = new List<SquareMechanics_Gameboard>() { null, null, null, null };
    [Header("Game Objects")]
    [SerializeField] SpriteRenderer numberSpriteRenderer = default;
    [SerializeField] List<SpriteRenderer> connectionDisplaySpriteRenderers = default;
    [SerializeField] GameObject connectionDisplayGRP = default;
    [SerializeField] List<Sprite> numberSprites = new List<Sprite>();
    [SerializeField] List<Sprite> completed_umberSprites = new List<Sprite>();
    [SerializeField] GameObject completedImage = default;
    [SerializeField] GameObject blockerImage = default;
    private GameBoardMechanics gameboard;

    public void SetSquareDisplay() {
        NumberDisplay();
        SetBlockerDisplay();
        ConnectionDisplay();
    }

    private void SetBlockerDisplay() {
        if (number == 5) {
            blocker = true;
            completed = true;
            blockerImage.SetActive(true);
        }
    }

    private void ConnectionDisplay() {
        if (number < 5) { 
            connectionDisplayGRP.SetActive(true);

            for (int i = 0; i < adjescentConnections.Count; i++) {
                connectionDisplaySpriteRenderers[i].color = new Color(0f, 0f, 0f);
            }

            for (int i = 0; i < adjescentConnections.Count; i++) {
                if (adjescentConnections[i] == true) {
                    connectionDisplaySpriteRenderers[i].color = new Color(1f, 0f, 0f);
                }
            }
        }
    }

    private void NumberDisplay() {
        numberSpriteRenderer.gameObject.SetActive(true);
        numberSpriteRenderer.sprite = numberSprites[number-1];
        numberSpriteRenderer.sortingOrder = 5;
        PopSFX();
        Pop();
    }

    private void PopSFX() {
        switch (number) {
            case 1:
                //Debug.Log("sfx1");
                FindObjectOfType<SoundManager>().PlayOneShotSound("monster1");
                break;
            case 2:
                //Debug.Log("sfx2");
                FindObjectOfType<SoundManager>().PlayOneShotSound("monster2");
                break;
            case 3:
                //Debug.Log("sfx3");
                FindObjectOfType<SoundManager>().PlayOneShotSound("monster3");
                break;
            default:
                //Debug.Log("nosfx");
                break;
        }
    }

    private void Pop() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        hash.Add("oncomplete", "PopDone");
        iTween.PunchScale(gameObject, hash);
    }

    private void PopDone() {
        numberSpriteRenderer.sortingOrder = 2;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void PopAway() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(-0.5f, -0.5f, 0f));
        hash.Add("time", .5f);
        hash.Add("oncomplete", "PopAwayDone");
        iTween.PunchScale(gameObject, hash);
    }

    private void PopAwayDone() {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }


    internal void RecalculateAdjescentSquares() {
        for (int i = 0; i < adjescentSquares.Count; i++) {
            if (adjescentSquares[i] != null) {
                if (adjescentSquares[i].completed == false && adjescentSquares[i].number > 0) {
                    adjescentSquares[i].CalculateConnections();
                }
            }
        }
    }

    public void CalculateConnections() {
        if (blocker==false) {
            //Debug.Log("CalculateConnections " + gameObject.name);
            List<bool> adjescentSquaresEligible = GetAdjescentSquaresEligible();
            List<int> randomOrder = GetRandomOrder();
            int currentConnections = GetCurrentTotalConnections();

            for (int i = 0; i < adjescentSquaresEligible.Count; i++) {
                if (currentConnections < number) {
                    if (adjescentSquaresEligible[randomOrder[i]] == true) {
                        adjescentConnections[randomOrder[i]] = true;
                        //Get Connection Squares info for this square.
                        int oppositeRandomOrderNumber = GetOppositeRandomOrderNumber(randomOrder[i]);
                        adjescentSquares[randomOrder[i]].adjescentConnections[oppositeRandomOrderNumber] = true;
                        //Debug.Log(gameObject.name + " CONNECTION " + adjescentSquares[randomOrder[i]].gameObject.name);
                        adjescentSquares[randomOrder[i]].CheckCompleted();
                        currentConnections++;
                    }
                }
            }
            CheckCompleted();
        }
    }

    public void ResetSquare_OnClick() {
        RemoveConnectionsOnAdjescentSquareInfo();
        ZerOutSquareInfo();
    }



    private void RemoveConnectionsOnAdjescentSquareInfo() {

        for (int i = 0; i < adjescentConnections.Count; i++) {

            if (adjescentConnections[i] == true) {
                if (i == 0) {
                    adjescentSquares[i].adjescentConnections[1] = false;
                }
                else if (i == 1) {
                    adjescentSquares[i].adjescentConnections[0] = false;
                }
                else if (i == 2) {
                    adjescentSquares[i].adjescentConnections[3] = false;
                }
                if (i == 3) {
                    adjescentSquares[i].adjescentConnections[2] = false;
                }
                
                adjescentSquares[i].CheckCompleted();
            }

        }
    }

    public void ResetSquare_OnCompletion() {
        PopAway();
        ZerOutSquareInfo();
    }

    public void ResetSquare_BlockerClear() {
        ZerOutSquareInfo();
        ZeroOutBlockerInfo();
    }

    private void ZeroOutBlockerInfo() {
        blocker = false;
        blockerImage.SetActive(false);
    }

    public void ZerOutSquareInfo() {
        number = 0;
        adjescentConnections = new List<bool> { false, false, false, false };
        completed = false;
        completedImage.SetActive(false);
        numberSpriteRenderer.gameObject.SetActive(false);
        connectionDisplayGRP.gameObject.SetActive(false);
    }

    private int GetOppositeRandomOrderNumber(int number) {
        int oppositeNumber = 0;
        switch (number) {
            case 0:
                oppositeNumber = 1;
                break;
            case 1:
                oppositeNumber = 0;
                break;
            case 2:
                oppositeNumber = 3;
                break;
            case 3:
                oppositeNumber = 2;
                break;
        }
        return oppositeNumber;
    }

    private List<int> GetRandomOrder() {
        List<int> order = new List<int>() { 0, 1, 2, 3 };
        for (int i = 0; i < order.Count; i++) {
            int tempNumber = order[0];
            int randomIndex = UnityEngine.Random.Range(0, 4);
            order[0] = order[randomIndex];
            order[randomIndex] = tempNumber;
        }
        return order;
    }

    private List<bool> GetAdjescentSquaresEligible() {
        List<bool> adjescentSquaresEligible = new List<bool> { false, false, false, false };
        adjescentSquaresEligible[0] = GetSquareEligibility(adjescentSquares[0],"Top");
        adjescentSquaresEligible[1] = GetSquareEligibility(adjescentSquares[1], "Bottom");
        adjescentSquaresEligible[2] = GetSquareEligibility(adjescentSquares[2], "Left");
        adjescentSquaresEligible[3] = GetSquareEligibility(adjescentSquares[3], "Right");
        return adjescentSquaresEligible;
    }

    private bool GetSquareEligibility(SquareMechanics_Gameboard square,string side) {
        bool eligible = false;
        if (square != null) {
            if (square.completed == false && square.number > 0) {
                eligible = true;
                //Debug.Log(square.gameObject.name + " (" + side + ")" + " eligible");
            }
            //else {
                //Debug.Log(square.gameObject.name + " (" + side + ")" + " not eligible");
            //}
        }
        return eligible;
    }


    private int GetCurrentTotalConnections() {
        int connections = 0;

        for (int i = 0; i < adjescentConnections.Count; i++) {
            if (adjescentConnections[i]) {
                connections++;
            }
        }
        return connections;
    }

    public void CheckCompleted() {
        int connections = GetCurrentTotalConnections();
        
        if (connections == number) {
            completed = true;
            completedImage.GetComponent<SpriteRenderer>().sprite = completed_umberSprites[number - 1];
            completedImage.SetActive(true);
            ConnectionDisplay();
            gameboard.CheckForCompleteLink(gameObject);
        }
        else {
            completed = false;
            completedImage.SetActive(false);
            ConnectionDisplay();
        }
    }

    public void SetAdjescentSquares(GameBoardMechanics gb) {
        gameboard = gb;
        adjescentSquares[0] = SetTopSquare();
        adjescentSquares[1] = SetBottomSquare();
        adjescentSquares[2] = SetLeftSquare();
        adjescentSquares[3] = SetRightSquare();
    }

    private SquareMechanics_Gameboard SetTopSquare() {
        int topSquare_gamePositionY = gamePositionY + 1;
        int height = gameboard.gameBoardHeight;
        bool edgeCase = false;
        if (topSquare_gamePositionY >= height) {
            edgeCase = true;
        }
        if (!edgeCase) {
            int index = gamePositionIndex + 1;
            return gameboard.gameBoardSquares[index].GetComponent<SquareMechanics_Gameboard>();
        }
        else {
            //Debug.Log(gameObject.name + " has no top");
            return null;
        }
    }

    private SquareMechanics_Gameboard SetBottomSquare() {
        int botSquare_gamePositionY = gamePositionY - 1;
        bool edgeCase = false;
        if (botSquare_gamePositionY < 0) {
            edgeCase = true;
        }
        if (!edgeCase) {
            int index = gamePositionIndex - 1;
            return gameboard.gameBoardSquares[index].GetComponent<SquareMechanics_Gameboard>();
        }
        else {
            //Debug.Log(gameObject.name + " has no bottom");
            return null;
        }
    }

    private SquareMechanics_Gameboard SetLeftSquare() {
        int leftSquare_gamePositionX = gamePositionX - 1;
        bool edgeCase = false;
        if (leftSquare_gamePositionX < 0) {
            edgeCase = true;
        }
        if (!edgeCase) {
            int height = gameboard.gameBoardHeight;
            int index = gamePositionIndex - height;
            return gameboard.gameBoardSquares[index].GetComponent<SquareMechanics_Gameboard>();
        }
        else {
            //Debug.Log(gameObject.name + " has no left");
            return null;
        }
    }

    private SquareMechanics_Gameboard SetRightSquare() {
        int width = gameboard.gameBoardWidth;
        int rightSquare_gamePositionX = gamePositionX + 1;
        bool edgeCase = false;
        if (rightSquare_gamePositionX >= width) {
            edgeCase = true;
        }
        if (!edgeCase) {
            int height = gameboard.gameBoardHeight;
            int index = gamePositionIndex + height;
            return gameboard.gameBoardSquares[index].GetComponent<SquareMechanics_Gameboard>();
        }
        else {
            //Debug.Log(gameObject.name + " has no right");
            return null;
        }
    }
}

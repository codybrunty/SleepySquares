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
    [SerializeField] List<GameObject> faces = new List<GameObject>();
    public bool useFaces = false;

    private Vector3 squareScale;
    private Vector3 squarePos;

    [Header("Swap Animation")]
    [SerializeField] private Transform route;
    private float tParam = 0f;
    private Vector2 routePos;
    public AnimationCurve swapEase;
    public List<BezierPosition> bezPoints = new List<BezierPosition>();


    private void Awake()
    {
        squareScale = gameObject.transform.localScale;
        squarePos = gameObject.transform.localPosition;
    }

    public void SetNextBoard() {
        nextBoard = gameObject.transform.parent.GetComponent<NextBoardMechanics>();
    }

    public void SetRandomNumber() {
        number = RandomSquareNumber();
    }

    public void SetFakeDisplay(int fakeNumber)
    {
        gameObject.GetComponent<Image>().color = spriteColors[fakeNumber - 1];
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

        int resultNumber = UnityEngine.Random.Range(1, randomMax);



        //HardMode
        if (randomMax == 5)
        {
            //decreasee the frequency of 4 getting picked
            if (resultNumber == 4)
            {
                int randomNumber = UnityEngine.Random.Range(1, 3);
                if (randomNumber != 1)
                {
                    randomMax = 4;
                    resultNumber = UnityEngine.Random.Range(1, randomMax);
                }
            }
        }
        //Normal Mode
        else
        {
            if(gameboard.score < 150)
            {
                //decreasee the frequency of 3 getting picked when score under 250 in normal mode
                if (resultNumber == 3)
                {
                    int randomNumber = UnityEngine.Random.Range(1, 3);
                    if (randomNumber != 1)
                    {
                        randomMax = 3;
                        resultNumber = UnityEngine.Random.Range(1, randomMax);
                    }
                }
            }
        }





        return resultNumber;
    }

    public void SetNumberDisplay() {
        if (number != 0) {
            gameObject.GetComponent<Image>().color = spriteColors[number - 1];
            SetFaceDisplay();
            PopAnim();
        }
        
    }

    private void SetFaceDisplay(){
        if (useFaces){
            foreach(GameObject face in faces){
                face.SetActive(false);
            }
            faces[number-1].SetActive(true);
        }
    }

    public void PopAnim() {
        if (bounce) {
            gameObject.transform.localScale = squareScale;


            Hashtable hash = new Hashtable();
            hash.Add("amount", new Vector3(1f, 1f, 0f));
            hash.Add("time", 0.75f);
            iTween.PunchScale(gameObject, hash);
        }
    }


    public void MoveNextSquareAlongRoute(float timeDuration, GameObject square)
    {
        SetUpBezierCurvePoints(square);
        StartCoroutine(MoveSquareOnRoute(timeDuration));
    }

    private void SetUpBezierCurvePoints(GameObject square)
    {
        route.GetChild(3).position = RectTransformUtility.WorldToScreenPoint(Camera.main, square.transform.position);
        int squareIndex = square.GetComponent<SquareMechanics_Gameboard>().gamePositionIndex;
        route.GetChild(1).position = new Vector2 (bezPoints[squareIndex].p1_x, bezPoints[squareIndex].p1_y); 
        route.GetChild(2).position = new Vector2(bezPoints[squareIndex].p2_x, bezPoints[squareIndex].p2_y);
    }

    IEnumerator MoveSquareOnRoute(float timeDuration)
    {
        Vector2 p0 = route.GetChild(0).position;
        Vector2 p1 = route.GetChild(1).position;
        Vector2 p2 = route.GetChild(2).position;
        Vector2 p3 = route.GetChild(3).position;


        for (float t = 0f; t < timeDuration; t += Time.deltaTime)
        {
            tParam = swapEase.Evaluate(t / timeDuration);

            routePos = Mathf.Pow(1 - tParam, 3) * p0 +
            3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
            3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
            Mathf.Pow(tParam, 3) * p3;

            gameObject.transform.position = routePos;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        gameObject.transform.localPosition = squarePos;
    }
}

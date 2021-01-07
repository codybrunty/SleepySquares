using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareMechanics_Next : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameboard = default;
    public int number = 0;
    public List<Color> spriteColors = new List<Color>();
    [SerializeField] NextBoardMechanics nextBoard = default;
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

    private Image mainImage;


    private void Awake()
    {
        mainImage = gameObject.GetComponent<Image>();
        squareScale = gameObject.transform.localScale;
        squarePos = gameObject.transform.localPosition;
    }

    public void SetRandomNumber() {
        number = RandomSquareNumber();
    }

    public void SetFakeDisplay(int fakeNumber)
    {
        mainImage.color = spriteColors[fakeNumber - 1];
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

        //shift back to 4 when in daily leaderboards 
        if (gameboard.DailyModeOn) {
            randomMax = 4;
        }

        int resultNumber = UnityEngine.Random.Range(1, randomMax);



        //HardMode
        if (randomMax == 5)
        {
            //decreasee the frequency of 4 getting picked untill 1000 points
            if (gameboard.score < 1000) {
                if (resultNumber == 4) {
                    //50% chance of changing a 4
                    int randomNumber = UnityEngine.Random.Range(1, 3);
                    if (randomNumber == 2) {
                        //Debug.LogWarning("4 Square Switch Up");
                        randomMax = 4;
                        resultNumber = UnityEngine.Random.Range(1, randomMax);
                    }
                }
            }
            else {
                if (resultNumber == 4) {
                    //20% chance of changing a 4
                    int randomNumber = UnityEngine.Random.Range(1, 5);
                    if (randomNumber == 2) {
                        //Debug.LogWarning("4 Square Switched Out");
                        randomMax = 4;
                        resultNumber = UnityEngine.Random.Range(1, randomMax);
                    }
                }
            }
        }
        //Normal Mode or daily
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
            mainImage.color = spriteColors[number - 1];
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


    public void MoveNextSquareAlongRoute(float timeDuration, SquareMechanics_Gameboard squareMechanics)
    {
        SetUpBezierCurvePoints(squareMechanics);
        StartCoroutine(MoveSquareOnRoute(timeDuration));
    }

    private void SetUpBezierCurvePoints(SquareMechanics_Gameboard squareMechanics)
    {
        route.GetChild(3).position = RectTransformUtility.WorldToScreenPoint(Camera.main, squareMechanics.gameObject.transform.position);
        int squareIndex = squareMechanics.gamePositionIndex;
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

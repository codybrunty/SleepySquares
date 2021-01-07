using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Next_Square : MonoBehaviour
{

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

    public void MoveNextSquareAlongRoute(float timeDuration, GameObject square)
    {
        SetUpBezierCurvePoints(square);
        StartCoroutine(MoveSquareOnRoute(timeDuration));
        StartCoroutine(ScaleSquareOnRoute(timeDuration));
    }

    private void SetUpBezierCurvePoints(GameObject square)
    {
        route.GetChild(3).position = RectTransformUtility.WorldToScreenPoint(Camera.main, square.transform.position);
        int squareIndex = square.GetComponent<Tutorial_Square>().gamePositionIndex;
        route.GetChild(1).position = new Vector2(bezPoints[squareIndex].p1_x, bezPoints[squareIndex].p1_y);
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

    IEnumerator ScaleSquareOnRoute(float timeDuration)
    {
        Vector3 newScale = new Vector3(1.5465f, 1.5465f, 1f);

        for (float t = 0f; t < timeDuration; t += Time.deltaTime)
        {
            gameObject.transform.localScale = Vector3.Lerp(squareScale,newScale, swapEase.Evaluate(t / timeDuration));
            yield return new WaitForEndOfFrame();
        }

        gameObject.transform.localScale = squareScale;
    }

}

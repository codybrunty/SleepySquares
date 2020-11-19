using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBoardBGMovement : MonoBehaviour
{
    private Vector3 startPosition;
    public float moveDuration = 1f;
    public AnimationCurve ease;

    private Coroutine coroutine;
    //private RectTransform rt;



    public void GetStartPosition()
    {
        startPosition = gameObject.transform.position;
        //rt = gameObject.GetComponent<RectTransform>();
        //startPosition = rt.position;
    }

    public void MoveOnScreen()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        gameObject.transform.position = startPosition;
    }

    public void MoveOffScreen()
    {
        coroutine = StartCoroutine(MoveOverTime());
    }

    IEnumerator MoveOverTime()
    {

        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + 11f, startPosition.z);

        for (float t = 0f; t < moveDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / moveDuration;
            gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, ease.Evaluate(normalizedTime));
            yield return null;
        }

        gameObject.transform.position = endPosition;
    }
}

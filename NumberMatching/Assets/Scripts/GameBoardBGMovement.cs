using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardBGMovement : MonoBehaviour
{
    public float moveDuration = 0.5f;
    public AnimationCurve ease;
    private Coroutine coroutine;
    private Vector3 bg_scale;

    private void Awake()
    {
        bg_scale = gameObject.transform.localScale;
    }

    public void TrueSize()
    {
        //Debug.LogWarning("true size");
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        gameObject.transform.localScale = bg_scale;
    }

    public void Shrink()
    {
        //Debug.LogWarning("shrink");
        coroutine = StartCoroutine(ScaleOverTime());
    }

    IEnumerator ScaleOverTime()
    {

        Vector3 endScale = new Vector3(.01f, .01f, bg_scale.z);

        for (float t = 0f; t < moveDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / moveDuration;
            gameObject.transform.localScale = Vector3.Lerp(bg_scale, endScale, ease.Evaluate(normalizedTime));
            yield return null;
        }

        gameObject.transform.localScale = endScale;
    }


}

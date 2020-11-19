using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordArtPopUps : MonoBehaviour
{

    [SerializeField] List<Sprite> wordArts = new List<Sprite>();
    [SerializeField] SpriteRenderer mainSprite = default;
    public float fadeInTime = 0.25f;
    public float holdTime = 0.25f;
    public float fadeOutTime = 0.25f;
    public Vector3 moveAmmount = new Vector3(195f, 0f, 0f);
    public float moveDuration = 1f;
    public float delayBeforeMove = 0f;
    public float delayBeforeSound = 0f;
    private Vector3 orginalPos;
    private Vector3 originalScale;
    private int randomIndex;
    public AnimationCurve scaleEase;
    public AnimationCurve scaleEase2;
    public float scaleInTime = 0.25f;
    public float scaleInTime2 = 0.25f;
    [SerializeField] GameObject poof = default;
    public AnimationCurve moveEase;
    public float delayBeforeScale = 0f;

    private void Start()
    {
        orginalPos = mainSprite.gameObject.transform.position;
        originalScale = mainSprite.gameObject.transform.localScale;
    }

    public void WordArtAnimation(int number)
    {
        SetRandomWordArt();
        StartCoroutine(PopWordArt());
    }

    private void SetRandomWordArt()
    {
        randomIndex = UnityEngine.Random.Range(0, wordArts.Count);
        mainSprite.sprite = wordArts[randomIndex];

    }

    IEnumerator PopWordArt()
    {
        poof.SetActive(true);
        Vector3 movePos = new Vector3(orginalPos.x + moveAmmount.x, orginalPos.y + moveAmmount.y, orginalPos.z + moveAmmount.z);
        mainSprite.gameObject.SetActive(true);
        StartCoroutine(AlphaOverTime(1f));
        StartCoroutine(ScaleOverTime());
        StartCoroutine(MoveOverTime(movePos));
        yield return new WaitForSeconds(fadeInTime);
        yield return new WaitForSeconds(holdTime);
        StartCoroutine(AlphaOverTime(0f));
        yield return new WaitForSeconds(fadeOutTime);
        ResetPosition();
        mainSprite.gameObject.SetActive(false);
        poof.SetActive(false);
    }

    private void ResetPosition()
    {
        mainSprite.gameObject.transform.position = orginalPos;
        mainSprite.gameObject.transform.localScale = originalScale;
    }

    IEnumerator ScaleOverTime()
    {
        yield return new WaitForSeconds(delayBeforeScale);
        Vector3 oldScale = mainSprite.gameObject.transform.localScale;
        Vector3 newScale = new Vector3(1.25f,1.25f,1.25f);

        for (float t = 0f; t < scaleInTime; t += Time.deltaTime)
        {
            mainSprite.gameObject.transform.localScale = Vector3.Lerp(oldScale, newScale, scaleEase.Evaluate(t / scaleInTime));
            yield return null;
        }
        mainSprite.gameObject.transform.localScale = newScale;
        StartCoroutine(ScaleOverTime2());
    }

    IEnumerator ScaleOverTime2()
    {
        Vector3 oldScale = mainSprite.gameObject.transform.localScale;
        Vector3 newScale = new Vector3(1f, 1f, 1f);
        for (float t = 0f; t < scaleInTime2; t += Time.deltaTime)
        {
            mainSprite.gameObject.transform.localScale = Vector3.Lerp(oldScale, newScale, scaleEase2.Evaluate(t / scaleInTime2));
            yield return null;
        }
        mainSprite.gameObject.transform.localScale = newScale;
    }

    IEnumerator MoveOverTime(Vector3 newPos)
    {
        Vector3 oldPos = mainSprite.gameObject.transform.position;

        yield return new WaitForSeconds(delayBeforeMove);

        for (float t = 0f; t < moveDuration; t += Time.deltaTime)
        {
            mainSprite.gameObject.transform.position = Vector3.Lerp(oldPos, newPos, moveEase.Evaluate(t / moveDuration));
            yield return null;
        }
        mainSprite.gameObject.transform.position = newPos;
    }

    IEnumerator AlphaOverTime(float newAlpha)
    {
        float oldAlpha = 0f;
        float duration = fadeInTime;
        if (newAlpha == 0f)
        {
            oldAlpha = 1f;
            duration = fadeOutTime;
        }
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(oldAlpha, newAlpha, t / duration);
            mainSprite.color = new Color(1f,1f,1f,alpha);
            yield return null;
        }
        mainSprite.color = new Color(1f, 1f, 1f, newAlpha);
    }

}
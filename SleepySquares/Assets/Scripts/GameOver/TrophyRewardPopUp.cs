using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrophyRewardPopUp : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI txt = default;
    private Vector3 originalPosition;
    private Vector3 originalScale;

    public Vector3 moveAmmount = new Vector3(195f, 0f, 0f);
    public float moveDuration = 0.5f;
    public AnimationCurve moveEase;

    public float scaleDuration = 0.5f;
    public AnimationCurve scaleEase;

    public float scaleDuration2 = 0.1f;
    public AnimationCurve scaleEase2;

    public float holdBeforeFadeOut = 0.75f;
    public float fadeOutDuration = 0.25f;


    public void RewardPopUp()
    {
        StartCoroutine(PopWordArt());
    }

    IEnumerator PopWordArt()
    {
        gameObject.GetComponent<Image>().enabled = true;
        GetOriginalPosition();
        GetOriginalScale();
        Vector3 movePos = new Vector3(originalPosition.x + moveAmmount.x, originalPosition.y + moveAmmount.y, originalPosition.z + moveAmmount.z);
        StartCoroutine(ScaleOverTime());
        StartCoroutine(MoveOverTime(movePos));

        yield return new WaitForSeconds(.15f);
        SoundManager.SM.PlayOneShotSound("yahoo");
        yield return new WaitForSeconds(holdBeforeFadeOut-.15f);
        StartCoroutine(FadeOutIMGOverTime());
        StartCoroutine(FadeOutTextOverTime());
        yield return new WaitForSeconds(fadeOutDuration);

        ResetPosition();
        ResetScale();
        ResetAlpha();
        gameObject.GetComponent<Image>().enabled = false;
    }

    IEnumerator FadeOutIMGOverTime()
    {
        float oldAlpha = 1f;
        float newAlpha = 0f;

        for (float t = 0f; t < fadeOutDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(oldAlpha, newAlpha, t / fadeOutDuration);
            gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, newAlpha);
    }

    IEnumerator FadeOutTextOverTime()
    {
        float oldAlpha = 1f;
        float newAlpha = 0f;

        for (float t = 0f; t < fadeOutDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(oldAlpha, newAlpha, t / fadeOutDuration);
            txt.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, newAlpha);
    }

    IEnumerator MoveOverTime(Vector3 newPos)
    {
        Vector3 oldPos = originalPosition;

        for (float t = 0f; t < moveDuration; t += Time.deltaTime)
        {
            gameObject.transform.localPosition = Vector3.Lerp(oldPos, newPos, moveEase.Evaluate(t / moveDuration));
            yield return null;
        }
        gameObject.transform.localPosition = newPos;
    }

    IEnumerator ScaleOverTime()
    {
        Vector3 oldScale = originalScale;
        Vector3 newScale = new Vector3(1.1f, 1.1f, 1.1f);

        for (float t = 0f; t < scaleDuration; t += Time.deltaTime)
        {
            gameObject.transform.localScale = Vector3.Lerp(oldScale, newScale, scaleEase.Evaluate(t / scaleDuration));
            yield return null;
        }
        gameObject.transform.localScale = newScale;
        StartCoroutine(ScaleOverTime2());
    }

    IEnumerator ScaleOverTime2()
    {
        Vector3 oldScale = new Vector3(1.1f, 1.1f, 1.1f);
        Vector3 newScale = new Vector3(1f, 1f, 1f);

        for (float t = 0f; t < scaleDuration2; t += Time.deltaTime)
        {
            gameObject.transform.localScale = Vector3.Lerp(oldScale, newScale, scaleEase2.Evaluate(t / scaleDuration2));
            yield return null;
        }
        gameObject.transform.localScale = newScale;
    }

    private void GetOriginalPosition()
    {
        originalPosition = gameObject.transform.localPosition;
    }
    private void GetOriginalScale()
    {
         originalScale = gameObject.transform.localScale;
    }
    private void ResetPosition()
    {
        gameObject.transform.localPosition = originalPosition;
    }
    private void ResetScale()
    {
        gameObject.transform.localScale = originalScale;
    }
    private void ResetAlpha()
    {
        gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        txt.color = new Color(1f, 1f, 1f, 1f);
    }

}

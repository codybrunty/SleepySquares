using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialAnimation : MonoBehaviour{

    public float currentSeconds;
    public List<bool> eyeShutStatus = new List<bool>();
    public List<GameObject> eyeballs = new List<GameObject>();
    public List<GameObject> eyelids = new List<GameObject>();
    public List<EyeBlink> eyeblinks = new List<EyeBlink>();
    private List<string> anims = new List<string>{"Blink"};
    private bool startAnimating = false;
    private Coroutine coroutine;
    public float blinkSpeed = .25f;
    private Coroutine dimBlinkCo;
    //[SerializeField] Color dimColor = default;
    //[SerializeField] Color orgColor = default;
    [SerializeField] Sprite awakeMouth = default;
    [SerializeField] Sprite sleepingMouth = default;
    [SerializeField] SpriteRenderer mouth = default;

    public void StartFacialAnimation() {
        if (coroutine == null) {
            startAnimating = true;
        }
        else {

            //string squareName = gameObject.transform.parent.parent.name;
            //string colorName  = gameObject.transform.name;
            //Debug.Log("tried to start facial animatino but it is already going on " + colorName + " " + squareName);
        }
    }

    public void StopFacialAnimation() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void ResetEyes() {

        for (int i = 0; i < eyeballs.Count; i++) {
            eyeShutStatus[i] = false;
            eyeballs[i].SetActive(true);
            eyelids[i].SetActive(false);
            OpenEyeAt(i);
        }
        for (int i=0; i < eyeblinks.Count; i++)
        {
            ResetEyeBlinkSquare(i);
        }

    }

    public void ShutThisManyEyes(int shutNumber) {

        List<int> needToBeShut = new List<int>();
        int alreadyShut = 0;

        for (int i = 0; i < eyeballs.Count; i++) {
            if (eyeShutStatus[i] == false) {
                needToBeShut.Add(i);
            }
            else {
                alreadyShut++;
            }
        }

        if (shutNumber > alreadyShut) {
            int difference = shutNumber - alreadyShut;

            for (int i = 0; i < difference; i++) {
                int randomIndex = UnityEngine.Random.Range(0, needToBeShut.Count);
                int eyeIndex = needToBeShut[randomIndex];

                needToBeShut.RemoveAt(randomIndex);
                eyeShutStatus[eyeIndex] = true;
                StartCoroutine(ShutEyeAt(eyeIndex));
            }
        }
        else if (shutNumber < alreadyShut) {
            int difference = alreadyShut - shutNumber;

            List<int> needToBeOpen = new List<int>();
            for (int i = 0; i < eyeballs.Count; i++) {
                if (eyeShutStatus[i] == true) {
                    needToBeOpen.Add(i);
                }
            }

            for (int i = 0; i < difference; i++) {
                int randomIndex = UnityEngine.Random.Range(0, needToBeOpen.Count);
                int eyeIndex = needToBeOpen[randomIndex];
                needToBeOpen.RemoveAt(randomIndex);

                OpenEyeAt(eyeIndex);
            }
        }

    }

    private void ResetEyeBlinkSquare(int eyeIndex)
    {
        eyeblinks[eyeIndex].eyeBlink.gameObject.transform.localPosition = eyeblinks[eyeIndex].startPosition;
        eyeblinks[eyeIndex].eyeBlink.gameObject.transform.localScale = eyeblinks[eyeIndex].startScale;
        eyeblinks[eyeIndex].eyeBlink.gameObject.SetActive(false);
    }

    IEnumerator ShutEyeAt(int eyeIndex)
    {
        ResetEyeBlinkSquare(eyeIndex);
        eyeblinks[eyeIndex].eyeBlink.gameObject.SetActive(true);

        for (float t = 0; t < blinkSpeed; t += Time.deltaTime)
        {
            eyeblinks[eyeIndex].eyeBlink.gameObject.transform.localPosition = Vector3.Lerp(eyeblinks[eyeIndex].startPosition, eyeblinks[eyeIndex].endPosition, t / .25f);
            eyeblinks[eyeIndex].eyeBlink.gameObject.transform.localScale = Vector3.Lerp(eyeblinks[eyeIndex].startScale, eyeblinks[eyeIndex].endScale, t / .25f);
            yield return null;
        }

        eyeblinks[eyeIndex].eyeBlink.gameObject.SetActive(false);
        eyeblinks[eyeIndex].eyeBlink.gameObject.transform.localPosition = eyeblinks[eyeIndex].startPosition;
        eyeblinks[eyeIndex].eyeBlink.gameObject.transform.localScale = eyeblinks[eyeIndex].startScale;

        eyeballs[eyeIndex].SetActive(false);
        eyelids[eyeIndex].SetActive(true);
    }

    private void OpenEyeAt(int eyeIndex)
    {
        eyeShutStatus[eyeIndex] = false;
        eyeballs[eyeIndex].transform.localScale = new Vector3(1f,1f,1f);
        eyeballs[eyeIndex].SetActive(true);
        eyelids[eyeIndex].SetActive(false);

    }

    private void Update() {

        if (startAnimating == true) {
            startAnimating = false;
            coroutine = StartCoroutine(AnimateFace());
        }

    }

    public IEnumerator AnimateFace() {
        //Debug.Log("facial anim starting");
        float randomSeconds = UnityEngine.Random.Range(5f,25f);
        currentSeconds = randomSeconds;
        //Debug.Log("anim waiting "+ randomSeconds);
        yield return new WaitForSeconds(randomSeconds);
        int randomAnimationNumber = UnityEngine.Random.Range(0, anims.Count);
        gameObject.GetComponent<Animator>().SetTrigger(anims[randomAnimationNumber]);
        startAnimating = true;
    }
    /*
    public void DimBlinks(float duration)
    {
        if (dimBlinkCo == null)
        {
            dimBlinkCo = StartCoroutine(DimAllBlinks(duration));
        }
        else
        {
            StopCoroutine(dimBlinkCo);
            dimBlinkCo = StartCoroutine(DimAllBlinks(duration));
        }
    }
    
    IEnumerator DimAllBlinks(float duration)
    {
        mouth.sprite = sleepingMouth;

        //Color normalColor = eyeblinks[0].eyeBlink.color;
        //Color dimColor = new Color(normalColor.r, normalColor.g, normalColor.b, 100f / 255f);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            for (int i = 0; i < eyeblinks.Count; i++)
            {
                eyeblinks[i].eyeBlink.color = Color.Lerp(orgColor, dimColor, t / duration);
            }
            yield return null;
        }

        for (int i = 0; i < eyeblinks.Count; i++)
        {
            eyeblinks[i].eyeBlink.color = dimColor;
        }
    }

    public void UnDimBlinks()
    {
        

        if (dimBlinkCo == null)
        {
            UnDimAllBlinks();
        }
        else
        {
            StopCoroutine(dimBlinkCo);
            UnDimAllBlinks();
        }
    }

    private void UnDimAllBlinks()
    {
        mouth.sprite = awakeMouth;

        //Color normalColor = eyeblinks[0].eyeBlink.color;
        //Color dimColor = new Color(normalColor.r, normalColor.g, normalColor.b, 1f);

        for (int i = 0; i < eyeblinks.Count; i++)
        {
            eyeblinks[i].eyeBlink.color = orgColor;
        }
    }

    */

}



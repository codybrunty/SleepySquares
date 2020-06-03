using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialAnimation : MonoBehaviour{

    public List<GameObject> eyeballs = new List<GameObject>();
    public List<GameObject> eyelids = new List<GameObject>();
    private List<string> anims = new List<string>{"Blink"};
    private bool startAnimating = false;
    private Coroutine coroutine;

    public void StartFacialAnimation() {
        if (coroutine == null) {
            startAnimating = true;
        }
        else {

            string squareName = gameObject.transform.parent.parent.name;
            string colorName  = gameObject.transform.name;
            //Debug.Log("tried to start facial animatino but it is already going on " + colorName + " " + squareName);
        }
    }

    public void StopFacialAnimation() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        else {
            string squareName = gameObject.transform.parent.parent.name;
            string colorName = gameObject.transform.name;
            //Debug.Log("Tried to Stop facial animation but it isnt going on " + colorName + " " + squareName);
        }
    }

    public void ResetEyes() {

        for (int i = 0; i < eyeballs.Count; i++) {
            eyeballs[i].SetActive(true);
            eyelids[i].SetActive(false);
        }

    }

    public void ShutThisManyEyes(int shutNumber) {
        string squareName = gameObject.transform.parent.parent.name;
        string colorName = gameObject.transform.name;
        //Debug.Log("Shut " + shutNumber + " eyes of " + eyeballs.Count + " on " + colorName + " " + squareName);


        List<int> needToBeShut = new List<int>();
        int alreadyShut = 0;
        for (int i = 0; i < eyeballs.Count; i++) {
            if (eyeballs[i].activeSelf == true) {
                needToBeShut.Add(i);
            }
            else {
                alreadyShut++;
            }
        }

        //Debug.Log(alreadyShut);

        if (shutNumber > alreadyShut) {
            //Debug.Log("more");
            int difference = shutNumber - alreadyShut;

            for (int i = 0; i < difference; i++) {
                int randomIndex = UnityEngine.Random.Range(0, needToBeShut.Count);
                int eyeIndex = needToBeShut[randomIndex];

                needToBeShut.RemoveAt(randomIndex);
                eyeballs[eyeIndex].SetActive(false);
                eyelids[eyeIndex].SetActive(true);

            }
        }
        else if (shutNumber < alreadyShut) {
            //Debug.Log("less");
            int difference = alreadyShut - shutNumber;

            List<int> needToBeOpen = new List<int>();
            for (int i = 0; i < eyeballs.Count; i++) {
                if (eyeballs[i].activeSelf == false) {
                    needToBeOpen.Add(i);
                }
            }

            for (int i = 0; i < difference; i++) {
                int randomIndex = UnityEngine.Random.Range(0, needToBeOpen.Count);
                int eyeIndex = needToBeOpen[randomIndex];
                needToBeOpen.RemoveAt(randomIndex);
                eyeballs[eyeIndex].SetActive(true);
                eyelids[eyeIndex].SetActive(false);

            }
        }

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
        //Debug.Log("anim waiting "+ randomSeconds);
        yield return new WaitForSeconds(randomSeconds);
        int randomAnimationNumber = UnityEngine.Random.Range(0, anims.Count);
        gameObject.GetComponent<Animator>().SetTrigger(anims[randomAnimationNumber]);
        startAnimating = true;
    }



}

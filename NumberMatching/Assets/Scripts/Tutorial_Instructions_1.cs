using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Instructions_1 : MonoBehaviour{

    [SerializeField] GameObject green = default;
    [SerializeField] GameObject red = default;
    [SerializeField] GameObject purple = default;
    [SerializeField] TutorialGoNext goNext = default;
    [SerializeField] TypewriterEffect instructions = default;

    private void OnEnable() {
        StartCoroutine(Tutorial1_Animations());
    }

    IEnumerator Tutorial1_Animations() {
        yield return new WaitForSeconds(0.4f);
        GreenPop();
        yield return new WaitForSeconds(0.4f);
        RedPop();
        yield return new WaitForSeconds(0.4f);
        PurplePop();
        yield return new WaitForSeconds(0.5f);
        goNext.animationDone = true;
    }


    private void GreenPop() {
        green.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(green, hash);
        SoundManager.SM.PlayOneShotSound("monster1");
    }

    private void RedPop() {
        red.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(red, hash);
        SoundManager.SM.PlayOneShotSound("monster2");
    }

    private void PurplePop() {
        purple.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(purple, hash);
        SoundManager.SM.PlayOneShotSound("monster3");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Instructions_1 : MonoBehaviour{

    [SerializeField] GameObject white1 = default;
    [SerializeField] GameObject white2 = default;
    [SerializeField] GameObject white3 = default;
    [SerializeField] GameObject green = default;
    [SerializeField] GameObject red = default;
    [SerializeField] GameObject purple = default;
    [SerializeField] GameObject clickNext = default;
    [SerializeField] GameObject flashyButton = default;

    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial1_Animations());
    }

    IEnumerator Tutorial1_Animations() {
        GrowWhiteSquares();
        yield return new WaitForSeconds(1.5f);
        GreenPop();
        yield return new WaitForSeconds(0.25f);
        RedPop();
        yield return new WaitForSeconds(0.25f);
        PurplePop();
        yield return new WaitForSeconds(1.2f);
        clickNext.SetActive(true);
        flashyButton.SetActive(true);
    }

    private void GrowWhiteSquares() {
        white1.SetActive(true);
        white2.SetActive(true);
        white3.SetActive(true);

        Hashtable hash = new Hashtable();
        hash.Add("scale", new Vector3(1.5f, 1.5f, 1.5f));
        hash.Add("time", 1f);
        iTween.ScaleTo(white1, hash);

        iTween.ScaleTo(white2, hash);

        iTween.ScaleTo(white3, hash);
    }

    private void GreenPop() {
        white1.SetActive(false);
        green.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(green, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster1");
    }

    private void RedPop() {
        white2.SetActive(false);
        red.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(red, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster2");
    }

    private void PurplePop() {
        white3.SetActive(false);
        purple.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(purple, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster3");
    }

}

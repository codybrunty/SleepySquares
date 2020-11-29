using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Instructions_3 : MonoBehaviour{

    [SerializeField] GameObject green = default;
    [SerializeField] GameObject green1 = default;
    [SerializeField] GameObject green2 = default;
    [SerializeField] GameObject red = default;
    [SerializeField] GameObject purple = default;
    [SerializeField] GameObject connection1 = default;
    [SerializeField] GameObject connection2 = default;
    [SerializeField] GameObject connection3 = default;
    [SerializeField] GameObject connection4 = default;
    [SerializeField] TutorialGoNext goNext = default;
    [SerializeField] GameObject specialEffects_green = default;
    [SerializeField] GameObject specialEffects_red= default;
    [SerializeField] GameObject specialEffects_purple = default;

    [SerializeField] Tutorial_Dissapear green_d = default;
    [SerializeField] Tutorial_Dissapear green1_d = default;
    [SerializeField] Tutorial_Dissapear green2_d = default;
    [SerializeField] Tutorial_Dissapear red_d = default;
    [SerializeField] Tutorial_Dissapear purple_d = default;

    [SerializeField] Tutorial_Dissapear green_dim = default;
    [SerializeField] Tutorial_Dissapear green1_dim = default;
    [SerializeField] Tutorial_Dissapear green2_dim = default;
    [SerializeField] Tutorial_Dissapear red_dim = default;
    [SerializeField] Tutorial_Dissapear purple_dim = default;

    [SerializeField] GameObject green_f = default;
    [SerializeField] GameObject green1_f = default;
    [SerializeField] GameObject green2_f = default;
    [SerializeField] GameObject red_f = default;
    [SerializeField] GameObject purple_f = default;

    [SerializeField] GameObject green_c = default;
    [SerializeField] GameObject green1_c = default;
    [SerializeField] GameObject green2_c = default;
    [SerializeField] GameObject red_c = default;
    [SerializeField] GameObject purple_c = default;

    [SerializeField] TutorialScoreboard score = default;
    public float scalingDuration = 0.35f;



    private void OnEnable() {
        StartCoroutine(Tutorial3_Animations());
    }

    IEnumerator Tutorial3_Animations()
    {

        StartCoroutine(ScaleUpSquares());

        yield return new WaitForSeconds(scalingDuration);

        green.SetActive(false);
        green1.SetActive(false);
        green2.SetActive(false);
        red.SetActive(false);
        purple.SetActive(false);
        connection1.SetActive(false);
        connection2.SetActive(false);
        connection3.SetActive(false);
        connection4.SetActive(false);
        Pop(green);
        Pop(green1);
        Pop(green2);
        Pop(red);
        Pop(purple);
        PlaySpecialEffects();
        SoundManager.SM.PlayOneShotSound("clearboard2");
        green_f.SetActive(false);
        green_c.SetActive(false);
        green1_f.SetActive(false);
        green1_c.SetActive(false);
        green2_f.SetActive(false);
        green2_c.SetActive(false);
        red_f.SetActive(false);
        red_c.SetActive(false);
        purple_f.SetActive(false);
        purple_c.SetActive(false);

        green_d.MakeDissapear(0.15f);
        green1_d.MakeDissapear(0.15f);
        green2_d.MakeDissapear(0.15f);
        red_d.MakeDissapear(0.15f);
        purple_d.MakeDissapear(0.15f);

        green_dim.MakeDissapear(0.15f);
        green1_dim.MakeDissapear(0.15f);
        green2_dim.MakeDissapear(0.15f);
        red_dim.MakeDissapear(0.15f);
        purple_dim.MakeDissapear(0.15f);

        score.ScoreboardAdd(8);

        yield return new WaitForSeconds(1f);
        green.SetActive(false);
        green1.SetActive(false);
        green2.SetActive(false);
        red.SetActive(false);
        purple.SetActive(false);
        yield return new WaitForSeconds(1f);
        goNext.animationDone = true;
    }

    IEnumerator ScaleUpSquares() {
        Vector3 normalScale = green.transform.localScale;
        Vector3 newScale = new Vector3(1.75f, 1.75f, 1.75f);


        for (float t = 0; t < scalingDuration; t += Time.deltaTime) {
            green.transform.localScale = Vector3.Lerp(normalScale, newScale, t / scalingDuration);
            green1.transform.localScale = Vector3.Lerp(normalScale, newScale, t / scalingDuration);
            green2.transform.localScale = Vector3.Lerp(normalScale, newScale, t / scalingDuration);
            red.transform.localScale = Vector3.Lerp(normalScale, newScale, t / scalingDuration);
            purple.transform.localScale = Vector3.Lerp(normalScale, newScale, t / scalingDuration);
            yield return null;
        }

        green.transform.localScale = newScale;
        green1.transform.localScale = newScale;
        green2.transform.localScale = newScale;
        red.transform.localScale = newScale;
        purple.transform.localScale = newScale;
    }

    private void PlaySpecialEffects()
    {
        Instantiate(specialEffects_green, green.transform.position, Quaternion.identity, green.transform);
        Instantiate(specialEffects_green, green1.transform.position, Quaternion.identity, green1.transform);
        Instantiate(specialEffects_green, green2.transform.position, Quaternion.identity, green2.transform);
        Instantiate(specialEffects_red, red.transform.position, Quaternion.identity, red.transform);
        Instantiate(specialEffects_purple, purple.transform.position, Quaternion.identity, purple.transform);
    }

    private void Pop(GameObject go)
    {
        go.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(go, hash);
    }

}

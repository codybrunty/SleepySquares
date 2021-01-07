using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial_RepairButton : MonoBehaviour
{
    [SerializeField] Image fill = default;
    [SerializeField] Image clearButtonImage = default;
    [SerializeField] Image clearButtonIconImage = default;
    [SerializeField] Image clearButtonBGImage = default;
    [SerializeField] GameObject starEffects = default;
    [SerializeField] TextMeshProUGUI clearText = default;
    [SerializeField] GameObject clearScaleGRP = default;
    [SerializeField] TutorialGameboard gameBoard = default;


    private void OnEnable()
    {
        DisabledClearButton();
        EnabledClearButton();
    }

    public void EnabledClearButton()
    {
        StartCoroutine(FillOverTime());
    }

    private void ActivateButton()
    {
        clearText.text = "1";
        gameObject.GetComponent<Button>().interactable = true;
        gameObject.GetComponent<Image>().raycastTarget = true;
        clearButtonImage.GetComponent<CollectionColor_Image>().key = "Button1";
        clearButtonImage.GetComponent<CollectionColor_Image>().GetColor();
        clearButtonIconImage.GetComponent<CollectionColor_Image>().key = "Second";
        clearButtonIconImage.GetComponent<CollectionColor_Image>().GetColor();
        clearButtonBGImage.GetComponent<CollectionColor_Image>().key = "Button2";
        clearButtonBGImage.GetComponent<CollectionColor_Image>().GetColor();
        starEffects.SetActive(true);
    }

    public void DisabledClearButton()
    {
        clearText.text = "0";
        fill.fillAmount = 1f;
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Image>().raycastTarget = false;
        clearButtonImage.GetComponent<CollectionColor_Image>().key = "Button3";
        clearButtonImage.GetComponent<CollectionColor_Image>().GetColor();
        clearButtonIconImage.GetComponent<CollectionColor_Image>().key = "Button4";
        clearButtonIconImage.GetComponent<CollectionColor_Image>().GetColor();
        clearButtonBGImage.GetComponent<CollectionColor_Image>().key = "Button5";
        clearButtonBGImage.GetComponent<CollectionColor_Image>().GetColor();
        starEffects.SetActive(false);
        gameBoard.TurnOffRepairArrow();
    }

    IEnumerator FillOverTime()
    {
        float startNumber = 1f;
        float endNumber = 0f;
        float duration = 1f;

        for (float t = 0; t < duration; t+=Time.deltaTime )
        {
            fill.fillAmount = Mathf.Lerp(startNumber,endNumber,t/duration);
            yield return null;
        }
        fill.fillAmount = 0f;
        Pop();
        ActivateButton();
    }

    public void Pop()
    {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 1f);
        iTween.PunchScale(clearScaleGRP, hash);
        SoundManager.SM.PlayOneShotSound("clearReady1");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RateMeMechanics : MonoBehaviour{
    public int hasRated;
    [SerializeField] GameObject rateText = default;

    private void OnEnable(){

        int trophyIndex = PlayerPrefs.GetInt("TrophyIndex", 0);

        if (trophyIndex >= 2)
        {
            hasRated = PlayerPrefs.GetInt("hasRated", 0);
            if (hasRated == 1)
            {
                HideRateButton();
            }
        }
        else
        {
            HideRateButton();
        }

    }

    public void RateButtonOnClick(){

        PlayClickSFX();

#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
        PlayerPrefs.SetInt("hasRated", 1);

#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1528662701");
        PlayerPrefs.SetInt("hasRated", 1);
#endif
        hasRated = 1;
        HideRateButton();
    }

    public void PlayClickSFX(){
        SoundManager.SM.PlayOneShotSound("select1");
        hasRated = 1;
    }

    private void HideRateButton(){

        gameObject.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Image>().enabled = false;
        rateText.SetActive(false);

    }

}

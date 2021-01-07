using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;


public class AdManager : MonoBehaviour, IUnityAdsListener{

    private string playStoreID = "3757843";
    private string appStoreID = "3757842";
    private string platformID = "";
    private string rewardedAd = "rewardedVideo";
    public bool isTestAd;

    [SerializeField] SwitchButton switchButton = default;
    [SerializeField] SettingsMenu settingsMenu = default;

    [SerializeField] Button adButton = default;
    [SerializeField] Color activeColor = default;
    [SerializeField] Color deactiveColor = default;
    [SerializeField] GameObject dcImage = default;
    [SerializeField] GameObject playImage = default;
    private Image adButtonImage;
    public bool m_bIsHearts = false;
    public DailyManager m_oDailyManager;

    private void Awake() {
        adButtonImage = adButton.GetComponent<Image>();
    }

    private void Start() {
        GetPlatformID();
        Advertisement.AddListener(this);
        InitializeAdManager();
    }

    private void GetPlatformID() {
#if UNITY_IPHONE
        platformID = appStoreID;
#endif
#if UNITY_ANDROID
        platformID = playStoreID;
#endif
    }

    public void AdButtonActive() {
        adButton.interactable = true;
        adButtonImage.color = activeColor;
        dcImage.SetActive(false);
        playImage.SetActive(true);
    }

    public void AdButtonDeactive() {
        adButton.interactable = false;
        adButtonImage.color = deactiveColor;
        dcImage.SetActive(true);
        playImage.SetActive(false);
    }

    private void InitializeAdManager() {
        Advertisement.Initialize(platformID, isTestAd);
    }

    public void PlayRewardedAd_store() {
        if (!Advertisement.IsReady(rewardedAd)) { return; }
        Advertisement.Show(rewardedAd);
    }
    public void PlayRewardedAd_hearts() {
        m_bIsHearts = true;
        if (!Advertisement.IsReady(rewardedAd)) { return; }
        Advertisement.Show(rewardedAd);
    
    }


    //IUnityAdsListener
    public void OnUnityAdsReady(string placementId) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        switch (showResult) {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                if (placementId == rewardedAd) { RewardPlayer(); }
                break;
        }
    }

    private void RewardPlayer() {
        if (m_bIsHearts) {
            m_bIsHearts = false;
            Debug.Log("Watched Ad for 3 hearts");
            m_oDailyManager.ContinueDaily();


            //for playfab tracking
            int counter = PlayerPrefs.GetInt("Ads_Watched_Daily", 0);
            counter++;
            PlayerPrefs.SetInt("Ads_Watched_Daily", counter);
        }
        else {
            Debug.Log("Watched Ad Free 2 switches");
            settingsMenu.ExitSettings();
            switchButton.AddSwitches(2);
            SoundManager.SM.PlayOneShotSound("yahoo");

            //for playfab tracking
            int counter = PlayerPrefs.GetInt("Ads_Watched", 0);
            counter++;
            PlayerPrefs.SetInt("Ads_Watched", counter);
        }
    }
}

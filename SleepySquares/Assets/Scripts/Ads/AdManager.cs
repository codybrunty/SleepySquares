using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsShowListener, IUnityAdsLoadListener,IUnityAdsInitializationListener {


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
    private bool adIsReady = false;

    public DailyManager m_oDailyManager;

    private void Awake() {
        adButtonImage = adButton.GetComponent<Image>();
    }

    private void Start() {
        GetPlatformID();
        InitializeAdManager();
        Advertisement.Load(rewardedAd, this);
    }

    private void GetPlatformID() {
#if UNITY_IOS
    platformID = appStoreID;
#elif UNITY_ANDROID
    platformID = playStoreID;
#else
        platformID = "3757843"; 
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
        Advertisement.Initialize(platformID, isTestAd, this);
    }

    public void PlayRewardedAd_store() {
        if (!adIsReady) return;
        Advertisement.Show(rewardedAd, this);
    }
    public void PlayRewardedAd_hearts() {
        m_bIsHearts = true; 
        if (!adIsReady) return;
        Advertisement.Show(rewardedAd, this);

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        if (placementId == rewardedAd && showCompletionState == UnityAdsShowCompletionState.COMPLETED) {
            RewardPlayer();
        }
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }

    public void OnUnityAdsAdLoaded(string placementId) {
        if (placementId == rewardedAd) {
            adIsReady = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }


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

    public void OnInitializationComplete() {
        Debug.Log("Unity Ads Initialization Complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class AdManager : MonoBehaviour, IUnityAdsListener{

    private string playStoreID = "3757843";
    private string appStoreID = "3757842";
    private string platformID = "";
    private string rewardedAd = "rewardedVideo";
    public bool isTestAd;

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

    private void InitializeAdManager() {
        Advertisement.Initialize(platformID, isTestAd);
    }

    public void PlayRewardedAd() {
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
        Debug.Log("Watched Ad Free 2 switches");
        FindObjectOfType<SettingsMenu>().ExitSettings();
        FindObjectOfType<SwitchButton>().AddSwitches(2);
        FindObjectOfType<SoundManager>().PlayOneShotSound("yahoo");

        //for playfab tracking
        int counter = PlayerPrefs.GetInt("Ads_Watched", 0);
        counter++;
        PlayerPrefs.SetInt("Ads_Watched", counter);
    }
}

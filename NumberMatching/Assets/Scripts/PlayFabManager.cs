using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSG.iOSKeychain;
using PlayFab.ProfilesModels;
using PlayFab.AuthenticationModels;

public class PlayFabManager : MonoBehaviour
{
    private string entityToken = "";

    #region Singleton
    public static PlayFabManager PFM;
    private void Awake()
    {
        if (PFM == null)
        {
            PFM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    #region Playfab Login
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "A12EF";
        }
        PlayfabMobileLogin();
    }

    void OnGlobalPolicyReceived(GetGlobalPolicyResponse response) {

        Debug.Log(response.ToJson());

    }

    private void PlayfabMobileLogin()
    {
#if UNITY_ANDROID
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
#if UNITY_IOS
        var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileIOSID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    public static string ReturnMobileIOSID()
    {
        string deviceID = Keychain.GetValue("iosDeviceID");
        if (deviceID == "")
        {
            deviceID = SystemInfo.deviceUniqueIdentifier;
            Keychain.SetValue("iosDeviceID", deviceID);
        }
        return deviceID;
    }

    private void OnLoginMobileSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, Login Mobile Success");
        /*
        PlayFabSettings.DeveloperSecretKey = "849WHR6UOOCSDG7E6XJPPPTJ53RPK1WMOO64HE5JIAA91S8HQB";
        PlayFabSettings.TitleId = "A12EF";
        PlayFabAuthenticationAPI.GetEntityToken(new GetEntityTokenRequest(),

        (entityResult) => {

            var entityId = entityResult.Entity.Id;

            var entityType = entityResult.Entity.Type;

            entityToken = entityResult.EntityToken;

        }, OnError);

        StartCoroutine(DelayGetGlobal());

        */
    }

    /*
    IEnumerator DelayGetGlobal( ) {
        
        yield return new WaitForSeconds(5f);

        GetGlobalPolicyRequest request = new GetGlobalPolicyRequest(); //create the request
        request.AuthenticationContext = new PlayFabAuthenticationContext();
        request.AuthenticationContext.EntityToken = entityToken;
        request.AuthenticationContext.EntityId = "A12EF";
        request.AuthenticationContext.EntityType = "title";
        PlayFabProfilesAPI.GetGlobalPolicy(request, OnGlobalPolicyReceived, OnError); //Make the call to retrieve the existing policy

    }
    

    private void OnError(PlayFabError error) {
        Debug.LogError(error.GenerateErrorReport());
    }
    */
    private void OnLoginMobileFailure(PlayFabError error) {
        Debug.Log("Login Mobile Failure");
        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion

    #region Playfab Player Statistics
    public int Trophy_Level;
    public int Score;
    public int HighScore;
    public int GameOver;
    public int Score_HM;
    public int HighScore_HM;
    public int GameOver_HM;
    public int Swaps;
    public int Swaps_Used;
    public int Swaps_Found;
    public int Ads_Watched;
    public int Purchase_30;
    public int Purchase_75;
    public int Purchase_200;
    public int TimePlayed;
    public int TimePlayed_HM;

    public void SetPlayfabPlayerStatistics()
    {
        TimeManager.TM.SaveTimer();
        GetUserStatistics();

        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate { StatisticName = "Trophy_Level", Value = Trophy_Level },
            new StatisticUpdate { StatisticName = "Score", Value = Score },
            new StatisticUpdate { StatisticName = "HighScore", Value = HighScore },
            new StatisticUpdate { StatisticName = "GameOver", Value = GameOver },
            new StatisticUpdate { StatisticName = "Score_HM", Value = Score_HM },
            new StatisticUpdate { StatisticName = "HighScore_HM", Value = HighScore_HM },
            new StatisticUpdate { StatisticName = "GameOver_HM", Value = GameOver_HM },
            new StatisticUpdate { StatisticName = "Swaps", Value = Swaps },
            new StatisticUpdate { StatisticName = "Swaps_Used", Value = Swaps_Used },
            new StatisticUpdate { StatisticName = "Swaps_Found", Value = Swaps_Found },
            new StatisticUpdate { StatisticName = "Ads_Watched", Value = Ads_Watched },
            new StatisticUpdate { StatisticName = "Purchase_30", Value = Purchase_30 },
            new StatisticUpdate { StatisticName = "Purchase_75", Value = Purchase_75 },
            new StatisticUpdate { StatisticName = "Purchase_200", Value = Purchase_200 },
            new StatisticUpdate { StatisticName = "TimePlayed", Value = TimePlayed },
            new StatisticUpdate { StatisticName = "TimePlayed_HM", Value = TimePlayed_HM },}
},
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    public void GetUserStatistics()
    {
    Trophy_Level = PlayerPrefs.GetInt("TrophyIndex", 0) + 1;
    Score = GameDataManager.GDM.currentPoints;
    HighScore = GameDataManager.GDM.HighScore_AllTime; ;
    GameOver = PlayerPrefs.GetInt("GameOver", 0);
    Score_HM = GameDataManager.GDM.HM_currentPoints;
    HighScore_HM = GameDataManager.GDM.HardModeHighScore_AllTime;
    GameOver_HM = PlayerPrefs.GetInt("GameOver_HM", 0);
    Swaps = GameDataManager.GDM.currentSwitches;
    Swaps_Used = PlayerPrefs.GetInt("Swaps_Used", 0);
    Swaps_Found = PlayerPrefs.GetInt("Swaps_Found", 0);
    Ads_Watched = PlayerPrefs.GetInt("Ads_Watched", 0);
    Purchase_30 = PlayerPrefs.GetInt("Purchase_30", 0);
    Purchase_75 = PlayerPrefs.GetInt("Purchase_75", 0);
    Purchase_200 = PlayerPrefs.GetInt("Purchase_200", 0);
    TimePlayed = Convert.ToInt32(PlayerPrefs.GetFloat("TimePlayed", 0));
    TimePlayed_HM = Convert.ToInt32(PlayerPrefs.GetFloat("TimePlayed_HM", 0));
    }
    #endregion

    #region Application Close/Pause

    void OnApplicationQuit()
    {
        SetPlayfabPlayerStatistics();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SetPlayfabPlayerStatistics();
        }
    }
    #endregion
}
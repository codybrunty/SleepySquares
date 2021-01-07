using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSG.iOSKeychain;
using PlayFab.ProfilesModels;
using PlayFab.AuthenticationModels;
using UnityEngine.SceneManagement;

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
        GetPlayfabStatistics();
    }


    private void OnLoginMobileFailure(PlayFabError error) {
        Debug.Log("Login Mobile Failure");
        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion

    #region Set Playfab Player Statistics
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
    public int Ads_Watched_Daily;
    public int Daily_Complete;
    public int Daily_Failed;

    public void SetPlayfabPlayerStatistics()
    {
        TimeManager.TM.SaveTimer();
        GetLocalUserStatistics();

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
            new StatisticUpdate { StatisticName = "TimePlayed_HM", Value = TimePlayed_HM },
            new StatisticUpdate { StatisticName = "Ads_Watched_Daily", Value = Ads_Watched_Daily },
            new StatisticUpdate { StatisticName = "Daily_Complete", Value = Daily_Complete },
            new StatisticUpdate { StatisticName = "Daily_Failed", Value = Daily_Failed },}
        },
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    public void GetLocalUserStatistics(){
        Trophy_Level = PlayerPrefs.GetInt("TrophyIndex", 0) + 1;
        Score = GameDataManager.GDM.currentPoints;
        HighScore = GameDataManager.GDM.HighScore_AllTime;
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
        Ads_Watched_Daily = PlayerPrefs.GetInt("Ads_Watched_Daily", 0);
        Daily_Complete = PlayerPrefs.GetInt("Daily_Complete", 0);
        Daily_Failed = PlayerPrefs.GetInt("Daily_Failed", 0);
    }
    #endregion

    #region Get Playfab Player Statistics
    public void GetPlayfabStatistics() {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), OnGetPlayfabStatistics, error=>Debug.LogError(error.GenerateErrorReport()));
    }

    public void OnGetPlayfabStatistics(GetPlayerStatisticsResult result) {
        foreach(var stat in result.Statistics) {
            switch (stat.StatisticName) {
                case "Trophy_Level":
                    CheckLocalAggainstPlayfab(stat.Value, result);
                    break;
                default:
                    break;
            }
        }
    }

    public void CheckLocalAggainstPlayfab(int playfabTrophyLevel, GetPlayerStatisticsResult result) {
        int localTrophyLevel = PlayerPrefs.GetInt("TrophyIndex", 0) + 1;
        if (playfabTrophyLevel > localTrophyLevel) {
            Debug.Log("Local Data Lost");
            //Debug.Log("playfab trophy: " + playfabTrophyLevel);
            //Debug.Log("local trophy: " + localTrophyLevel);
            LoadPlayfabStatisticsIntoGameData(result);
        }
        else {
            Debug.Log("Local Data Good");
            //Debug.Log("playfab trophy: " + playfabTrophyLevel);
            //Debug.Log("local trophy: " + localTrophyLevel);
        }
    }

    public void LoadPlayfabStatisticsIntoGameData(GetPlayerStatisticsResult result) {
        foreach (var stat in result.Statistics) {
            switch (stat.StatisticName) {
                case "Trophy_Level":
                    PlayerPrefs.SetInt("TrophyIndex", stat.Value-1);
                    SetTrophyPoints(stat.Value);
                    break;
                    /* Score resets or else users can exploit the uninstal reseting their board. 
                case "Score":
                    GameDataManager.GDM.currentPoints = stat.Value;
                    break;
                    */
                case "HighScore":
                    GameDataManager.GDM.HighScore_AllTime = stat.Value;
                    break;
                case "GameOver":
                    PlayerPrefs.SetInt("GameOver", stat.Value);
                    break;
                    /* Score_HM resets or else users can exploit the uninstal reseting their board. 
                case "Score_HM":
                    GameDataManager.GDM.HM_currentPoints = stat.Value;
                    break;
                    */
                case "HighScore_HM":
                    GameDataManager.GDM.HardModeHighScore_AllTime = stat.Value;
                    break;
                case "GameOver_HM":
                    PlayerPrefs.SetInt("GameOver_HM", stat.Value);
                    break;
                case "Swaps":
                    GameDataManager.GDM.currentSwitches = stat.Value;
                    break;
                case "Swaps_Used":
                    PlayerPrefs.SetInt("Swaps_Used", stat.Value);
                    break;
                case "Swaps_Found":
                    PlayerPrefs.SetInt("Swaps_Found", stat.Value);
                    break;
                case "Ads_Watched":
                    PlayerPrefs.SetInt("Ads_Watched", stat.Value);
                    break;
                case "Purchase_30":
                    PlayerPrefs.SetInt("Purchase_30", stat.Value);
                    break;
                case "Purchase_75":
                    PlayerPrefs.SetInt("Purchase_75", stat.Value);
                    break;
                case "Purchase_200":
                    PlayerPrefs.SetInt("Purchase_200", stat.Value);
                    break;
                case "TimePlayed":
                    float tp = (float)stat.Value;
                    PlayerPrefs.SetFloat("TimePlayed", tp);
                    break;
                case "TimePlayed_HM":
                    float tp_hm = (float)stat.Value;
                    PlayerPrefs.SetFloat("TimePlayed_HM", tp_hm);
                    break;
                case "Ads_Watched_Daily":
                    PlayerPrefs.SetInt("Ads_Watched_Daily", stat.Value);
                    break;
                case "Daily_Complete":
                    PlayerPrefs.SetInt("Daily_Complete", stat.Value);
                    break;
                case "Daily_Failed":
                    PlayerPrefs.SetInt("Daily_Failed", stat.Value);
                    break;
                default:
                    break;
            }
        }
        GameDataManager.GDM.SaveGameData();
    }

    private void SetTrophyPoints(int trophyLevel) {
        List<int> trophyLevelScores = new List<int> {   0,      150,    500,    1500,   2500,
                                                                5000,   7500,   10000,  12500,  15000,
                                                                18000,  21000,  24000,  27000,  30000,
                                                                35000,  40000,  45000,  50000,  60000,
                                                                70000,  80000,  90000,  105000,  120000,
                                                                135000,  150000, 175000, 200000, 225000};
        PlayerPrefs.SetInt("TrophyPoints", trophyLevelScores[trophyLevel - 1]);
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
 
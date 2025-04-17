using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : MonoBehaviour
{
    #region Singleton
    public static TimeManager TM;
    private void Awake()
    {
        if (TM == null)
        {
            TM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        mainCoroutine=StartCoroutine(GetDateTime());

    }
    #endregion

    #region Variables
    public float currentTime = 0f;
    public float currentTime_HM = 0f;

    public bool timerOn = false;
    private bool hardMode = false;
    [SerializeField] GameBoardMechanics gameboard = default;

    private string _timeData;
    private string _currentTime;
    public string _currentDate;

    private int totalSecondsInADay = 86400;
    public bool countdownOn = false;
    public float timeLeftInSeconds;
    private Coroutine  mainCoroutine;
    #endregion

    #region Play Times
    public void StartNormalTimer()
    {
        hardMode = false;
        timerOn = true;
    }

    public void StartHardTimer()
    {
        hardMode = true;
        timerOn = true;
    }

    private void Update()
    {
        if (timerOn)
        {
            if (hardMode == false)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime_HM += Time.deltaTime;
            }
        }

        if (countdownOn) {
            UpdateCountdown();
        }
    }

    public void SaveTimer()
    {
        float normalTime = PlayerPrefs.GetFloat("TimePlayed", 0f);
        float hardTime = PlayerPrefs.GetFloat("TimePlayed_HM", 0f);
        PlayerPrefs.SetFloat("TimePlayed", normalTime + currentTime);
        PlayerPrefs.SetFloat("TimePlayed_HM", hardTime + currentTime_HM);

        Debug.Log("Saved Normal Time " + (normalTime + currentTime));
        Debug.Log("Saved Hard Time " + (hardTime + currentTime_HM));

        currentTime = 0f;
        currentTime_HM = 0f;
    }

    public void StopTimer()
    {
        timerOn = false;
    }
    #endregion

    #region GetDateTime

    public IEnumerator GetDateTime() {
        Debug.Log("Connecting to Internet");
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("https://www.microsoft.com");
        yield return myHttpWebRequest.SendWebRequest();

        if (myHttpWebRequest.error == null) {
            Debug.Log("Got DateTime from the Internet");
            string netTime = myHttpWebRequest.GetResponseHeader("date");
            DateTime netTimeParsed = DateTime.ParseExact(netTime, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
            _timeData = netTimeParsed.ToString("MM-dd-yyyy/HH:mm:ss");
        }

        else {
            Debug.Log("Error Connecting to Internet, Grabbing Local DateTime");
            DateTime localTime = DateTime.Now;
            _timeData = localTime.ToString("MM-dd-yyyy/HH:mm:ss");
        }

        if (_timeData != "") {
            string[] words = _timeData.Split('/');
            Debug.Log("The date is: " + words[0]);
            Debug.Log("The time is: " + words[1]);
            _currentDate = words[0];
            _currentTime = words[1];
            countdownOn = true;
            CountdownStart();
        }

    }


    public int GetDateInt() {
        string[] words = _currentDate.Split('-');
        int x = int.Parse(words[0] + words[1] + words[2]);
        return x;
    }

    public string GetTimeString() {
        return _currentTime;
    }

    #endregion

    #region Daily Countdown

    public void CountdownStart() {

        string[] timeArray = _currentTime.Split(':');
        int hr = int.Parse(timeArray[0]);
        int min = int.Parse(timeArray[1]);
        int sec = int.Parse(timeArray[2]);

        int totalSeconds = hr * 3600;
        totalSeconds += (min * 60);
        totalSeconds += sec;

        timeLeftInSeconds = totalSecondsInADay - totalSeconds;
    }

    public void UpdateCountdown() {
        timeLeftInSeconds -= Time.deltaTime;


        if(timeLeftInSeconds < 0) {
            countdownOn = false;
            if (mainCoroutine != null) {
                StopCoroutine(mainCoroutine);
            }
            mainCoroutine = StartCoroutine(GetDateTime());
        }
    }

    #endregion

    #region Application Pause
    void OnApplicationFocus(bool focusStatus) {
        if (focusStatus) {
            if (mainCoroutine != null) {
                StopCoroutine(mainCoroutine);
            }
            mainCoroutine = StartCoroutine(GetDateTime());
        }
    }
    #endregion
}

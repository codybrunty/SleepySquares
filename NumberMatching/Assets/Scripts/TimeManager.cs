using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    #endregion

    public float currentTime = 0f;
    public float currentTime_HM = 0f;

    public bool timerOn = false;
    private bool hardMode = false;
    [SerializeField] GameBoardMechanics gameboard = default;

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
}

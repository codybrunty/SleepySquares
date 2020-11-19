using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour
{
    [SerializeField] Image alert = default;
    [SerializeField] GameObject ScaleGRP = default;

    private void Start()
    {
        CheckAlertStatus();
    }

    public void CheckAlertStatus()
    {
        alert.gameObject.SetActive(false);
        int trophyIndex = PlayerPrefs.GetInt("TrophyIndex", 0);

        if (trophyIndex + 1 >= 3)
        {
            int playedHardMode = PlayerPrefs.GetInt("PlayedHardMode", 0);
            if (playedHardMode == 0)
            {
                StartCoroutine(ShowAlert());
            }
        }
    }

    IEnumerator ShowAlert()
    {
        yield return new WaitForSeconds(0.5f);
        alert.gameObject.SetActive(true);
        SoundManager.SM.PlayOneShotSound("yahoo");
        iTween.PunchScale(ScaleGRP, new Vector3(2f, 2f, 2f), 0.75f);
    }
}

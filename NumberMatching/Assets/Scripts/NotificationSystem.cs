using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour
{
    [SerializeField] Image alert = default;
    [SerializeField] GameObject ScaleGRP = default;

    public void ActivateNotification()
    {
        StartCoroutine(ShowAlert());
    }

    IEnumerator ShowAlert()
    {
        yield return new WaitForSeconds(0.5f);
        alert.gameObject.SetActive(true);
        SoundManager.SM.PlayOneShotSound("yahoo");
        iTween.PunchScale(ScaleGRP, new Vector3(2f, 2f, 2f), 0.75f);
    }

    public void HideAlert() {
        alert.gameObject.SetActive(false);
    }
}

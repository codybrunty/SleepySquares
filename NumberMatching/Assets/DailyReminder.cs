using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyReminder : MonoBehaviour{

    public NotificationSystem m_oNotificationSystem;

    private void Start() {

        //makes sure they are above trophy level 1
        if (PlayerPrefs.GetInt("TrophyIndex", 0) > 0) {
            if (PlayerPrefs.GetInt("DailyReminder", 0) == 0) {
                m_oNotificationSystem.ActivateNotification();
            }
        }

    }

}

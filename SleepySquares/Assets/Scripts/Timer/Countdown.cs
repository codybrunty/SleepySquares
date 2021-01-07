using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI countdown;
    void Update()
    {
        if (TimeManager.TM.countdownOn) {
            UpdateCountdownDisplay();
        }
    }

    public void UpdateCountdownDisplay() {
        int secondsLeft = (int)TimeManager.TM.timeLeftInSeconds;

        int h = (secondsLeft / 3600);
        int m = (secondsLeft - (3600 * h)) / 60;
        int s = (secondsLeft - (3600 * h) - (m * 60));

        countdown.text = h.ToString("00")+":"+m.ToString("00") + ":"+s.ToString("00");
    }
}

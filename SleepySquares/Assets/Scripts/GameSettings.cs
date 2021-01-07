using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public static GameSettings GS;

    public int timerStatus = 0;

    private void Awake() {
        if (GS == null) {
            GS = this;
            //timerStatus = PlayerPrefs.GetInt("TimerSettings", 1);
            //force timer off
            timerStatus = 0;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
    }


}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public static GameSettings GS;

    public int timerStatus = 1;

    private void Awake() {
        if (GS == null) {
            GS = this;
            timerStatus = PlayerPrefs.GetInt("TimerSettings", 1);
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
    }


}




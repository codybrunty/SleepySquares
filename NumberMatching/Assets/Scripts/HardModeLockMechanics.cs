using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardModeLockMechanics : MonoBehaviour{

    [SerializeField] GameObject HardModeLocked = default;
    [SerializeField] GameObject HardModeUnlocked = default;


    private void OnEnable() {

        int trophyIndex = PlayerPrefs.GetInt("TrophyIndex", 0);

        if (trophyIndex+1 >= 3) {
            HardModeLocked.SetActive(false);
            HardModeUnlocked.SetActive(true);
        }
        else {
            HardModeLocked.SetActive(true);
            HardModeUnlocked.SetActive(false);
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardModeLockMechanics : MonoBehaviour{

    [SerializeField] TrophySystem trophies = default;
    [SerializeField] GameObject HardModeLocked = default;
    [SerializeField] GameObject HardModeUnlocked = default;


    private void OnEnable() {
        
        if (trophies.trophyIndex+1 >=5) {
            HardModeLocked.SetActive(false);
            HardModeUnlocked.SetActive(true);
        }
        else {
            HardModeLocked.SetActive(true);
            HardModeUnlocked.SetActive(false);
        }

    }


}

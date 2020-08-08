using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardModeHideButtons : MonoBehaviour{

    [SerializeField] GameObject hardModeLocked = default;
    [SerializeField] GameObject hardModeUnLocked = default;
    [SerializeField] TrophySystem trophyPanel = default;


    private void OnEnable() {
        int lvl = trophyPanel.trophyIndex + 1;
        if (lvl >= 5) {
            hardModeUnLocked.SetActive(true);
            hardModeLocked.SetActive(false);
        }
        else {
            hardModeUnLocked.SetActive(false);
            hardModeLocked.SetActive(true);
        }
    }


}

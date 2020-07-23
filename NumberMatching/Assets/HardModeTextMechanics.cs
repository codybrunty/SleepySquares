using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HardModeTextMechanics : MonoBehaviour{

    private void Start() {
        UpdateHardText();
    }

    public void UpdateHardText() {
        if (GameDataManager.GDM.hardModeOn == 1) {
            gameObject.GetComponent<TextMeshProUGUI>().text = "HARD";
        }
        else {
            gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}

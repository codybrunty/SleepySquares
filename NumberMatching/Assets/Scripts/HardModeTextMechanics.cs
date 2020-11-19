using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HardModeTextMechanics : MonoBehaviour{

    private SpriteRenderer hardmodeImage;

    private void Start() {
        hardmodeImage = gameObject.GetComponent<SpriteRenderer>();
        UpdateHardText();
    }

    public void UpdateHardText() {
        if (GameDataManager.GDM.hardModeOn == 1) {
            TurnHardModeImageOn();
        }
        else {
            TurnHardModeImageOff();
        }
    }

    private void TurnHardModeImageOn() {
        Color newColorImage = new Color(hardmodeImage.color.r, hardmodeImage.color.g, hardmodeImage.color.b, 1f);
        hardmodeImage.color = newColorImage;
    }

    private void TurnHardModeImageOff() {
        Color newColorImage = new Color(hardmodeImage.color.r, hardmodeImage.color.g, hardmodeImage.color.b, 0f);
        hardmodeImage.color = newColorImage;
    }
}

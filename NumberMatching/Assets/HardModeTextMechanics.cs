using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HardModeTextMechanics : MonoBehaviour{

    [SerializeField] TextMeshProUGUI hardmodeText = default;
    private Image hardmodeImage;

    private void Start() {
        hardmodeImage = gameObject.GetComponent<Image>();
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
        Color newColorText = new Color(hardmodeText.color.r, hardmodeText.color.g, hardmodeText.color.b, 1f);
        hardmodeImage.color = newColorImage;
        hardmodeText.color = newColorText;
    }

    private void TurnHardModeImageOff() {
        Color newColorImage = new Color(hardmodeImage.color.r, hardmodeImage.color.g, hardmodeImage.color.b, 0f);
        Color newColorText = new Color(hardmodeText.color.r, hardmodeText.color.g, hardmodeText.color.b, 0f);
        hardmodeImage.color = newColorImage;
        hardmodeText.color = newColorText;
    }
}

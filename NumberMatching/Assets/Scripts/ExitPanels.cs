using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanels : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] GameObject settingsPanel = default;

    public void ExitOnClick() {
        PlayClickSFX();
        settingsPanel.SetActive(false);
        gameObject.SetActive(false);

        if (gameboard.gameOver != true) {
            gameboard.touchEnabled = true;
        }

    }

    private void PlayClickSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("deselect1");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanels : MonoBehaviour{

    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] GameObject settingsPanel = default;

    public void ExitOnClick() {
        gameboard.touchEnabled = true;
        settingsPanel.SetActive(false);
        gameObject.SetActive(false);

    }


}

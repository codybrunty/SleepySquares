using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPanels : MonoBehaviour{
    
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] GameObject panelOn = default;
    [SerializeField] GameObject exitButton = default;

    public void EnterOnClick() {
        PlayClickSFX();

        gameboard.touchEnabled = false;
        panelOn.SetActive(true);
        exitButton.SetActive(true);
    }

    private void PlayClickSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
    }


}

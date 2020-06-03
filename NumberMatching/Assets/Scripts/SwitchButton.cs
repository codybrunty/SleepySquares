using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchButton : MonoBehaviour{

    public int switchAmmount = 3;
    [SerializeField] RaycastMouse click = default;
    [SerializeField] TextMeshProUGUI switchText = default;
    public bool activated = false;



    private void Start() {
        switchAmmount = GameDataManager.GDM.currentSwitches;
        UpdateSwitchAmmountDisplay();
    }

    public void UpdateSwitchAmmountDisplay() {
        switchText.text = switchAmmount.ToString();
    }

    public void SwitchButtonOnClick() {

        if (!activated) {
            TurnOnSwitchMode();
        }
        else {
            TurnOffSwitchMode();
        }

    }

    public void TurnOnSwitchMode() {
        PlayPositiveSFX();

        if (switchAmmount > 0) {
            activated = true;
            click.switchSquares = true;
            //gameboard switch mode off
        }
        else {
            Debug.Log("you outta switches sucka");
        }

    }

    private void PlayPositiveSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
    }

    private void PlayNegativeSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("deselect1");
    }

    public void TurnOffSwitchMode() {
        PlayNegativeSFX();

        activated = false;
        click.switchSquares = false;
    }

    public void ReduceSwitchAmmount() {
        switchAmmount--;
        UpdateSwitchAmmountDisplay();
    }
}

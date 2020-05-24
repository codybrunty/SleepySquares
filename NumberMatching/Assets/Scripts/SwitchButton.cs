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
        UpdateSwitchAmmountDisplay();
    }

    private void UpdateSwitchAmmountDisplay() {
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
        if (switchAmmount > 0) {
            activated = true;
            click.switchSquares = true;
            //gameboard switch mode off
        }
        else {
            Debug.Log("you outta switches sucka");
        }

    }

    public void TurnOffSwitchMode() {
        activated = false;
        click.switchSquares = false;
        //gameboard switch mode off
    }

    public void ReduceSwitchAmmount() {
        switchAmmount--;
        UpdateSwitchAmmountDisplay();
    }
}

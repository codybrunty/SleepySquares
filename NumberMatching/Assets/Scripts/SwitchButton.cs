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
        PopAnim();
        PlayClickSFX();

        if (switchAmmount > 0) {
            activated = true;
            click.switchSquares = true;
            //gameboard switch mode off
        }
        else {
            Debug.Log("you outta switches sucka");
        }

    }

    private void PlayClickSFX() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
    }

    private void PopAnim() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(.25f, .25f, 0f));
        hash.Add("time", 0.5f);
        hash.Add("oncomplete", "PopDone");
        iTween.PunchScale(gameObject, hash);
    }

    private void PopDone() {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void TurnOffSwitchMode() {
        PopAnim();
        PlayClickSFX();

        activated = false;
        click.switchSquares = false;
    }

    public void ReduceSwitchAmmount() {
        switchAmmount--;
        UpdateSwitchAmmountDisplay();
    }
}

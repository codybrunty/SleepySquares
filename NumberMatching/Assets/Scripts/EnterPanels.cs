using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPanels : MonoBehaviour{
    
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] GameObject panelOn = default;
    [SerializeField] GameObject exitButton = default;

    public void EnterOnClick() {
        PlayClickSFX();
        PopAnim();

        gameboard.touchEnabled = false;
        panelOn.SetActive(true);
        exitButton.SetActive(true);
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

}

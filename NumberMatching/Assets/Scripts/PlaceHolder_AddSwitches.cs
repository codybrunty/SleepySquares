using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder_AddSwitches : MonoBehaviour {

    [SerializeField] SwitchButton switchButton = default;

    public void AddSwitchesOnClick() {
        GameDataManager.GDM.currentSwitches++;
        GameDataManager.GDM.SaveGameData();
        
        switchButton.switchAmmount = GameDataManager.GDM.currentSwitches;
        switchButton.UpdateSwitchAmmountDisplay();
    }

}

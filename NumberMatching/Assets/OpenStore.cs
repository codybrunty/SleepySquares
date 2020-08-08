using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStore : MonoBehaviour{

    [SerializeField] SettingsPanels settings = default;

    public void OpenStoreDisplay() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
        settings.ShowStore();
    }

}

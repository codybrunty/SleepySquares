using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStore : MonoBehaviour{

    [SerializeField] GameObject store = default;

    public void OpenStoreDisplay() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
        store.SetActive(true);
    }

}

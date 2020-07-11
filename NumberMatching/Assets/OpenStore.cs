using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStore : MonoBehaviour{

    [SerializeField] GameObject store = default;

    public void OpenStoreDisplay() {
        store.SetActive(true);
    }

}

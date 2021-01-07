using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeMechanics : MonoBehaviour{

    public float destroyTime = 1f;

    private void Start() {
        Destroy(gameObject, destroyTime);
    }

}

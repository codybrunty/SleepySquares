using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCameraHolder : MonoBehaviour{

    private void Start() {
        gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(0f,0.5f,0f);
    }


}

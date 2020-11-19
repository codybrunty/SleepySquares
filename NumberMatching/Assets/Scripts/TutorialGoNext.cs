using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoNext : MonoBehaviour{

    [SerializeField] TutorialManager tutorialManager = default;
    public bool animationDone = false;

    private void Update() {
        RaycastForClickArea();
    }

    private void RaycastForClickArea() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask_clickArea = LayerMask.NameToLayer(layerName: "TutorialClickArea");
            RaycastHit2D hit_onClickArea = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_clickArea);

            if (hit_onClickArea.collider != null) {
                GoToNextOnClick();
            }
        }
    }

    public void GoToNextOnClick() {
        animationDone = false;
        tutorialManager.UpdateTutorialDisplay();
    }

}

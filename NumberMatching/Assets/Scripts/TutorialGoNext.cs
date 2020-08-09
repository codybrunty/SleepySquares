using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoNext : MonoBehaviour{

    [SerializeField] TutorialManager tutorialManager = default;

    public void GoToNextOnClick() {
        tutorialManager.UpdateTutorialDisplay();
    }

}

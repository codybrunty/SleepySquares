using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour{

    public int tutorialIndex = -1;
    public List<GameObject> tutorial_UI = default;
    public List<GameObject> tutorial_Game = default;

    private void Start() {
        tutorialIndex = -1;
        UpdateTutorialDisplay();
    }

    public void UpdateTutorialDisplay() {
        tutorialIndex++;
        if (tutorialIndex == tutorial_UI.Count) {
            tutorialIndex = 0;
        }
        for (int i =0; i < tutorial_UI.Count; i++) {
            tutorial_UI[i].SetActive(false);
            tutorial_Game[i].SetActive(false);
        }
        tutorial_UI[tutorialIndex].SetActive(true);
        tutorial_Game[tutorialIndex].SetActive(true);
    }
}

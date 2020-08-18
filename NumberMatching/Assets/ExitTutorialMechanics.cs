using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitTutorialMechanics : MonoBehaviour{

    private Button exitButton;
    private Image exitImage;

    private void Start() {
        exitButton = gameObject.GetComponent<Button>();
        exitImage = gameObject.GetComponent<Image>();

        int tutorialStatus = PlayerPrefs.GetInt("TutorialComplete", 0);
        if (tutorialStatus == 0) {
            exitButton.interactable = false;
            exitImage.raycastTarget = false;
            exitImage.enabled = false;
        }
        else {
            exitButton.interactable = true;
            exitImage.raycastTarget = true;
            exitImage.enabled = true;
        }
    }

    public void ExitTutorialOnClick() {
        SceneManager.LoadScene("Game");
    }

}

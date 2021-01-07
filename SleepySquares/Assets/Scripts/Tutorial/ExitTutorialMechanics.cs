using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitTutorialMechanics : MonoBehaviour{

    private Button exitButton;
    private Image exitImage;
    [SerializeField] SplashScreenTransition splash;
    [SerializeField] GameObject languageButton;

    private void Start() {
        exitButton = gameObject.GetComponent<Button>();
        exitImage = gameObject.GetComponent<Image>();

        int tutorialStatus = PlayerPrefs.GetInt("TutorialComplete", 0);
        if (tutorialStatus == 0) {
            exitButton.interactable = false;
            exitImage.raycastTarget = false;
            exitImage.enabled = false;
            languageButton.SetActive(true);
        }
        else {
            exitButton.interactable = true;
            exitImage.raycastTarget = true;
            exitImage.enabled = true;
            languageButton.SetActive(false);
        }
    }

    public void ExitTutorialOnClick() {
        splash.FadeInSplash();
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadSceneAsync("Game");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadTutorialScene : MonoBehaviour{

    [SerializeField] SplashScreenTransition splash = default;

    public void LoadScene() {
        splash.FadeInSplash();
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
        StartCoroutine(LoadTutorial());

    }

    IEnumerator LoadTutorial()
    {
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadSceneAsync("Tutorial");
    }

}

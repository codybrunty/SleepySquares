using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    [SerializeField] GameObject BombChompSplash = default;
    public float HoldBombChomp = 2f;
    public float HoldSleepy = 2f;
    public string sceneName = "";

    private void Start()
    {
        StartCoroutine(HideBombChomp());
    }

    IEnumerator HideBombChomp()
    {
        yield return new WaitForSeconds(HoldBombChomp);
        BombChompSplash.SetActive(false);
        StartCoroutine(LoadNextScene());
    }
    IEnumerator LoadNextScene()
    {
        int tutorial = PlayerPrefs.GetInt("TutorialComplete",0);
        if (tutorial == 1)
        {
            sceneName = "Gameboard";
        }
        else {
            sceneName = "Tutorial";
        }
        yield return new WaitForSeconds(HoldSleepy);
        SceneManager.LoadSceneAsync(sceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGameData : MonoBehaviour
{

    public void ResetGameDataOnClick()
    {
        PlayerPrefs.DeleteAll();
        GameDataManager.GDM.ResetGameData();
        GameDataManager.GDM.SaveGameData();
        SceneManager.LoadScene("Game");
    }

}

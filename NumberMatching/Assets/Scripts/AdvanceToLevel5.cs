using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvanceToLevel5 : MonoBehaviour
{
    [SerializeField] ResetGameScene resetMechanics = default;

    private void Start()
    {
        CheckTrophyLevelAndDebugButtonDisplay();
    }

    private void CheckTrophyLevelAndDebugButtonDisplay()
    {
        int trophyIndex = PlayerPrefs.GetInt("TrophyIndex", 0);

        if (trophyIndex > 3)
        {
            gameObject.SetActive(false);
        }
    }

    public void ProfileLevel5OnClick()
    {
        PlayerPrefs.SetInt("TrophyIndex", 4);
        PlayerPrefs.SetInt("TrophyPoints", 2501);
        GameDataManager.GDM.TotalPoints_AllTime = 2501;

        resetMechanics.ResetGameOverOnClick();
        SceneManager.LoadScene("Game");
    }

}

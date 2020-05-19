using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGameScene : MonoBehaviour{

    public void ResetOnClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}

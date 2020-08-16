using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadTutorialScene : MonoBehaviour{


    public void LoadScene() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
        SceneManager.LoadScene("Tutorial");
    }


}

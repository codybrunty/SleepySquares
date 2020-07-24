using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPanel : MonoBehaviour{

    public long score = default;
    public long highscore = default;
    [SerializeField] TextMeshProUGUI text_score = default;
    [SerializeField] TextMeshProUGUI text_highscore = default;

    public void UpdateGameOverPanel() {
        text_score.text = score.ToString();
        text_highscore.text = highscore.ToString();
    }

    public void GameOverPanelAnimation() {
        Hashtable hash = new Hashtable();
        hash.Add("scale", new Vector3(1f, 1f, 1f));
        hash.Add("time", .5f);
        iTween.ScaleTo(gameObject.transform.GetChild(0).gameObject, hash);
    }

    public void ResetGameOveerPanelScale() {
        gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(.01f, .01f, 1f);
    }
}

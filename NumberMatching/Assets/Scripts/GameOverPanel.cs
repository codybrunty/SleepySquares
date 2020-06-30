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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HardModeTextMechanics : MonoBehaviour{

    private SpriteRenderer flagImage;
    public GameBoardMechanics gameboard;
    public List<Sprite> flags = new List<Sprite>();
    public DailyManager m_oDailyManager;
    public BestScoreIcon m_oBestScoreIcon;


    private void Start() {
        flagImage = gameObject.GetComponent<SpriteRenderer>();
        UpdateHardText();
    }

    public void UpdateHardText() {
        if (gameboard.DailyModeOn) {
            flagImage.sprite = flags[2];
            m_oDailyManager.EnableHearts();
            m_oBestScoreIcon.EnableArrow();
        }
        else {
            flagImage.sprite = flags[GameDataManager.GDM.hardModeOn];
            m_oDailyManager.DisableHearts();
            m_oBestScoreIcon.EnableCrown();
        }
    }

}

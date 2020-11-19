using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophiesPanel : MonoBehaviour
{
    [SerializeField] List<TrophiesPanel_Trophy> trophy_icons = new List<TrophiesPanel_Trophy>();
    [SerializeField] List<Sprite> trophies = new List<Sprite>();
    [SerializeField] List<Color> trophy_color = new List<Color>();
    [SerializeField] List<Color> outline_color = new List<Color>();

    private void OnEnable()
    {
        SetUpDisplays();
    }

    private void SetUpDisplays()
    {
        int trophyIndex = PlayerPrefs.GetInt("TrophyIndex", 0);
        if (trophyIndex > 29)
        {
            trophyIndex = 29;
        }


        for (int i = 0; i < trophy_icons.Count; i++)
        {
            trophy_icons[i].lvl.text = (i + 1).ToString();
            trophy_icons[i].trophy.sprite = trophies[i];
            trophy_icons[i].trophy.color = trophy_color[i%3];
            trophy_icons[i].outline.GetComponent<Image>().color = outline_color[0];

        }

        for (int i = 0; i < trophyIndex+1; i++)
        {
            trophy_icons[i].lvl.text = (i + 1).ToString();
            trophy_icons[i].trophy.gameObject.SetActive(true);
            trophy_icons[i].questionMark.gameObject.SetActive(false);
        }
        trophy_icons[trophyIndex].outline.GetComponent<Image>().color = outline_color[1];
    }
}

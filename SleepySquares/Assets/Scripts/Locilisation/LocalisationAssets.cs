using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LocalisationAssets : MonoBehaviour
{

    public string currentLanguage;

    [Header("Font Assets")]
    [SerializeField] TMP_FontAsset font_TH = default;
    [SerializeField] TMP_FontAsset font_JP = default;
    [SerializeField] TMP_FontAsset font_CN = default;
    [SerializeField] TMP_FontAsset font_KO = default;

    private void Awake()
    {
        currentLanguage = PlayerPrefs.GetString("Language", "English");
        LocalisationSystem.SetLocalisedLanguage(currentLanguage);
    }

    private void Start()
    {
        string currentLanguage = PlayerPrefs.GetString("Language", "English");

        
        if (currentLanguage == "Thai")
        {
            foreach (GameObject root in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (root.transform.parent == null)
                {
                    TextMeshProUGUI[] allTexts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI text in allTexts)
                    {
                        if (text.GetComponent<TextLocalisation>() != null)
                        {
                            text.font = font_TH;
                        }
                    }
                }
            }
        }

        if (currentLanguage == "Japan")
        {
            foreach (GameObject root in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (root.transform.parent == null)
                {
                    TextMeshProUGUI[] allTexts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI text in allTexts)
                    {
                        if (text.GetComponent<TextLocalisation>() != null)
                        {
                            text.font = font_JP;
                        }
                    }
                }
            }
        }

        if (currentLanguage == "Chinese")
        {
            foreach (GameObject root in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (root.transform.parent == null)
                {
                    TextMeshProUGUI[] allTexts = root.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI text in allTexts)
                    {
                        if (text.GetComponent<TextLocalisation>() != null)
                        {
                            text.font = font_CN;
                        }
                    }
                }
            }
        }

        if (currentLanguage == "Korean")
        {
            foreach (GameObject root in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (root.transform.parent == null)
                {
                    TextMeshProUGUI[] allTexts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI text in allTexts)
                    {
                        if (text.GetComponent<TextLocalisation>() != null)
                        {
                            text.font = font_KO;
                        }
                    }
                }
            }
        }
        
    }

}


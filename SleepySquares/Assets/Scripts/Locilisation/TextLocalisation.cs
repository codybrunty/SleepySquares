﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalisation : MonoBehaviour
{

    TextMeshProUGUI textfield;
    public string key;

    private void Start()
    {
        if (key != "")
        {
            textfield = GetComponent<TextMeshProUGUI>();
            string value = LocalisationSystem.GetLocalisedValue(key);
            //Debug.LogWarning(value);
            textfield.text = value;
        }
    }
}

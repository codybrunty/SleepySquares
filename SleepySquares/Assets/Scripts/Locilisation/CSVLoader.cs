using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVLoader
{
    private TextAsset csvFile;
    private char lineSeperator = '\n';
    private char surround = '\'';
    private string[] fieldSeperator = { "'|'" };

    public void LoadCSV()
    {
        csvFile = Resources.Load<TextAsset>("localisation");
    }

    public Dictionary<string, string> GetDictionaryValues(string attributeID)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        string[] lines = csvFile.text.Split(lineSeperator);
        int attributeIndex = -1;
        //Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        string[] headers = lines[0].Split(fieldSeperator, StringSplitOptions.None);
        //string[] headers = CSVParser.Split(lines[0]);

        for (int i = 0; i < headers.Length; i++) {
            headers[i] = TrimPhrase(headers[i]);
            if (headers[i].Contains(attributeID))
            {
                attributeIndex = i;
                break;
            }
        }
        
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] fields = line.Split(fieldSeperator, StringSplitOptions.None);

            for (int f = 0; f < fields.Length; f++)
            {
                fields[f] = TrimPhrase(fields[f]);
            }
            if (fields.Length > attributeIndex)
            {
                string key = fields[0].ToString();
                if (dictionary.ContainsKey(key)) { continue; }
                string value = fields[attributeIndex];
                dictionary.Add(key, value);
            }
            
        }
        return dictionary;
    }

    private string TrimPhrase(string phrase) {
        phrase = phrase.Trim();
        phrase = phrase.TrimStart(' ', surround);
        phrase = phrase.TrimEnd(surround);
        phrase = phrase.Trim();
        return phrase;
    }

}


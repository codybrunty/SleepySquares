using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongNameTest : MonoBehaviour{

    void Update(){
        foreach (Sound song in MusicManager.MM.songs) {
            if (song.source.isPlaying == true) {
                gameObject.GetComponent<TextMeshProUGUI>().text = song.name;
            }
        }
    }
}

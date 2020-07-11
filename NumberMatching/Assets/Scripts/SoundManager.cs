using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour{

    public static SoundManager SM;
    public int soundOn;
    public Sound[] sounds;

    private void Awake() {
        if (SM == null) {
            SM = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }


        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        soundOn = PlayerPrefs.GetInt("UserSoundOn", 1);
    }

    public void PlayOneShotSound(string name) {
        if (soundOn == 1) {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null) {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            else {
                s.source.PlayOneShot(s.source.clip);
            }
        }
    }

    public void TurnOffSound() {
        Debug.Log("Turn Sound Off");
        soundOn = 0;
        PlayerPrefs.SetInt("UserSoundOn", soundOn);
    }

    public void TurnOnSound() {
        Debug.Log("Turn Sound On");
        soundOn = 1;
        PlayerPrefs.SetInt("UserSoundOn", soundOn);
        PlayClickSound();
    }

    private void PlayClickSound() {
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
    }

}

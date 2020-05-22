using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour{

    public static SoundManager SM;

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
    }

    public void PlayOneShotSound(string name) {
        //int soundOn = FindObjectOfType<SoundButtonMechanics>().soundOn;
        //if (soundOn == 1) {
        //}

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

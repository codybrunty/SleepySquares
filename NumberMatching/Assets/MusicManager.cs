using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicManager : MonoBehaviour{

    public static MusicManager MM;
    public int musicOn;
    public int songIndex;
    public Sound[] songs;
    public bool fadeOut = false;

    private bool audioBlendInprogress = false;

    private void Awake() {
        if (MM == null) {
            MM = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }

        foreach (Sound song in songs) {
            song.source = gameObject.AddComponent<AudioSource>();
            song.source.clip = song.clip;
            song.source.volume = song.volume;
            song.source.pitch = song.pitch;
            song.source.loop = song.loop;
        }
        
        musicOn = PlayerPrefs.GetInt("UserMusicOn", 1);
    }

    private void Start() {

        if (musicOn == 1) {
            StartMusic();
        }

    }

    private void PlayRandomSong() {
        songIndex = UnityEngine.Random.Range(0, songs.Length);
        songs[songIndex].source.Play();
    }

    private void Update() {
        if (musicOn == 1) {
            IsMusicPlaying();
        }
    }

    private void IsMusicPlaying() {
        bool musicPlaying = false;
        foreach (Sound song in songs) {
            if (song.source.isPlaying == true) {
                musicPlaying = true;
            }
        }

        if (musicPlaying == false && fadeOut == false) {
            ContinueLoopingMusic();
        }
    }

    public void FadeOutCurrentMusic() {
        StartCoroutine(FadeOut(songs[songIndex].source, 1f));
    }

    public void FadeInNewMusic() {
        IterateMusicIndex();
        StartCoroutine(FadeIn(songs[songIndex].source, 1f));
        fadeOut = false;
    }


    private void ContinueLoopingMusic() {
        songs[songIndex].source.Play();
    }

    public void StopMusic() {
        PlayerPrefs.SetInt("UserMusicOn", 0);
        musicOn = PlayerPrefs.GetInt("UserMusicOn", 0);

        foreach (Sound song in songs) {
            song.source.Stop();
        }
    }

    public void StartMusic() {
        PlayerPrefs.SetInt("UserMusicOn", 1);
        musicOn = PlayerPrefs.GetInt("UserMusicOn", 1);

        PlayRandomSong();
    }

    private void IterateMusicIndex() {
        songIndex++;
        if (songIndex == songs.Length){
            songIndex = 0;
        }
    }


    IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
       float startVolume = audioSource.volume;

       while (audioSource.volume > 0) {
           audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
           yield return null;
       }

       audioSource.Stop();
       audioSource.volume = startVolume;
       fadeOut = true;
    }


    IEnumerator FadeIn(AudioSource audioSource, float FadeTime) {

        audioSource.volume = 0;
        audioSource.Play();

        for (float t = 0; t < FadeTime; t += Time.deltaTime) {
            audioSource.volume = Mathf.Lerp(0f, .2f, t / FadeTime);
            yield return null;
        }
        audioSource.volume = 0.2f;
    }


}

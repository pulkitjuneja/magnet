using UnityEngine;
using System;
using System.Collections;

class AudioManager {
    static bool isSFXEnabled = true;
    public static AudioSource musicSource;
    public static void toggleSFX(bool var){
        isSFXEnabled = var;
    }

    public static void toggleMusic(bool var){
        musicSource.enabled = var;
    }

    public static void play(AudioSource aud, AudioClip clip) {
        if(isSFXEnabled) {
            aud.clip = clip;
            aud.Play();
        }
    }
}
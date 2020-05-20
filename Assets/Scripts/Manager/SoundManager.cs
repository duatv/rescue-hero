﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip acMainMenuMusic, acGamePlayMusic;
    public AudioClip acClick;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void PlaySound(AudioClip _ac) {
        if (Utils.isSoundOn) {
            audioSource.PlayOneShot(_ac);
        }
    }
    public void PlayBackgroundMusic(bool isMainMenu) {
        if (Utils.isMusicOn) {
            audioSource.clip = isMainMenu ? acMainMenuMusic : acGamePlayMusic;
            audioSource.Play();
        }
    }
}

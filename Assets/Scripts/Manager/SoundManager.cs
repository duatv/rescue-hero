using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip acMainMenuMusic, acGamePlayMusic;
    public AudioClip acClick, acWin, acLose, acHeroDie, acTakeSword, acOpenChest, acEnemyDie, acMoveStick,acMeleeAttack;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void PlaySound(AudioClip _ac) {
        if (Utils.isSoundOn)
        {
            audioSource.mute = false;
            audioSource.PlayOneShot(_ac);
        }
        else audioSource.mute = true;
    }
    public void PlayBackgroundMusic() {
        bool isMainMenu = Application.loadedLevelName.Equals("MainMenu") ? true : false;
        if (Utils.isMusicOn)
        {
            audioSource.mute = false;
            AudioClip _acPlay = isMainMenu ? acMainMenuMusic : acGamePlayMusic;
            audioSource.clip = _acPlay;
            audioSource.Play();
        }
        else
        {
            audioSource.mute = true;
        }
    }
}

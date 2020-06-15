using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip acMainMenuMusic, acGamePlayMusic;
    public AudioClip acClick, acWin, acLose, acHeroDie, acTakeSword, acOpenChest, acEnemyDie, acMoveStick, acMeleeAttack, acPrincessApear, acLavaOnWater, acLavaApear, acStoneApear,btnStart,
                     acMoveStickClick, acFoundOtherStick, acCutRope,acPrincessSave,acPrincessDie;
    public AudioClip[] acCoinApear;
    public AudioSource audioSource;
    public AudioSource audioSouceBG;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void PlaySound(AudioClip _ac)
    {
        if (Utils.isSoundOn)
        {
            audioSource.mute = false;
            audioSource.PlayOneShot(_ac);
        }
        else audioSource.mute = true;
    }
    public void PlayBackgroundMusic()
    {
        bool isMainMenu = Application.loadedLevelName.Equals("MainMenu") ? true : false;
        if (Utils.isMusicOn)
        {
            audioSouceBG.mute = false;
            AudioClip _acPlay = isMainMenu ? acMainMenuMusic : acGamePlayMusic;
            audioSouceBG.clip = _acPlay;
            audioSouceBG.Play();
        }
        else
        {
            audioSouceBG.mute = true;
        }
    }
    public void CheckBGMusic()
    {
        if (Utils.isMusicOn)
        {
            audioSouceBG.mute = false;
        }
        else
        {
            audioSouceBG.mute = true;
        }
    }
    public void CheckSound()
    {
        if (Utils.isSoundOn)
        {
            audioSource.mute = false;
        }
        else
        {
            audioSource.mute = true;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Text txtVersion;
    public GameObject gQualityOn, gQualityOff, gSoundOn, gSoundOff, gMusicOn, gMusicOff, gVibrateOn, gVibrateOff;
    private void OnEnable()
    {
        ChangeSprite();
    }
    private void Start()
    {
        txtVersion.text = "Version " + Application.version;
    }
    private void ChangeSprite() {
        ChangeQualitySprite();
        ChangeSoundSprite();
        ChangeMusicSprite();
        ChangeVibrateSprite();
    }

    public void ChangeQuality() {
        Utils.useMediumImage = !Utils.useMediumImage;
        ChangeSprite();
        Utils.SaveImageSeting();
    }
    public void ChangeSound() {
        Utils.isSoundOn = !Utils.isSoundOn;
        ChangeSoundSprite();
        Utils.ChangeSound();
    }
    public void ChangeMusic() {
        Utils.isMusicOn = !Utils.isMusicOn;
        ChangeMusicSprite();
        Utils.ChangeMusic();
    }
    public void ChangeVibrate() {
        Utils.isVibrateOn = !Utils.isVibrateOn;
        ChangeVibrateSprite();
        Utils.ChangeVibrate();
    }
    public void OnClose() {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBackgroundMusic();
        }
        gameObject.SetActive(false);
    }
    public void OnRestorePurchase() {
    }


    private void ChangeVibrateSprite() {
        gVibrateOff.SetActive(!Utils.isVibrateOn);
        gVibrateOn.SetActive(Utils.isVibrateOn);
    }
    private void ChangeMusicSprite() {
        gMusicOff.SetActive(!Utils.isMusicOn);
        gMusicOn.SetActive(Utils.isMusicOn);
    }
    private void ChangeSoundSprite() {
        gSoundOff.SetActive(!Utils.isSoundOn);
        gSoundOn.SetActive(Utils.isSoundOn);
    }
    private void ChangeQualitySprite() {
        gQualityOff.SetActive(!Utils.useMediumImage);
        gQualityOn.SetActive(Utils.useMediumImage);
    }
}

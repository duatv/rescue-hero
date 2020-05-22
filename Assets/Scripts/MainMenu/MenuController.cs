﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] public LevelConfig levelConfig;
    public SettingPanel _settingPanel;
    public DailyGiftPanel _dailyGiftPanel;

    public Text txtCurLevel;
    void Start()
    {
        Utils.LoadGameData();
        txtCurLevel.text ="LEVEL "+ (Utils.LEVEL_INDEX+1) + "/" + levelConfig.lstAllLevel.Count;
        if (SoundManager.Instance != null) {
            SoundManager.Instance.PlayBackgroundMusic();
        }

        MyAnalytic.LogEventOpenByDay();

        CheckShowDailyGift();
    }

    private void CheckShowDailyGift() {
        Debug.LogError("curLogin: " + Utils.curDailyGift);

        if (!Utils.IsClaimReward())
        {
            _dailyGiftPanel.gameObject.SetActive(true);
            _dailyGiftPanel.OnShowPanel();
        }
    }

    public void SoundClickButton() {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acClick);
        }
    }
    public void ShowSetting() {
        Debug.LogError("Show Setting Panel");
        _settingPanel.gameObject.SetActive(true);
    }
    public void LoadScenePlay() {
        SceneManager.LoadSceneAsync("MainGame");
    }
}

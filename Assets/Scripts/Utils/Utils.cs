﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public enum QUAL_IMAGE { VERY_LOW = 0, LOW = 1, MEDIUM = 2, HEIGH = 3, VERY_HEIGH = 4, ULTRA = 5 }
    public QUAL_IMAGE _quality;
    public const string QUAL_VERY_HEIGHT = "Very High";
    public const string QUAL_HEIGHT = "High";
    public const string QUAL_MEDIUM = "Medium";
    public const string QUAL_LOW = "Low";

    public const string INAPP_REMOVE_ADS = "com.ohze.game.rescuehero";
    public const string APP_ID = "ca-app-pub-3940256099942544~3347511713";
    public const string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    public const string INTERS_ID = "ca-app-pub-3940256099942544/1033173712";
    public const string VIDEO_ID = "ca-app-pub-3940256099942544/5224354917";
    public const int BASE_COIN = 100;
    private const string GAME_KEY = "ohze.rescue.hero";
    public const string COIN_KEY = GAME_KEY + ".coin";
    public const string LEVEL_KEY = GAME_KEY + ".level";
    public const string QUALITY_IMAGE = GAME_KEY + ".quality.image";
    public const string CHANGE_SOUND = GAME_KEY + ".change.sound";
    public const string CHANGE_MUSIC = GAME_KEY + ".change.music";
    public const string CHANGE_VIBRATE = GAME_KEY + ".change.vibrate";
    public const string HAS_REMOVE_ADS = GAME_KEY + ".removeads";
    public const string KEY_DAILY_REWARD = GAME_KEY + ".KEY_DAILY_REWARD";
    public const string KEY_CURRENT_DAILY_GIFT = GAME_KEY + ".KEY_CURRENT_DAILY_GIFT";

    public const string TAG_STICKBARRIE = "StickBarrie";
    public const string TAG_TRAP = "Trap_Lava";
    public const string TAG_WIN = "Tag_Win";
    public const string TAG_STONE = "Tag_Stone";
    public const string TAG_WALL_BOTTOM = "Wall_Bottom";
    public const string TAG_SWORD = "Sword";

    public static int LEVEL_INDEX = 0;
    public static int currentCoin = 0;

    public static void SaveCoin()
    {
        Debug.Log("currentCoin: " + currentCoin);
        PlayerPrefs.SetInt(COIN_KEY, currentCoin);
        PlayerPrefs.Save();
    }
    public static void SaveLevel()
    {
        PlayerPrefs.SetInt(LEVEL_KEY, LEVEL_INDEX);
        PlayerPrefs.Save();
    }
    public static void SaveGameData()
    {
        SaveCoin();
        SaveLevel();
    }
    public static void LoadGameData()
    {
        LEVEL_INDEX = PlayerPrefs.GetInt(LEVEL_KEY, 0);
        currentCoin = PlayerPrefs.GetInt(COIN_KEY, 0);
        useMediumImage = PlayerPrefs.GetInt(QUALITY_IMAGE, 0) == 0 ? false : true;
        isSoundOn = PlayerPrefs.GetInt(CHANGE_SOUND, 0) == 0 ? false : true;
        isMusicOn = PlayerPrefs.GetInt(CHANGE_MUSIC, 0) == 0 ? false : true;
        isVibrateOn = PlayerPrefs.GetInt(CHANGE_VIBRATE, 0) == 0 ? false : true;
        isRemoveAds = PlayerPrefs.GetInt(HAS_REMOVE_ADS, 0) == 0 ? false : true;


        curDailyGift = PlayerPrefs.GetInt(KEY_CURRENT_DAILY_GIFT, 1);
        if (curDailyGift > 7)
        {
            curDailyGift = 1;
        }
        Debug.LogError("curentDailyGift: " + curDailyGift + " vs " + PlayerPrefs.GetInt(KEY_CURRENT_DAILY_GIFT, 1));
    }



    public static bool useMediumImage;
    public static void SaveImageSeting()
    {
        PlayerPrefs.SetInt(QUALITY_IMAGE, useMediumImage ? 1 : 0);
        PlayerPrefs.Save();
    }
    public static bool isSoundOn;
    public static void ChangeSound()
    {
        PlayerPrefs.SetInt(CHANGE_SOUND, isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    public static bool isMusicOn;
    public static void ChangeMusic()
    {
        PlayerPrefs.SetInt(CHANGE_MUSIC, isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    public static bool isVibrateOn;
    public static void ChangeVibrate()
    {
        PlayerPrefs.SetInt(CHANGE_VIBRATE, isVibrateOn ? 1 : 0);
        PlayerPrefs.Save();
    }


    public static bool isRemoveAds;
    public static void SaveRemoveAds() {
        PlayerPrefs.SetInt(HAS_REMOVE_ADS, isRemoveAds ? 1 : 0);
        PlayerPrefs.Save();
    }


    #region Daily reward
    public static bool IsClaimReward()
    {
        string _key = System.DateTime.Now.Day + "_" + System.DateTime.Now.Month;
        return _key.Equals(SReward());
    }
    public static string SReward()
    {
        return PlayerPrefs.GetString(KEY_DAILY_REWARD, "");
    }
    public static void HasClaimReward()
    {
        string _key = System.DateTime.Now.Day + "_" + System.DateTime.Now.Month;
        PlayerPrefs.SetString(KEY_DAILY_REWARD, _key);
        PlayerPrefs.Save();
    }
    public static int curDailyGift;
    public static bool cantakegiftdaily;
    public static void SaveDailyGift() {
        PlayerPrefs.SetInt(KEY_CURRENT_DAILY_GIFT, curDailyGift);
        PlayerPrefs.Save();
    }
    #endregion
}
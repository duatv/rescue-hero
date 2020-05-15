using System.Collections;
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
        Debug.Log("LEVEL_INDEX: " + LEVEL_INDEX);
        PlayerPrefs.SetInt(LEVEL_KEY, LEVEL_INDEX);
        PlayerPrefs.Save();
    }
    public static void SaveGameData()
    {
        Debug.LogError("SaveGame");
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public const string APP_ID = "ca-app-pub-3940256099942544~3347511713";
    public const string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    public const string INTERS_ID = "ca-app-pub-3940256099942544/1033173712";
    public const string VIDEO_ID = "ca-app-pub-3940256099942544/5224354917";
    public const int BASE_COIN = 100;
    private const string GAME_KEY = "ohze.rescue.hero";
    public const string COIN_KEY = GAME_KEY + ".coin";
    public const string LEVEL_KEY = GAME_KEY + ".level";

    public const string TAG_STICKBARRIE = "StickBarrie";
    public const string TAG_TRAP = "Trap_Lava";
    public const string TAG_WIN = "Tag_Win";
    public const string TAG_STONE = "Tag_Stone";
    public const string TAG_WALL_BOTTOM = "Wall_Bottom";
    public const string TAG_SWORD = "Sword";

    public static int LEVEL_INDEX = 0;
    public static int currentCoin = 0;
    
    public static void SaveCoin() {
        Debug.Log("currentCoin: " + currentCoin);
        PlayerPrefs.SetInt(COIN_KEY, currentCoin);
        PlayerPrefs.Save();
    }
    public static void SaveLevel() {
        Debug.Log("LEVEL_INDEX: " + LEVEL_INDEX);
        PlayerPrefs.SetInt(LEVEL_KEY, LEVEL_INDEX);
        PlayerPrefs.Save();
    }
    public static void SaveGameData() {
        Debug.LogError("SaveGame");
        SaveCoin();
        SaveLevel();
    }
    public static void LoadGameData()
    {
        LEVEL_INDEX = PlayerPrefs.GetInt(LEVEL_KEY, 0);
        currentCoin = PlayerPrefs.GetInt(COIN_KEY, 0);
    }
}
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

    public const string INAPP_REMOVE_ADS = "com.ohze.game.rescuehero";
    public const string APP_ID = "ca-app-pub-8566745611252640~6170725017";
    public const string BANNER_ID = "ca-app-pub-8566745611252640/6830228231";
    public const string INTERS_ID = "ca-app-pub-8566745611252640/4474499962";
    public const string VIDEO_ID = "ca-app-pub-8566745611252640/6717519925";
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
    public const string KEY_PLAYER_SKIN = GAME_KEY + ".player.skin";
    public const string KEY_HERO_SELECTED = GAME_KEY + ".hero.selected";
    public const string KEY_SKIN_NORMAL = GAME_KEY + ".skin.hero.normal";
    public const string KEY_SKIN_SWORD = GAME_KEY + ".skin.hero.sword";

    public const string TAG_STICKBARRIE = "StickBarrie";
    public const string TAG_LAVA = "Trap_Lava";
    public const string TAG_GAS = "Trap_Gas";
    public const string TAG_WIN = "Tag_Win";
    public const string TAG_STONE = "Tag_Stone";
    public const string TAG_CHEST = "Chest";
    public const string TAG_WALL_BOTTOM = "Wall_Bottom";
    public const string TAG_SWORD = "Sword";

    public static int LEVEL_INDEX = 0;
    public static int currentCoin = 0;

    public const string REAL_INDEX_LEVEL_PLAY = "real_index_level_play";

    public static int RealLevelIndex 
    { 
        get => PlayerPrefs.GetInt(REAL_INDEX_LEVEL_PLAY, 0);
        set => PlayerPrefs.SetInt(REAL_INDEX_LEVEL_PLAY, value);
    }
      
    public static void SaveCoin()
    {
     //   PlayerPrefs.SetInt(COIN_KEY, currentCoin);
      //  PlayerPrefs.Save();
    }
    public static void SaveLevel()
    {
        PlayerPrefs.SetInt(LEVEL_KEY, LEVEL_INDEX);
      //  PlayerPrefs.Save();
    }
    public static void SaveGameData()
    {
        SaveCoin();
        SaveLevel();
    }
    public static void LoadGameData()
    {
        LEVEL_INDEX = PlayerPrefs.GetInt(LEVEL_KEY, 0);
       // currentCoin = PlayerPrefs.GetInt(COIN_KEY, 0);
        useMediumImage = PlayerPrefs.GetInt(QUALITY_IMAGE, 0) == 0 ? false : true;
        isSoundOn = PlayerPrefs.GetInt(CHANGE_SOUND, 1) == 0 ? false : true;
        isMusicOn = PlayerPrefs.GetInt(CHANGE_MUSIC, 1) == 0 ? false : true;
        isVibrateOn = PlayerPrefs.GetInt(CHANGE_VIBRATE, 0) == 0 ? false : true;
        isRemoveAds = PlayerPrefs.GetInt(HAS_REMOVE_ADS, 0) == 0 ? false : true;

        //curHero = PlayerPrefs.GetString(KEY_PLAYER_SKIN, "HENRY");
        //heroSelected = PlayerPrefs.GetString(KEY_HERO_SELECTED, "1");
     //   skinNormal = GetCurSkinNormal();
        skinSword = GetCurSkinSword();

        curDailyGift = PlayerPrefs.GetInt(KEY_CURRENT_DAILY_GIFT, 1);
        if (curDailyGift > 7)
        {
            curDailyGift = 1;
        }
    }



    public static bool useMediumImage;
    public static void SaveImageSeting()
    {
        PlayerPrefs.SetInt(QUALITY_IMAGE, useMediumImage ? 1 : 0);
      //  PlayerPrefs.Save();
    }
    public static bool isSoundOn;
    public static void ChangeSound()
    {
        PlayerPrefs.SetInt(CHANGE_SOUND, isSoundOn ? 1 : 0);
      //  PlayerPrefs.Save();
    }
    public static bool isMusicOn;
    public static void ChangeMusic()
    {
        PlayerPrefs.SetInt(CHANGE_MUSIC, isMusicOn ? 1 : 0);
       // PlayerPrefs.Save();
    }
    public static bool isVibrateOn;
    public static void ChangeVibrate()
    {
        PlayerPrefs.SetInt(CHANGE_VIBRATE, isVibrateOn ? 1 : 0);
     //   PlayerPrefs.Save();
    }


    public static bool isRemoveAds;
    public static void SaveRemoveAds()
    {
        PlayerPrefs.SetInt(HAS_REMOVE_ADS, isRemoveAds ? 1 : 0);
      //  PlayerPrefs.Save();
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
       // PlayerPrefs.Save();
    }
    public static int curDailyGift;
    public static bool cantakegiftdaily;
    public static void SaveDailyGift()
    {
        PlayerPrefs.SetInt(KEY_CURRENT_DAILY_GIFT, curDailyGift);
      //  PlayerPrefs.Save();
    }
    #endregion

    #region Player Skin
   // private static string curHero = "";
    //public static void UnlockHero(string heroName)
    //{
    //    //if (!curHero.Equals("HENRY"))
    //    {
    //        curHero += "," + heroName;
    //        PlayerPrefs.SetString(KEY_PLAYER_SKIN, curHero);
    //      //  PlayerPrefs.Save();
    //    }
    //}
    //public static bool IsHeroUnlock(string heroName)
    //{
    //    return curHero.Contains(heroName);
    //}
    //public static string heroSelected = "";
    //public static bool IsHeroSelect(string heroName)
    //{
    //    return heroSelected.Equals(heroName);
    //}
    //public static void SetSelectedHero(string heroName)
    //{
    //    PlayerPrefs.SetString(KEY_HERO_SELECTED, heroName);
    //  //  PlayerPrefs.Save();
    //}

 //   public static string skinNormal = "";
    public static string skinSword = "";

    //public static string GetCurSkinNormal()
    //{
    //    return PlayerPrefs.GetString(KEY_SKIN_NORMAL, "1");
    //}
    public static void SetSkinNormal(string skinName)
    {
        PlayerPrefs.SetString(KEY_SKIN_NORMAL, skinName);
      //  PlayerPrefs.Save();
    }
    public static string GetCurSkinSword()
    {
        return PlayerPrefs.GetString(KEY_SKIN_SWORD, "kiem");
    }
    public static void SetSkinSword(string skinName)
    {
        PlayerPrefs.SetString(KEY_SKIN_SWORD, skinName);
      //  PlayerPrefs.Save();
    }
    //public static void SavePlayerSkin(string _skinNormal/*, string _skinSword*/)
    //{
    //    skinNormal = _skinNormal;
    //   // skinSword = _skinSword;

    //    SetSkinNormal(skinNormal);
    //   // SetSkinSword(skinSword);
    //}
    #endregion
}


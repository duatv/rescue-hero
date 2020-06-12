using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataParam
{
    public static int currentHero,currentLevelCastle;
    public const string CURRENTHERO = "currenthero";

    public const string SAVEHERO = "savehero";
    public const string SAVEACHIEVEMENT = "saveachievement";

    public const string SAVECASTLE = "savecastle";
    public const string CURRENTLEVELCASTLE = "currentlevelcastle";

    public const string TOTALCOIN = "totalcoin";

    public static int firsttime = 0,levelpassshowad = 5,delayshowAds = 2;
    public static float timedelayShowAds = 30;
    public const string FIRSTTIME = "firsttime";

    public static System.DateTime oldTimeShowAds = System.DateTime.Now;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Unity;
using Firebase.Analytics;

public class MyAnalytic : MonoBehaviour
{
    const string EVENT_LOGIN_BY_DAY = "event_login_by_day";
    const string EVENT_WIN = "level_complete";
    const string EVENT_LOSE = "level_fail";
    const string EVENT_REWARD = "watch_video_complete";
    const string EVENT_SHOWINTER = "interstitial_show";
    const string EVENT_SHOWVIDEO = "reward_video_show";
    const string EVENT_REMOVEADS = "buy_remove_ads_pack";
    const string EVENT_AD_CLICK = "interstitial_ads_click";
    const string EVENT_SKIP_LEVEL = "skip_level_";
    const string EVENT_REPLAY_LEVEL = "replay_level_";
    const string EVENT_PLAY_LEVEL = "play_level_";

    public static void LogEventOpenByDay()
    {
        FirebaseAnalytics.LogEvent(EVENT_LOGIN_BY_DAY);
    }
    public static void LogEventWin(int level)
    {
        Parameter[] _paramWin = { new Parameter("finish_level", level), };
        FirebaseAnalytics.LogEvent("level_" + level + "_complete", _paramWin);
    }
    public static void LogEventLose(int level)
    {
        Parameter[] _paramLose = { new Parameter("lose_level", level), };
        FirebaseAnalytics.LogEvent("level_" + level + "_fail", _paramLose);
    }
    public static void LogEventRewarded(string _strAt)
    {
        FirebaseAnalytics.LogEvent(EVENT_REWARD + "_" + _strAt);
    }
    public static void LogEventShowInters()
    {
        FirebaseAnalytics.LogEvent(EVENT_SHOWINTER);
    }
    public static void LogEventShowVideo()
    {
        FirebaseAnalytics.LogEvent(EVENT_SHOWVIDEO);
    }
    public static void LogEventBuyRemoveAds()
    {
        FirebaseAnalytics.LogEvent(EVENT_REMOVEADS);
    }
    public static void LogEventAdClick()
    {
        FirebaseAnalytics.LogEvent(EVENT_AD_CLICK);
    }
    public static void LogEventSkipLevel(int _level) {
        FirebaseAnalytics.LogEvent(EVENT_SKIP_LEVEL + _level);
    }
    public static void LogEventReplay(int _level) {
        FirebaseAnalytics.LogEvent(EVENT_REPLAY_LEVEL + _level);
    }
    public static void LogEventPlayLevel(int _level) {
        FirebaseAnalytics.LogEvent(EVENT_PLAY_LEVEL + _level);
    }
}

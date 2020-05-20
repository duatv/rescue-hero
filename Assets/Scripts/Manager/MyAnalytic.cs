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

    public static void LogEventOpenByDay()
    {
        FirebaseAnalytics.LogEvent(EVENT_LOGIN_BY_DAY);
    }
    public static void LogEventWin(int level) {
        Parameter[] _paramWin = {new Parameter("finish_level", level)};
        FirebaseAnalytics.LogEvent(EVENT_WIN, _paramWin);
    }
    public static void LogEventLose(int level) {
        Parameter[] _paramLose = { new Parameter("lose_level", level) };
        FirebaseAnalytics.LogEvent(EVENT_LOSE, _paramLose);
    }
    public static void LogEventRewarded(string _strAt) {
        FirebaseAnalytics.LogEvent(EVENT_REWARD + "_" + _strAt);
    }
    public static void LogEventShowInters() {
        FirebaseAnalytics.LogEvent(EVENT_SHOWINTER);
    }
    public static void LogEventShowVideo() {
        FirebaseAnalytics.LogEvent(EVENT_SHOWVIDEO);
    }
}

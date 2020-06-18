using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MyAnalytic : MonoBehaviour
{
    const string EVENT_LEVEL_COMPLETED = "event_level_completed";
    const string EVENT_LEVEL_FAILED = "event_level_failed";
    const string EVENT_LEVEL_START = "event_level_start";
    const string EVENT_REWARD_ADS = "event_reward_ads";
    const string EVENT_SHOW_INTER = "event_show_inter";
    const string EVENT_UNLOCK_HERO = "event_unlock_hero";
    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                InitalizeFirebase();
                FetchDataAsync();


            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }
    protected bool isFirebaseInittialized = false;
    string DELAYSHOWAD = "LevelDelay";
    string TIMEDELAYSHOWAD = "TimeDelay";
    string LEVELPASSSHOWAD = "FirstOpenDelay";
    void InitalizeFirebase()
    {
        defauls.Add(DELAYSHOWAD, 2);
        defauls.Add(LEVELPASSSHOWAD, 5);
        defauls.Add(TIMEDELAYSHOWAD, 30);

        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defauls);
        isFirebaseInittialized = true;
    }

    public Task FetchDataAsync()
    {
        Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync();
        return fetchTask.ContinueWith(FetchComplete);
    }

    Dictionary<string, object> defauls = new System.Collections.Generic.Dictionary<string, object>();

    void FetchComplete(Task fetchTask)
    {

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.LogError("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.LogError("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.LogError("Latest Fetch call still pending.");
                break;
        }

        if (fetchTask.IsCanceled)
        {
        }
        else if (fetchTask.IsFaulted)
        {
        }
        else if (fetchTask.IsCompleted)
        {
        }

        DataParam.delayshowAds = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(DELAYSHOWAD).StringValue);
        DataParam.timedelayShowAds = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(TIMEDELAYSHOWAD).StringValue);
        DataParam.levelpassshowad = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(LEVELPASSSHOWAD).StringValue);

        Debug.LogError("Delay show ad:" + DataParam.delayshowAds + ": time delay show ad" + DataParam.timedelayShowAds + ": level pass show ad" + DataParam.levelpassshowad);
    }
    public static void EventLevelCompleted(int level)
    {
        FirebaseAnalytics.LogEvent(EVENT_LEVEL_COMPLETED + "_" + level);
    }
    public static void EventLevelFailed(int level)
    {
        FirebaseAnalytics.LogEvent(EVENT_LEVEL_FAILED + "_" + level);
    }
    public static void EventLevelStart(int level)
    {
        FirebaseAnalytics.LogEvent(EVENT_LEVEL_START + "_" + level);
    }
    public static void EventReward(string name)
    {
        FirebaseAnalytics.LogEvent(EVENT_REWARD_ADS + "_" + name);
    }
    public static void EventShowInter()
    {
        FirebaseAnalytics.LogEvent(EVENT_SHOW_INTER);
    }
    public static void EventUnlockHero(string name)
    {
       FirebaseAnalytics.LogEvent(EVENT_UNLOCK_HERO + "_" + name);
    }
}

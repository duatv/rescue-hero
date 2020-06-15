using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using GoogleMobileAds.Api.Mediation.UnityAds;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    private Action<bool> acInterClosed, acRewarded;

    #region Admob
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private BannerView banner;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            InitAds();
        }
    }
    private void InitAds()
    {
        MobileAds.Initialize(Application.platform == RuntimePlatform.Android ? Utils.APP_ID : Utils.APP_ID);
        InitInterstitial();
        InitRewarded();
        InitBanner();
    //   UnityAds.SetGDPRConsentMetaData(true);
    }

    private void InitBanner()
    {
        banner = new BannerView(Application.platform == RuntimePlatform.Android ? Utils.BANNER_ID : Utils.BANNER_ID, AdSize.Banner, AdPosition.Bottom);
        banner.OnAdLoaded += Banner_OnAdLoaded;
        banner.LoadAd(CreateRequest());
    }
    public void ShowBannerWithPos()
    {
        if (banner != null)
        {
            banner.SetPosition(AdPosition.Bottom);
            banner.Show();
        }
    }
    public void ShowBanner()
    {
        if (banner != null)
        {
            banner.SetPosition(AdPosition.Bottom);
            banner.Show();
        }
    }
    public void HideBanner()
    {
        if (banner != null)
        {
            banner.Hide();
        }
    }
    private void Banner_OnAdLoaded(object sender, EventArgs e)
    {
        HideBanner();

        Debug.LogError("====== load banner ====");
    }

    private bool IsIntersLoaded()
    {
        return interstitial.IsLoaded();
    }

    public bool IsRewardLoaded()
    {
        return rewardedAd.IsLoaded();
    }
    public void ShowInterstitial(Action<bool> _ac)
    {
        if (!Utils.isRemoveAds) {
            if (IsIntersLoaded())
            {
                acInterClosed = _ac;
                interstitial.Show();

            }
            else
            {
                interstitial.LoadAd(CreateRequest());
                if (acInterClosed != null)
                    acInterClosed(true);
            }
        }
    }


    public void ShowRewardedVideo(Action<bool> _ac)
    {
        if (IsRewardLoaded())
        {
            Debug.Log("[Ads] Manager realy loaded");
            acRewarded = _ac;
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("[Ads] Manager request");
            rewardedAd.LoadAd(CreateRequest());
            acRewarded(false);
        }
    }


    #region Init Admob
    AdRequest CreateRequest()
    {
        AdRequest request = new AdRequest.Builder().AddTestDevice("BA730DD6C0C19894C11CB7FDF6D75AA8").AddTestDevice("D96EFB8D3BB99E5B5CAF739EB1EB5E9D").AddTestDevice("256FC58E8184F47CC4E7BE3570B2AC3B").Build();
        return request;
    }
    void InitInterstitial()
    {
        interstitial = new InterstitialAd(Application.platform == RuntimePlatform.Android ? Utils.INTERS_ID : Utils.INTERS_ID);
        interstitial.OnAdOpening += Interstitial_OnAdOpening;
        interstitial.OnAdClosed += HandleOnAdClosed;
        interstitial.OnAdFailedToLoad += HandleLoadFail;
        interstitial.LoadAd(CreateRequest());
    }

    private void HandleLoadFail(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.LogError("========load fail ads=======");
    }

    private void Interstitial_OnAdOpening(object sender, EventArgs e)
    {
        MyAnalytic.EventShowInter();
    }

    void InitRewarded()
    {
        this.rewardedAd = new RewardedAd(Application.platform == RuntimePlatform.Android ? Utils.VIDEO_ID : Utils.VIDEO_ID);
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdLoaded += RewardedAd_OnAdLoaded;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        this.rewardedAd.LoadAd(CreateRequest());
    }



    #region Handler
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        if (acInterClosed != null)
            acInterClosed(true);
        interstitial.LoadAd(CreateRequest());
    }

    private void RewardedAd_OnAdLoaded(object sender, EventArgs e)
    {
        Debug.LogError("====== load video ====");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        if (acRewarded != null)
        {
            acRewarded(false);
        }
        InitRewarded();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        if (acRewarded != null)
        {
            acRewarded(true);
        }
    }
    #endregion
    #endregion
}

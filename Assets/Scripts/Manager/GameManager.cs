using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GAMESTATE { PLAYING, WIN, LOSE }
    public static GameManager Instance;

    public Text txtLevel;
    public Text txtCoin;
    public bool isTest;
    [HideInInspector] public bool canUseTrail;
    public GAMESTATE gameState;
    [SerializeField] public LevelConfig levelConfig;
    [HideInInspector] public MapLevelManager mapLevel;
    public int totalGems;
    public List<GameObject> lstAllGems = new List<GameObject>();
    public bool isNotEnoughGems;
    public CamFollow _camFollow;
    public GameObject gPanelWin;
    public GameObject gPanelLose;

    [HideInInspector] public GameObject gTargetFollow;

    private void Awake()
    {
        Instance = this;
    }
    private void OnUpdateCoin()
    {
        txtCoin.text = Utils.currentCoin + "";
        Utils.SaveCoin();
    }
    void Start()
    {

        txtLevel.text = "LEVEL " + (Utils.LEVEL_INDEX + 1).ToString("00,#");
        OnUpdateCoin();

        MyAnalytic.LogEventPlayLevel(Utils.LEVEL_INDEX + 1);
        if (!isTest)
            LoadLevelToPlay(Utils.LEVEL_INDEX);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBackgroundMusic();
        }
        if (AdsManager.Instance != null) {
            AdsManager.Instance.ShowBanner();
        }
    }
    private void OnDisable()
    {
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.HideBanner();
        }
    }
    private void LoadLevelToPlay(int levelIndex)
    {
        mapLevel = levelConfig.lstAllLevel[levelIndex];
        Instantiate(mapLevel.gameObject, Vector3.zero, Quaternion.identity);
    }

    private void ActiveCamEff()
    {
        _camFollow.objectToFollow = gTargetFollow;
        _camFollow.beginFollow = true;
    }
    public void ShowWinPanel()
    {
        StartCoroutine(IEWaitToShowWinLose(true));
    }
    private IEnumerator IEWaitToShowWinLose(bool isWin)
    {
        yield return new WaitForSeconds(1.0f);
        if (isWin)
        {
            ActiveCamEff();
            if (!gPanelWin.activeSelf)
            {
                Utils.currentCoin += Utils.BASE_COIN;
                OnUpdateCoin();
                gPanelWin.SetActive(true);
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.acWin);
                }
                MyAnalytic.LogEventWin(Utils.LEVEL_INDEX + 1);
            }
        }
        else
        {
            ActiveCamEff();
            if (!gPanelLose.activeSelf)
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.acLose);
                }
                gPanelLose.SetActive(true);
                MyAnalytic.LogEventLose(Utils.LEVEL_INDEX + 1);
                if (AdsManager.Instance != null)
                {
                    if (Random.Range(0, 10) <= 5)
                    {
                        AdsManager.Instance.ShowInterstitial(null);
                    }
                }
            }
        }
    }
    private bool playingSoundLava = false;
    public void PlaySoundLavaOnWater()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acStoneApear);
            if (!playingSoundLava)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.acLavaOnWater);
            }
            playingSoundLava = true;
        }
    }
    public void ShowLosePanel()
    {
        StartCoroutine(IEWaitToShowWinLose(false));
    }
    public void OnNextLevel()
    {
        Debug.LogError(Utils.LEVEL_INDEX + " vs " + levelConfig.lstAllLevel.Count);
        if (Utils.LEVEL_INDEX < levelConfig.lstAllLevel.Count - 1)
        {
            Utils.LEVEL_INDEX += 1;
            Utils.SaveLevel();
            SceneManager.LoadSceneAsync("MainGame");
        }
    }
    public void OnX2Coin()
    {
        Debug.LogError("X2 Coin");
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.ShowRewardedVideo((b) =>
            {
                if (b)
                {
                    MyAnalytic.LogEventRewarded("x2_coin");
                    Utils.currentCoin += 3 * Utils.BASE_COIN;
                    OnUpdateCoin();
                    OnNextLevel();
                }
            });
        }
    }
    public void OnSkipByVideo()
    {
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.ShowRewardedVideo((b) =>
            {
                if (b)
                {
                    MyAnalytic.LogEventRewarded("skip_level");
                    MyAnalytic.LogEventSkipLevel(Utils.LEVEL_INDEX + 1);
                    OnNextLevel();
                }
            });
        }
    }
    public void OnReplay()
    {
        MyAnalytic.LogEventReplay(Utils.LEVEL_INDEX + 1);
        SceneManager.LoadSceneAsync("MainGame");
    }
    public void GoToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    public void BuyRemoveAds()
    {
        Debug.LogError("Buy Remove Ads");
    }


    private void OnApplicationFocus(bool focus)
    {
        //if (!focus)
        //    Utils.SaveGameData();
    }


    public void SoundClickButton()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acClick);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    [SerializeField] public LevelConfig levelConfig;
    public SettingPanel _settingPanel;
    public DailyGiftPanel _dailyGiftPanel;
    public SkinShopManager shopManager;
    public AchievmentPanel achievementPanel;
    public GameObject loadingPanel;
    public bool animLoading;
    public Animator playAnimLoading;
    public CastlePanel castlePanel;
    public GameObject warningAchievement, warningDailyReward;
    public static bool openAchievement, openCastle;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public Text txtCurLevel;
    void Start()
    {
        Utils.LoadGameData();
        txtCurLevel.text = "LEVEL " + (Utils.LEVEL_INDEX + 1);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBackgroundMusic();
        }



        CheckShowDailyGift();
        CheckDisplayWarningAchievement();

        shopManager.DisplayBegin();

        castlePanel.DisplayBegin();

        shopManager.Init();
        if(openAchievement)
        {
         //   StartCoroutine(OpenAchievement());
            OpenAchievement(true);
            openAchievement = false;
            Debug.LogError("=zoooooooooo= open achievment");
        }
        if(openCastle)
        {
         //   StartCoroutine(OpenCastle());
            BtnShowCastle();
            openCastle = false;
            Debug.LogError("=zoooooooooo= open castle");
        }

       //   Utils.currentCoin = 1000000;
    }
    IEnumerator OpenAchievement()
    {
        yield return new WaitForSeconds(0.3f);
        OpenAchievement(true);
        openAchievement = false;
        Debug.LogError("=zoooooooooo= open achievment");
    }
    IEnumerator OpenCastle()
    {
        yield return new WaitForSeconds(0.3f);
        BtnShowCastle();
        openCastle = false;
        Debug.LogError("=zoooooooooo= open castle");
    }
    public void OpenAchievement(bool open)
    {
        if (animLoading)
            return;
        warningAchievement.SetActive(false);
        achievementPanel.OpenMe(open);

    }
    public void BtnShowCastle()
    {
        castlePanel.gameObject.SetActive(true);
    }

    public void CheckShowDailyGift()
    {
        if (!Utils.IsClaimReward())
        {
            warningDailyReward.SetActive(true);
            //_dailyGiftPanel.gameObject.SetActive(true);
            //_dailyGiftPanel.OnShowPanel();
        }
    }

    public void SoundClickButton()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acClick);
        }
    }
    public void ShowSetting()
    {

        if (animLoading)
            return;
        Debug.LogError("Show Setting Panel");
        _settingPanel.gameObject.SetActive(true);
    }
    public void LoadScenePlay()
    {
        //  SceneManager.LoadSceneAsync("MainGame");
        //loadingPanel.SetActive(true);
        animLoading = true;
        playAnimLoading.Play("loadingprocessAnim");
        SoundManager.Instance.PlaySound(SoundManager.Instance.btnStart);
    }

    public void CheckDisplayWarningAchievement()
    {
        warningAchievement.SetActive(DataController.instance.CheckWarningAchievement());
    }

    public void ShowDailyReward()
    {
        if (animLoading)
            return;
        _dailyGiftPanel.gameObject.SetActive(true);
       // _dailyGiftPanel.OnShowPanel();
        warningDailyReward.SetActive(false);
    }
    public void ShowSkinShop()
    {
        if (animLoading)
            return;
        shopManager.gameObject.SetActive(true);
        shopManager.ChangeSkin(DataController.instance.heroData.infos[DataParam.currentHero].nameSkin);
    }


}

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
        if (SoundManager.Instance != null) {
            SoundManager.Instance.PlayBackgroundMusic();
        }

        MyAnalytic.LogEventOpenByDay();

        CheckShowDailyGift();

        shopManager.DisplayBegin();

       castlePanel.DisplayBegin();

      //  Utils.currentCoin = 100000;
    }

    public void OpenAchievement(bool open)
    {
        if (animLoading)
            return;

        achievementPanel.OpenMe(open);
    }
    public void BtnShowCastle()
    {
       castlePanel.gameObject.SetActive(true);
    }

    private void CheckShowDailyGift() {
        if (!Utils.IsClaimReward())
        {
            //_dailyGiftPanel.gameObject.SetActive(true);
            //_dailyGiftPanel.OnShowPanel();
        }
    }
    
    public void SoundClickButton() {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acClick);
        }
    }
    public void ShowSetting() {

        if (animLoading)
            return;
        Debug.LogError("Show Setting Panel");
        _settingPanel.gameObject.SetActive(true);
    }
    public void LoadScenePlay() {
        //  SceneManager.LoadSceneAsync("MainGame");
        //loadingPanel.SetActive(true);
        animLoading = true;
        playAnimLoading.Play("loadingprocessAnim");
    }



    public void ShowDailyReward() {
        if (animLoading)
            return;
        _dailyGiftPanel.gameObject.SetActive(true);
        _dailyGiftPanel.OnShowPanel();
    }
    public void ShowSkinShop() {
        if (animLoading)
            return;
        shopManager.gameObject.SetActive(true);
    }


}

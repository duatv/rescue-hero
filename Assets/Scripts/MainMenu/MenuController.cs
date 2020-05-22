using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] public LevelConfig levelConfig;
    public SettingPanel _settingPanel;
    public DailyGiftPanel _dailyGiftPanel;

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

        //StartCoroutine(ReadData());
    }
    
    //string _url = "https://firebasestorage.googleapis.com/v0/b/rescue-hero.appspot.com/o/MyAnalytics.cs?alt=media&token=145aa322-2acd-4d95-8f81-8587ddc29965";
    //private IEnumerator ReadData() {
    //    WWW _ww = new WWW(_url);
    //    yield return _ww;
    //    Debug.LogError("Data: " + _ww.text);
    //}

    private void CheckShowDailyGift() {
        Debug.LogError("curLogin: " + Utils.curDailyGift);

        if (!Utils.IsClaimReward())
        {
            _dailyGiftPanel.gameObject.SetActive(true);
            _dailyGiftPanel.OnShowPanel();
        }
    }

    public void SoundClickButton() {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acClick);
        }
    }
    public void ShowSetting() {
        Debug.LogError("Show Setting Panel");
        _settingPanel.gameObject.SetActive(true);
    }
    public void LoadScenePlay() {
        SceneManager.LoadSceneAsync("MainGame");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            _dailyGiftPanel.gameObject.SetActive(true);
            _dailyGiftPanel.OnShowPanel();
        }
    }
}

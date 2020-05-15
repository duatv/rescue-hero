using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] public LevelConfig levelConfig;
    public SettingPanel _settingPanel;

    public Text txtCurLevel;
    // Start is called before the first frame update
    void Start()
    {
        Utils.LoadGameData();
        txtCurLevel.text ="LEVEL "+ (Utils.LEVEL_INDEX+1) + "/" + levelConfig.lstAllLevel.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowSetting() {
        Debug.LogError("Show Setting Panel");
        _settingPanel.gameObject.SetActive(true);
    }
    public void LoadScenePlay() {
        SceneManager.LoadSceneAsync("MainGame");
    }
}

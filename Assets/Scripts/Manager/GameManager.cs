using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GAMESTATE { PLAYING, WIN, LOSE}
    public static GameManager Instance;

    public bool isTest;
    public GAMESTATE gameState;
    [SerializeField]public LevelConfig levelConfig;
    [HideInInspector]public MapLevelManager mapLevel;
    public CamFollow _camFollow;
    public GameObject gPanelWin;
    public GameObject gPanelLose;

    [HideInInspector] public GameObject gTargetFollow;
    
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!isTest)
            LoadLevelToPlay(Utils.LEVEL_INDEX);
    }
    private void LoadLevelToPlay(int levelIndex) {
        mapLevel = levelConfig.lstAllLevel[levelIndex];
        Instantiate(mapLevel.gameObject, Vector3.zero, Quaternion.identity);
    }

    private void ActiveCamEff() {
        _camFollow.objectToFollow = gTargetFollow;
        _camFollow.beginFollow = true;
    }
    public void ShowWinPanel()
    {
        ActiveCamEff();
        if (!gPanelWin.activeSelf) gPanelWin.SetActive(true);
    }
    public void ShowLosePanel()
    {
        ActiveCamEff();
        if (!gPanelLose.activeSelf) gPanelLose.SetActive(true);
    }
    public void OnNextLevel()
    {
        Debug.LogError(Utils.LEVEL_INDEX + " vs " + levelConfig.lstAllLevel.Count);
        if (Utils.LEVEL_INDEX < levelConfig.lstAllLevel.Count-1) {
            Utils.LEVEL_INDEX += 1;
            SceneManager.LoadSceneAsync("MainGame");
        }
    }
    public void OnX2Coin()
    {
        Debug.LogError("X2 Coin");
    }
    public void OnSkipByVideo() {
        Debug.LogError("Skip by video");
    }
    public void OnReplay()
    {
        SceneManager.LoadSceneAsync("MainGame");
    }
}

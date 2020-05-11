using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]public LevelConfig levelConfig;
    public MapLevelManager mapLevel;

    private GameObject gLevelClone;
    // Start is called before the first frame update
    void Start()
    {
        LoadLevelToPlay(Utils.LEVEL_INDEX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LoadLevelToPlay(int levelIndex) {
        mapLevel = levelConfig.lstAllLevel[levelIndex];
        Instantiate(mapLevel.gameObject,Vector3.zero,Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class SaveHero
{
    public bool unlock;
}
public class SaveDataAchievement
{
    public int currentLevel = 0, currentNumber = 0;
    public bool pass = false;
}
public class DataController : MonoBehaviour
{
    public List<SaveDataAchievement> saveDataAchievement = new List<SaveDataAchievement>();
    public DataAchievement dataAchievement;
    public HeroData heroData;
    public List<SaveHero> savehero = new List<SaveHero>();
    public static DataController instance;



    private void Awake()
    {
        if (instance == null)
        {
             //  Debug.unityLogger.logEnabled = false;
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();

        }
        else
            DestroyImmediate(gameObject);
    }

    TextAsset _ta;
    JsonData jData;
    string strHero,strAchievement;
    void LoadData()
    {
        DataParam.currentHero = PlayerPrefs.GetInt(DataParam.CURRENTHERO);

        LoadHero();
        LoadAchievment();
    }
    void LoadAchievment()
    {
        for(int i = 0; i < dataAchievement.infos.Length; i ++)
        {
            SaveDataAchievement _saveDataAchievement = new SaveDataAchievement();
            saveDataAchievement.Add(_saveDataAchievement);
        }
        strAchievement = PlayerPrefs.GetString(DataParam.SAVEACHIEVEMENT);
        if (!string.IsNullOrEmpty(strAchievement))
        {
            jData = JsonMapper.ToObject(strAchievement);
            for (int i = 0; i < jData.Count; i++)
            {
                if (jData[i] != null)
                {
                    saveDataAchievement[i] = JsonMapper.ToObject<SaveDataAchievement>(jData[i].ToJson());
                }
            }
        }

    }
    void LoadHero()
    {
        for (int i = 0; i < heroData.infos.Length; i++)
        {
            SaveHero _saveHero = new SaveHero();
            savehero.Add(_saveHero);
        }

        strHero = PlayerPrefs.GetString(DataParam.SAVEHERO);
        Debug.LogError("str:" + strHero);

        if (!string.IsNullOrEmpty(strHero))
        {
            jData = JsonMapper.ToObject(strHero);
            for (int i = 0; i < jData.Count; i++)
            {
                if (jData[i] != null)
                {
                    savehero[i] = JsonMapper.ToObject<SaveHero>(jData[i].ToJson());
                }
            }
        }

        savehero[0].unlock = true;
    }
    void SaveData()
    {
        PlayerPrefs.SetString(DataParam.SAVEHERO, JsonMapper.ToJson(savehero));
        PlayerPrefs.SetString(DataParam.SAVEACHIEVEMENT, JsonMapper.ToJson(saveDataAchievement));
        PlayerPrefs.SetInt(DataParam.CURRENTHERO, DataParam.currentHero);
    }
    int currentLevel;
    public void DoAchievment(int index, int add)
    {
        if (saveDataAchievement[index].pass)
            return;

        saveDataAchievement[index].currentNumber += add;

        if (saveDataAchievement[index].currentLevel < dataAchievement.infos[index].contents.Length - 1)
        {
            currentLevel = saveDataAchievement[index].currentLevel;
        }
        else
            currentLevel = dataAchievement.infos[index].contents.Length - 1;

        if (saveDataAchievement[index].currentNumber >= dataAchievement.infos[index].contents[currentLevel].numberRequire)
            saveDataAchievement[index].pass = true;
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveData();
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }
}

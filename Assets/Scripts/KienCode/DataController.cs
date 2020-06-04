using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class SaveHero
{
    public bool unlock;
}
public class DataController : MonoBehaviour
{
    public HeroData heroData;
    public List<SaveHero> savehero = new List<SaveHero>();
    public static DataController instance;



    private void Awake()
    {
        if (instance == null)
        {
            Debug.unityLogger.logEnabled = false;
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();

        }
        else
            DestroyImmediate(gameObject);
    }

    TextAsset _ta;
    JsonData jData;
    string strHero;
    void LoadData()
    {
        DataParam.currentHero = PlayerPrefs.GetInt(DataParam.CURRENTHERO);

        LoadHero();
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
        PlayerPrefs.SetInt(DataParam.CURRENTHERO, DataParam.currentHero);
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

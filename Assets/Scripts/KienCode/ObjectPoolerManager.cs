using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolerManager : MonoBehaviour
{
    [HideInInspector]
    public ObjectPooler effectWaterFirePooler,effectDestroyPooler;
    public GameObject effectWaterFirePrefab, effectDestroyPrefab;
    [HideInInspector]
    public static ObjectPoolerManager Instance { get; private set; }
    public List<ObjectPooler> AllPool = new List<ObjectPooler>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(gameObject);
    }
    public void ClearAllPool()
    {
        for (int i = 0; i < AllPool.Count; i++)
        {
            for (int j = 0; j < AllPool[i].transform.childCount; j++)
            {
                if (!AllPool[i].transform.GetChild(j).gameObject.activeSelf)
                    continue;

                AllPool[i].transform.GetChild(j).gameObject.SetActive(false);
            }
        }
    }
    public void Init()
    {
        if (effectWaterFirePooler == null)
        {
            go = new GameObject("effectWaterFirePooler");
            effectWaterFirePooler = go.AddComponent<ObjectPooler>();
            effectWaterFirePooler.PooledObject = effectWaterFirePrefab;
            go.transform.parent = this.gameObject.transform;
            effectWaterFirePooler.Initialize(30);
            AllPool.Add(effectWaterFirePooler);
        }
        if (effectDestroyPooler == null)
        {
            go = new GameObject("effectDestroyPooler");
            effectDestroyPooler = go.AddComponent<ObjectPooler>();
            effectDestroyPooler.PooledObject = effectDestroyPrefab;
            go.transform.parent = this.gameObject.transform;
            effectDestroyPooler.Initialize(30);
            AllPool.Add(effectDestroyPooler);
        }
    }
    GameObject go;

}
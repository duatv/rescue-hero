using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManagerHaveScript : MonoBehaviour
{
    [HideInInspector]
    public ObjectPoolerHaveScript /*effectWaterFirePooler,*//*waterPooler,firePooler,gemPooler,*/stonePooler;
    public Unit /*effectWaterFirePrefab,*/ /*waterPrefab, firePrefab, gemPrefab,*/ stonePrefab;

    [HideInInspector]
    public static ObjectPoolManagerHaveScript Instance { get; private set; }
    public List<ObjectPoolerHaveScript> AllPool = new List<ObjectPoolerHaveScript>();
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
                AllPool[i].transform.GetChild(j).gameObject.SetActive(false);
            }
        }
    }
    public void Init()
    {
        //if (effectWaterFirePooler == null)
        //{
        //    go = new GameObject("effectWaterFirePooler");
        //    effectWaterFirePooler = go.AddComponent<ObjectPoolerHaveScript>();
        //    effectWaterFirePooler.unitPooledObject = effectWaterFirePrefab;
        //    go.transform.parent = this.gameObject.transform;
        //    effectWaterFirePooler.InitializeUnit(60);
        //    AllPool.Add(effectWaterFirePooler);
        //}
        //if (waterPooler == null)
        //{
        //    go = new GameObject("waterPooler");
        //    waterPooler = go.AddComponent<ObjectPoolerHaveScript>();
        //    waterPooler.unitPooledObject = waterPrefab;
        //    go.transform.parent = this.gameObject.transform;
        //    waterPooler.InitializeUnit(200);
        //    AllPool.Add(waterPooler);
        //}
        //if (firePooler == null)
        //{
        //    go = new GameObject("firePooler");
        //    firePooler = go.AddComponent<ObjectPoolerHaveScript>();
        //    firePooler.unitPooledObject = firePrefab;
        //    go.transform.parent = this.gameObject.transform;
        //    firePooler.InitializeUnit(200);
        //    AllPool.Add(firePooler);
        //}
        //if (gemPooler == null)
        //{
        //    go = new GameObject("gemPooler");
        //    gemPooler = go.AddComponent<ObjectPoolerHaveScript>();
        //    gemPooler.unitPooledObject = gemPrefab;
        //    go.transform.parent = this.gameObject.transform;
        //    gemPooler.InitializeUnit(200);
        //    AllPool.Add(gemPooler);
        //}
        if (stonePooler == null)
        {
            go = new GameObject("stonePooler");
            stonePooler = go.AddComponent<ObjectPoolerHaveScript>();
            stonePooler.unitPooledObject = stonePrefab;
            go.transform.parent = this.gameObject.transform;
            stonePooler.InitializeUnit(60);
            AllPool.Add(stonePooler);
        }
    }
    GameObject go;
}

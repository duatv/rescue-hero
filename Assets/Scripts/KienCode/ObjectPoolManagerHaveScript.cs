using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManagerHaveScript : MonoBehaviour
{
    [HideInInspector]
    public ObjectPoolerHaveScript stonePooler;
    public Unit stonePrefab;

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
        if (stonePooler == null)
        {
            go = new GameObject("stonePooler");
            stonePooler = go.AddComponent<ObjectPoolerHaveScript>();
            stonePooler.unitPooledObject = stonePrefab;
            go.transform.parent = this.gameObject.transform;
            stonePooler.InitializeUnit(150);
            AllPool.Add(stonePooler);
        }
    }
    GameObject go;
}

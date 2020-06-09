using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataAchievement", menuName = "Data Achievement")]
public class DataAchievement : ScriptableObject
{
    public Info[] infos;
    [System.Serializable]
    public struct Info
    {
        public string des, name;
        public Content[] contents;
    }
    [System.Serializable]
    public struct Content
    {
        public int numberReward, numberRequire;
    }
}

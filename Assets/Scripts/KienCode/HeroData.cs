using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data Hero", menuName = "Data Hero")]
public class HeroData : ScriptableObject
{
    public Info[] infos;
    [System.Serializable]
    public struct Info
    {
        public string itemName, nameSkin,nameSkinSword;
        public TypeUnlock typeUnlock;
        public int price;
    }
    public enum TypeUnlock
    {
        DAILYREWARD, COIN
    }
}

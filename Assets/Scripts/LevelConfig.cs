using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Creare Level Config")]
public class LevelConfig : ScriptableObject
{
    public List<MapLevelManager> lstAllLevel;

    public List<int> levelSkips;
}

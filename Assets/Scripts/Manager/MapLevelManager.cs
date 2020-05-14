using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapLevelManager : MonoBehaviour
{
    public enum SPAWNTYPE { WATER, LAVA, STONE, GEMS }
    public enum QUEST_TYPE {NONE, COLLECT, KILL, SAVE_HOSTAGE,OPEN_CHEST, ALL}
    public static MapLevelManager Instance;

    [HideInInspector]
    public bool isReadOnly;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public Transform trHostage, trGems;

    public QUEST_TYPE questType;
    [HideInInspector]
    public Transform trTarget;

    public Dictionary<string, GameObject> dAllStone = new Dictionary<string, GameObject>();
    public List<EnemyBase> lstAllEnemies = new List<EnemyBase>();
    public List<StickBarrier> lstAllStick = new List<StickBarrier>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        switch (questType) {
            case QUEST_TYPE.COLLECT:
                trTarget = trGems;
                break;
            case QUEST_TYPE.SAVE_HOSTAGE:
                trTarget = trHostage;
                break;
        }
    }
    public void OnWin()
    {
        GameManager.Instance.ShowWinPanel();
    }
    public void OnLose() {
        GameManager.Instance.ShowLosePanel();
    }
    

    public void OnClearMap()
    {
    }
}
#region Editor Mode
#if UNITY_EDITOR
[CustomEditor(typeof(MapLevelManager))]
public class MapLevelEditor : Editor
{
    string prefabPath = "Assets/";

    private MapLevelManager myScript;
    private void OnSceneGUI()
    {
        myScript = (MapLevelManager)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUIStyle SectionNameStyle = new GUIStyle();
        SectionNameStyle.fontSize = 16;
        SectionNameStyle.normal.textColor = Color.blue;

        if (myScript == null) return;

        EditorGUILayout.LabelField("----------\t----------\t----------\t----------\t----------", SectionNameStyle);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            if (GUILayout.Button("Save MapLevel", GUILayout.Height(50)))
            {
                prefabPath = "Assets/" + myScript.gameObject.name + ".prefab";
                PrefabUtility.SaveAsPrefabAsset(myScript.gameObject, prefabPath);
            }
        }
        EditorGUILayout.EndVertical();
    }
}
#endif
#endregion
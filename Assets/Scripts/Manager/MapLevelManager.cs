using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapLevelManager : MonoBehaviour
{
    public SpawnObject lavaObj, waterObj;
    public GameObject BarrierParent;
    public bool loadListStick;

    public enum SPAWNTYPE { WATER, LAVA, STONE, GEMS,GAS }
    public enum QUEST_TYPE { NONE, COLLECT, KILL, SAVE_HOSTAGE, OPEN_CHEST, ALL }
    public static MapLevelManager Instance;

    [HideInInspector]
    public bool isReadOnly;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public Transform trHostage, trGems;

    public QUEST_TYPE questType;

    [HideInInspector] public Transform trTarget;
    //[HideInInspector]
    public Transform trChest;

    public List<StickBarrier> lstAllStick = new List<StickBarrier>();
    public List<EnemyBase> lstAllEnemies = new List<EnemyBase>();
    private void Awake()
    {
        Instance = this;
    }
    private void OnValidate()
    {
        if (!loadListStick)
        {

            StickBarrier[] stick = GetComponentsInChildren<StickBarrier>();
            for (int i = 0; i < stick.Length; i++)
            {
                if (stick[i].key && !lstAllStick.Contains(stick[i]))
                {
                    lstAllStick.Add(stick[i]);
                }
            }
            loadListStick = true;

            Debug.LogError("chay vao day");
        }
    }
    private void Start()
    {
        switch (questType)
        {
            case QUEST_TYPE.COLLECT:
                trTarget = trGems;
                break;
            case QUEST_TYPE.SAVE_HOSTAGE:
                trTarget = trHostage;
                break;
        }

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.isTest)
            {
                GameManager.Instance.gameState = GameManager.GAMESTATE.PLAYING;
                GameManager.Instance.mapLevel = this;
                if (lstAllStick.Count > 0)
                    GameManager.Instance.playerMove = true;
            }
            GameManager.Instance.OnInitQuestText(questType);
        }
    }
    public void OnWin()
    {
        GameManager.Instance.ShowWinPanel();
    }
    public void OnLose()
    {
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
        if (myScript == null)
            myScript = (MapLevelManager)target;
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
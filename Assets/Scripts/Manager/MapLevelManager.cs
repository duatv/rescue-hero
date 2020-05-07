using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MapLevelManager : MonoBehaviour
{
    public enum SPAWNTYPE { WATER, LAVA, STONE, GEMS }
    public enum QUEST_TYPE {NONE, COLLECT, KILL, SAVE_HOSTAGE, ALL}
    public static MapLevelManager Instance;
    public QUEST_TYPE questType;
    public Dictionary<string, GameObject> dAllStone = new Dictionary<string, GameObject>();
    public List<StickBarrier> lstAllStick = new List<StickBarrier>();

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSaveMap()
    {

    }
    public void OnClearMap()
    {
    }
}
#region Editor Mode
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
                myScript.OnSaveMap();
                prefabPath = "Assets/" + myScript.gameObject.name + ".prefab";
                PrefabUtility.SaveAsPrefabAsset(myScript.gameObject, prefabPath);

                //EditorUtility.SetDirty(myScript);
            }
            if (GUILayout.Button("Clear MapLevel", GUILayout.Height(50)))
            {
                myScript.OnClearMap();
                //EditorUtility.SetDirty(myScript);
            }
        }
        EditorGUILayout.EndVertical();
    }

    //private void OnDisable()
    //{
    //    if (!myScript) return;
    //    EditorUtility.SetDirty(myScript);
    //}
}
#endregion
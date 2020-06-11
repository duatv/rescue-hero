using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyRange : EnemyBase
{
    // Start is called before the first frame update
   public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    //override public void Update()
    //{
    //    base.Update();
    //}
}
//#region Editor Mode
//#if UNITY_EDITOR
//[CustomEditor(typeof(EnemyRange))]
//public class MyEnemyRangeEditor : Editor
//{
//    private EnemyRange myScript;
//    private void OnSceneGUI()
//    {
//        myScript = (EnemyRange)target;
//    }
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        GUIStyle SectionNameStyle = new GUIStyle();
//        SectionNameStyle.fontSize = 16;
//        SectionNameStyle.normal.textColor = Color.blue;
//        if (myScript == null) return;
//        EditorGUILayout.LabelField("\t           Choose Enemy", SectionNameStyle);
//        EditorGUILayout.BeginVertical(GUI.skin.box);
//        {
//            if (GUILayout.Button("Enemy 1", GUILayout.Height(50)))
//            {
//                myScript.ChoosePlayer(0);
//                EditorUtility.SetDirty(myScript);
//            }
//            if (GUILayout.Button("Enemy 2", GUILayout.Height(50)))
//            {
//                myScript.ChoosePlayer(1);
//                EditorUtility.SetDirty(myScript);
//            }
//        }
//        EditorGUILayout.EndVertical();
//    }
//    private void OnDisable()
//    {
//        if (!myScript) return;
//        EditorUtility.SetDirty(myScript);
//    }
//}
//#endif
//#endregion
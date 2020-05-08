using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyMonster : EnemyBase
{

}
//#region Editor Mode
//#if UNITY_EDITOR
//[CustomEditor(typeof(EnemyMonster))]
//public class MyEnemyMonsterEditor : Editor
//{
//    private EnemyMonster myScript;
//    private void OnSceneGUI()
//    {
//        myScript = (EnemyMonster)target;
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
//            }
//            if (GUILayout.Button("Enemy 2", GUILayout.Height(50)))
//            {
//                myScript.ChoosePlayer(1);
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
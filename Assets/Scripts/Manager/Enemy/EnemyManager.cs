using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyManager : MonoBehaviour
{

}
//#region Editor Mode
//#if UNITY_EDITOR
//[CustomEditor(typeof(EnemyManager))]
//public class MyEnemyEditor : Editor
//{
//    private EnemyManager myScript;
//    private void OnSceneGUI()
//    {
//        myScript = (EnemyManager)target;
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
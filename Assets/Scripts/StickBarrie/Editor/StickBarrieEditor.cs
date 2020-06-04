using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(StickBarrier))]
public class StickBarrieEditor : Editor
{
    private StickBarrier myScript;
    private void OnSceneGUI()
    {

    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        myScript = (StickBarrier)target;
        GUIStyle SectionNameStyle = new GUIStyle();
        SectionNameStyle.fontSize = 16;
        SectionNameStyle.normal.textColor = Color.blue;
        if (myScript == null) return;
        if (myScript._moveType == StickBarrier.MOVETYPE.FREE)
        {
            EditorGUILayout.LabelField("\t        Set Stick Barrie Possition", SectionNameStyle);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                if (GUILayout.Button("Save Start Position", GUILayout.Height(50)))
                {
                    if(myScript.isMove2Pos)
                        myScript.vStartPos = myScript.gameObject.transform./*localPosition*/position;
                    else myScript.vStartPos = myScript.gameObject.transform.localPosition;
                    EditorUtility.SetDirty(myScript);
                }
                if (GUILayout.Button("Save End Position", GUILayout.Height(50)))
                {
                    if (myScript.isMove2Pos)
                    {
                        myScript.vEndPos = myScript.gameObject.transform./*localPosition*/position;
                        myScript.gameObject.transform./*localPosition*/position = myScript.vStartPos;
                    }
                    else {
                        myScript.vEndPos = myScript.gameObject.transform.localPosition;
                        myScript.gameObject.transform.localPosition = myScript.vStartPos;
                    }
                    EditorUtility.SetDirty(myScript);
                }
                if (GUILayout.Button("Move to Start Position", GUILayout.Height(50)))
                {
                    Vector2 vSwap = myScript.vStartPos;
                    myScript.vStartPos = myScript.vEndPos;
                    myScript.vEndPos = vSwap;
                    if(myScript.isMove2Pos)
                        myScript.gameObject.transform./*localPosition*/position = myScript.vStartPos;
                    else myScript.gameObject.transform.localPosition = myScript.vStartPos;
                    EditorUtility.SetDirty(myScript);
                }

            }
            EditorGUILayout.EndVertical();
        }
    }
    private void OnDisable()
    {
        if (!myScript) return;
        EditorUtility.SetDirty(myScript);
    }
}
#endif
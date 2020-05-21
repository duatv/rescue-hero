using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class HostageManager : CharsBase
{
    public bool isMeetPlayer;
    void Start()
    {
        if (SoundManager.Instance != null)
        {
            StartCoroutine(PlaySoundApear());
        }


        if (MapLevelManager.Instance != null)
        {
            MapLevelManager.Instance.trTarget = transform;
        }
        saPlayer.AnimationState.End += delegate
        {
            if (saPlayer.AnimationName.Equals(str_Win))
            {
                saPlayer.AnimationState.SetAnimation(0, str_Win2, true);
                PlayerManager.Instance.OnWin();
            }
        };


        if (PlayerManager.Instance != null)
        {
            if (transform.localPosition.x > PlayerManager.Instance.transform.localPosition.x)
            {
                saPlayer.skeleton.ScaleX = -1;
            }
            else saPlayer.skeleton.ScaleX = 1;
        }
        else
        {
            GameManager.Instance.gTargetFollow = gameObject;
        }
    }

    IEnumerator PlaySoundApear()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.acPrincessApear);
    }
    public void PlayWin()
    {
        saPlayer.AnimationState.SetAnimation(0, str_Win, false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
            {
                GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
                PlayWin();
            }
        }
    }
}
#region Editor Mode
#if UNITY_EDITOR
[CustomEditor(typeof(HostageManager))]
public class MyHostageEditor : Editor
{
    private HostageManager myScript;
    private void OnSceneGUI()
    {
        myScript = (HostageManager)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUIStyle SectionNameStyle = new GUIStyle();
        SectionNameStyle.fontSize = 16;
        SectionNameStyle.normal.textColor = Color.blue;
        if (myScript == null) return;
        EditorGUILayout.LabelField("\t           Choose Hostage", SectionNameStyle);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            if (GUILayout.Button("Hostage 1", GUILayout.Height(50)))
            {
                myScript.ChoosePlayer(0);
            }
            if (GUILayout.Button("Hostage 2", GUILayout.Height(50)))
            {
                myScript.ChoosePlayer(1);
            }
        }
        EditorGUILayout.EndVertical();
    }
    private void OnDisable()
    {
        if (!myScript) return;
        EditorUtility.SetDirty(myScript);
    }
}
#endif
#endregion
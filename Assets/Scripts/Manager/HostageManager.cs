using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class HostageManager : CharsBase
{
    public ParticleSystem pheart, bloodeffect;
    public bool isMeetPlayer;
    bool win = false;
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
        saPlayer.AnimationState.Complete += delegate
        {
            if (saPlayer.AnimationName.Equals(str_Win))
            {
                saPlayer.AnimationState.SetAnimation(0, str_Win2, true);
                PlayerManager.Instance.OnWin(true);
            }
        };


        if (PlayerManager.Instance != null)
        {
            if (transform.position.x > PlayerManager.Instance.transform.position.x)
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
        if (!win)
        {
            StartCoroutine(IEWaitToIdle());
            win = true;
            if (transform.position.x > PlayerManager.Instance.transform.position.x)
            {
                saPlayer.skeleton.ScaleX = -1;
            }
            else saPlayer.skeleton.ScaleX = 1;
        }
        //saPlayer.AnimationState.SetAnimation(0, str_Win, false);
    }
    public void PlayDie()
    {
        saPlayer.AnimationState.SetAnimation(0, str_Lose, false);
        skull.SetActive(true);
        bloodeffect.gameObject.SetActive(true);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acPrincessDie);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BodyPlayer")
        {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
            {
                GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
                // PlayerManager.Instance.OnPlayAnimOpenChest();
                //PlayerManager.Instance._rig2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                PlayerManager.Instance.OnWin(true);
                PlayWin();
            }
        }
        else if (collision.gameObject.tag == Utils.TAG_LAVA || collision.gameObject.tag == "Trap_Other" || collision.gameObject.name == "arrow")
        {
            if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
            {
                Debug.LogError("zoooooooooooooooooooo");
                if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                {
                    // PlayerManager.Instance.isContinueDetect = false;
                    PlayerManager.Instance.OnPlayerDie(false);
                    PlayDie();
                    if (collision.gameObject.tag == "arrow")
                    {
                        collision.gameObject.SetActive(false);
                    }
                }
            }
        }

        else if (collision.gameObject.tag == Utils.TAG_GAS)
        {
            if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
            {
                Debug.LogError("zoooooooooooooooooooo");
                if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                {
                    // PlayerManager.Instance.isContinueDetect = false;
                    PlayerManager.Instance.OnPlayerDie(false);
                    PlayDie();
                }
            }
        }
    }
    IEnumerator IEWaitToIdle()
    {
        yield return new WaitForSeconds(0.3f);

        if (!pheart.gameObject.activeSelf)
        {
            pheart.gameObject.SetActive(true);

            saPlayer.AnimationState.SetAnimation(0, str_Win, false);
            SoundManager.Instance.PlaySound(SoundManager.Instance.acPrincessSave);
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
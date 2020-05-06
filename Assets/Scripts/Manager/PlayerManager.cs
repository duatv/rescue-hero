using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerManager : MonoBehaviour
{
    public enum P_STATE { PLAYING, DIE, WIN , RUNNING}
    [HideInInspector]
    public bool isReadOnly = true;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public SkeletonDataAsset sdaP1, sdaP2;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    [SpineAnimation]
    public string str_idle, str_Win, str_Lose, str_Move;

    public SkeletonAnimation saPlayer;
    public Rigidbody2D _rig2D;
    public float moveSpeed;

    public P_STATE pState;


    private bool isContinueDetect = true;
    private bool beginMove = false;
    private bool isMoveLeft = false;
    private bool isMoveRight = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            beginMove = true;
            saPlayer.skeleton.ScaleX = -1;
            isMoveLeft = true;
            isMoveRight = false;
            PlayAnim(str_Move, true);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            beginMove = true;
            saPlayer.skeleton.ScaleX = 1;
            isMoveLeft = false;
            isMoveRight = true;
            PlayAnim(str_Move, true);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            beginMove = false;
            PlayAnim(str_idle, true);
            //saPlayer.skeleton.ScaleX *= -1;
        }
    }

    private void FixedUpdate()
    {
        if (!IsCanMove()) _rig2D.velocity = Vector2.zero;
        else {
            if (beginMove)
            {
                if (isMoveLeft) MoveLeft();
                else if (isMoveRight) MoveRight();
            }
        }
    }
    private bool IsCanMove() {
        return pState != P_STATE.DIE || pState != P_STATE.WIN;
    }
    #region Player movement
    private void MoveLeft()
    {
        _rig2D.velocity = Vector2.left * moveSpeed;
    }
    public void MoveRight() {
        _rig2D.velocity = Vector2.right * moveSpeed;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (pState == P_STATE.PLAYING)
        {
            if ((isContinueDetect && collision.gameObject.name.Contains("Lava_Pr") && collision.gameObject.tag.Contains(Utils.TAG_TRAP)) || (isContinueDetect && collision.gameObject.tag.Contains(Utils.TAG_TRAP)))
            {
                Debug.LogError(collision.gameObject.name + " ------> Tao die roi nhe.");
                isContinueDetect = false;
                pState = P_STATE.DIE;
                PlayAnim(str_Lose, false);
            }
            if (isContinueDetect && collision.gameObject.tag.Contains(Utils.TAG_WIN)) {
                Debug.LogError("Tao win roi");
                StartCoroutine(IEWin());
            }
        }
    }
    IEnumerator IEWin() {
        pState = P_STATE.WIN;
        yield return new WaitForSeconds(1.0f);
        Debug.LogError("Real Win");
        PlayAnim(str_Win, true);
    }

    public void ChoosePlayer(int i)
    {
        switch (i)
        {
            case 0:
                saPlayer.skeletonDataAsset = sdaP1;
                saPlayer.Initialize(true);
                PlayAnim(str_idle, true);
                break;
            case 1:
                saPlayer.skeletonDataAsset = sdaP2;
                saPlayer.Initialize(true);
                PlayAnim(str_idle, true);
                break;
        }
    }
    private void PlayAnim(string anim_, bool isLoop) {
        saPlayer.AnimationState.SetAnimation(0, anim_, isLoop);
    }
}
#region Editor Mode
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerManager))]
public class MyPlayerEditor : Editor
{
    private PlayerManager myScript;
    private void OnSceneGUI()
    {
        myScript = (PlayerManager)target;
    }
    int selected = 0;
    public static string[] opsChoosePosition = new string[] { "Male", "Female" };

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUIStyle SectionNameStyle = new GUIStyle();
        SectionNameStyle.fontSize = 16;
        SectionNameStyle.normal.textColor = Color.blue;
        if (myScript == null) return;
        EditorGUILayout.LabelField("\t        Choose Player", SectionNameStyle);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            if (GUILayout.Button("Male", GUILayout.Height(50)))
            {
                myScript.ChoosePlayer(0);
            }
            if (GUILayout.Button("Female", GUILayout.Height(50)))
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
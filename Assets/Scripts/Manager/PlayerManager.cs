using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;


    public enum P_STATE { PLAYING, DIE, WIN, RUNNING }
    [HideInInspector]
    public bool isReadOnly = true;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public SkeletonDataAsset sdaP1, sdaP2;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    [SpineAnimation]
    public string str_idle, str_Win, str_Lose, str_Move;

    public LayerMask lmColl;
    public SkeletonAnimation saPlayer;
    public Rigidbody2D _rig2D;
    public float moveSpeed;

    public P_STATE pState;

    [HideInInspector]
    public bool isContinueDetect = true;
    private bool beginMove = false;
    private bool isMoveLeft = false;
    private bool isMoveRight = false;

    private RaycastHit2D hit2D;
    private Vector3 vEnd, vStart;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.gTargetFollow = gameObject;
    }
    private void PrepareMoveLeft()
    {
        if (pState != P_STATE.DIE)
        {
            beginMove = true;
            saPlayer.skeleton.ScaleX = -1;
            isMoveLeft = true;
            isMoveRight = false;
            PlayAnim(str_Move, true);
        }
    }
    private void PrepareMoveRight()
    {
        if (pState != P_STATE.DIE)
        {
            beginMove = true;
            saPlayer.skeleton.ScaleX = 1;
            isMoveLeft = false;
            isMoveRight = true;
            PlayAnim(str_Move, true);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrepareMoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PrepareMoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            beginMove = false;
            PlayAnim(str_idle, true);
        }
    }
    private bool _isCanMoveToTarget;
    private void CheckHitAhead()
    {
        vStart = new Vector3(transform.localPosition.x + saPlayer.skeleton.ScaleX * 0.35f, transform.localPosition.y - 1.5f, transform.localPosition.z);
        vEnd = new Vector3(vStart.x + saPlayer.skeleton.ScaleX * 2f, vStart.y, vStart.z);
        Debug.DrawLine(vStart, vEnd, Color.red);

        hit2D = Physics2D.Linecast(vStart, vEnd, lmColl);
        if (hit2D.collider != null)
        {
            if (hit2D.collider.gameObject.GetComponent<HostageManager>() != null)
            {
                HostageManager _hm = hit2D.collider.gameObject.GetComponent<HostageManager>();

                if (_hm._charStage == CharsBase.CHAR_STATE.DIE) _isCanMoveToTarget = false;
                else
                {
                    _isCanMoveToTarget = true;
                }
            }else _isCanMoveToTarget = false;
        }
    }
    private void MoveToTarget()
    {
        Debug.LogError(gameObject.name + " <<<<<<<<<< ");
        _rig2D.velocity = moveSpeed * (saPlayer.skeleton.ScaleX > 0 ? Vector2.right : Vector2.left);
    }

    private void FixedUpdate()
    {
        CheckHitAhead();

        if (_isCanMoveToTarget) MoveToTarget();


        if (!IsCanMove()) _rig2D.velocity = Vector2.zero;
        else
        {
            if (beginMove)
            {
                if (isMoveLeft) MoveLeft();
                else if (isMoveRight) MoveRight();
            }
        }
    }
    private bool IsCanMove()
    {
        return pState != P_STATE.DIE || pState != P_STATE.WIN;
    }
    #region Player movement
    private void MoveLeft()
    {
        if (pState != P_STATE.DIE)
            _rig2D.velocity = Vector2.left * moveSpeed;
        else _rig2D.velocity = Vector2.zero;
    }
    public void MoveRight()
    {
        if (pState != P_STATE.DIE)
            _rig2D.velocity = Vector2.right * moveSpeed;
        else _rig2D.velocity = Vector2.zero;
    }
    #endregion

    public void OnPlayerDie()
    {
        pState = P_STATE.DIE;
        GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
        _rig2D.velocity = Vector2.zero;
        Physics2D.IgnoreLayerCollision(13, 11);
        Physics2D.IgnoreLayerCollision(13, 14);
        Physics2D.IgnoreLayerCollision(13, 9);
        Physics2D.IgnoreLayerCollision(13, 4);

        if(GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
            MapLevelManager.Instance.OnLose();
        PlayAnim(str_Lose, false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (pState == P_STATE.PLAYING || pState == P_STATE.RUNNING)
        {
            if ((isContinueDetect && collision.gameObject.name.Contains("Lava_Pr") && collision.gameObject.tag.Contains(Utils.TAG_TRAP)) || (isContinueDetect && collision.gameObject.tag.Contains(Utils.TAG_TRAP)))
            {
                isContinueDetect = false;
                OnPlayerDie();
            }
            if (isContinueDetect && collision.gameObject.tag.Contains(Utils.TAG_WIN))
            {
                _rig2D.velocity = Vector2.zero;
                beginMove = false;
                if(GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
                    StartCoroutine(IEWin());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pState == P_STATE.PLAYING || pState == P_STATE.RUNNING)
        {
            if (collision.gameObject.tag.Contains(Utils.TAG_STICKBARRIE))
            {
                beginMove = false;
                PlayAnim(str_idle, true);
            }
            if (MapLevelManager.Instance.questType == MapLevelManager.QUEST_TYPE.SAVE_HOSTAGE)
            {
                if (collision.gameObject.GetComponent<HostageManager>() != null)
                {
                    _rig2D.velocity = Vector2.zero;
                    beginMove = false;
                    if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE) {
                        collision.gameObject.GetComponent<HostageManager>().PlayWin();
                        StartCoroutine(IEWin());
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pState == P_STATE.PLAYING || pState == P_STATE.RUNNING)
        {
            if (collision.gameObject.tag.Contains(Utils.TAG_STICKBARRIE))
            {
                OnBeginRun();
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (pState == P_STATE.PLAYING || pState == P_STATE.RUNNING)
        {
            if (collision.gameObject.tag.Contains(Utils.TAG_STICKBARRIE))
            {
                OnBeginRun();
            }
        }
    }

    public void OnBeginRun()
    {
        StartCoroutine(IEWaitToRun());
    }
    IEnumerator IEWaitToRun()
    {
        yield return new WaitForSeconds(2.0f);
        if (pState != P_STATE.DIE)
        {
            pState = P_STATE.RUNNING;
            if (MapLevelManager.Instance != null)
            {
                if (MapLevelManager.Instance.trTarget != null && MapLevelManager.Instance.trTarget.gameObject.activeSelf)
                {
                    PrepareRotate(MapLevelManager.Instance.trTarget);
                }
            }
        }
    }

    private void PrepareRotate(Transform _trTarget)
    {
        if (transform.localPosition.x > _trTarget.localPosition.x)
        {
            saPlayer.skeleton.ScaleX = -1;
            PrepareMoveLeft();
        }
        else
        {
            saPlayer.skeleton.ScaleX = 1;
            PrepareMoveRight();
        }
    }
    IEnumerator IEWin()
    {
        GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
        pState = P_STATE.WIN;
        _rig2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.0f);
        MapLevelManager.Instance.OnWin();
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
    private void PlayAnim(string anim_, bool isLoop)
    {
        saPlayer.AnimationState.SetAnimation(0, anim_, isLoop);
    }
}
//#region Editor Mode
//#if UNITY_EDITOR
//[CustomEditor(typeof(PlayerManager))]
//public class MyPlayerEditor : Editor
//{
//    private PlayerManager myScript;
//    private void OnSceneGUI()
//    {
//        myScript = (PlayerManager)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        GUIStyle SectionNameStyle = new GUIStyle();
//        SectionNameStyle.fontSize = 16;
//        SectionNameStyle.normal.textColor = Color.blue;
//        if (myScript == null) return;
//        EditorGUILayout.LabelField("\t        Choose Player", SectionNameStyle);
//        EditorGUILayout.BeginVertical(GUI.skin.box);
//        {
//            if (GUILayout.Button("Male", GUILayout.Height(50)))
//            {
//                myScript.ChoosePlayer(0);
//                EditorUtility.SetDirty(myScript);
//            }
//            if (GUILayout.Button("Female", GUILayout.Height(50)))
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
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

    [HideInInspector] public bool isReadOnly = true;

    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public SkeletonDataAsset sdaP1, sdaP2;

    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    [SpineAnimation]
    public string str_idle, str_Win, str_Lose, str_Move, str_Att, str_TakeSword, str_OpenWithoutSword, str_OpenWithSword, str_MoveWithSword, str_Blink;
    [SpineSkin]
    public string skinDefault, skinSword;
    public PlayerDetectGems _detectGems;
    public Transform trSword, trSwordPos;
    public bool isTakeSword;
    public LayerMask lmColl, lmMapObject;
    public SkeletonAnimation saPlayer;
    public Rigidbody2D _rig2D;
    public float moveSpeed;
    public P_STATE pState;
    public Vector2 vJumpHeigh;

    // [HideInInspector] public bool isContinueDetect = true;
    [HideInInspector] public bool beginMove = false;
    private bool isMoveLeft = false;
    private bool isMoveRight = false;
    [HideInInspector] public RaycastHit2D hit2D, hitDown, hitForward;
    private Vector3 vEnd, vStart;
    private EnemyBase enBase;
    private bool _isCanMoveToTarget;
    Vector3 _vStart, _vEnd;

    private void Awake()
    {
        Instance = this;
        if (!string.IsNullOrEmpty(Utils.skinNormal))
        {
            saPlayer.Skeleton.SetSkin(Utils.skinNormal);
            saPlayer.Skeleton.SetSlotsToSetupPose();
            saPlayer.LateUpdate();
        }
    }
    private void Start()
    {
        GameManager.Instance.gTargetFollow = gameObject;

        OnIdleState();
        saPlayer.AnimationState.Complete += delegate
        {
            if (saPlayer.AnimationName.Equals(str_Att))
            {
                enBase.OnDie_();
                OnIdleState();
                //StartCoroutine(IEWaitToIdle());
            }
            if (saPlayer.AnimationName.Equals(str_OpenWithSword) || saPlayer.AnimationName.Equals(str_OpenWithoutSword))
            {
                StartCoroutine(ISShowWin());
            }
        };
    }
    IEnumerator IEWaitToIdle()
    {
        yield return new WaitForSeconds(0.3f);
        enBase.OnDie_();
        OnIdleState();
    }
    IEnumerator ISShowWin()
    {
        yield return new WaitForSeconds(1.2f);
        PlayAnim(str_Win, true);
        yield return new WaitForSeconds(1.2f);
        MapLevelManager.Instance.OnWin();
    }

    const float timeConst = 2;
    private float timeBlink = 0;
    private void HeroBlink()
    {
        timeBlink += Time.deltaTime;
        if (timeBlink >= timeConst)
        {
            saPlayer.AnimationState.SetAnimation(1, str_Blink, false);
            timeBlink = 0;
        }
    }
    private void HeroJump()
    {
        if (pState != P_STATE.DIE && pState != P_STATE.WIN)
        {
            if (!isJump)
            {
                //vJumpHeigh = new Vector2(saPlayer.skeleton.ScaleX, 2);
                //_rig2D.AddForce(vJumpHeigh, ForceMode2D.Impulse);
                _rig2D.velocity = new Vector2(saPlayer.skeleton.ScaleX, 5);
                isJump = true;
                // Debug.LogError("jump:");
            }
        }
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    HeroJump();
        //}
        if (saPlayer.AnimationName.Equals(str_idle))
        {
            HeroBlink();
        }

    }

    private void CheckHitAhead()
    {
        vStart = new Vector3(transform.localPosition.x + saPlayer.skeleton.ScaleX * 0.35f, transform.localPosition.y - 1.5f, transform.localPosition.z);
        vEnd = new Vector3(vStart.x + saPlayer.skeleton.ScaleX * 2f, vStart.y, vStart.z);
        hit2D = Physics2D.Linecast(vStart, vEnd, lmColl);
        if (hit2D.collider != null)
        {
            if (hit2D.collider.gameObject.GetComponent<Chest>() != null)
            {
                _isCanMoveToTarget = true;
            }
            else if (hit2D.collider.gameObject.GetComponent<HostageManager>() != null)
            {
                HostageManager _hm = hit2D.collider.gameObject.GetComponent<HostageManager>();
                if (_hm._charStage == CharsBase.CHAR_STATE.DIE) _isCanMoveToTarget = false;

                else
                {
                    _isCanMoveToTarget = true;
                }
            }
            else if (hit2D.collider.gameObject.GetComponent<StickBarrier>() != null)
            {
                _isCanMoveToTarget = false;
            }
            else _isCanMoveToTarget = false;
        }
        //    Debug.DrawLine(vStart, vEnd, Color.red);
    }
    private void MoveToTarget()
    {
        if (GameManager.Instance.gameState == GameManager.GAMESTATE.PLAYING)
        {
            PlayAnim(isTakeSword ? str_MoveWithSword : str_Move, true);
            _rig2D.velocity = moveSpeed * (saPlayer.skeleton.ScaleX > 0 ? Vector2.right : Vector2.left);
        }
    }

    private void HitDownMapObject()
    {
        _vStart = ground.transform.position;
        _vEnd = new Vector2(_vStart.x, _vStart.y - 0.15f);
        hitDown = Physics2D.Linecast(_vStart, _vEnd, lmMapObject);
        Debug.DrawLine(_vStart, _vEnd, Color.red);
    }
    public Transform leftJump, rightJump, ground;
    Vector2 checkJumpStart, checkJumpEnd;
    private void CheckVatCan()
    {
        checkJumpStart = ground.transform.position;
        checkJumpEnd = saPlayer.skeleton.ScaleX > 0 ? rightJump.transform.position : leftJump.transform.position;
        hitForward = Physics2D.Linecast(checkJumpStart, checkJumpEnd, lmMapObject);
        Debug.DrawLine(checkJumpStart, checkJumpEnd);
    }
    public bool isJump;
    private void FixedUpdate()
    {

        CheckHitAhead();
        CheckVatCan();
        HitDownMapObject();
        if (hitDown.collider != null)
        {
            if (isJump)
                isJump = false;
            // Debug.LogError(hitDown.collider.tag);
            //if (_isCanMoveToTarget)
            //{
            //    if (hitDown.collider.gameObject.tag == Utils.TAG_WALL_BOTTOM)
            //    {
            //        pState = P_STATE.RUNNING;
            //        beginMove = true;
            //        MoveToTarget();
            //    }
            //}
        }
        //else
        //{
        //    beginMove = false;
        //}
        if (hitForward.collider != null)
        {
            //if (!_isCanMoveToTarget)
            //{
            //if (hitForward.collider.gameObject.tag == Utils.TAG_STONE)
            //{
            if (hitForward.collider.gameObject.tag != "Wall_Bottom")
            {
                if (hitForward.collider.gameObject.tag == "Tag_Stone" || hitForward.collider.gameObject.tag == "Chan")
                    HeroJump();
                //   Debug.LogError(hitForward.collider.name);
            }

            // }
            //}
        }

        if (!IsCanMove()) _rig2D.velocity = Vector2.zero;
        else
        {
            if (beginMove)
            {
                if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                {
                    if (isMoveLeft) MoveLeft();
                    else if (isMoveRight) MoveRight();
                }
            }
        }
    }
    private bool IsCanMove()
    {
        return pState != P_STATE.DIE || pState != P_STATE.WIN;
    }
    #region Player movement
    Vector2 movement;
    private void MoveLeft()
    {
        if (pState != P_STATE.DIE)
        {
            movement = Vector2.left * moveSpeed;
            _rig2D.velocity = new Vector2(movement.x, _rig2D.velocity.y);
            PlayAnim(isTakeSword ? str_MoveWithSword : str_Move, true);
        }
        else _rig2D.velocity = Vector2.zero;


        //  Debug.LogError("moveleft");
    }
    public void MoveRight()
    {
        if (pState != P_STATE.DIE)
        {
            movement = Vector2.right * moveSpeed;
            _rig2D.velocity = new Vector2(movement.x, _rig2D.velocity.y);
            PlayAnim(isTakeSword ? str_MoveWithSword : str_Move, true);
        }
        else _rig2D.velocity = Vector2.zero;

        //  Debug.LogError("moveright");
    }

    public void PrepareMoveLeft()
    {
        if (pState != P_STATE.DIE && pState != P_STATE.WIN)
        {
            if (!beginMove)
            {
                pState = P_STATE.RUNNING;
                beginMove = true;
                saPlayer.skeleton.ScaleX = -1;
                isMoveLeft = true;
                isMoveRight = false;
                PlayAnim(isTakeSword ? str_MoveWithSword : str_Move, true);
            }
        }
    }
    public void PrepareMoveRight()
    {
        if (pState != P_STATE.DIE && pState != P_STATE.WIN)
        {
            if (!beginMove)
            {
                pState = P_STATE.RUNNING;
                beginMove = true;
                saPlayer.skeleton.ScaleX = 1;
                isMoveLeft = false;
                isMoveRight = true;
                PlayAnim(isTakeSword ? str_MoveWithSword : str_Move, true);
            }
        }
    }
    //public void PrepareRotate_(Transform _trTarget, bool rotateOnly)
    //{
    //    if (pState != P_STATE.DIE && pState != P_STATE.WIN)
    //    {
    //        if (transform.localPosition.x > _trTarget.localPosition.x)
    //        {
    //            saPlayer.skeleton.ScaleX = -1;
    //            PrepareMoveRight();
    //        }
    //        else
    //        {
    //            saPlayer.skeleton.ScaleX = 1;
    //            PrepareMoveLeft();
    //        }
    //    }
    //}

    public void PrepareRotate(Transform _trTarget, bool rotateOnly)
    {
        if (pState != P_STATE.DIE && pState != P_STATE.WIN)
        {
            if (hitDown.collider != null)
            {
                beginMove = true;
                if (transform.localPosition.x > _trTarget.localPosition.x)
                {
                    saPlayer.skeleton.ScaleX = -1;
                    PrepareMoveLeft();
                    isMoveLeft = true;
                    isMoveRight = false;
                }
                else
                {
                    saPlayer.skeleton.ScaleX = 1;
                    isMoveLeft = false;
                    isMoveRight = true;
                    //PrepareMoveRight();
                }
            }
        }
    }
    #endregion

    #region Player action
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
                    PrepareRotate(MapLevelManager.Instance.trTarget, false);
                }
            }
        }
    }


    public void OnIdleState()
    {
        pState = P_STATE.PLAYING;
        _rig2D.velocity = Vector2.zero;
        beginMove = false;
        saPlayer.AnimationState.SetAnimation(1, str_Blink, false);
        PlayAnim(str_idle, true);
    }
    public void OnAttackEnemy(EnemyBase _enBase)
    {
        enBase = _enBase;
        PlayAnim(str_Att, false);
        MapLevelManager.Instance.trTarget = null;
    }
    public void OnTakeSword(Transform _tr)
    {
        isTakeSword = true;
        _tr.gameObject.SetActive(false);
        if (!string.IsNullOrEmpty(Utils.skinSword))
            saPlayer.Skeleton.SetSkin(Utils.skinSword);
        else saPlayer.Skeleton.SetSkin(skinSword);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acTakeSword);
        }

        OnIdleState();
    }
    public void OnPlayerDie()
    {
        if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
        {
            pState = P_STATE.DIE;
            GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
            _rig2D.velocity = Vector2.zero;
            _rig2D.constraints = RigidbodyConstraints2D.FreezePosition;
            PlayAnim(str_idle, true);
            StartCoroutine(IEWait());
        }
    }
    IEnumerator IEWait()
    {
        yield return new WaitForSeconds(0.6f);
        MapLevelManager.Instance.OnLose();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acHeroDie);
        }

        saPlayer.AnimationState.SetEmptyAnimation(1, 0.2f);
        PlayAnim(str_Lose, false);
    }

    #endregion
    #region Collision
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (pState == P_STATE.PLAYING || pState == P_STATE.RUNNING)
    //    {
    //        if (isContinueDetect && collision.gameObject.tag == Utils.TAG_TRAP)
    //        {
    //            if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
    //            {
    //                isContinueDetect = false;
    //                OnPlayerDie();
    //            }
    //        }
    //        if (isContinueDetect && collision.gameObject.tag == Utils.TAG_WIN)
    //        {
    //            _rig2D.velocity = Vector2.zero;
    //            beginMove = false;
    //            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
    //                OnWin();
    //        }
    //    }
    //}

    public void OnWin()
    {
        GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
        pState = P_STATE.WIN;
        _rig2D.velocity = Vector2.zero;
        _rig2D.constraints = RigidbodyConstraints2D.FreezePosition;
        beginMove = false;
        PlayAnim(isTakeSword ? str_OpenWithSword : str_OpenWithoutSword, false);
        GameManager.Instance.ShowWinPanel();
    }

    public void OnPlayAnimOpenChest()
    {
        beginMove = false;
        PlayAnim(isTakeSword ? str_OpenWithSword : str_OpenWithoutSword, false);
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (pState == P_STATE.PLAYING || pState == P_STATE.RUNNING)
    //    {
    //        if (collision.gameObject.tag.Contains(Utils.TAG_STICKBARRIE))
    //        {
    //            if (hitDown.collider != null)
    //                if (hitDown.collider.gameObject.tag.Contains(Utils.TAG_WALL_BOTTOM))
    //                    OnBeginRun();
    //        }
    //    }
    //}
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (pState == P_STATE.PLAYING || pState == P_STATE.RUNNING)
        {
            if (collision.gameObject.tag == Utils.TAG_STICKBARRIE)
            {
                OnBeginRun();
            }
        }
    }
    #endregion

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
        if (!saPlayer.AnimationName.Equals(anim_))
        {
            saPlayer.AnimationState.SetAnimation(0, anim_, isLoop);
        }
    }
}
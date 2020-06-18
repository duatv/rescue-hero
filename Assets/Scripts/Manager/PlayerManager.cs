using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerManager : MonoBehaviour
{
    public GameObject skull;
    public static PlayerManager Instance;
    public GameObject effectDie;
    public enum P_STATE { PLAYING, DIE, WIN, RUNNING }

    [HideInInspector] public bool isReadOnly = true;

    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public SkeletonDataAsset sdaP1, sdaP2;

    // [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    [SpineAnimation]
    public string str_idle, str_Win, str_Win2, str_Lose, str_Move, str_Att, str_TakeSword, str_OpenWithoutSword, str_OpenWithSword, str_MoveWithSword, str_Blink;
    [SpineSkin]
    public string skinDefault, skinSword;
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
    public bool isMoveLeft = false;
    public bool isMoveRight = false;
    [HideInInspector] public RaycastHit2D hit2D, hitDown, hitForward;
    private Vector3 vEnd, vStart;
    private EnemyBase enBase;
    private bool _isCanMoveToTarget;
    Vector3 _vStart, _vEnd;

    private void Awake()
    {
        Instance = this;
        //if (!string.IsNullOrEmpty(Utils.skinNormal))
        //{
        if (DataController.instance != null)
        {
            //saPlayer.Skeleton.SetSkin(DataController.instance.heroData.infos[DataParam.currentHero].nameSkin);
            //saPlayer.Skeleton.SetSlotsToSetupPose();
            //saPlayer.LateUpdate();

            var skeleton = saPlayer.Skeleton;
            var skeletonData = skeleton.Data;
            var newSkin = new Skin("new-skin");
            newSkin.AddSkin(skeletonData.FindSkin(DataController.instance.heroData.infos[DataParam.currentHero].nameSkin));


            skeleton.SetSkin(newSkin);
            skeleton.SetSlotsToSetupPose();
            saPlayer.AnimationState.Apply(skeleton);
        }
        //}
    }
    private void Event(TrackEntry trackEntry, Spine.Event e)
    {
        if (trackEntry.Animation.Name.Equals(str_Att))
        {
            //    Debug.LogError("======= Bua dap ========");
            enBase.OnDie_();
        }
    }
    private void Start()
    {
        GameManager.Instance.gTargetFollow = gameObject;

        OnIdleState();
        saPlayer.AnimationState.Complete += delegate
        {
            //if (saPlayer.AnimationName.Equals(str_Att))
            //{
            //    enBase.OnDie_();
            //    //   OnIdleState();
            //    //StartCoroutine(IEWaitToIdle());
            //}
            if (saPlayer.AnimationName.Equals(str_OpenWithSword) || saPlayer.AnimationName.Equals(str_OpenWithoutSword))
            {
                StartCoroutine(ISShowWin());
                Debug.LogError("wwwwwtttttttffffffff");
            }
        };
        saPlayer.AnimationState.Event += Event;
    }
    IEnumerator IEWaitToIdle()
    {
        yield return new WaitForSeconds(0.3f);
        enBase.OnDie_();
        OnIdleState();
    }

    IEnumerator IEWaitToIdleAfterAttack()
    {
        yield return new WaitForSeconds(1f);
        OnIdleState();
    }


    IEnumerator ISShowWin()
    {
        yield return new WaitForSeconds(0.5f);
        if (!saPlayer.AnimationName.Equals(str_Lose) || pState != P_STATE.DIE)
        {

            if (GameManager.Instance.mapLevel.questType == MapLevelManager.QUEST_TYPE.SAVE_HOSTAGE)
                PlayAnim(str_Win2, false);
            else
            {
                PlayAnim(str_Win, true);
                Debug.LogError("====sdkahfjkd=====");
            }
        }
        yield return new WaitForSeconds(0.5f);
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
                _rig2D.velocity = new Vector2(saPlayer.skeleton.ScaleX, 4);
                isJump = true;
            }
        }
    }
    private void Update()
    {
        if (saPlayer.AnimationName.Equals(str_idle))
        {
            HeroBlink();
        }

    }

    private void CheckStickBarrier()
    {
        if (pState == P_STATE.DIE)
            return;
        vStart = body.position;
        vEnd = saPlayer.skeleton.ScaleX > 0 ? rightCheckStick.transform.position : leftCheckStick.transform.position;
        hit2D = Physics2D.Linecast(vStart, vEnd, lmColl);
        Debug.DrawLine(vStart, vEnd, Color.yellow);
    }
    private void MoveToTarget()
    {
        if (GameManager.Instance.gameState == GameManager.GAMESTATE.PLAYING)
        {
            _rig2D.velocity = moveSpeed * (saPlayer.skeleton.ScaleX > 0 ? Vector2.right : Vector2.left);
            if (saPlayer.AnimationName == str_OpenWithSword || saPlayer.AnimationName == str_OpenWithoutSword)
                return;
            PlayAnim(isTakeSword ? str_MoveWithSword : str_Move, true);
            Debug.LogError("====move target====");
        }
    }

    private void HitDownMapObject()
    {
        if (pState == P_STATE.DIE)
            return;
        _vStart = ground.transform.position;
        _vEnd = new Vector2(_vStart.x, _vStart.y - 0.15f);
        hitDown = Physics2D.Linecast(_vStart, _vEnd, lmMapObject);
        Debug.DrawLine(_vStart, _vEnd, Color.red);
    }
    public Transform leftCheckStick, rightCheckStick;
    public Transform leftJump, rightJump, ground, body;
    Vector2 checkJumpStart, checkJumpEnd;
    private void CheckVatCan()
    {
        if (pState == P_STATE.DIE || !beginMove)
            return;
        checkJumpStart = ground.transform.position;
        checkJumpEnd = saPlayer.skeleton.ScaleX > 0 ? rightJump.transform.position : leftJump.transform.position;
        hitForward = Physics2D.Linecast(checkJumpStart, checkJumpEnd, lmMapObject);
        Debug.DrawLine(checkJumpStart, checkJumpEnd);
    }
    public bool isJump;
    private void FixedUpdate()
    {

        if (!GameManager.Instance.playerMove)
            return;
        if (saPlayer.AnimationName == str_Win || saPlayer.AnimationName == str_Win2 || saPlayer.AnimationName == str_OpenWithSword || saPlayer.AnimationName == str_OpenWithoutSword)
        {
            Debug.LogError("Animation Name:" + saPlayer.AnimationName);
            return;
        }
        CheckStickBarrier();
        CheckVatCan();
        HitDownMapObject();
        if (hitDown.collider != null)
        {
            if (isJump)
                isJump = false;

            if (!beginMove)
            {
                if (!isTakeSword)
                {
                    if (GameManager.Instance.mapLevel.lstAllStick.Count <= 0)
                        PrepareRotate();
                }

            }
        }

        if (hitForward.collider != null)
        {
            //if (_rig2D.velocity.y == 0)
            //{
                if (hitForward.collider.gameObject.tag != "Wall_Bottom")
                {
                    if ((hitForward.collider.gameObject.tag == "Tag_Stone" && hitForward.collider.gameObject.name != "FallingStone") || hitForward.collider.gameObject.tag == "Chan")
                        HeroJump();
                }
            //}
        }

        //if (isTakeSword)
        //{
        //    Debug.LogError("move:" + beginMove + ":" + isMoveLeft + ":" + isMoveRight);
        //}

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

        if (hit2D.collider != null && !isTakeSword)
        {
            if (pState != P_STATE.DIE)
            {
                _rig2D.velocity = new Vector2(0, _rig2D.velocity.y);
                if (saPlayer.AnimationName == str_Win || saPlayer.AnimationName == str_Win2 || saPlayer.AnimationName == str_OpenWithSword || saPlayer.AnimationName == str_OpenWithoutSword)
                    return;
                PlayAnim(str_idle, true);
            }
            return;
        }

        if (pState != P_STATE.DIE)
        {
            movement = Vector2.left * moveSpeed;
            _rig2D.velocity = new Vector2(movement.x, _rig2D.velocity.y);
            if (saPlayer.AnimationName == str_Win || saPlayer.AnimationName == str_Win2 || saPlayer.AnimationName == str_OpenWithSword || saPlayer.AnimationName == str_OpenWithoutSword)
                return;
            PlayAnim(isTakeSword ? str_MoveWithSword : str_Move, true);
            Debug.LogError("====move left====");
        }
        else _rig2D.velocity = new Vector2(0, _rig2D.velocity.y);


    }
    public void MoveRight()
    {
        if (hit2D.collider != null && !isTakeSword)
        {
            if (pState != P_STATE.DIE)
            {
                _rig2D.velocity = new Vector2(0, _rig2D.velocity.y);
                if (saPlayer.AnimationName == str_Win || saPlayer.AnimationName == str_Win2 || saPlayer.AnimationName == str_OpenWithSword || saPlayer.AnimationName == str_OpenWithoutSword)
                    return;
                PlayAnim(str_idle, true);
            }
            return;
        }
        if (pState != P_STATE.DIE)
        {
            movement = Vector2.right * moveSpeed;
            _rig2D.velocity = new Vector2(movement.x, _rig2D.velocity.y);
            if (saPlayer.AnimationName == str_Win || saPlayer.AnimationName == str_Win2 || saPlayer.AnimationName == str_OpenWithSword || saPlayer.AnimationName == str_OpenWithoutSword)
                return;
            PlayAnim(isTakeSword ? str_MoveWithSword : str_Move, true);
            Debug.LogError("====move right====");
        }
        else _rig2D.velocity = new Vector2(0, _rig2D.velocity.y);

    }
    IEnumerator delayMove()
    {
        yield return new WaitForSeconds(0.25f);

        if (saPlayer.AnimationName != str_Win && saPlayer.AnimationName != str_Win2 && saPlayer.AnimationName != str_OpenWithSword && saPlayer.AnimationName != str_OpenWithoutSword)
        {
            if (!isTakeSword)
            {
                if (pState != P_STATE.DIE && pState != P_STATE.WIN)
                {
                    if (hitDown.collider != null)
                    {
                        beginMove = true;
                        if (saPlayer.skeleton.ScaleX < 0)
                        {
                            isMoveLeft = true;
                            isMoveRight = false;
                            Debug.LogError("=====dectect left");
                        }
                        else
                        {
                            isMoveLeft = false;
                            isMoveRight = true;
                            Debug.LogError("=====dectect right");
                        }
                    }
                }
            }
        }

    }

    public void PrepareRotate()
    {
        if (!GameManager.Instance.playerMove)
            return;

        StartCoroutine(delayMove());
    }
    #endregion

    #region Player action

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
        //if (beginMove)
        //{
        // saPlayer.AnimationState.SetEmptyAnimation(0, 0);
        enBase = _enBase;
        PlayAnim(str_Att, false);
        MapLevelManager.Instance.trTarget = null;
        beginMove = false;
        isMoveLeft = false;
        isMoveRight = false;
        _rig2D.velocity = Vector2.zero;
        Debug.LogError("wtfkkkkkk");

        StartCoroutine(nameof(IEWaitToIdleAfterAttack));
        //}
    }
    public void OnTakeSword(Transform _tr)
    {
        isTakeSword = true;
        _tr.gameObject.SetActive(false);

        //if (!string.IsNullOrEmpty(Utils.skinSword))
        //    saPlayer.Skeleton.SetSkin(Utils.skinSword);

        //else saPlayer.Skeleton.SetSkin(skinSword);

        var skeleton = saPlayer.Skeleton;
        var skeletonData = skeleton.Data;
        var newSkin = new Skin("new-skin");
        //    Debug.LogError(DataController.instance.heroData.infos[DataParam.currentHero].nameSkin);
        if (GameManager.Instance.isTest)
            newSkin.AddSkin(skeletonData.FindSkin("kiem"));
        else
        {
            newSkin.AddSkin(skeletonData.FindSkin(DataController.instance.heroData.infos[DataParam.currentHero].nameSkinSword));
            Debug.LogError(DataController.instance.heroData.infos[DataParam.currentHero].nameSkinSword);
        }

        skeleton.SetSkin(newSkin);
        skeleton.SetSlotsToSetupPose();
        saPlayer.AnimationState.Apply(skeleton);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.acTakeSword);
        }
        isMoveRight = false;
        isMoveLeft = false;
        OnIdleState();
    }
    public void OnPlayerDie(bool effect)
    {
        if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
        {
            pState = P_STATE.DIE;
            GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
            _rig2D.velocity = Vector2.zero;
            _rig2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            transform.rotation = Quaternion.identity;
            _rig2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            PlayAnim(str_idle, true);
            effectDie.SetActive(effect);
            skull.SetActive(effect);
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.acHeroDie);
            }

            saPlayer.AnimationState.SetEmptyAnimation(1, 0.2f);
            PlayAnim(str_Lose, false);
            StartCoroutine(IEWait(effect));
        }
    }
    IEnumerator IEWait(bool effect)
    {
        yield return new WaitForSeconds(0.1f);
        MapLevelManager.Instance.OnLose();

    }

    #endregion
    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
      //  Debug.LogError("zo day");
        if (collision.gameObject.tag == "ChangeDir" /*|| collision.gameObject.tag == ""*/)
        {
            Debug.LogError("zo day");
            if (collision.gameObject.transform.localScale.x > 0)
            {
                isMoveLeft = false;
                isMoveRight = true;
            }
            else
            {
                isMoveLeft = true;
                isMoveRight = false;
            }
            if (isTakeSword)
                beginMove = true;
            saPlayer.skeleton.ScaleX = collision.gameObject.transform.localScale.x > 0 ? 1 : -1;
            Debug.LogError("zo day");
        }
        else
        if (collision.gameObject.tag == "Trap_Other")
        {
            if (pState == P_STATE.PLAYING || pState == P_STATE.RUNNING)
            {
                //  Debug.LogError("zoooooooooooooooooooo");
                if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                {
                    // PlayerManager.Instance.isContinueDetect = false;
                    OnPlayerDie(true);
                }
            }
        }
        else if(collision.gameObject.tag == "arrow")
        {
            collision.gameObject.SetActive(false);
            if (pState == P_STATE.PLAYING || pState == P_STATE.RUNNING)
            {
                //  Debug.LogError("zoooooooooooooooooooo");
                if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                {
                    // PlayerManager.Instance.isContinueDetect = false;
                    OnPlayerDie(true);
                }
            }
        }
    }

    IEnumerator delayWin()
    {
        yield return new WaitForSeconds(1f);
        if (pState != P_STATE.DIE)
        {
            GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
            pState = P_STATE.WIN;
            // GameManager.Instance.ShowWinPanel();
        }
    }
    bool win = false;
    public void OnWin(bool playcollect)
    {
        _rig2D.velocity = new Vector2(0, _rig2D.velocity.y);
        transform.rotation = Quaternion.identity;
        _rig2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        beginMove = false;
        if (playcollect)
        {
            if (!win)
            {
                PlayAnim(isTakeSword ? str_OpenWithSword : str_OpenWithoutSword, false);
                win = true;
                isMoveLeft = false;
                isMoveRight = false;
                StartCoroutine(delayWin());
                Debug.LogError("win đi");
            }

            // Debug.LogError("zooooooooooooooo 1");
        }
        else
        {
            StartCoroutine(ISShowWin());
            //  Debug.LogError("zooooooooooooooo 2");
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
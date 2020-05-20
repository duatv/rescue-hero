using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyBase : MonoBehaviour
{
    public enum ENEMY_TYPE { MELEE, RANGE, MONSTER }
    public enum CHAR_STATE { PLAYING, DIE, WIN, RUNNING }

    [SerializeField]
    public ENEMY_TYPE enemyType;
    [SerializeField]
    public CHAR_STATE _charStage;
    [HideInInspector]
    public bool isReadOnly = false;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public SkeletonDataAsset sdaP1, sdaP2;
    public SkeletonAnimation saPlayer;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    [SpineAnimation]
    public string str_idle, str_Win, str_Lose, str_Att, str_Run;
    public float moveSpeed;
    [SerializeField] public Rigidbody2D rig;
    public LayerMask lmColl, lmPlayer, lmMapObject;
    public bool isRangerAtt;
    public GameObject gGround;

    [SerializeField]
    private RaycastHit2D hit2D, hit2D_1, hitPlayer, hitDown;
    private bool isContinueDetect = true;
    private Vector3 vEnd, vStart, _vEnd, _vStart;
    private bool isBeginAtt, isBeginMove;
    private string targetName;
    private EnemyBase ebTarget;
    private HostageManager hmTarget;
    private PlayerManager pmTarget;
    private Transform trTargetAttack;

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
    protected void PlayAnim(string anim_, bool isLoop)
    {
        if (!saPlayer.AnimationName.Equals(anim_))
            saPlayer.AnimationState.SetAnimation(0, anim_, isLoop);
    }

    public virtual void OnEnable()
    {
    }
    public virtual void Start()
    {
        if (MapLevelManager.Instance != null)
        {
            MapLevelManager.Instance.lstAllEnemies.Add(this);
        }

        saPlayer.AnimationState.End += delegate
        {
            if (saPlayer.AnimationName.Equals(str_Lose))
            {
                gGround.layer = 0;
            }
            if (saPlayer.AnimationName.Equals(str_Att))
            {
                if (hitPlayer.collider != null)
                {
                    isContinueDetect = false;
                    if (hitPlayer.collider.gameObject.GetComponent<PlayerManager>())
                    {
                        if (hitPlayer.collider.gameObject.GetComponent<PlayerManager>().isTakeSword)
                        {
                            hitPlayer.collider.gameObject.GetComponent<PlayerManager>().OnAttackEnemy(this);
                        }
                        else
                        {
                            hitPlayer.collider.gameObject.GetComponent<PlayerManager>().OnPlayerDie();
                            GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
                        }
                    }
                    else if (hitPlayer.collider.gameObject.GetComponent<HostageManager>())
                    {
                        hitPlayer.collider.gameObject.GetComponent<HostageManager>().OnDie_();
                        GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
                    }
                }
            }
        };
    }
    private bool _isCanMoveToTarget;

    Vector3 _vStartHitDown, _vEndHitDown;
    private void HitDownMapObject()
    {
        _vStartHitDown = new Vector3(transform.localPosition.x, transform.localPosition.y - 1.5f, transform.localPosition.z);
        _vEndHitDown = new Vector3(_vStartHitDown.x, _vStartHitDown.y - 0.15f, _vStartHitDown.z);
        hitDown = Physics2D.Linecast(_vStartHitDown, _vEndHitDown, lmMapObject);

        Debug.DrawLine(_vStartHitDown, _vEndHitDown, Color.red);
    }



    private Vector3 vStartForward, vEndForward;
    public virtual void FixedUpdate()
    {
        if (_charStage == CHAR_STATE.PLAYING)
        {
            #region Check Hit Down
            HitDownMapObject();
            #endregion


            if (hitDown.collider != null)
            {
                #region Check Hit Ahead
                vStart = new Vector3(transform.localPosition.x - 0.35f, transform.localPosition.y - 1.2f, transform.localPosition.z);
                vEnd = new Vector3(vStart.x - 2f, vStart.y, vStart.z);

                vStartForward = new Vector3(transform.localPosition.x + 0.35f, transform.localPosition.y - 1.2f, transform.localPosition.z);
                vEndForward = new Vector3(vStartForward.x + 2f, vStartForward.y, vStartForward.z);
                Debug.DrawLine(vStart, vEnd, Color.red);
                Debug.DrawLine(vStartForward, vEndForward, Color.green);
                hit2D_1 = Physics2D.Linecast(vStartForward, vEndForward, lmColl);
                hit2D = Physics2D.Linecast(vStart, vEnd, lmColl);

                if (hit2D.collider != null)
                {
                    if (!hit2D.collider.gameObject.tag.Contains(Utils.TAG_STICKBARRIE) && !isBeginAtt)
                    {
                        if (hit2D.collider.gameObject.GetComponent<EnemyBase>() != null)
                        {
                            if (hit2D.collider.gameObject.GetComponent<EnemyBase>().isRangerAtt)
                            {
                                _isCanMoveToTarget = hit2D.collider.gameObject.GetComponent<EnemyBase>()._charStage == CHAR_STATE.DIE ? false : true;
                                trTargetAttack = hit2D.collider.gameObject.transform;
                            }
                        }
                        else
                        if (hit2D.collider.gameObject.GetComponent<HostageManager>() != null)
                        {
                            _isCanMoveToTarget = hit2D.collider.gameObject.GetComponent<HostageManager>()._charStage == CharsBase.CHAR_STATE.DIE ? false : true;
                            trTargetAttack = hit2D.collider.gameObject.transform;
                        }
                        else if (hit2D.collider.gameObject.GetComponent<PlayerManager>() != null)
                        {
                            _isCanMoveToTarget = hit2D.collider.gameObject.GetComponent<PlayerManager>().pState == PlayerManager.P_STATE.DIE ? false : true;
                            trTargetAttack = hit2D.collider.gameObject.transform;
                        }
                        else
                        {
                            _isCanMoveToTarget = false;
                        }

                        targetName = hit2D.collider.name;
                        if (_isCanMoveToTarget)
                        {
                            OnMoveToTarget();
                            isBeginAtt = true;
                        }
                    }
                }
                else if (hit2D_1.collider != null)
                {
                    if (!hit2D_1.collider.gameObject.tag.Contains(Utils.TAG_STICKBARRIE) && !isBeginAtt)
                    {
                        if (hit2D_1.collider.gameObject.GetComponent<EnemyBase>() != null)
                        {
                            if (hit2D_1.collider.gameObject.GetComponent<EnemyBase>().isRangerAtt)
                            {
                                _isCanMoveToTarget = hit2D_1.collider.gameObject.GetComponent<EnemyBase>()._charStage == CHAR_STATE.DIE ? false : true;
                                trTargetAttack = hit2D_1.collider.gameObject.transform;
                                PrepareRotate_(trTargetAttack);
                            }
                        }
                        else
                        if (hit2D_1.collider.gameObject.GetComponent<HostageManager>() != null)
                        {
                            _isCanMoveToTarget = hit2D_1.collider.gameObject.GetComponent<HostageManager>()._charStage == CharsBase.CHAR_STATE.DIE ? false : true;
                            trTargetAttack = hit2D_1.collider.gameObject.transform;
                            PrepareRotate_(trTargetAttack);
                        }
                        else if (hit2D_1.collider.gameObject.GetComponent<PlayerManager>() != null)
                        {
                            _isCanMoveToTarget = hit2D_1.collider.gameObject.GetComponent<PlayerManager>().pState == PlayerManager.P_STATE.DIE ? false : true;
                            trTargetAttack = hit2D_1.collider.gameObject.transform;
                            PrepareRotate_(trTargetAttack);
                        }
                        else
                        {
                            _isCanMoveToTarget = false;
                        }

                        targetName = hit2D_1.collider.name;
                        if (_isCanMoveToTarget)
                        {
                            OnMoveToTarget();
                            isBeginAtt = true;
                        }
                    }
                }
                #endregion

                #region Check Hit Player
                _vStart = new Vector3(transform.localPosition.x - saPlayer.skeleton.ScaleX * 0.35f, transform.localPosition.y - 1.2f, transform.localPosition.z);
                _vEnd = new Vector3(_vStart.x - saPlayer.skeleton.ScaleX * 0.1f, _vStart.y, vStart.z);
                hitPlayer = Physics2D.Linecast(_vStart, _vEnd, lmPlayer);
                if (hitPlayer.collider != null)
                {
                    pmTarget = hitPlayer.collider.gameObject.GetComponent<PlayerManager>();
                    OnPrepareAttack();
                }
                #endregion
            }


            if (isBeginMove && _isCanMoveToTarget)
            {
                MoveToTarget();
            }
        }
    }

    public void PrepareRotate_(Transform _trTarget)
    {
        if (transform.localPosition.x > _trTarget.localPosition.x)
        {
            saPlayer.skeleton.ScaleX = 1;
        }
        else
        {
            saPlayer.skeleton.ScaleX = -1;
        }
    }

    public virtual void Update()
    {
    }
    public virtual void OnMoveToTarget()
    {
        switch (enemyType)
        {
            case ENEMY_TYPE.MELEE:
                PlayAnim(str_Run, true);
                isBeginMove = true;
                break;
            case ENEMY_TYPE.MONSTER:
                PlayAnim(str_Run, true);
                isBeginMove = true;
                break;
            case ENEMY_TYPE.RANGE:
                PlayAnim(str_Att, true);
                break;
        }
    }
    public virtual void OnPrepareAttack()
    {
        isContinueDetect = false;
        isBeginMove = false;
        rig.velocity = Vector2.zero;
        Debug.LogError("PrepareAttack: " + enemyType.ToString());
        if (enemyType == ENEMY_TYPE.RANGE)
        {
            PlayAnim(str_Att, true);
        }
        else
        {
            PlayAnim(str_idle, true);
            if (hitPlayer.collider != null)
            {
                if (hitPlayer.collider.gameObject.GetComponent<PlayerManager>().isTakeSword)
                {
                    hitPlayer.collider.gameObject.GetComponent<PlayerManager>().OnAttackEnemy(this);
                }
            }
        }
    }


    protected void MoveToTarget()
    {
        rig.velocity = moveSpeed * (saPlayer.skeleton.ScaleX > 0 ? Vector2.left : Vector2.right);
    }
    public void OnDie_()
    {
        if (_charStage != CHAR_STATE.DIE)
        {
            isContinueDetect = false;
            PlayAnim(str_Lose, false);
            _charStage = CHAR_STATE.DIE;
            MapLevelManager.Instance.lstAllEnemies.Remove(this);
            if (MapLevelManager.Instance.questType == MapLevelManager.QUEST_TYPE.KILL && MapLevelManager.Instance.lstAllEnemies.Count == 0)
            {
                PlayerManager.Instance.OnWin();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_charStage == CHAR_STATE.PLAYING)
        {
            if (isContinueDetect && collision.gameObject.name.Contains("Lava_Pr") && collision.gameObject.tag.Contains(Utils.TAG_TRAP))
            {
                OnDie_();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_charStage == CHAR_STATE.PLAYING)
        {
            if (isContinueDetect && collision.gameObject.tag.Contains(Utils.TAG_TRAP))
            {
                OnDie_();
            }
        }
    }
}

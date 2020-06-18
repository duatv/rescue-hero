using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;

public class EnemyBase : MonoBehaviour
{
    bool shoot;
    public enum ENEMY_TYPE { MELEE, RANGE, MONSTER }
    public enum CHAR_STATE { PLAYING, DIE, WIN, RUNNING }
    public GameObject skull;
    public ParticleSystem pBlood;
    [SerializeField]
    public ENEMY_TYPE enemyType;
    [SerializeField]
    public CHAR_STATE _charStage;
    [HideInInspector]
    public bool isReadOnly = false;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public SkeletonDataAsset sdaP1, sdaP2;
    public SkeletonAnimation saPlayer;
    //  [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    [SpineAnimation]
    public string str_idle, str_Win, str_Lose, str_Att, str_Run;
    public float moveSpeed;
    [SerializeField] public Rigidbody2D rig;
    public LayerMask lmColl, lmPlayer, lmMapObject;
    public bool isRangerAtt;
    public GameObject gGround, body, head;

    [SerializeField]
    private RaycastHit2D hit2D, hit2D_1, hitPlayer;
    bool hitDown;
    //  private bool isContinueDetect = true;
    private Vector2 vEnd, vStart, _vEnd, _vStart;
    //  private string targetName;
    private EnemyBase ebTarget;
    private HostageManager hmTarget;
    private PlayerManager pmTarget;
    //  private Transform trTargetAttack;

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
        {
            saPlayer.AnimationState.SetAnimation(0, anim_, isLoop);
        }
    }
    public Transform center, left, right, ground, leftAttack, rightAttack;
    public virtual void Start()
    {
        if (MapLevelManager.Instance != null)
        {
            MapLevelManager.Instance.lstAllEnemies.Add(this);
        }
       // saPlayer.AnimationState.Complete += OnComplete;
        saPlayer.AnimationState.Event += delegate
        {
          //  Debug.LogError(str_Att);
            if (saPlayer.AnimationName.Equals(str_Lose))
            {
                // gGround.layer = 0;
            }
            if (saPlayer.AnimationName.Equals(str_Att))
            {
                if (hitPlayer.collider != null)
                {
                    if (enemyType == ENEMY_TYPE.MELEE)
                    {
                        if (hitPlayer.collider.gameObject/*.GetComponent<PlayerManager>()*/.tag == "BodyPlayer")
                        {
                            PlayerManager.Instance/*hitPlayer.collider.gameObject.GetComponent<PlayerManager>()*/.OnPlayerDie(true);
                            GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
                            Debug.LogError("chem chet player");
                        }
                        else if (hitPlayer.collider.gameObject.name == "Hostage_Female"/*.collider.gameObject.GetComponent<HostageManager>()*/)
                        {
                            hitPlayer.collider.gameObject.GetComponent<HostageManager>().OnDie_(true);
                            GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
                        }
                    }


                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlaySound(SoundManager.Instance.acMeleeAttack);
                    }
                }
              //  Debug.LogError(str_Att);
                if (enemyType == ENEMY_TYPE.RANGE)
                {
                    GameObject arrow = ObjectPoolerManager.Instance.arrowPooler.GetPooledObject();
                    arrow.transform.position = saPlayer.skeleton.ScaleX > 0 ? leftAttack.transform.position : rightAttack.transform.position;
                    arrow.transform.localScale = new Vector2(saPlayer.skeleton.ScaleX, arrow.transform.localScale.y);
                    arrow.SetActive(true);
                    arrow.GetComponent<Rigidbody2D>().velocity = saPlayer.skeleton.ScaleX > 0 ? Vector2.left * 4 : Vector2.right * 4;
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlaySound(SoundManager.Instance.acMeleeAttack);
                    }
                   // Debug.LogError("zoooooooooooo");
                }
            }
        };
    }

    private void OnComplete(TrackEntry trackEntry)
    {
        Debug.LogError(str_Att);
        if (trackEntry.Animation.Name.Equals(str_Att))
        {
            Debug.LogError(str_Att);
            if (enemyType == ENEMY_TYPE.RANGE)
            {
                GameObject arrow = ObjectPoolerManager.Instance.arrowPooler.GetPooledObject();
                arrow.transform.position = saPlayer.skeleton.ScaleX > 0 ? leftAttack.transform.position : rightAttack.transform.position;
                arrow.transform.localScale = new Vector2(saPlayer.skeleton.ScaleX, arrow.transform.localScale.y);
                arrow.SetActive(true);
                arrow.GetComponent<Rigidbody2D>().velocity = saPlayer.skeleton.ScaleX > 0 ? Vector2.left : Vector2.right;
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.acMeleeAttack);
                }
                Debug.LogError("wtf");
            }
        }
    }

    private bool _isCanMoveToTarget;

    Vector2 _vStartHitDown, _vEndHitDown;
    private void HitDownMapObject()
    {
        _vStartHitDown = center.position;
        _vEndHitDown = ground.position;
        hitDown = Physics2D.OverlapCircle(ground.position, 0.05f, lmMapObject);

        Debug.DrawLine(_vStartHitDown, _vEndHitDown, Color.red);
    }
    public GameObject target;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(ground.position, 0.05f);
    }

    private Vector2 vStartForward, vEndForward;
    public virtual void FixedUpdate()
    {
        if (_charStage == CHAR_STATE.PLAYING)
        {
            #region Check Hit Down
            HitDownMapObject();
            #endregion
            if (hitDown)
            {
                #region Check Hit Ahead

                vStart = center.position;
                vEnd = left.position;

                vStartForward = center.position;
                vEndForward = right.position;

                Debug.DrawLine(vStart, vEnd, Color.red);
                Debug.DrawLine(vStartForward, vEndForward, Color.green);

                hit2D_1 = Physics2D.Linecast(vStartForward, vEndForward, lmColl);
                hit2D = Physics2D.Linecast(vStart, vEnd, lmColl);

                if (hitPlayer.collider == null)
                {
                    if (hit2D.collider != null)
                    {
                        //    Debug.LogError("checkhit1" + hit2D.collider.tag);
                        if (hit2D.collider.gameObject.tag != Utils.TAG_STICKBARRIE && hit2D.collider.gameObject.tag != "Chan"/* && !isBeginAtt*/)
                        {
                            //if (hit2D.collider.gameObject.GetComponent<EnemyBase>() != null)
                            //{
                            //    if (hit2D.collider.gameObject.GetComponent<EnemyBase>().isRangerAtt)
                            //    {
                            //        _isCanMoveToTarget = hit2D.collider.gameObject.GetComponent<EnemyBase>()._charStage == CHAR_STATE.DIE ? false : true;
                            //        trTargetAttack = hit2D.collider.gameObject.transform;
                            //    }
                            //}
                            // else
                            if (/*hit2D.collider.gameObject.GetComponent<HostageManager>() != null*/hit2D.collider.gameObject.name == "Hostage_Female")
                            {
                                _isCanMoveToTarget = hit2D.collider.gameObject.GetComponent<HostageManager>()._charStage == CharsBase.CHAR_STATE.DIE ? false : true;
                                target = hit2D.collider.gameObject;
                                //    trTargetAttack = hit2D.collider.gameObject.transform;
                            }
                            else if (/*hit2D.collider.gameObject.GetComponent<PlayerManager>() != null*/hit2D.collider.gameObject.tag == "BodyPlayer" /*&& !PlayerManager.Instance.isTakeSword*/)
                            {
                                _isCanMoveToTarget = /*hit2D.collider.gameObject.GetComponent<PlayerManager>()*/PlayerManager.Instance.pState == PlayerManager.P_STATE.DIE ? false : true;

                                //  trTargetAttack = hit2D.collider.gameObject.transform;
                                target = hit2D.collider.gameObject;
                            }
                            else
                            {
                                _isCanMoveToTarget = false;
                            }

                            //   targetName = hit2D.collider.name;
                            if (_isCanMoveToTarget)
                            {
                                OnMoveToTarget();
                                //  isBeginAtt = true;
                            }
                        }
                    }
                    if (hit2D_1.collider != null)
                    {
                        //   Debug.LogError("checkhit2" + hit2D_1.collider.tag);
                        if (hit2D_1.collider.gameObject.tag != Utils.TAG_STICKBARRIE/* && !isBeginAtt*/&& hit2D_1.collider.gameObject.tag != "Chan")
                        {
                            //if (hit2D_1.collider.gameObject.GetComponent<EnemyBase>() != null)
                            //{
                            //    if (hit2D_1.collider.gameObject.GetComponent<EnemyBase>().isRangerAtt)
                            //    {
                            //        _isCanMoveToTarget = hit2D_1.collider.gameObject.GetComponent<EnemyBase>()._charStage == CHAR_STATE.DIE ? false : true;
                            //        trTargetAttack = hit2D_1.collider.gameObject.transform;
                            //        PrepareRotate_(trTargetAttack);
                            //    }
                            //}
                            // else
                            if (/*hit2D_1.collider.gameObject.GetComponent<HostageManager>() != null*/hit2D_1.collider.gameObject.name == "Hostage_Female")
                            {
                                _isCanMoveToTarget = hit2D_1.collider.gameObject.GetComponent<HostageManager>()._charStage == CharsBase.CHAR_STATE.DIE ? false : true;
                                //   trTargetAttack = hit2D_1.collider.gameObject.transform;
                                //    PrepareRotate_(trTargetAttack);
                                target = hit2D_1.collider.gameObject;
                            }
                            else if (/*hit2D_1.collider.gameObject.GetComponent<PlayerManager>() != null*/hit2D_1.collider.gameObject.tag == "BodyPlayer" /*&& !PlayerManager.Instance.isTakeSword*/)
                            {
                                _isCanMoveToTarget =/* hit2D_1.collider.gameObject.GetComponent<PlayerManager>()*/PlayerManager.Instance.pState == PlayerManager.P_STATE.DIE ? false : true;
                                //   trTargetAttack = hit2D_1.collider.gameObject.transform;
                                //  PrepareRotate_(trTargetAttack);
                                target = hit2D_1.collider.gameObject;
                            }
                            else
                            {
                                _isCanMoveToTarget = false;
                            }

                            //   targetName = hit2D_1.collider.name;
                            if (_isCanMoveToTarget)
                            {
                                OnMoveToTarget();
                                Debug.LogError("move");
                                //    isBeginAtt = true;
                            }
                        }
                    }
                }

                #endregion

                #region Check Hit Player
                _vStart = center.position;
                _vEnd = saPlayer.skeleton.ScaleX < 0 ? new Vector2(rightAttack.transform.position.x, rightAttack.transform.position.y) : new Vector2(leftAttack.transform.position.x, leftAttack.transform.position.y);
                hitPlayer = Physics2D.Linecast(_vStart, _vEnd, lmPlayer);
                Debug.DrawLine(_vStart, _vEnd, Color.yellow);
                if (hitPlayer.collider != null)
                {
                    Debug.LogError("phath ien player");
                    OnPrepareAttack();
                }
                #endregion
            }

        }
    }

    //public void PrepareRotate_(Transform _trTarget)
    //{
    //    if (transform.localPosition.x > _trTarget.localPosition.x)
    //    {
    //        saPlayer.skeleton.ScaleX = 1;
    //    }
    //    else
    //    {
    //        saPlayer.skeleton.ScaleX = -1;
    //    }
    //}


    public virtual void OnMoveToTarget()
    {

        switch (enemyType)
        {
            case ENEMY_TYPE.MELEE:
                rig.velocity = moveSpeed * (saPlayer.skeleton.ScaleX > 0 ? Vector2.left : Vector2.right);
                PlayAnim(str_Run, true);
                break;
            case ENEMY_TYPE.MONSTER:
                PlayAnim(str_Run, true);
                break;
            case ENEMY_TYPE.RANGE:
                PlayAnim(str_Att, false);
                break;
        }
        if (target != null)
        {
            if (transform.position.x > target.transform.position.x)
            {
                saPlayer.skeleton.ScaleX = 1;
            }
            else
            {
                saPlayer.skeleton.ScaleX = -1;
            }
        }


    }
    public virtual void OnPrepareAttack()
    {
        // isContinueDetect = false;

        rig.velocity = Vector2.zero;
        if (enemyType == ENEMY_TYPE.RANGE)
        {
            if (hitPlayer.collider != null)
            {
                if (/*hitPlayer.collider.gameObject.GetComponent<PlayerManager>()*/PlayerManager.Instance.isTakeSword)
                    PlayAnim(str_Att, false);
            }
        }
        else
        {
            if (hitPlayer.collider != null)
            {
                //if (hitPlayer.collider.gameObject.GetComponent<PlayerManager>() != null)
                //{
                if (/*hitPlayer.collider.gameObject.GetComponent<PlayerManager>()*/PlayerManager.Instance.isTakeSword)
                {
                    /*hitPlayer.collider.gameObject.GetComponent<PlayerManager>()*/
                    PlayerManager.Instance.OnAttackEnemy(this);
                    PlayAnim(str_idle, true);
                 //   Debug.LogError("chem");
                }
                else
                {
                    PlayAnim(str_Att, false);
                 //   Debug.LogError("ko chem");
                }
                // }
                //else PlayAnim(str_Att, false);
            }
            else
            {
                PlayAnim(str_idle, true);
            }
        }
    }

    public virtual void OnDie_()
    {
        if (_charStage != CHAR_STATE.DIE)
        {
            //    isContinueDetect = false;
            PlayAnim(str_Lose, false);
            _charStage = CHAR_STATE.DIE;
            rig.velocity = Vector2.zero;
            rig.constraints = RigidbodyConstraints2D.FreezePositionX;
            transform.rotation = Quaternion.identity;
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.SetActive(false);
            head.SetActive(false);
            pBlood.Play();
            skull.SetActive(true);
            GameManager.Instance.enemyKill++;
            MapLevelManager.Instance.lstAllEnemies.Remove(this);

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.acEnemyDie);
            }


            if (MapLevelManager.Instance.questType == MapLevelManager.QUEST_TYPE.KILL && MapLevelManager.Instance.lstAllEnemies.Count == 0)
            {
                PlayerManager.Instance.OnWin(false);
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (_charStage == CHAR_STATE.PLAYING)
    //    {
    //        if (/*isContinueDetect && */collision.gameObject.tag == Utils.TAG_LAVA)
    //        {
    //            OnDie_();
    //        }
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_charStage == CHAR_STATE.PLAYING)
        {
            if (/*isContinueDetect && */collision.gameObject.tag == Utils.TAG_LAVA || collision.gameObject.tag == "Trap_Other" || collision.gameObject.tag == Utils.TAG_GAS || collision.gameObject.tag == "arrow")
            {
                OnDie_();
                if (collision.gameObject.tag == Utils.TAG_LAVA)
                {
                    if (ObjectPoolerManager.Instance == null)
                        return;
                    GameObject destroyEffect = ObjectPoolerManager.Instance.effectDestroyPooler.GetPooledObject();
                    destroyEffect.transform.position = transform.position;
                    destroyEffect.SetActive(true);
                }
                else if (collision.gameObject.tag == "arrow")
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }
}

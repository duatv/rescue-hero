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
    public string str_idle, str_Win, str_Lose, strAtt, strRun;
    public float moveSpeed;
    [SerializeField] public Rigidbody2D rig;
    public LayerMask lmColl, lmPlayer;
    public bool isRangerAtt;

    [SerializeField]
    private RaycastHit2D hit2D, hitPlayer;
    private bool isContinueDetect = true;
    private Vector3 vEnd, vStart, _vEnd, _vStart;
    private bool isBeginAtt, isBeginMove;
    private string targetName;
    private EnemyBase ebTarget;
    private HostageManager hmTarget;
    private PlayerManager pmTarget;

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
    private void Start()
    {
        if (MapLevelManager.Instance != null) {
            MapLevelManager.Instance.lstAllEnemies.Add(this);
        }
    }
    private bool _isCanMoveToTarget;
    public virtual void FixedUpdate()
    {
        if (_charStage == CHAR_STATE.PLAYING)
        {
            #region Check Hit Ahead
            vStart = new Vector3(transform.localPosition.x - saPlayer.skeleton.ScaleX * 0.35f, transform.localPosition.y - 1.2f, transform.localPosition.z);
            vEnd = new Vector3(vStart.x - saPlayer.skeleton.ScaleX * 2f, vStart.y, vStart.z);
            hit2D = Physics2D.Linecast(vStart, vEnd, lmColl);
            if (hit2D.collider != null)
            {
                if (!hit2D.collider.gameObject.tag.Contains(Utils.TAG_STICKBARRIE) && !isBeginAtt)
                {
                    if (hit2D.collider.gameObject.GetComponent<EnemyBase>() != null)
                    {
                        if(hit2D.collider.gameObject.GetComponent<EnemyBase>().isRangerAtt)
                            _isCanMoveToTarget = hit2D.collider.gameObject.GetComponent<EnemyBase>()._charStage == CHAR_STATE.DIE ? false : true;
                    }
                    else 
                    if (hit2D.collider.gameObject.GetComponent<HostageManager>() != null)
                    {
                        _isCanMoveToTarget = hit2D.collider.gameObject.GetComponent<HostageManager>()._charStage == CharsBase.CHAR_STATE.DIE ? false : true;
                    }
                    else if (hit2D.collider.gameObject.GetComponent<PlayerManager>() != null)
                    {
                        _isCanMoveToTarget = hit2D.collider.gameObject.GetComponent<PlayerManager>().pState == PlayerManager.P_STATE.DIE ? false : true;
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
            #endregion
            _vStart = new Vector3(transform.localPosition.x - saPlayer.skeleton.ScaleX * 0.35f, transform.localPosition.y - 1.2f, transform.localPosition.z);
            _vEnd = new Vector3(_vStart.x - saPlayer.skeleton.ScaleX * 0.1f, _vStart.y, vStart.z);
            Debug.DrawLine(_vStart, _vEnd, Color.red);
            hitPlayer = Physics2D.Linecast(_vStart, _vEnd, lmPlayer);
            if (hitPlayer.collider != null) {
                OnPrepareAttack();
            }


            if (isBeginMove && _isCanMoveToTarget)
            {
                MoveToTarget();
            }
        }
    }
    public virtual void Update()
    {
        if (_charStage == CHAR_STATE.PLAYING)
        {
            if (!isBeginMove)
            {
                if (PlayerManager.Instance != null)
                {
                    if (transform.localPosition.x > PlayerManager.Instance.transform.localPosition.x)
                    {
                        saPlayer.skeleton.ScaleX = 1;
                    }
                    else saPlayer.skeleton.ScaleX = -1;
                }
            }
        }
    }
    public virtual void OnMoveToTarget()
    {
        switch (enemyType)
        {
            case ENEMY_TYPE.MELEE:
                PlayAnim(strRun, true);
                isBeginMove = true;
                break;
            case ENEMY_TYPE.MONSTER:
                PlayAnim(strRun, true);
                isBeginMove = true;
                break;
            case ENEMY_TYPE.RANGE:
                PlayAnim(strAtt, true);
                break;
        }
    }
    public virtual void OnPrepareAttack()
    {
        isContinueDetect = false;
        isBeginMove = false;
        rig.velocity = Vector2.zero;
        PlayAnim(strAtt, true);
    }
    protected void MoveToTarget()
    {
        rig.velocity = moveSpeed * (saPlayer.skeleton.ScaleX > 0 ? Vector2.left : Vector2.right);
    }
    public void OnDie_()
    {
        if (_charStage != CHAR_STATE.DIE) {
            isContinueDetect = false;
            PlayAnim(str_Lose, false);
            _charStage = CHAR_STATE.DIE;
            MapLevelManager.Instance.lstAllEnemies.Remove(this);
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
            //if (isContinueDetect && collision.gameObject.GetComponent<PlayerManager>()!= null) {
            //    OnPrepareAttack();
            //}
        }
    }
}

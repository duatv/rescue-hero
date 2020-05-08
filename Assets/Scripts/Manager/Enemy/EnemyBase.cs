using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyBase : MonoBehaviour
{
    public enum ENEMY_TYPE { MELEE, RANGE, MONSTER}
    public enum CHAR_STATE { PLAYING, DIE, WIN, RUNNING }

    public ENEMY_TYPE enemyType;
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
    public Rigidbody2D rig;
    public LayerMask lmColl;

    private RaycastHit2D hit2D;
    private bool isContinueDetect = true;
    private Vector3 vEnd, vStart;
    private bool isBeginAtt, isBeginMove;
    private string targetName;

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
        saPlayer.AnimationState.SetAnimation(0, anim_, isLoop);
    }

    public virtual void OnEnable()
    {
    }
    public virtual void FixedUpdate()
    {
        vStart = new Vector3(transform.localPosition.x - saPlayer.skeleton.ScaleX * 0.35f, transform.localPosition.y - 1.5f, transform.localPosition.z);
        vEnd = new Vector3(vStart.x - saPlayer.skeleton.ScaleX * 2f, vStart.y, vStart.z);
        Debug.DrawLine(vStart, vEnd, Color.green);
        hit2D = Physics2D.Linecast(vStart, vEnd,lmColl);
        if (hit2D.collider != null) {
            if (!hit2D.collider.gameObject.tag.Contains(Utils.TAG_STICKBARRIE)&& !isBeginAtt)
            {
                Debug.LogError("Begin Attack-----> " + hit2D.collider.name);
                targetName = hit2D.collider.name;
                OnAttack();
                isBeginAtt = true;
            }
        }

        if (isBeginMove) {
            MoveToTarget();
        }
    }
    public virtual void Update()
    {
        if (!isBeginMove) {
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
    public virtual void OnAttack()
    {
        switch (enemyType) {
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
    protected void MoveToTarget()
    {
        rig.velocity = moveSpeed * (saPlayer.skeleton.ScaleX > 0 ? Vector2.left : Vector2.right);
    }
    public void OnDie_()
    {
        isContinueDetect = false;
        PlayAnim(str_Lose, false);
        _charStage = CHAR_STATE.DIE;

        Physics2D.IgnoreLayerCollision(15, 11);
        Physics2D.IgnoreLayerCollision(15, 14);
        Physics2D.IgnoreLayerCollision(15, 9);
        Physics2D.IgnoreLayerCollision(15, 4);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isContinueDetect && collision.gameObject.name.Contains("Lava_Pr") && collision.gameObject.tag.Contains(Utils.TAG_TRAP))
        {
            OnDie_();
        }
    }
}

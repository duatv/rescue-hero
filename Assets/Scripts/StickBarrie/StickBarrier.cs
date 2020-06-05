using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class StickBarrier : MonoBehaviour
{
    public bool key;
    const float speedAdd = 2;
    public GameObject Head;
    public enum MOVETYPE { RIGHT, LEFT, UP, DOWN, FREE }
    [SerializeField] public MOVETYPE _moveType;
    [SerializeField] public Rigidbody2D _rig2D;
    [Range(0, 10)] [SerializeField] public float moveSpeed = 2;
    [SerializeField] public bool hasBlockGems;

    [HideInInspector]
    public bool isMove2Pos;
    [DrawIf("_moveType", MOVETYPE.FREE, ComparisonType.Equals, DisablingType.DontDraw)]
    [SerializeField] public Vector2 vEndPos;
    [SerializeField] public Vector2 vStartPos;
    [SerializeField] private Vector2 vUpPos, vDownPos, vLeftPos, vRightPos;
    [HideInInspector] public bool beginMove;
    [HideInInspector] public bool moveBack;

    //[HideInInspector]
    //public float time_ = 2;

    private void OnValidate()
    {
        if(_rig2D == null)
        {
            _rig2D = GetComponent<Rigidbody2D>();
        }
    }
    //private void OnMouseDown()
    //{
    //    if (!beginMove)
    //    {
    //        if (SoundManager.Instance != null)
    //        {
    //            if (_moveType == MOVETYPE.FREE)
    //            {
    //                SoundManager.Instance.PlaySound(SoundManager.Instance.acMoveStickClick);
    //                        if (GameManager.Instance.mapLevel.lstAllStick.Contains(this))
    //        GameManager.Instance.mapLevel.lstAllStick.Remove(this);
    //            }
    //            else
    //                SoundManager.Instance.PlaySound(SoundManager.Instance.acMoveStick);
    //        }
    //    }
    //    beginMove = true;



    //}
    void Start()
    {

      //  MapLevelManager.Instance.lstAllStick.Add(this);
        if (_moveType != MOVETYPE.FREE) {
            vStartPos = (Vector2)transform.localPosition;
        }
    }

    //private void PrepareBlockGem() {
    //    if (time_ > 0) {
    //        time_ -= Time.deltaTime;
    //    }
    //}

    private void OnBecameInvisible()
    {
        if (GameManager.Instance.gameState == GameManager.GAMESTATE.PLAYING)
        {
            if (!moveBack)
            {
                hasBlockGems = false;
                gameObject.SetActive(false);

                if (GameManager.Instance.mapLevel.lstAllStick.Contains(this))
                    GameManager.Instance.mapLevel.lstAllStick.Remove(this);
            }
            //Debug.LogError("Count:" + GameManager.Instance.mapLevel.lstAllStick.Count);
            //if (GameManager.Instance.mapLevel.lstAllStick.Count <= 0)
            //{
            //    PlayerManager.Instance.PrepareRotate();
            //}

        }

    }

    private void MoveStick(Vector2 endPos) {


        transform.localPosition = Vector2.Lerp(transform.localPosition, endPos, Time.deltaTime * moveSpeed);
    }

    [SerializeField]Vector2 dir;
    private void MoveStick2Pos(Vector2 endPos) {
        //dir = (endPos - (Vector2)transform.position).normalized;
        //_rig2D.MovePosition((Vector2)transform.position + dir * (moveSpeed * Time.fixedDeltaTime));
        transform.position = Vector2.Lerp(transform.position, endPos, Time.deltaTime * moveSpeed);
    }
    private void MoveStickBarrie()
    {
        switch (_moveType)
        {
            case MOVETYPE.FREE:
                if (isMove2Pos) {
                    if (beginMove && !moveBack) {
                        if (Vector2.Distance(transform.position, vEndPos) > 0.03f/*transform.position.x != vEndPos.x || transform.position.y != vEndPos.y*/)
                            MoveStick2Pos(vEndPos);
                        else
                        {
                            _rig2D.velocity = Vector2.zero;
                            beginMove = false;
                            moveBack = true;
                        }
                    }
                    if (moveBack && beginMove) {
                        if (Vector2.Distance(transform.position, vStartPos) > 0.03f/*transform.position.x != vStartPos.x || transform.position.y != vStartPos.y*/)
                            MoveStick2Pos(vStartPos);
                        else
                        {
                            _rig2D.velocity = Vector2.zero;
                            beginMove = false;
                            moveBack = false;
                        }
                    }
                }
                else
                {
                    if (beginMove && !moveBack)
                    {
                        if (Vector2.Distance(transform.localPosition, vEndPos) > 0.003f /*transform.localPosition.x != vEndPos.x || transform.localPosition.y != vEndPos.y*/)
                        {
                            MoveStick(vEndPos);
                        }
                        else
                        {
                            beginMove = false;
                            moveBack = true;
                        }
                    }

                    if (moveBack && beginMove)
                    {
                        if (Vector2.Distance(transform.localPosition, vStartPos) > 0.003f/*transform.localPosition.x != vStartPos.x || transform.localPosition.y != vStartPos.y*/)
                        {
                            MoveStick(vStartPos);
                        }
                        else
                        {
                            beginMove = false;
                            moveBack = false;
                        }
                    }
                }
                break;
            case MOVETYPE.LEFT:
                if (beginMove && !moveBack)
                {
                    _rig2D.velocity = Vector2.left * moveSpeed * speedAdd;
                }

                if (moveBack && beginMove)
                {
                    if (Vector2.Distance(transform.localPosition, vStartPos) > 0.003f)
                    {
                        MoveStick(vStartPos);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                break;
            case MOVETYPE.RIGHT:
                if (beginMove && !moveBack)
                {
                    _rig2D.velocity = Vector2.right * moveSpeed * speedAdd;
                }

                if (moveBack && beginMove)
                {
                    if (Vector2.Distance(transform.localPosition, vStartPos) > 0.003f)
                    {
                        MoveStick(vStartPos);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                break;
            case MOVETYPE.UP:
                if (beginMove && !moveBack)
                {
                    _rig2D.velocity = Vector2.up * moveSpeed * speedAdd;
                }

                if (moveBack && beginMove)
                {
                    if (Vector2.Distance(transform.localPosition, vStartPos) > 0.003f)
                    {
                        MoveStick(vStartPos);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                break;
            case MOVETYPE.DOWN:
                if (beginMove && !moveBack)
                {
                    _rig2D.velocity = Vector2.down * moveSpeed * speedAdd;
                }

                if (moveBack && beginMove)
                {
                    if (Vector2.Distance(transform.localPosition, vStartPos) > 0.003f)
                    {
                        MoveStick(vStartPos);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (beginMove)
            MoveStickBarrie();
    }
    //private void Update()
    //{
    //    PrepareBlockGem();
    //}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains(Utils.TAG_WIN))
        {
            if (!hasBlockGems) hasBlockGems = true;
        }
    }

    #region Editor
    public void SaveEndPos()
    {
        if (isMove2Pos)
        {
            vEndPos = transform.localPosition/*position*/;
            if (vStartPos == Vector2.zero)
            {
                vStartPos = vEndPos;
            }
            transform.localPosition/*position*/ = vStartPos;
        }
        else {
            vEndPos = transform.localPosition;
            if (vStartPos == Vector2.zero)
            {
                vStartPos = vEndPos;
            }
            transform.localPosition = vStartPos;
        }

    }

    public void SaveStartPos()
    {
        if (isMove2Pos)
        {
            vStartPos = transform.localPosition/*position*/;
            transform.localPosition/*position*/ = vStartPos;
        }
        else {
            vStartPos = transform.localPosition;
            transform.localPosition = vStartPos;
        }
    }
    #endregion
}
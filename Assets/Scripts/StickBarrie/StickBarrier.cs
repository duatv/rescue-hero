using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class StickBarrier : MonoBehaviour
{
    const float speedAdd = 2;

    public enum MOVETYPE { RIGHT, LEFT, UP, DOWN, FREE }
    [SerializeField] public MOVETYPE _moveType;
    [SerializeField] public Rigidbody2D _rig2D;
    [Range(0,4)]
    [SerializeField] public float moveSpeed = 2;
    [SerializeField] public bool hasBlockGems;

    [HideInInspector]
    public bool isMove2Pos;
    [DrawIf("_moveType", MOVETYPE.FREE, ComparisonType.Equals, DisablingType.DontDraw)]
    [SerializeField] public Vector3 vEndPos;
    [SerializeField] public Vector3 vStartPos;
    [SerializeField] private Vector3 vUpPos, vDownPos, vLeftPos, vRightPos;

    [HideInInspector] public bool beginMove;
    [HideInInspector] public bool moveBack;

    [HideInInspector]
    public float time_ = 2;

    void Start()
    {
        _rig2D = GetComponent<Rigidbody2D>();
        MapLevelManager.Instance.lstAllStick.Add(this);
        if (_moveType != MOVETYPE.FREE) {
            vStartPos = transform.localPosition;
        }
        Debug.LogError("-----> "+Vector3.Distance(vStartPos, vEndPos));
    }

    private void PrepareBlockGem() {
        if (time_ > 0) {
            time_ -= Time.deltaTime;
        }
    }

    private void OnBecameInvisible()
    {
        hasBlockGems = false;
        gameObject.SetActive(false);
    }

    private void MoveStick(Vector3 endPos) {
        transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, Time.deltaTime * moveSpeed);
    }

    [SerializeField]Vector3 dir;
    private void MoveStick2Pos(Vector3 endPos) {
        dir = (endPos - transform.position).normalized;
        _rig2D.MovePosition(transform.position + dir * (moveSpeed * Time.fixedDeltaTime));
    }
    private void MoveStickBarrie()
    {
        switch (_moveType)
        {
            case MOVETYPE.FREE:
                if (isMove2Pos) {
                    if (beginMove && !moveBack) {
                        if (Vector3.Distance(transform.position, vEndPos) > 0.03f)
                            MoveStick2Pos(vEndPos);
                        else
                        {
                            _rig2D.velocity = Vector2.zero;
                            beginMove = false;
                            moveBack = true;
                        }
                    }
                    if (moveBack && beginMove) {
                        if (Vector3.Distance(transform.position, vStartPos) > 0.03f)
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
                        if (Vector3.Distance(transform.localPosition, vEndPos) > 0.003f)
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
                        if (Vector3.Distance(transform.localPosition, vStartPos) > 0.003f)
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
                    if (Vector3.Distance(transform.localPosition, vStartPos) > 0.003f)
                    {
                        MoveStick(vStartPos);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                //_rig2D.velocity = Vector2.left * moveSpeed * speedAdd;
                break;
            case MOVETYPE.RIGHT:
                if (beginMove && !moveBack)
                {
                    _rig2D.velocity = Vector2.right * moveSpeed * speedAdd;
                }

                if (moveBack && beginMove)
                {
                    if (Vector3.Distance(transform.localPosition, vStartPos) > 0.003f)
                    {
                        MoveStick(vStartPos);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                //_rig2D.velocity = Vector2.right * moveSpeed * speedAdd;
                break;
            case MOVETYPE.UP:
                if (beginMove && !moveBack)
                {
                    _rig2D.velocity = Vector2.up * moveSpeed * speedAdd;
                }

                if (moveBack && beginMove)
                {
                    if (Vector3.Distance(transform.localPosition, vStartPos) > 0.003f)
                    {
                        MoveStick(vStartPos);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                //_rig2D.velocity = Vector2.up * moveSpeed * speedAdd;
                break;
            case MOVETYPE.DOWN:
                if (beginMove && !moveBack)
                {
                    _rig2D.velocity = Vector2.down * moveSpeed * speedAdd;
                }

                if (moveBack && beginMove)
                {
                    if (Vector3.Distance(transform.localPosition, vStartPos) > 0.003f)
                    {
                        MoveStick(vStartPos);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                //_rig2D.velocity = Vector2.down * moveSpeed * speedAdd;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (beginMove)
            MoveStickBarrie();
    }
    private void Update()
    {
        PrepareBlockGem();
    }


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
            vEndPos = transform./*localPosition*/position;
            if (vStartPos == Vector3.zero)
            {
                vStartPos = vEndPos;
            }
            transform./*localPosition*/position = vStartPos;
        }
        else {
            vEndPos = transform.localPosition;
            if (vStartPos == Vector3.zero)
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
            vStartPos = transform./*localPosition*/position;
            transform./*localPosition*/position = vStartPos;
        }
        else {
            vStartPos = transform.localPosition;
            transform.localPosition = vStartPos;
        }
    }
    #endregion
}
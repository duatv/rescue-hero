using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickBarrier : MonoBehaviour
{
    public enum MOVETYPE { RIGHT, LEFT, UP, DOWN, FREE }
    public MOVETYPE _moveType;
    public Rigidbody2D _rig2D;
    public float moveSpeed = 2;
    public bool hasBlockGems;

    [DrawIf("_moveType", MOVETYPE.FREE, ComparisonType.Equals, DisablingType.DontDraw)]
    public Vector3 vEndPos, vStartPos;
    private Vector3 vUpPos, vDownPos, vLeftPos, vRightPos;
    
    [HideInInspector]
    public bool beginMove;
    private bool moveBack;

    [HideInInspector]
    public float time_ = 2;

    void Start()
    {
        _rig2D = GetComponent<Rigidbody2D>();
        //if (MapLevelManager.Instance != null) {
        //    if (MapLevelManager.Instance.lstAllStick.Contains(this)) {
        //        MapLevelManager.Instance.lstAllStick.Add(this);
        //    }
        //}
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

    private void OnMouseDown()
    {
        beginMove = true;
    }
    public void OnMouseDrag()
    {
        beginMove = true;
    }
    bool playerBeginMove = false;
    private void PlayerBeginMove() {
        if (PlayerManager.Instance != null) {
            if (!playerBeginMove)
            {
                PlayerManager.Instance.OnBeginRun();
                playerBeginMove = true;
            }
        }
    }
    private void MoveStick(Vector3 endPos) {
        transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, Time.deltaTime * moveSpeed);
    }
    private void MoveStickBarrie()
    {
        switch (_moveType)
        {
            case MOVETYPE.FREE:
                if (beginMove && !moveBack) {
                    if (Vector3.Distance(transform.localPosition, vEndPos) > 0.003f)
                    {
                        //transform.localPosition = Vector3.Lerp(transform.localPosition, vEndPos, Time.deltaTime * moveSpeed);
                        MoveStick(vEndPos);
                        if (hasBlockGems)
                            PlayerBeginMove();
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
                        //transform.localPosition = Vector3.Lerp(transform.localPosition, vStartPos, Time.deltaTime * moveSpeed);
                        MoveStick(vStartPos);
                        if (hasBlockGems)
                            PlayerBeginMove();
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                break;
            case MOVETYPE.LEFT:
                _rig2D.velocity = Vector2.left * moveSpeed * speedAdd;
                //vDownPos = new Vector3(transform.localPosition.x - 5, transform.localPosition.y, transform.localPosition.z);
                //MoveStick(vDownPos);

                if (hasBlockGems)
                    PlayerBeginMove();
                break;
            case MOVETYPE.RIGHT:
                _rig2D.velocity = Vector2.right *  moveSpeed * speedAdd;
                //vDownPos = new Vector3(transform.localPosition.x + 5, transform.localPosition.y, transform.localPosition.z);
                //MoveStick(vDownPos);
                if (hasBlockGems)
                    PlayerBeginMove();
                break;
            case MOVETYPE.UP:
                _rig2D.velocity = Vector2.up *  moveSpeed * speedAdd;

                //vUpPos = new Vector3(transform.localPosition.x+5, transform.localPosition.y, transform.localPosition.z);
                //MoveStick(vUpPos);

                if (hasBlockGems)
                    PlayerBeginMove();
                break;
            case MOVETYPE.DOWN:
                _rig2D.velocity = Vector2.down * moveSpeed * speedAdd;
                //vDownPos = new Vector3(transform.localPosition.x+5, transform.localPosition.y, transform.localPosition.z);
                //MoveStick(vDownPos);

                if (hasBlockGems)
                    PlayerBeginMove();
                break;
        }
    }
    const float speedAdd = 2;
    private void /*Fixed*/Update()
    {
        PrepareBlockGem();

        if (beginMove)
            MoveStickBarrie();
    }
    #region Editor
    public void SaveEndPos()
    {
        vEndPos = transform.localPosition;
        if (vStartPos == Vector3.zero)
        {
            vStartPos = vEndPos;
        }
        transform.localPosition = vStartPos;
    }

    public void SaveStartPos()
    {
        vStartPos = transform.localPosition;
        transform.localPosition = vStartPos;
    }
    #endregion
}
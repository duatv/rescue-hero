﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickBarrier : MonoBehaviour
{
    const float speedAdd = 2;

    public enum MOVETYPE { RIGHT, LEFT, UP, DOWN, FREE }
    public MOVETYPE _moveType;
    public Rigidbody2D _rig2D;
    public float moveSpeed = 2;
    public bool hasBlockGems;

    [DrawIf("_moveType", MOVETYPE.FREE, ComparisonType.Equals, DisablingType.DontDraw)]
    public Vector3 vEndPos, vStartPos;
    private Vector3 vUpPos, vDownPos, vLeftPos, vRightPos;

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
    private void MoveStickBarrie()
    {
        switch (_moveType)
        {
            case MOVETYPE.FREE:
                if (beginMove && !moveBack) {
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

    private void Update()
    {
        PrepareBlockGem();

        if (beginMove)
            MoveStickBarrie();
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
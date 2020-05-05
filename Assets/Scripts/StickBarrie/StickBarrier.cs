using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickBarrier : MonoBehaviour
{
    public enum MOVETYPE { RIGHT, LEFT, UP, DOWN, FREE }
    public MOVETYPE _moveType;
    public Rigidbody2D _rig2D;
    public float moveSpeed = 2;

    [DrawIf("_moveType", MOVETYPE.FREE, ComparisonType.Equals, DisablingType.DontDraw)]
    public Vector3 vEndPos, vStartPos;
    
    private bool beginMove;
    private bool moveBack;

    void Start()
    {
        _rig2D = GetComponent<Rigidbody2D>();
    }
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

    private void OnMouseDown()
    {
        beginMove = true;
    }
    public void OnMouseDrag()
    {
        beginMove = true;
    }

    private void MoveStickBarrie()
    {
        switch (_moveType)
        {
            case MOVETYPE.DOWN:
                _rig2D.velocity = Vector2.down;
                break;
            case MOVETYPE.FREE:
                if (beginMove && !moveBack) {
                    if (Vector3.Distance(transform.localPosition, vEndPos) > 0.001f)
                    {
                        transform.localPosition = Vector3.Lerp(transform.localPosition, vEndPos, Time.deltaTime * moveSpeed);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = true;
                    }
                }

                if (moveBack && beginMove)
                {
                    if (Vector3.Distance(transform.localPosition, vStartPos) > 0.001f)
                    {
                        transform.localPosition = Vector3.Lerp(transform.localPosition, vStartPos, Time.deltaTime * moveSpeed);
                    }
                    else
                    {
                        beginMove = false;
                        moveBack = false;
                    }
                }
                break;
            case MOVETYPE.LEFT:
                _rig2D.velocity = Vector2.left * moveSpeed ;
                break;
            case MOVETYPE.RIGHT:
                _rig2D.velocity = Vector2.right *  moveSpeed ;
                break;
            case MOVETYPE.UP:
                _rig2D.velocity = Vector2.up *  moveSpeed;
                break;
        }
    }

    private void /*Fixed*/Update()
    {
        if (beginMove)
            MoveStickBarrie();
    }
}
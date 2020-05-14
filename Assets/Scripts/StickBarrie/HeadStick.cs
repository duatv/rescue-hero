using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadStick : MonoBehaviour
{
    public StickBarrier stickBarrier;
    private bool continueDetect = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!continueDetect && collision.gameObject.tag.Contains(Utils.TAG_STICKBARRIE)) {
            if (stickBarrier.beginMove) {
                stickBarrier._rig2D.velocity = Vector2.zero;
                stickBarrier.moveBack = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!continueDetect && collision.gameObject.tag.Contains(Utils.TAG_STICKBARRIE))
        {
            continueDetect = false;
        }
    }
}
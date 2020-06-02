using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPlayer : MonoBehaviour
{
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING/* || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING*/)
    //    {
    //        if (/*PlayerManager.Instance.isContinueDetect &&*/ collision.gameObject.tag == Utils.TAG_TRAP)
    //        {
    //            Debug.LogError("zoooooooooooooooooooo");
    //            if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
    //            {
    //               // PlayerManager.Instance.isContinueDetect = false;
    //                PlayerManager.Instance.OnPlayerDie();
    //            }
    //        }
    //        if (/*PlayerManager.Instance.isContinueDetect && */collision.gameObject.tag == Utils.TAG_WIN)
    //        {
    //            PlayerManager.Instance._rig2D.velocity = Vector2.zero;
    //            PlayerManager.Instance.beginMove = false;
    //            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
    //                PlayerManager.Instance.OnWin();
    //        }
    //    }
    //}
}

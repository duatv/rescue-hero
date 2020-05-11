using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectGems : MonoBehaviour
{
    public PlayerManager pManager;
    public GameObject gTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains(Utils.TAG_WIN)) {
            if (gTarget == null)
            {
                gTarget = collision.gameObject;
                pManager.PrepareRotate(transform, false);
                //if (pManager.gameObject.transform.localPosition.x < transform.localPosition.x)
                //{
                //    pManager.saPlayer.skeleton.ScaleX = -1;
                //    pManager.PrepareMoveLeft();
                //}
                //else
                //{
                //    pManager.saPlayer.skeleton.ScaleX = 1;
                //    pManager.PrepareMoveRight();
                //}
            }
        }
    }
}

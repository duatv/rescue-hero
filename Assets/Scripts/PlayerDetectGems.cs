using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectGems : MonoBehaviour
{
    public PlayerManager pManager;
    public GameObject gTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag.Contains(Utils.TAG_SWORD))
        //{
        //    if(!pManager.isTakeSword)
        //        pManager.PrepareRotate(collision.transform, false);
        //}

        //if (collision.gameObject.tag.Contains(Utils.TAG_WIN)) {
        //    if (gTarget == null)
        //    {
        //        gTarget = collision.gameObject;
        //        pManager.PrepareRotate(collision.transform, false);
        //    }
        //}
        //if (collision.gameObject.GetComponent<HostageManager>() != null) {
        //    Debug.LogError("11111");
        //    pManager.PrepareRotate(collision.transform, false);
        //}
        //if (collision.gameObject.name.Contains("FallingChest")) {
        //    pManager.PrepareRotate_(collision.transform, true);
        //}
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains(Utils.TAG_SWORD))
        {
            if (!pManager.isTakeSword)
                pManager.PrepareRotate(collision.transform, false);
        }

        if (collision.gameObject.tag.Contains(Utils.TAG_WIN))
        {
            if (gTarget == null)
            {
                gTarget = collision.gameObject;
                pManager.PrepareRotate(collision.transform, false);
            }
        }
        if (collision.gameObject.GetComponent<HostageManager>() != null)
        {
            pManager.PrepareRotate(collision.transform, false);
        }
        if (collision.gameObject.name.Contains("FallingChest"))
        {
            pManager.PrepareRotate_(collision.transform, true);
        }
    }
}

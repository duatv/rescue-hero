using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectGems : MonoBehaviour
{
    public PlayerManager pManager;
    public GameObject gTarget;
    
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
            if (!collision.gameObject.GetComponent<HostageManager>().isMeetPlayer)
            {
                if (pManager.hitDown.collider != null)
                {
                    pManager.PrepareRotate(collision.transform, false);
                    collision.gameObject.GetComponent<HostageManager>().isMeetPlayer = true;
                }
            }
        }
        if (collision.gameObject.GetComponent<Chest>() != null)
        {
            if(!pManager.beginMove)
                pManager.PrepareRotate(collision.transform, false);
        }
    }
}

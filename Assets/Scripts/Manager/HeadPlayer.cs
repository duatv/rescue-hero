using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPlayer : MonoBehaviour
{
    public PlayerManager pPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains(Utils.TAG_STONE))
        {
            pPlayer.OnPlayerDie();
        }
    }
}

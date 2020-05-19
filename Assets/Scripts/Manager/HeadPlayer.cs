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
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
            {
                //if (collision.gameObject.GetComponent<Chest>() != null) {
                //    if (collision.gameObject.GetComponent<Chest>().rig2d.velocity == Vector2.zero)
                //    {
                //        pPlayer.OnWin();
                //    }
                //    else pPlayer.OnPlayerDie();
                //}else

                    pPlayer.OnPlayerDie();
            }
        }
    }
}

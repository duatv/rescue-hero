using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPlayer : MonoBehaviour
{
    public PlayerManager pPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Utils.TAG_STONE || collision.gameObject.name == "Sword")
        {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
            {
                    pPlayer.OnPlayerDie(true);
            }
        }
        if (collision.gameObject.tag == Utils.TAG_LAVA)
        {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
            {
                pPlayer.OnPlayerDie(true);
            }
        }
    }
}

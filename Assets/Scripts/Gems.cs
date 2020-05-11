using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains(Utils.TAG_STICKBARRIE)) {
            if (collision.gameObject.GetComponent<StickBarrier>() != null) {
                /*
                if (!collision.gameObject.GetComponent<StickBarrier>().hasBlockGems&& collision.gameObject.GetComponent<StickBarrier>().time_> 0) {
                    collision.gameObject.GetComponent<StickBarrier>().hasBlockGems = true;
                }
                */
            }
        }
        if (collision.gameObject.name.Contains("Lava_Pr"))
        {
            if (PlayerManager.Instance != null) {
                if (PlayerManager.Instance.isContinueDetect) {
                    if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                    {
                        PlayerManager.Instance.isContinueDetect = false;
                        PlayerManager.Instance.OnPlayerDie();
                    }
                }
            }
            gameObject.layer = 0;
        }
        if(collision.gameObject.GetComponent<PlayerManager>() != null){
            GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
            {
                StartCoroutine(collision.gameObject.GetComponent<PlayerManager>().IEWin());
            }
        }
    }
}

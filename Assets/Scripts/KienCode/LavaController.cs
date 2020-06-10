using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : Unit
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tag_Stone" && collision.gameObject.name != "FallingStone")
        {
            ChangeStone();
        }
        if (activeChangeStone)
            return;
        if ( collision.gameObject.tag == "BodyPlayer")
        {
            if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
            {
              //  Debug.LogError("zoooooooooooooooooooo");
                if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                {
                    // PlayerManager.Instance.isContinueDetect = false;
                    PlayerManager.Instance.OnPlayerDie(true);
                }
            }
        }
    }

}

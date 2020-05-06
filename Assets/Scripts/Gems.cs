using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Lava_Pr"))
        {
            if (PlayerManager.Instance != null) {
                if (PlayerManager.Instance.isContinueDetect) {
                    PlayerManager.Instance.isContinueDetect = false;
                    PlayerManager.Instance.OnPlayerDie();
                }
            }
            gameObject.layer = 0;
        }
    }
}

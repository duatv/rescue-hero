using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool hasDetect;
    // Start is called before the first frame update
    void Start()
    {
        if(MapLevelManager.Instance.questType == MapLevelManager.QUEST_TYPE.OPEN_CHEST)
        {
            MapLevelManager.Instance.trTarget = transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null) {
            Debug.LogError("1111");
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
            {
                if (!hasDetect)
                {
                    collision.gameObject.GetComponent<PlayerManager>().OnWin();
                }
            }
            hasDetect = true;
        }
    }
}

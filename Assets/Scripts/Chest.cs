using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool fallingChest;
    private bool hasDetect;
    [HideInInspector]public Rigidbody2D rig2d;
    private void OnEnable()
    {
        rig2d = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(MapLevelManager.Instance.questType == MapLevelManager.QUEST_TYPE.OPEN_CHEST)
        {
            if(!fallingChest)
                MapLevelManager.Instance.trTarget = transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            if ((int)rig2d.velocity.y == 0)
            {
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

        if (collision.gameObject.name.Contains("Lava_Pr")) {
            if (!hasDetect) {
                PlayerManager.Instance.OnPlayerDie();
            }
            hasDetect = true;
        }
    }
}

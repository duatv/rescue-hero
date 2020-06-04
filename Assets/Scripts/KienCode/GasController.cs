using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasController : Unit
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BodyPlayer")
        {
            if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
            {
                if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                {
                    PlayerManager.Instance.OnPlayerDie();
                }
            }
        }
    }
    void Update()
    {
        if (!isGravity)
        {
            if (timeFly > 0)
            {
                timeFly -= Time.deltaTime;
                if (timeFly <= 2)
                    rid.AddForce(transform.up * 0.3f);
            }
            if (timeFly <= 0)
                timeFly = Random.Range(3, 8);
        }
        else
        {
            rid.AddForce(transform.up * 0.3f);
            if (timeFly > 0)
            {
                timeFly -= Time.deltaTime;
                if (timeFly <= 2)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
            }
            if (timeFly <= 0)
                timeFly = Random.Range(3, 8);
        }
    }
}

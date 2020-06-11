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
                    PlayerManager.Instance.OnPlayerDie(true);
                }
            }
        }
    }
    //private void Start()
    //{
    //    //        rid.AddForce(transform.up * speed);

    //}
    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        //if (isGravity)
        //{
            timeFly -= deltaTime;

            if (timeFly <= 0)
            {
                rid.velocity = transform.up * speedMove;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-360, 360)));
                timeFly = 1f;
            }
        //}
    }
}

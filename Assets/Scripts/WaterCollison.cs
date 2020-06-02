using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollison : Unit
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (GameManager.Instance.counthatlava < 0)
        //    return;
        if (collision.gameObject.tag == "Trap_Lava" /*|| collision.gameObject.tag == "Tag_Stone"*/ /*|| collision.gameObject.tag == "Wall_Bottom"*/)
        {
            GameManager.Instance.PlaySoundLavaOnWater();
            if(collision.gameObject.tag == "Trap_Lava")
                collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Unit stone = ObjectPoolManagerHaveScript.Instance.stonePooler.GetUnitPooledObject();
            stone.transform.position = gameObject.transform.position;
            stone.gameObject.SetActive(true);


            GameManager.Instance.counthatwater--;

            if (GameManager.Instance.counthatwater <= 0)
            {
                GameManager.Instance.DisableAllLava();
            }

        }
        //if (!canDectectStone)
        //    return;

        //if (collision.gameObject.tag == "Tag_Stone" || collision.gameObject.tag == "Wall_Bottom")
        //{
        //    GameManager.Instance.PlaySoundLavaOnWater();
        //   // collision.gameObject.SetActive(false);
        //    gameObject.SetActive(false);
        //    Unit stone = ObjectPoolManagerHaveScript.Instance.stonePooler.GetUnitPooledObject();
        //    stone.transform.position = gameObject.transform.position;
        //    stone.gameObject.SetActive(true);
        //}

    }
}

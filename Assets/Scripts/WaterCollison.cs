using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollison : Unit
{

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Trap_Lava")
        {

            GameManager.Instance.PlaySoundLavaOnWater();
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Unit stone = ObjectPoolManagerHaveScript.Instance.stonePooler.GetUnitPooledObject();
            stone.transform.position = gameObject.transform.position;
            stone.gameObject.SetActive(true);


            GameManager.Instance.counthatlava--;

            if (GameManager.Instance.counthatlava <= 0)
            {
                GameManager.Instance.DisableAllLava();
            }

        }

    }
}

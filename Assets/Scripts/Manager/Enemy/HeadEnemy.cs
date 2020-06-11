using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadEnemy : MonoBehaviour
{
    public EnemyBase enemyBase;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Utils.TAG_STONE || collision.gameObject.tag == Utils.TAG_CHEST || collision.gameObject.tag == Utils.TAG_SWORD)
        {
            enemyBase.OnDie_();
        }
    }
}

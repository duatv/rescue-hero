using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : Unit
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tag_Stone")
        {
            ChangeStone();
        }
    }
}

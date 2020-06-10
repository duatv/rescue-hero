using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollison : Unit
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Trap_Lava")
        {
            ChildUnit _current = collision.GetComponent<ChildUnit>();
            _current.myUnit.ChangeStone();
            ChangeStone();
        }
        else if(collision.tag == "Tag_Stone" && collision.gameObject.name != "FallingStone")
        {
            ChangeStone();
          //  Debug.LogError("utwwtwtaf");
        }
    }
}

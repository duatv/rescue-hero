using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyMelee : EnemyBase
{

    override public void OnPrepareAttack()
    {
        Debug.Log("Pre attack");
        base.OnPrepareAttack();
    }
    public override void OnDie_()
    {
        base.OnDie_();
     //   pBlood.Play();
    //    skull.SetActive(true);
    }
}
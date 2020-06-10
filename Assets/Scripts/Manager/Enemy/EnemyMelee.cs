using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyMelee : EnemyBase
{
    public ParticleSystem pBlood;
    override public void OnPrepareAttack()
    {
        base.OnPrepareAttack();
    }
    public override void OnDie_()
    {
        base.OnDie_();
        pBlood.Play();
        skull.SetActive(true);
    }
}
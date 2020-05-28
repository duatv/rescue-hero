﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Chest : MonoBehaviour
{
    public bool fallingChest;
    private bool hasDetect;
    public ParticleSystem _pOpenChest;
    public SkeletonAnimation saChest;
    [SpineAnimation]
    public string animOpen;
    [HideInInspector] public Rigidbody2D rig2d;
    private void OnEnable()
    {
        rig2d = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (MapLevelManager.Instance.questType == MapLevelManager.QUEST_TYPE.OPEN_CHEST)
        {
            if (!fallingChest)
                MapLevelManager.Instance.trTarget = transform;
        }
        saChest.AnimationState.Complete += delegate
        {
            if (saChest.AnimationName.Equals(animOpen))
            {
                _pOpenChest.gameObject.SetActive(true);
                PlayerManager.Instance.OnWin();
            }
        };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {
            if ((int)rig2d.velocity.y == 0)
            {
                if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
                {
                    GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
                    PlayerManager.Instance.OnPlayAnimOpenChest();
                    PlayerManager.Instance._rig2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlaySound(SoundManager.Instance.acOpenChest);
                    }
                    StartCoroutine(IEOpenChest());
                }
            }
        }

        if (collision.gameObject.name.Contains("Lava_Pr"))
        {
            if (!hasDetect)
            {
                PlayerManager.Instance.OnPlayerDie();
            }
            hasDetect = true;
        }
    }

    IEnumerator IEOpenChest()
    {
        yield return new WaitForSeconds(0.2f);
        saChest.AnimationState.SetAnimation(0, animOpen, false);
    }
    private void PlayAnim(SkeletonAnimation saPlayer, string anim_, bool isLoop)
    {
        if (!saPlayer.AnimationName.Equals(anim_))
        {
            saPlayer.AnimationState.SetAnimation(0, anim_, isLoop);
        }
    }
}

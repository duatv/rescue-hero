﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CharsBase : MonoBehaviour
{
    public enum CHAR_TYPE { ENEMY, HOSTAGE}
    public enum CHAR_STATE { PLAYING, DIE, WIN, RUNNING }
    public CHAR_STATE _charStage;
    public CHAR_TYPE _charType;
    [HideInInspector]
    public bool isReadOnly = true;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    public SkeletonDataAsset sdaP1, sdaP2;
    public SkeletonAnimation saPlayer;
    [DrawIf("isReadOnly", true, ComparisonType.Equals, DisablingType.ReadOnly)]
    [SpineAnimation]
    public string str_idle, str_Win, str_Lose, strAtt;



    private bool isContinueDetect = true;
    public void ChoosePlayer(int i)
    {
        switch (i)
        {
            case 0:
                saPlayer.skeletonDataAsset = sdaP1;
                saPlayer.Initialize(true);
                PlayAnim(str_idle, true);
                break;
            case 1:
                saPlayer.skeletonDataAsset = sdaP2;
                saPlayer.Initialize(true);
                PlayAnim(str_idle, true);
                break;
        }
    }
    protected void PlayAnim(string anim_, bool isLoop)
    {
        saPlayer.AnimationState.SetAnimation(0, anim_, isLoop);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (isContinueDetect && collision.gameObject.name.Contains("Lava_Pr") && collision.gameObject.tag.Contains(Utils.TAG_TRAP)) {
            Debug.LogError(_charType + " ------> bon tao die roi nhe.");
            isContinueDetect = false;
            PlayAnim(str_Lose, false);
            PlayerManager.Instance.OnPlayerDie();
            _charStage = CHAR_STATE.DIE;
            if (_charType == CHAR_TYPE.HOSTAGE)
            {
                MapLevelManager.Instance.OnLose();
            }
        }
    }
}

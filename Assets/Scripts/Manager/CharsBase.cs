using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CharsBase : MonoBehaviour
{
    public GameObject skull;
    public enum CHAR_TYPE { ENEMY, HOSTAGE }
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
    public string str_idle, str_Win, str_Lose, strAtt, str_Win2;

   
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
        //if (anim_.Equals(str_Win))
        //{
        //    saPlayer.AnimationState.SetAnimation(1, str_Win, true);
        //}
        //else saPlayer.AnimationState.SetAnimation(0, anim_, isLoop);
    }
    public void OnDie_(bool effect) {

        isContinueDetect = false;
        if (_charType == CHAR_TYPE.HOSTAGE)
        {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN) {
                _charStage = CHAR_STATE.DIE;
                GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
                PlayAnim(str_Lose, false);

                if (PlayerManager.Instance != null)
                    PlayerManager.Instance.OnPlayerDie(effect);
                MapLevelManager.Instance.OnLose();
            }
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.acPrincessDie);
            }
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (isContinueDetect && collision.gameObject.name.Contains("Lava_Pr") && collision.gameObject.tag.Contains(Utils.TAG_LAVA))
    //    {
    //        OnDie_(false);
    //    }
    //    if (isContinueDetect && collision.gameObject.tag.Contains(Utils.TAG_STONE)) {
    //        OnDie_(false);
    //    }
    //}
}

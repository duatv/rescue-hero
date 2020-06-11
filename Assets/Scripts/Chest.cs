using System.Collections;
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
                PlayerManager.Instance.OnWin(true);
            }
        };
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Utils.TAG_LAVA)
        {
            if (!hasDetect)
            {
                if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING /*|| PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING*/)
                {
                   // Debug.LogError("huy hom");
                    if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                    {
                        // PlayerManager.Instance.isContinueDetect = false;
                        gameObject.SetActive(false);
                        PlayerManager.Instance.OnPlayerDie(false);

                        if (ObjectPoolerManager.Instance != null)
                        {
                            GameObject destroyEffect = ObjectPoolerManager.Instance.effectDestroyPooler.GetPooledObject();
                            destroyEffect.transform.position = collision.transform.position;
                            destroyEffect.SetActive(true);
                        }
                    }
                }
            }
            hasDetect = true;
        }
        if (collision.gameObject.tag == "BodyPlayer")
        {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE)
            {
                GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
                //PlayerManager.Instance.OnPlayAnimOpenChest();
                rig2d.constraints = RigidbodyConstraints2D.FreezePosition;
                PlayerManager.Instance.OnWin(true);
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.acOpenChest);
                }
                StartCoroutine(IEOpenChest());
            }
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

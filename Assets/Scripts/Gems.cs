using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : Unit
{

    IEnumerator PlaySoundApear(int _ranIndex)
    {
        yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.acCoinApear[_ranIndex]);
    }
    int _ranIndex = 0;
    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            _ranIndex = Random.Range(0, SoundManager.Instance.acCoinApear.Length);
            StartCoroutine(PlaySoundApear(_ranIndex));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap_Lava")
        {
            GameManager.Instance.totalGems--;
            //if (GameManager.Instance.lstAllGems.Contains(this))
            //{
            //    GameManager.Instance.lstAllGems.Remove(this);
            //    if (GameManager.Instance.lstAllGems.Count * 1.0f / GameManager.Instance.totalGems <= 0.2f)
            //    {
            //        GameManager.Instance.isNotEnoughGems = true;
            //        Debug.LogError("het gem roi");
            //    }
            //}


            //if (PlayerManager.Instance.isContinueDetect)
            //{
            if (GameManager.Instance.totalGems <= 0)
            {
                if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
                {
                    Debug.LogError("zoooooooooooooooooooo");
                    if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                    {
                        // PlayerManager.Instance.isContinueDetect = false;
                        PlayerManager.Instance.OnPlayerDie(false);
                    }
                }
            }
          //  }
            gameObject.layer = 0;
            gameObject.SetActive(false);

            int randomEffect = Random.Range(0, 100);
            if (randomEffect <= 10)
            {
                if (ObjectPoolerManager.Instance == null)
                    return;
                GameObject destroyEffect = ObjectPoolerManager.Instance.effectDestroyPooler.GetPooledObject();
                destroyEffect.transform.position = collision.transform.position;
                destroyEffect.SetActive(true);
            }
        }
        if (collision.gameObject.tag == "BodyPlayer")
        {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE || GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
            {
                //if (PlayerManager.Instance.isContinueDetect)
                //{
                  //  GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
                    PlayerManager.Instance.OnWin(true);
                //    PlayerManager.Instance.isContinueDetect = false;
                //}
            }
        }
    }

}

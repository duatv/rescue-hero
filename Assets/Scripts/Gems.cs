using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Lava_Pr"))
        {
            if (GameManager.Instance.lstAllGems.Contains(gameObject))
            {
                GameManager.Instance.lstAllGems.Remove(gameObject);
                if (GameManager.Instance.lstAllGems.Count * 1.0f / GameManager.Instance.totalGems <= 0.2f)
                {

                    GameManager.Instance.isNotEnoughGems = true;
                }
            }

            if (PlayerManager.Instance != null)
            {
                if (PlayerManager.Instance.isContinueDetect)
                {
                    if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                    {
                        if (GameManager.Instance.isNotEnoughGems)
                        {
                            PlayerManager.Instance.isContinueDetect = false;
                            PlayerManager.Instance.OnPlayerDie();
                        }
                    }
                }
            }
            gameObject.layer = 0;
        }
        if (collision.gameObject.GetComponent<PlayerManager>() != null)
        {

            if (GameManager.Instance.gameState != GameManager.GAMESTATE.LOSE || GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
            {
                if (PlayerManager.Instance.isContinueDetect)
                {
                    GameManager.Instance.gameState = GameManager.GAMESTATE.WIN;
                    collision.gameObject.GetComponent<PlayerManager>().OnWin();
                    PlayerManager.Instance.isContinueDetect = false;
                }
            }
        }
    }

}

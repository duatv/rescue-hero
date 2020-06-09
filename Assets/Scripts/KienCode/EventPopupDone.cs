using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventPopupDone : MonoBehaviour
{
    public void EventDone()
    {
        MenuController.instance.loadingPanel.SetActive(true);
    }
    public void EventLoadScene()
    {
        SceneManager.LoadSceneAsync("MainGame");
    }
   public void EventDisableLoading()
    {
        gameObject.SetActive(false);
        GameManager.Instance.gameState = GameManager.GAMESTATE.PLAYING;
    }
}

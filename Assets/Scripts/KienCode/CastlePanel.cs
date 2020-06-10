﻿using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CastlePanel : MonoBehaviour
{
    public int[] prices;
    public List<Image> castle;
    public GameObject BG, btnClose,effectTake,btnBuy,effectsmokehouse,effectsmokebua;
    public SkeletonGraphic sk;
    public bool load;
    public Text priceText, coinText;
    public Image displayImg;
    public int countChangeImg;
    public Animator AnimPanel;
    private void OnValidate()
    {
        if (load)
            return;
        for (int i = 0; i < BG.transform.childCount; i++)
        {
            if (!castle.Contains(BG.transform.GetChild(i).gameObject.GetComponent<Image>()))
            {
                castle.Add(BG.transform.GetChild(i).gameObject.GetComponent<Image>());
            }
        }
        load = true;
    }
    private void OnEnable()
    {
        coinText.text = Utils.currentCoin.ToString(/*"00,#"*/);
        if(DataParam.currentLevelCastle >= castle.Count)
        {
            btnBuy.SetActive(false);
        }
        sk.AnimationState.Event += Event;
    }
    private void Event(TrackEntry trackEntry, Spine.Event e)
    {
        if (trackEntry.Animation.Name.Equals("Bua dap"))
        {
            effectsmokebua.SetActive(true);
        }
    }
    public void DisplayBegin()
    {
        for (int i = 0; i < DataController.instance.saveCastle.Count; i++)
        {
            castle[i].gameObject.SetActive(DataController.instance.saveCastle[i].unlock);
        }

        priceText.text = prices[DataParam.currentLevelCastle].ToString(/*"00,#"*/);
    }
    int randomCastle;
    bool findout;
    public void BtnFindObj()
    {
        if (sk.gameObject.activeSelf || effectsmokehouse.activeSelf)
            return;
        if (Utils.currentCoin >= prices[DataParam.currentLevelCastle])
        {
            while (!findout)
            {
                if (DataParam.currentLevelCastle != 0)
                {
                    randomCastle = Random.Range(0, DataController.instance.saveCastle.Count);
                }
                else
                {
                    randomCastle = 10;
                }
                if (!DataController.instance.saveCastle[randomCastle].unlock)
                {
                    findout = true;
                    countChangeImg = Random.Range(5, 10);
                    Debug.LogError("random castle:" + randomCastle);
                    AnimPlay();
                }
            }
            MenuController.instance.SoundClickButton();
        }
    }
    void AnimPlay()
    {
        displayImg.sprite = castle[Random.Range(0, castle.Count)].sprite;
        AnimPanel.gameObject.SetActive(true);
        AnimPanel.enabled = true;
    }
    void UnlockCastle()
    {
        // castle[randomCastle].gameObject.SetActive(true);

        DataController.instance.saveCastle[randomCastle].unlock = true;
        findout = false;
        Utils.currentCoin -= prices[DataParam.currentLevelCastle];
        coinText.text = Utils.currentCoin.ToString(/*"00,#"*/);
        if (DataParam.currentLevelCastle < castle.Count - 1)
            DataParam.currentLevelCastle++;
        else
            DataParam.currentLevelCastle = castle.Count - 1;
        priceText.text = prices[DataParam.currentLevelCastle].ToString(/*"00,#"*/);
        if (DataParam.currentLevelCastle >= castle.Count)
        {
            btnBuy.SetActive(false);
        }

    }
    IEnumerator delayActive()
    {
        yield return new WaitForSeconds(3);
        sk.gameObject.SetActive(false);
        effectsmokehouse.transform.position = castle[randomCastle].transform.position;
        effectsmokehouse.SetActive(true);
        effectsmokebua.SetActive(false);
        castle[randomCastle].gameObject.SetActive(true);
    }
    public void EventChangeImg()
    {
        if (countChangeImg > 0)
        {
            countChangeImg--;
            displayImg.sprite = castle[Random.Range(0, castle.Count)].sprite;
            if (countChangeImg == 0)
            {
                displayImg.sprite = castle[randomCastle].sprite;
            }
        }


    }
    public void EventDisableAnim()
    {
        if (countChangeImg == 0)
        {
            // displayImg.sprite = castle[randomCastle].sprite;
            AnimPanel.enabled = false;
            UnlockCastle();
            btnClose.SetActive(true);
            effectTake.SetActive(true);
        }
    }
    public void CloseANimPanel()
    {
        if (AnimPanel.enabled)
            return;
        btnClose.SetActive(false);
        AnimPanel.gameObject.SetActive(false);
        effectTake.SetActive(false);
        MenuController.instance.castlePanel.DisplayEffectBua(castle[randomCastle].gameObject);
        StartCoroutine(delayActive());
    }
    public void DisplayEffectBua(GameObject target)
    {
        sk.gameObject.SetActive(true);
        sk.transform.position = target.transform.position;
    }
    public void CloseMe()
    {
        if (sk.gameObject.activeSelf || effectsmokehouse.activeSelf)
            return;
        gameObject.SetActive(false);
    }
}

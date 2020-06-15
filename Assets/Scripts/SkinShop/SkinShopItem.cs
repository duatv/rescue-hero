using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;


public class SkinShopItem : MonoBehaviour
{

    public SkinShopManager shopManager;
    public int index;

    public Image icon,bouder;
    public Text nameText;

    public GameObject lockObj;

    public void DisplayBegin()
    {
        if (!DataController.instance.savehero[index].unlock)
        {
            icon.sprite = MenuController.instance.shopManager.spriteHero[index].info.avatar[0];
            //if (DataController.instance.heroData.infos[index].typeUnlock == HeroData.TypeUnlock.COIN)
            //{
            //    MenuController.instance.shopManager.btnBuyNow.gameObject.SetActive(true);
            //    MenuController.instance.shopManager.btnDailyReward.gameObject.SetActive(false);
            //}
            //else
            //{
            //    MenuController.instance.shopManager.btnBuyNow.gameObject.SetActive(false);
            //    MenuController.instance.shopManager.btnDailyReward.gameObject.SetActive(true);
            //}
        }
        else
        {
            icon.sprite = MenuController.instance.shopManager.spriteHero[index].info.avatar[1];
            //MenuController.instance.shopManager.btnBuyNow.gameObject.SetActive(false);
            //MenuController.instance.shopManager.btnDailyReward.gameObject.SetActive(false);
        }

        nameText.text = DataController.instance.heroData.infos[index].itemName;
        lockObj.SetActive(!DataController.instance.savehero[index].unlock);
    }
    public void Unlock()
    {
        icon.sprite = MenuController.instance.shopManager.spriteHero[index].info.avatar[1];
        MenuController.instance.shopManager.btnBuyNow.gameObject.SetActive(false);
        MenuController.instance.shopManager.btnDailyReward.gameObject.SetActive(false);
        DataController.instance.savehero[index].unlock = true;
        lockObj.SetActive(false);
        MyAnalytic.EventUnlockHero(DataController.instance.heroData.infos[index].itemName);
    }
    public void Click()
    {
        if (DataController.instance.savehero[index].unlock)
        {
            DataParam.currentHero = index;
            MenuController.instance.shopManager.btnBuyNow.gameObject.SetActive(false);
            MenuController.instance.shopManager.btnDailyReward.gameObject.SetActive(false);
        }
        else
        {
            if (DataController.instance.heroData.infos[index].typeUnlock == HeroData.TypeUnlock.COIN)
            {
                MenuController.instance.shopManager.btnBuyNow.gameObject.SetActive(true);
                MenuController.instance.shopManager.btnDailyReward.gameObject.SetActive(false);
            }
            else
            {
                MenuController.instance.shopManager.btnBuyNow.gameObject.SetActive(false);
                MenuController.instance.shopManager.btnDailyReward.gameObject.SetActive(true);
            }
        }
        MenuController.instance.shopManager.currentClickHero = index;
        MenuController.instance.shopManager.DisplaySelect();

        MenuController.instance.shopManager.ChangeSkin(DataController.instance.heroData.infos[index].nameSkin);

        MenuController.instance.SoundClickButton();

    }
}

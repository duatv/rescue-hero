﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Spine;

[System.Serializable]
public class SpriteHero
{
    public Info info;
    [System.Serializable]
    public struct Info
    {
        public Sprite[] avatar;
    }
}
public class SkinShopManager : MonoBehaviour
{
    public Animator anim;
    public SpriteHero[] spriteHero;
    public MenuController menuController;
    public SkeletonGraphic saPlayer;
    public SkinShopItem[] skinShopItem;
    //[SpineSkin]
    //public string skinHenry, skinHercules, skinKael, skinArthur, skinGabriel, skinDragon;
    //[SpineSkin]
    //public string skinHenryWSword, skinHerculesWSword, skinKaelWSword, skinArthurWSword, skinGabrielWSword, skinDragonWSword;
    // public Color clSelect, clNormal;
    public Text txtPrice, txtCurCoin;
    public Button btnBuyNow, btnDailyReward;
    //  [SerializeField] public SkinShopItem shopItem;
    //  public Dictionary<string, SkinShopItem> dicAllSkin = new Dictionary<string, SkinShopItem>();
    private void OnValidate()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }
    public int currentClickHero;
    public void DisplayBegin()
    {
        for (int i = 0; i < skinShopItem.Length; i++)
        {
            skinShopItem[i].DisplayBegin();
        }
        DisplaySelect();
        ChangeSkin(DataController.instance.heroData.infos[DataParam.currentHero].nameSkin);
        MenuController.instance.shopManager.btnBuyNow.gameObject.SetActive(false);
        MenuController.instance.shopManager.btnDailyReward.gameObject.SetActive(false);
    }
    public void DisplaySelect()
    {
        for (int i = 0; i < skinShopItem.Length; i++)
        {
            skinShopItem[i].bouder.enabled = false;
        }
        skinShopItem[DataParam.currentHero].bouder.enabled = true;
        txtPrice.text = DataController.instance.heroData.infos[currentClickHero].price.ToString("#,##0");
    }
    public void BtnUnlock()
    {
        if (DataController.instance.heroData.infos[currentClickHero].typeUnlock == HeroData.TypeUnlock.COIN)
        {
            if (Utils.currentCoin >= DataController.instance.heroData.infos[currentClickHero].price)
            {
                skinShopItem[currentClickHero].Unlock();
                Utils.currentCoin -= DataController.instance.heroData.infos[currentClickHero].price;
                txtCurCoin.text = Utils.currentCoin.ToString("#,##0");
            }
        }
        else
        {
            gameObject.SetActive(false);
            menuController.ShowDailyReward();
        }

    }
    //
    //private void Start()
    //{
    //    //btnBuyNow.onClick.AddListener(() =>
    //    //{
    //    //    if (Utils.currentCoin >= shopItem.priceValue)
    //    //    {
    //    //        Utils.currentCoin -= shopItem.priceValue;
    //    //        Utils.UnlockHero(shopItem.txtName.text);
    //    //        RefreshShop();
    //    //    }
    //    //    else
    //    //    {
    //    //        if (Utils.isVibrateOn)
    //    //        {
    //    //            Handheld.Vibrate();
    //    //        }
    //    //    }
    //    //});

    //    //btnDailyReward.onClick.AddListener(() =>
    //    //{
    //    //    gameObject.SetActive(false);
    //    //    menuController.ShowDailyReward();
    //    //});
    //}

    //private void RefreshShop() {
    //    foreach (SkinShopItem ssItem in dicAllSkin.Values)
    //    {
    //        if (ssItem.txtName.text.Equals(shopItem.txtName.text))
    //        {
    //            ssItem.txtName.color = clSelect;
    //            ssItem.imgSelect.enabled = true;
    //            shopItem.imgLock.enabled = false;
    //            shopItem.imgPreview.sprite = shopItem.sprSelect;
    //            btnBuyNow.gameObject.SetActive(false);
    //            shopItem = ssItem;
    //        }
    //        else
    //        {
    //            ssItem.txtName.color = clNormal;
    //            ssItem.imgSelect.enabled = false;
    //        }
    //    }
    //}
    public void ChangeSkin(string skinName)
    {
     //   saPlayer.Skeleton.SetSkin(skinName);

        //   saPlayer.Skeleton.SetSlotsToSetupPose();
        // saPlayer.LateUpdate();

        var skeleton = saPlayer.Skeleton;
        var skeletonData = skeleton.Data;
        var newSkin = new Skin("new-skin");
        newSkin.AddSkin(skeletonData.FindSkin(skinName));


        skeleton.SetSkin(newSkin);
        skeleton.SetSlotsToSetupPose();
        saPlayer.AnimationState.Apply(skeleton);
    }
    private void OnEnable()
    {
        txtCurCoin.text = Utils.currentCoin.ToString("#,##0");
        anim.Play("PopupAnim");
    }
    public void OnHideShop()
    {
        //   Utils.SetSelectedHero(shopItem./*txtName.text*/skinName);
        //  Utils.SavePlayerSkin(Utils.heroSelected/*shopItem.skinName*//*, shopItem.skinWithSword*/);
        //btnDailyReward.gameObject.SetActive(false);
        //btnBuyNow.gameObject.SetActive(false);
        //   gameObject.SetActive(false);
        anim.Play("PopUpAnimClose");
    }
    private void OnDisable()
    {
        Utils.SaveCoin();
    }
}
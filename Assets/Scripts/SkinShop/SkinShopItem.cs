using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class SkinShopItem : MonoBehaviour
{
    public SkinShopManager shopManager;
    public Text txtName;
    public int priceValue;
    public Image imgLock, imgPreview, imgSelect;
    public Sprite sprGray, sprSelect;
    public Button btn;
    public string skinName;
    public string skinWithSword;

    private void Start()
    {
        if (!shopManager.dicAllSkin.ContainsKey(txtName.text))
        {
            shopManager.dicAllSkin.Add(txtName.text, this);
        }

        btn.onClick.AddListener(() =>
        {
            if (Utils.IsHeroUnlock(txtName.text) || txtName.text.Equals("DRAGON"))
            {
                foreach (SkinShopItem ssItem in shopManager.dicAllSkin.Values)
                {
                    if (ssItem.txtName.text.Equals(txtName.text))
                    {
                        ssItem.txtName.color = shopManager.clSelect;
                        ssItem.imgSelect.enabled = true;
                        shopManager.shopItem = this;
                    }
                    else
                    {
                        ssItem.txtName.color = shopManager.clNormal;
                        ssItem.imgSelect.enabled = false;
                    }
                }


                shopManager.btnBuyNow.gameObject.SetActive(false);
                if (Utils.IsHeroUnlock(txtName.text) && txtName.text.Equals("DRAGON"))
                {
                    shopManager.btnDailyReward.gameObject.SetActive(false);
                    Utils.heroSelected = txtName.text;
                }
                else if (!Utils.IsHeroUnlock(txtName.text) && txtName.text.Equals("DRAGON"))
                    shopManager.btnDailyReward.gameObject.SetActive(true);
                else
                {
                    Utils.heroSelected = txtName.text;
                    shopManager.btnBuyNow.gameObject.SetActive(false);
                    shopManager.btnDailyReward.gameObject.SetActive(false);
                }
            }
            else
            {
                shopManager.txtPrice.text = priceValue.ToString("#,##0");
                shopManager.shopItem = this;
                shopManager.btnDailyReward.gameObject.SetActive(false);
                shopManager.btnBuyNow.gameObject.SetActive(true);
            }

            shopManager.ChangeSkin(/*skinName*/skinWithSword);
        });
    }

    private void OnEnable()
    {
        CheckHeroUnlock();
    }

    private void CheckHeroUnlock()
    {
        if (Utils.IsHeroUnlock(txtName.text) || txtName.text.Equals("DRAGON"))
        {
            if (Utils.IsHeroSelect(txtName.text))
            {
                imgSelect.enabled = true;
                shopManager.shopItem = this;
                txtName.color = shopManager.clSelect;
                shopManager.ChangeSkin(/*skinName*/skinWithSword);
            }
            else
            {
                imgSelect.enabled = false;
                txtName.color = shopManager.clNormal;
            }
            imgLock.enabled = false;
            imgPreview.sprite = sprSelect;
        }
        else
        {
            txtName.color = shopManager.clNormal;
            imgPreview.sprite = sprGray;
            imgSelect.enabled = false;
            imgLock.enabled = true;
        }
    }
}

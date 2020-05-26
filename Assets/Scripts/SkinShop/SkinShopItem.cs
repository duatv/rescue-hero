using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopItem : MonoBehaviour
{
    public SkinShopManager shopManager;
    public Text txtName;
    public int priceValue;
    public Image imgLock, imgPreview, imgSelect;
    public Sprite sprGray, sprSelect;
    public Button btn;
    private void OnEnable()
    {
        CheckHeroUnlock();

        btn.onClick.AddListener(() =>
        {
            if (Utils.IsHeroUnlock(txtName.text) || txtName.text.Equals("DRAGON"))
            {
                shopManager.btnBuyNow.gameObject.SetActive(false);
                if (Utils.IsHeroUnlock(txtName.text) && txtName.text.Equals("DRAGON"))
                    shopManager.btnDailyReward.gameObject.SetActive(false);
                else if (!Utils.IsHeroUnlock(txtName.text) && txtName.text.Equals("DRAGON")) shopManager.btnDailyReward.gameObject.SetActive(true);
                else
                {
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
        });
    }
    private void CheckHeroUnlock() {
        if (Utils.IsHeroUnlock(txtName.text) || txtName.text.Equals("DRAGON"))
        {
            if (Utils.IsHeroSelect(txtName.text))
            {
                imgSelect.enabled = true;
                txtName.color = shopManager.clSelect;
            }
            else
            {
                imgSelect.enabled = false;
                txtName.color = shopManager.clNormal;
            }
            shopManager.btnBuyNow.gameObject.SetActive(false);
            imgLock.enabled = false;
            imgPreview.sprite = sprSelect;
        }
        else {
            shopManager.btnBuyNow.gameObject.SetActive(true);
            shopManager.btnDailyReward.gameObject.SetActive(false);

            txtName.color = shopManager.clNormal;
            imgPreview.sprite = sprGray;
            imgSelect.enabled = false;
            imgLock.enabled = true;
        }
    }
}

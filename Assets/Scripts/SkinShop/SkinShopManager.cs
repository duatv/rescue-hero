using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopManager : MonoBehaviour
{
    public MenuController menuController;
    public Color clSelect, clNormal;
    public Text txtPrice, txtCurCoin;
    public Button btnBuyNow, btnDailyReward;
    [SerializeField]public SkinShopItem shopItem;

    private void Start()
    {
        btnBuyNow.onClick.AddListener(() => {
            if (Utils.currentCoin >= shopItem.priceValue)
            {
                Utils.currentCoin -= shopItem.priceValue;
                Utils.UnlockHero(shopItem.txtName.text);
                Debug.LogError("Buy Success");
            }
            else
            {
                Debug.LogError("Not Enough Coin");
                if (Utils.isVibrateOn)
                {
                    Handheld.Vibrate();
                }
            }
        });

        btnDailyReward.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            menuController.ShowDailyReward();
        });
    }

    private void OnEnable()
    {
        txtCurCoin.text = Utils.currentCoin.ToString("#,##0");
    }
    public void OnHideShop() {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Utils.SaveCoin();
    }
}
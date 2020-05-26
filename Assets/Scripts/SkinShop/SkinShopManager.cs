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
    public SkinShopItem shopItem;

    private void Start()
    {
        btnBuyNow.onClick.AddListener(() => {
            Debug.LogError("Check Buy Hero: " + shopItem.txtName.text);
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftPanel : MonoBehaviour
{
    public Button btnClaimX3, btnClaim;
    public Button[] gAllGift;
    public RewardItem[] _allRewardItems;
    public int _dayIndex = 1;
    public Animator anim;
    public Text coinText;
    private void OnValidate()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    public void OnShowPanel() {
        //for (int i = 0; i < gAllGift.Length; i++) {
        //    if (i == Utils.curDailyGift - 1 && !Utils.cantakegiftdaily) {
        //        gAllGift[i].interactable = true;
        //    }
        //    else
        //    {
        //        gAllGift[i].interactable = false;
        //    }
        //}
    }
    private void OnEnable()
    {
        anim.Play("PopupAnim");
        coinText.text = Utils.currentCoin.ToString();
    }
    public void DisplayAgain()
    {
        for(int i = 0; i < _allRewardItems.Length; i ++)
        {
            _allRewardItems[i].DisplayAgain();
        }
    }
    public void OnClosePanel() {

        //  gameObject.SetActive(false);
        anim.Play("PopUpAnimClose");
        Utils.SaveCoin();
        Utils.SaveDailyGift();
        MenuController.instance.CheckShowDailyGift();
    }
    private void AddCoin(int _coinAdd) {

        Utils.currentCoin += _coinAdd;
        if (!Utils.cantakegiftdaily)
        {

            Utils.curDailyGift++;
            Utils.cantakegiftdaily = true;

        }
        coinText.text = Utils.currentCoin.ToString();
    }
    public void TakeGift(int _index) {
        //MyAnalytic.TakeDailyGift();
        //Utils.HasClaimReward();
        //switch (_index)
        //{
        //    case 1:
        //        AddCoin(1000);
        //        break;
        //    case 2:
        //        AddCoin(2000);
        //        break;
        //    case 3:
        //        AddCoin(3000);
        //        break;
        //    case 4:
        //        AddCoin(4000);
        //        break;
        //    case 5:
        //        AddCoin(5000);
        //        break;
        //    case 6:
        //        AddCoin(6000);
        //        break;
        //}
        //OnClosePanel();
    }
    public void TakeX2Gift(int _index) {


        AdsManager.Instance.ShowRewardedVideo((b) =>
        {
            if (b) {
                Utils.HasClaimReward();

                switch (_index)
                {
                    case 1:
                        AddCoin(3000);
                        break;
                    case 2:
                        AddCoin(6000);
                        break;
                    case 3:
                        AddCoin(9000);
                        break;
                    case 4:
                        AddCoin(12000);
                        break;
                    case 5:
                        AddCoin(15000);
                        break;
                    case 6:
                        AddCoin(18000);
                        break;
                }
            }
            Utils.SaveCoin();
            Utils.SaveDailyGift();
            DisplayAgain();
            MyAnalytic.EventReward("x2_dailygift");
        });
      //  OnClosePanel();
    }
    public void TakeSpecialGift() {
        if (!Utils.cantakegiftdaily && Utils.curDailyGift == 7)
        {

            Utils.HasClaimReward();
            Utils.curDailyGift++;

            Debug.LogError("take hero");
        }
        MenuController.instance.shopManager.skinShopItem[5].Unlock();
        Utils.SaveCoin();
        Utils.SaveDailyGift();
        DisplayAgain();
        //   OnClosePanel();
    }

    public void OnClaim() {


        Utils.HasClaimReward();
        Debug.LogError("day index:" +  _dayIndex);
        switch (_dayIndex)
        {
            case 1:
                AddCoin(1000);
                break;
            case 2:
                AddCoin(2000);
                break;
            case 3:
                AddCoin(3000);
                break;
            case 4:
                AddCoin(4000);
                break;
            case 5:
                AddCoin(5000);
                break;
            case 6:
                AddCoin(6000);
                break;
            case 7:
                TakeSpecialGift();
                break;
        }
        Utils.SaveCoin();
        Utils.SaveDailyGift();
        DisplayAgain();
        //  OnClosePanel();
    }
    public void OnX3Claim() {
        TakeX2Gift(_dayIndex);
    }
}

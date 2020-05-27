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
    public void OnClosePanel() {
        gameObject.SetActive(false);

        Utils.SaveCoin();
        Utils.SaveDailyGift();
    }
    private void AddCoin(int _coinAdd) {
        Utils.currentCoin += _coinAdd;
        if (!Utils.cantakegiftdaily)
        {
            Utils.curDailyGift++;
            Utils.cantakegiftdaily = true;
        }
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
                MyAnalytic.TakeDailyGiftX2();
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
        });
        OnClosePanel();
    }
    public void TakeSpecialGift() {
        if (!Utils.cantakegiftdaily && Utils.curDailyGift == 7)
        {
            Utils.HasClaimReward();
            Utils.curDailyGift++;
        }
        OnClosePanel();
    }

    public void OnClaim() {
        MyAnalytic.TakeDailyGift();
        Utils.HasClaimReward();
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
        OnClosePanel();
    }
    public void OnX3Claim() {
        TakeX2Gift(_dayIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftPanel : MonoBehaviour
{
    public Button[] gAllGift;

    public void OnShowPanel() {
        for (int i = 0; i < gAllGift.Length; i++) {
            if (i == Utils.curDailyGift - 1) {
                gAllGift[i].interactable = true;
            }
            else
            {
                gAllGift[i].interactable = false;
            }
        }
    }
    public void OnClosePanel() {
        gameObject.SetActive(false);

        Utils.SaveCoin();
        Utils.SaveDailyGift();
    }
    private void AddCoin(int _coinAdd) {
        Utils.currentCoin = _coinAdd;
        if (!Utils.cantakegiftdaily)
        {
            Utils.curDailyGift++;
            Utils.cantakegiftdaily = true;
        }
    }
    public void TakeGift(int _index) {
        MyAnalytic.TakeDailyGift();
        Utils.HasClaimReward();
        switch (_index)
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
        }
        OnClosePanel();
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
                        AddCoin(2000);
                        break;
                    case 2:
                        AddCoin(4000);
                        break;
                    case 3:
                        AddCoin(6000);
                        break;
                    case 4:
                        AddCoin(8000);
                        break;
                    case 5:
                        AddCoin(10000);
                        break;
                    case 6:
                        AddCoin(12000);
                        break;
                }
            }
        });
        OnClosePanel();
    }
    public void TakeSpecialGift() {
        Utils.HasClaimReward();
        Utils.curDailyGift++;
        OnClosePanel();
    }
}

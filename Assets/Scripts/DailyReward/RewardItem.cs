using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    public DailyGiftPanel _dailyGift;
    public Sprite sprSelected, sprHasClaim, sprDisable;
    public GameObject gTick;
    public Image imgPreview;
    public int dayIndex;
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("Utils.curDailyGift: " + Utils.curDailyGift);
        if (dayIndex == Utils.curDailyGift && !Utils.cantakegiftdaily)
        {
            imgPreview.sprite = sprSelected;
            _dailyGift._dayIndex = dayIndex;
            gTick.SetActive(false);
            if (dayIndex == 7)
            {
                _dailyGift.btnClaimX3.interactable = false;
            }
            else _dailyGift.btnClaimX3.interactable = true;
        }
        else if (dayIndex < Utils.curDailyGift)
        {
            imgPreview.sprite = sprHasClaim;
            gTick.SetActive(true);
        }
        else
        {
            imgPreview.sprite = sprDisable;
            gTick.SetActive(false);
        }
    }
}
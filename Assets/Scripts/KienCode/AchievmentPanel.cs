using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchievmentPanel : MonoBehaviour
{
    public ScrollRect sc;
    public Animator anim;
    public Text coinText;
    private void OnValidate()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }
    public void OpenMe(bool open)
    {

        if (open)
        {
            coinText.text = Utils.currentCoin.ToString();
            gameObject.SetActive(true);
            anim.Play("PopupAnim");
            sc.verticalNormalizedPosition = 0;
        }
        else
        {
            //    gameObject.SetActive(false);
            anim.Play("PopUpAnimClose");
            MenuController.instance.CheckDisplayWarningAchievement();
        }
        MenuController.instance.SoundClickButton();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BouderAchievment : MonoBehaviour
{
    public int index;
    public Text nameQuestText, desQuestText, rewardText, processText;
    public Image fillImg;
    public GameObject lockClaim;
    int currentLevel;
    private void OnEnable()
    {
        DisplayContent();
    }
    public void DisplayContent()
    {
        if (DataController.instance.saveDataAchievement[index].currentLevel < DataController.instance.dataAchievement.infos[index].contents.Length - 1)
        {
            currentLevel = DataController.instance.saveDataAchievement[index].currentLevel;
        }
        else
            currentLevel = DataController.instance.dataAchievement.infos[index].contents.Length - 1;

        nameQuestText.text = DataController.instance.dataAchievement.infos[index].name + " " + (currentLevel + 1);
        desQuestText.text = DataController.instance.dataAchievement.infos[index].des.Replace("xx", DataController.instance.dataAchievement.infos[index].contents[currentLevel].numberRequire.ToString());
        rewardText.text = "" + DataController.instance.dataAchievement.infos[index].contents[currentLevel].numberReward;
        processText.text = DataController.instance.saveDataAchievement[index].currentNumber + "/" + DataController.instance.dataAchievement.infos[index].contents[currentLevel].numberRequire;
        fillImg.fillAmount = (float)DataController.instance.saveDataAchievement[index].currentNumber / DataController.instance.dataAchievement.infos[index].contents[currentLevel].numberRequire;
        lockClaim.SetActive(!DataController.instance.saveDataAchievement[index].pass);
    }
    public void BtnClaim()
    {
        if (!DataController.instance.saveDataAchievement[index].pass)
            return;
        Utils.currentCoin += DataController.instance.dataAchievement.infos[index].contents[currentLevel].numberReward;
        Utils.SaveCoin();

        DataController.instance.saveDataAchievement[index].currentLevel++;
        DataController.instance.saveDataAchievement[index].currentNumber = 0;
        DataController.instance.saveDataAchievement[index].pass = false;
        MenuController.instance.achievementPanel.coinText.text = Utils.currentCoin.ToString();
        DisplayContent();

        MenuController.instance.SoundClickButton();
    }
}

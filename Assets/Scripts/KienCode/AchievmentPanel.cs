using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchievmentPanel : MonoBehaviour
{
    public ScrollRect sc;
    public void OpenMe(bool open)
    {
        if(open)
        {
            gameObject.SetActive(true);
            sc.verticalNormalizedPosition = 0;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

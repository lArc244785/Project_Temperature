using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIView
{
    public GameObject DayPanel;
    public GameObject NightPanel;

    public Image dayIcon;
    public Image nightIcon;

    public TimeManager timeManager;

    public void UpdateDayNightIcon(bool isNight)
    {
        if(isNight)
        {
            nightIcon.fillAmount = .0f;

            NightPanel.SetActive(true);
            DayPanel.SetActive(false);

            //dayIcon.fillAmount += 0.00625f * Time.deltaTime;
            //test
            dayIcon.fillAmount += 0.0625f * Time.deltaTime;
        }
        else
        {
            dayIcon.fillAmount = .0f;

            DayPanel.SetActive(true);
            NightPanel.SetActive(false);

            nightIcon.fillAmount += 0.0625f * Time.deltaTime;
        }
    }
}

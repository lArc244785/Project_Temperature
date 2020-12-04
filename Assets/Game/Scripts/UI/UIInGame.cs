using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGame : UIView
{
    public GameObject DayPanel;
    public GameObject NightPanel;

    public Image dayIcon;
    public Image nightIcon;

    public Slider temperatureSlider;

    public TimeManager timeManager;

    public TextMeshProUGUI temperature;
    private float temperatureValue;
    public void UpdateTemperature()
    {
        temperature.text = GameManager.Instance.GetPlayerControl().temperature.ToString();

        temperatureValue = (GameManager.Instance.GetPlayerControl().temperature - 23.0f) * 0.037f;
        temperatureSlider.value = temperatureValue;
    }

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

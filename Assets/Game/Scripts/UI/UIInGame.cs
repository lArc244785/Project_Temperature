using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIInGame : UIView
{
    public GameObject DayPanel;
    public GameObject NightPanel;

    public Image dayIcon;
    public Image nightIcon;

    public RectTransform clockHand;
    private float degreePerSecond;
    private float daySecond;

    //public Slider temperatureSlider;
    public Image temperatureIcon;
    private Color originalColor;


    public TimeManager timeManager;

    public TextMeshProUGUI temperature;
    private float temperatureValue;

    private void Start()
    {
        originalColor = temperatureIcon.color;
    }

    public void UpdateTemperature()
    {
        temperature.text = GameManager.Instance.GetPlayerControl().temperature.ToString();
        if (GameManager.Instance.GetPlayerControl().temperature > 40.0f)
        {
            temperatureIcon.DOColor(Color.red, 1.0f);
            Debug.Log("test");
        }
        else if (GameManager.Instance.GetPlayerControl().temperature < 32.0f)
            temperatureIcon.DOColor(Color.blue, 1.0f);
        else
            temperatureIcon.DOColor(originalColor, 1.0f);

        temperatureValue = (GameManager.Instance.GetPlayerControl().temperature - 23.0f) * 0.037f;
        temperatureIcon.fillAmount = temperatureValue;
    }

    public void UpdateDayNightIcon(bool isNight)
    {
        clockHand.Rotate(Vector3.back, 20 * timeManager.degreePerSecond * Time.deltaTime);
        if (isNight)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIView
{
    public Sprite dayIcon;
    public Sprite nightIcon;

    public Image day;
    public Image night;

    public TimeManager timeManager;

    //public void DayNIghtIcon(bool value)
    //{
    //    if(value)
    //    {
    //        dayNight.sprite = nightIcon;
    //    }
    //    else
    //    {
    //        dayNight.sprite = dayIcon;
    //    }
    //}

    public void DayNIghtIcon(bool value)
    {
        if (value)
        {
            night.gameObject.SetActive(value);
            day.gameObject.SetActive(false);
        }
        else
        {
            night.gameObject.SetActive(value);
            day.gameObject.SetActive(true);
        }
    }
}

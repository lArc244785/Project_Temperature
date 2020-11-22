using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIView
{
    public Sprite dayIcon;
    public Sprite nightIcon;

    public Image dayNight;

    public TimeManager timeManager;

    public void DayNIghtIcon(bool value)
    {
        if(value)
        {
            dayNight.sprite = nightIcon;
        }
        else
        {
            dayNight.sprite = dayIcon;
        }
    }

}

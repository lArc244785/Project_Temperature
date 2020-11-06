using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DevelopEdit : MonoBehaviour
{
    private PlayerControl pc;

    public TMP_InputField MoveSpeedValue;
    public TMP_InputField RollingSpeedValue;
    public TMP_InputField RollingTimeValue;
    public TMP_InputField StopTimeScaleValue;
    public TMP_InputField StopTimeWaitTimeValue;


    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        pc = GameMagner.Instance.GetPlayerControl();
        MoveSpeedValue.text = pc.speed.ToString();
        RollingSpeedValue.text = pc.rollingSpeed.ToString();
        RollingTimeValue.text = pc.RollingTime.ToString();
        StopTimeScaleValue.text = pc.stopTimeScale.ToString();
        StopTimeWaitTimeValue.text = pc.stopTime.ToString();
    }



    public void EditMoveSpeed()
    {
        if(MoveSpeedValue.text.ToString() != null)
        pc.speed = float.Parse(MoveSpeedValue.text.ToString());
    }

    public void EditRollingSpeedValue()
    {
        if (RollingSpeedValue.text.ToString() != null)
            pc.rollingSpeed = float.Parse(RollingSpeedValue.text.ToString());
    }
    public void EditRollingTimeValue()
    {
        if (RollingTimeValue.text.ToString() != null)
            pc.RollingTime = float.Parse(RollingTimeValue.text.ToString());
    }
    public void EditStopTimeScale()
    {
        if (StopTimeScaleValue.text.ToString() != null)
            pc.stopTimeScale = float.Parse(StopTimeScaleValue.text.ToString());
    }
    public void EditStopTimeWaitTime()
    {
        if(StopTimeWaitTimeValue.text.ToString() != null)
        pc.stopTime = float.Parse(StopTimeWaitTimeValue.text.ToString());
    }
}

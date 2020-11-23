using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DevelopEdit : MonoBehaviour
{
    private PlayerControl pc;
    private CamerManger camManger;

    public TMP_InputField MoveSpeedValue;
    public TMP_InputField RollingSpeedValue;
    public TMP_InputField RollingTimeValue;
    public TMP_InputField StopTimeScaleValue;
    public TMP_InputField StopTimeWaitTimeValue;

    public TMP_InputField MouseOffsetRadiusXValue;
    public TMP_InputField MouseOffsetRadiusYValue;
    public TMP_InputField MoveOffsetSpeedValue;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        pc = GameManager.Instance.GetPlayerControl();
        camManger = GameManager.Instance.GetCamerManger();

        MoveSpeedValue.text = pc.speed.ToString();
        RollingSpeedValue.text = pc.rollingSpeed.ToString();
        RollingTimeValue.text = pc.DeshTime.ToString();
        StopTimeScaleValue.text = pc.stopTimeScale.ToString();
        StopTimeWaitTimeValue.text = pc.stopTime.ToString();
        MouseOffsetRadiusXValue.text = camManger.moveOffsetRadius.x.ToString();
        MouseOffsetRadiusYValue.text = camManger.moveOffsetRadius.y.ToString();
        MoveOffsetSpeedValue.text = camManger.offsetTime.ToString();
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
            pc.DeshTime = float.Parse(RollingTimeValue.text.ToString());
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

    public void EditMouseOffSetRadiusX()
    {
        if (MouseOffsetRadiusXValue.text.ToString() != null)
        {
            camManger.moveOffsetRadius.x =
                float.Parse(MouseOffsetRadiusXValue.text.ToString());
            camManger.SetOffset();
        }
    }

    public void EditMouseOffSetRadiusY()
    {
        if (MouseOffsetRadiusYValue.text.ToString() != null)
        {
            camManger.moveOffsetRadius.y =
                float.Parse(MouseOffsetRadiusYValue.text.ToString());
            camManger.SetOffset();
        }
    }

    public void EditMouseOffSetSpeed()
    {
        if(MoveOffsetSpeedValue.text.ToString() != null)
        {
            camManger.offsetTime =
                float.Parse(MoveOffsetSpeedValue.text.ToString());
        }
    }
}

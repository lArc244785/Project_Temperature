using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData
{
    public float BGMVolume;
    public float SFXVolume;

    public OptionData()
    {
        BGMVolume = 0.5f;
        SFXVolume = 0.5f;
    }

    public OptionData(OptionData data)
    {
        this.BGMVolume = data.BGMVolume;
        this.SFXVolume = data.SFXVolume;
    }
}

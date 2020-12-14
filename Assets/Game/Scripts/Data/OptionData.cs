using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData
{
    public float BGMVolume;
    public float SFXVolume;
    public int isMuteBGM;
    public int isMuteSFX;
    public float originalBGMVolume;
    public float originalSFXVolume;


    public OptionData()
    {
        BGMVolume = 0.5f;
        SFXVolume = 0.5f;
        isMuteBGM = 0;
        isMuteSFX = 0;
        originalBGMVolume = BGMVolume;
        originalSFXVolume = SFXVolume;
    }

    public OptionData(OptionData data)
    {
        this.BGMVolume = data.BGMVolume;
        this.SFXVolume = data.SFXVolume;
        this.isMuteBGM = data.isMuteBGM;
        this.isMuteSFX = data.isMuteSFX;
        this.originalBGMVolume = data.originalBGMVolume;
        this.originalSFXVolume = data.originalSFXVolume;
    }
}

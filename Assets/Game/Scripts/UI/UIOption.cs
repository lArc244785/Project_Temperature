using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIView
{
    public CanvasGroup canvasGroup;

    public Slider BGMSlider;
    public Slider SFXSlider;

    private OptionData optionData;

    private bool isToggle;

    private void Update()
    {
        if (isToggle)
            OptionManager.Instance.ApplyOption(optionData);
    }

    public override void Toggle(bool value)
    {
        isToggle = value;
        if (value)
        {
            canvasGroup.interactable = false;
            OptionData data = OptionManager.Instance.GetCurrentOptionData();
            SetData(data);
            canvasGroup.interactable = true;
        }
        base.Toggle(value);
    }

    public void SetData(OptionData data)
    {
        optionData = data;

        BGMSlider.value = optionData.BGMVolume;
        SFXSlider.value = optionData.SFXVolume;
    }

    public void ApplyBGMSlider()
    {
        optionData.BGMVolume = BGMSlider.value;
    }

    public void ApplySFXSlider()
    {
        optionData.SFXVolume = SFXSlider.value;
    }

    public void ReturnToGame()
    {
        canvasGroup.interactable = false;
        StartCoroutine(OptionManager.Instance.SaveOptionData());
        Toggle(false);
    }

    public void ExitGame()
    {
        StartCoroutine(OptionManager.Instance.SaveOptionData());
        //GameMagner.Instance.ExitGame();
        if (Application.isPlaying)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

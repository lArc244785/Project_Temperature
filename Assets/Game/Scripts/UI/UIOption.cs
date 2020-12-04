using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIView
{
    public CanvasGroup canvasGroup;

    public Image BGMFill;
    public Image SFXFill;

    private Color originalBGMColor;
    private Color originalSFXColor;

    public Slider BGMSlider;
    public Slider SFXSlider;

    private OptionData optionData;

    public Button BGMButton;
    public Button SFXButton;

    public Sprite BGMMute;
    public Sprite BGMOn;
    public Sprite SFXMute;
    public Sprite SFXOn;

    private float originalBGMVolume;
    private float originalSFXVolume;

    private bool isBGMMute;
    private bool isSFXMute;

    private bool isToggle;

    private void Start()
    {
        isBGMMute = false;
        isSFXMute = false;
    }

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
            BGMSlider.value = originalBGMVolume;
            SFXSlider.value = originalSFXVolume;
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

    public void MuteBGM()
    {
        if(!isBGMMute)
        {
            BGMSlider.interactable = false;
            originalBGMVolume = optionData.BGMVolume;
            originalBGMColor = BGMFill.color;
            optionData.BGMVolume = 0;
            BGMButton.image.sprite = BGMMute;
            BGMFill.color = Color.gray;
            isBGMMute = true;
        }
        else
        {
            optionData.BGMVolume = originalBGMVolume;
            BGMButton.image.sprite = BGMOn;
            isBGMMute = false;
            BGMSlider.interactable = true;
            BGMFill.color = originalBGMColor;
        }
    }

    public void MuteSFX()
    {
        if (!isSFXMute)
        {
            SFXSlider.interactable = false;
            originalSFXVolume = optionData.SFXVolume;
            originalSFXColor = SFXFill.color;
            optionData.SFXVolume = 0;
            SFXButton.image.sprite = SFXMute;
            SFXFill.color = Color.gray;
            isSFXMute = true;
        }
        else
        {
            optionData.SFXVolume = originalSFXVolume;
            SFXButton.image.sprite = SFXOn;
            isSFXMute = false;
            SFXSlider.interactable = true;
            SFXFill.color = originalSFXColor;
        }
    }

    public void ApplyBGMSlider()
    {
        optionData.BGMVolume = BGMSlider.value;
        BGMFill.fillAmount = BGMSlider.value;
    }

    public void ApplySFXSlider()
    {
        optionData.SFXVolume = SFXSlider.value;
        SFXFill.fillAmount = SFXSlider.value;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIOption : UIView , IPointerEnterHandler, IPointerExitHandler
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

    private bool isBGMMute;
    private bool isSFXMute;

    public bool isToggle;

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
            canvasGroup.interactable = true;
        }
        base.Toggle(value);
    }

    public void SetData(OptionData data)
    {
        optionData = data;

        if (optionData.isMuteBGM == 0)
        {
            BGMSlider.value = optionData.BGMVolume;
            BGMFill.fillAmount = BGMSlider.value;
            BGMButton.image.sprite = BGMOn;
        }
        if (optionData.isMuteSFX == 0)
        {
            SFXSlider.value = optionData.SFXVolume;
            SFXFill.fillAmount = SFXSlider.value;
            SFXButton.image.sprite = SFXOn;
        }

        if (optionData.isMuteBGM == 1)
        {
            BGMSlider.value = data.originalBGMVolume;
            BGMButton.image.sprite = BGMMute;
            BGMFill.color = Color.gray;
            optionData.BGMVolume = 0;
            BGMSlider.interactable = false;
            isBGMMute = true;
        }
        if (optionData.isMuteSFX == 1)
        {
            SFXSlider.value = data.originalSFXVolume;
            SFXButton.image.sprite = SFXMute;
            SFXFill.color = Color.gray;
            optionData.SFXVolume = 0;
            SFXSlider.interactable = false;
            isSFXMute = true;
        }
    }

    public void MuteBGM()
    {
        if(!isBGMMute)
        {
            BGMSlider.interactable = false;
            optionData.originalBGMVolume = optionData.BGMVolume;
            originalBGMColor = BGMFill.color;
            optionData.BGMVolume = 0;
            BGMButton.image.sprite = BGMMute;
            BGMFill.color = Color.gray;
            isBGMMute = true;
            optionData.isMuteBGM = 1;
        }
        else
        {
            optionData.BGMVolume = optionData.originalBGMVolume;
            BGMButton.image.sprite = BGMOn;
            isBGMMute = false;
            BGMSlider.interactable = true;
            BGMFill.color = Color.white;
            optionData.isMuteBGM = 0;
        }
    }

    public void MuteSFX()
    {
        if (!isSFXMute)
        {
            SFXSlider.interactable = false;
            optionData.originalSFXVolume = optionData.SFXVolume;
            originalSFXColor = SFXFill.color;
            optionData.SFXVolume = 0;
            SFXButton.image.sprite = SFXMute;
            SFXFill.color = Color.gray;
            isSFXMute = true;
            optionData.isMuteSFX = 1;
        }
        else
        {
            optionData.SFXVolume = optionData.originalSFXVolume;
            SFXButton.image.sprite = SFXOn;
            isSFXMute = false;
            SFXSlider.interactable = true;
            SFXFill.color = Color.white;
            optionData.isMuteSFX = 0;
        }
    }

    public void ApplyBGMSlider()
    {
        optionData.BGMVolume = BGMSlider.value;
        BGMFill.fillAmount = BGMSlider.value;

        OptionManager.Instance.audioMixer.SetFloat("BGMVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, 0.5f * BGMSlider.value)) * 20);
    }

    public void ApplySFXSlider()
    {
        optionData.SFXVolume = SFXSlider.value;
        SFXFill.fillAmount = SFXSlider.value;

        OptionManager.Instance.audioMixer.SetFloat("SFXVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, SFXSlider.value)) * 20);
    }

    public void ReturnToGame()
    {
        canvasGroup.interactable = false;
        StartCoroutine(OptionManager.Instance.SaveOptionData());
        Toggle(false);

        AudioPool.Instance.Play2D("Click");

        //if(!UIManager.Instance.uiInGame.isToggle)
        //{
        //    //UIManager.Instance.uiInGame.
        //}
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.isOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.isOverUI = false;
    }
}

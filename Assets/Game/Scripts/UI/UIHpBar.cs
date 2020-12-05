using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHpBar : MonoBehaviour
{
    public Image hp1;
    public Image hp2;
    public Image hp3;
    public Image hp4;
    public Image hp5;

    public Image hpBarBackground;
    public Color originalColor;

    private Sequence sequence;

    public void UpdatePositionFromWorldPosition(Vector3 worldPos)
    {
        hpBarBackground.rectTransform.anchoredPosition = GameManager.Instance.GetCamerManger().GetMainCamera().WorldToScreenPoint(worldPos);
        //image.rectTransform.anchoredPosition = GameManager.Instance.GetCamerManger().GetMainCamera().WorldToScreenPoint(worldPos);
    }

    public void SetValue(float value, float maxValue)
    {
        float fillAmount = value / maxValue;
        if (fillAmount == .8f)
        {
            sequence.Insert(0, hp5.DOFillAmount(0, 0.3f));
            //hp5.gameObject.SetActive(false);
        }
        else if (fillAmount == .6f)
        {
            sequence.Insert(0, hp4.DOFillAmount(0, 0.3f));
            //hp4.gameObject.SetActive(false);
        }
        else if (fillAmount == .4f)
        {
            sequence.Insert(0, hp3.DOFillAmount(0, 0.3f));
            //hp3.gameObject.SetActive(false);
        }
        else if (fillAmount == .2f)
        {
            sequence.Insert(0, hp2.DOFillAmount(0, 0.3f));
            //hp2.gameObject.SetActive(false);
        }
        else if (fillAmount == 0.0f)
        {
            sequence.Insert(0, hp1.DOFillAmount(0, 0.3f));
            //hp1.gameObject.SetActive(false);
        }
        //image.fillAmount = fillAmount;
    }

    public void HandleDestroy()
    {
        Destroy(this.gameObject);
    }
}

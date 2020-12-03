using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
{
    public Image image;
    public Image hpBarBackground;
    public Color originalColor;

    public void UpdatePositionFromWorldPosition(Vector3 worldPos)
    {
        hpBarBackground.rectTransform.anchoredPosition = GameManager.Instance.GetCamerManger().GetMainCamera().WorldToScreenPoint(worldPos);
        //image.rectTransform.anchoredPosition = GameManager.Instance.GetCamerManger().GetMainCamera().WorldToScreenPoint(worldPos);
    }

    public void SetValue(float value, float maxValue)
    {
        float fillAmount = value / maxValue;
        image.fillAmount = fillAmount;
    }

    public void HandleDestroy()
    {
        Destroy(this.gameObject);
    }
}

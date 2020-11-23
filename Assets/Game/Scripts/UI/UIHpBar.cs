using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
{
    public Image image;
    public Color originalColor;

    public void UpdatePositionFromWorldPosition(Vector3 worldPos)
    {
        image.rectTransform.anchoredPosition = GameManager.Instance.GetCamerManger().GetMainCamera().WorldToScreenPoint(worldPos);
    }
}

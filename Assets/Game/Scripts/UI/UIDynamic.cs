using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDynamic : MonoBehaviour
{
    public RectTransform rootRect;
    public RectTransform anchorRect;

    public RectTransform GetAnchorTransform()
    {
        return anchorRect;
    }

    public  void Toggle(bool value)
    {
        rootRect.gameObject.SetActive(value);
    }
}

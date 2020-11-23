using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDynamic : UIView
{

    public RectTransform anchorRect;

    public RectTransform GetAnchorTransform()
    {
        return anchorRect;
    }

    public override void Toggle(bool value)
    {
        base.Toggle(value);

        rootRect.gameObject.SetActive(value);
    }
}

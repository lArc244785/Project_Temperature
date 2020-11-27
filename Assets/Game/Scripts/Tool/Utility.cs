﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Utility
{
    public static void KillTween(Tween tween)
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
    }

    public static float AngleBetweenTwoPoints(Vector3 point, Vector3 target)
    {
        return Mathf.Atan2(target.x - point.x, target.y - point.y) * Mathf.Rad2Deg;
    } 
}

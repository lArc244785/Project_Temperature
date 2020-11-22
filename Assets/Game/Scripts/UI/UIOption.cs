using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIView
{
    public Slider BGMSlider;
    public Slider SFXSlider;

    public void ReturnToGame()
    {
        Toggle(false);
    }

    public void ExitGame()
    {
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

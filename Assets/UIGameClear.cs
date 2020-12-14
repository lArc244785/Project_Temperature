using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameClear : UIView
{
    public void GameClear()
    {
        Toggle(true);
        StartCoroutine(GamerClearFX());
    }


    IEnumerator GamerClearFX()
    {
        yield return new WaitForSeconds(1.0f);
        UIManager.Instance.uiPadeIn.PadeInAnimation();
    }


}

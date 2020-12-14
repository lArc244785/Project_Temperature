using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOver : UIView
{
    public void GameOver()
    {
        Toggle(true);
        StartCoroutine(GameOverFX());
    }


    IEnumerator GameOverFX()
    {
        yield return new WaitForSeconds(1.0f);
        UIManager.Instance.uiPadeIn.PadeInAnimation();
    }

}

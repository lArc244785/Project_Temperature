
using UnityEngine;
using UnityEngine.UI;

public class UIGameStart : UIView
{
    private Animator ani;
    public Image image;

    public void Initializer()
    {
        ani = GetComponent<Animator>();
    }

    public override void Toggle(bool value)
    {
        base.Toggle(value);
        if (value)
        {
            image.gameObject.SetActive(false);
        }
    }

    public void GameStartAnimationStart()
    {
        ani.SetTrigger("GameStart");
    }


}

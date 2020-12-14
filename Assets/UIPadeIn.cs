using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPadeIn : UIView
{
    private Animator ani;

    public void Initializer()
    {
        ani = GetComponent<Animator>();
    }

    public void PadeInAnimation()
    {
        Toggle(true);
        ani.SetTrigger("PadeIn");
    }

    public void GoToMain()
    {
        AudioPool.Instance.DespawnAll();
        SceneManager.LoadScene("MainMenu");
    }

}

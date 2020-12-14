using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIInGame uiInGame;
    public UIMainMenu uiMainMenu;
    public UIOption uiOption;
    public UIDynamic uiDynamic;
    public Animator animator;
    public UIGameStart uiGameStart;
    public UIPadeIn uiPadeIn;
    public UIGameClear uiGameClear;
    public UIGameOver uiGameOver;

    public bool isOverUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
            Initializer();
        }

    }

    private void Initializer()
    {
        if (uiGameStart != null)
        {
            uiGameStart.Initializer();
            uiGameStart.Toggle(false);
        }

        if(uiGameClear != null)
        {
            uiGameClear.Toggle(false);
        }

        if(uiGameOver != null)
        {
            uiGameOver.Toggle(false);
        }


        if(uiPadeIn != null)
        {
            uiPadeIn.Toggle(false);
            uiPadeIn.Initializer();
        }

    }


}

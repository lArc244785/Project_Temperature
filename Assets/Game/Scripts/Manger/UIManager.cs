﻿using System.Collections;
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

    public bool isOverUI;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            //DontDestroyOnLoad(this.gameObject);
        }
    }
}

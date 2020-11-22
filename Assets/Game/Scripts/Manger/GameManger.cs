﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;

    private PlayerControl playerControl;
    private CamerManger camMagner;
    private InputManger inputManger;
<<<<<<< HEAD
<<<<<<< HEAD:Assets/Game/Scripts/Manger/GameManger.cs
    private MapMagner mapManger;
    private EnemyManger enemyManger;
=======

>>>>>>> Jun:Assets/Game/Scripts/Manger/GameMagner.cs
=======
    private MapMagner mapManger;
    private EnemyManger enemyManger;
>>>>>>> 3fa5113e526e5a67d0cd631bb812b482601fc58a

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else {
            Destroy(gameObject);
        }
        Initializer();
        DOTween.defaultAutoPlay = AutoPlay.None;
    }

    private void Initializer()
    {
        GetPlayerControl();
        GetCamerManger();
        GetInputManger();
        GetMapManger();
        GetEnemyManger();
    }


    public PlayerControl GetPlayerControl()
    {
        if(playerControl == null)
        {
            playerControl = GameObject.FindObjectOfType<PlayerControl>();
            playerControl.Initializer();
        }

        return playerControl;
    }

    public CamerManger GetCamerManger()
    {
        if(camMagner == null)
        {
            camMagner = GameObject.Find("CamerManger").GetComponent<CamerManger>();
            camMagner.Intilize();
        }
        return camMagner;
    }

    public Transform GetCamMangerPos()
    {
        return camMagner.transform;
    }

    public InputManger GetInputManger()
    {
        if (inputManger == null)
        {
            inputManger = GameObject.FindObjectOfType<InputManger>();
            inputManger.Initializer();
        }
        return inputManger;
    }

<<<<<<< HEAD
<<<<<<< HEAD:Assets/Game/Scripts/Manger/GameManger.cs
=======
>>>>>>> 3fa5113e526e5a67d0cd631bb812b482601fc58a
    public MapMagner GetMapManger()
    {
        if(mapManger == null)
        {
            mapManger = GameObject.FindObjectOfType<MapMagner>();
            mapManger.Initializer();
        }

        return mapManger;
    }

    public EnemyManger GetEnemyManger()
    {
        if(enemyManger == null)
        {
            enemyManger = GameObject.FindObjectOfType<EnemyManger>();
            enemyManger.Initializer();
        }

        return enemyManger;
<<<<<<< HEAD
=======
    public void SetMainMenu(bool value)
    {
        UIManager.Instance.uiMainMenu.Toggle(true);
        UIManager.Instance.uiInGame.Toggle(false);
        UIManager.Instance.uiOption.Toggle(false);
    }

    public void ExitGame()
    {
        if(Application.isPlaying)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
>>>>>>> Jun:Assets/Game/Scripts/Manger/GameMagner.cs
=======
>>>>>>> 3fa5113e526e5a67d0cd631bb812b482601fc58a
    }
}

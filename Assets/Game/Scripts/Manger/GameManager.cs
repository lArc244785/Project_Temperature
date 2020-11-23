using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerControl playerControl;
    private CamerManger camMagner;
    private InputManger inputManger;
    private MapMagner mapManger;
    private EnemyManger enemyManger;

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
    }
}

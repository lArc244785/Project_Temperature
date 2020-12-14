﻿using DG.Tweening;
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
    private SpawnManager spawnManager;
    private TimeManager timeManager;
    private TemperatureSystem temperatureSystem;

    public LampFx lamp;

    public int stage;
    public int wave;
    public int currentWave;

    public bool isWaveSetting = true;
    public bool isGameClear = false;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else {
            Destroy(gameObject);
        }
        //Initializer();
        DOTween.defaultAutoPlay = AutoPlay.None;
    }

    private void Start()
    {
        Initializer();

        UIManager.Instance.uiMainMenu.Toggle(false);
        UIManager.Instance.uiGameStart.Toggle(true);
        UIManager.Instance.uiGameStart.GameStartAnimationStart();

        AudioPool.Instance.DespawnAll();
    }

    private void Initializer()
    {
        GetPlayerControl();
        GetCamerManger();
        GetInputManger();
        GetMapManger();
        GetEnemyManger();
        GetTimeManager();
        GetSpawnManager();

    }

    //씬안에 데이터를 넘겨주는 녀석이 호출해주면 됩니다.
    public void GameStart()
    {
        stage = 0;
        wave = 19;
        currentWave = 0;
        StartCoroutine(spawnManager.NextWaveSpawn());
        isWaveSetting = false;
        isGameClear = false;


    }


    public void GameOver()
    {
        isGameOver = true;
        playerControl.ControlOff();

        UIManager.Instance.uiGameOver.GameOver();
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
        if (enemyManger == null)
        {
            enemyManger = GameObject.FindObjectOfType<EnemyManger>();
            enemyManger.Initializer();
        }

        return enemyManger;
    }

    public SpawnManager GetSpawnManager()
    {
        if(spawnManager == null)
        {
            spawnManager = GameObject.FindObjectOfType<SpawnManager>();
            spawnManager.Initializer(1);
        }
        return spawnManager;
    }

    public TimeManager GetTimeManager()
    {
        if (timeManager == null)
        {
            timeManager = GameObject.FindObjectOfType<TimeManager>();
        }
        return timeManager;
    }

    public TemperatureSystem GetTemperatureSystem()
    {
        if(temperatureSystem == null)
        {
            temperatureSystem = GameObject.FindObjectOfType<TemperatureSystem>();
            temperatureSystem.Initializer();
        }
        return temperatureSystem;
    }



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
    }

    public void GameClear()
    {
        isGameOver = true;
        isGameClear = true;

        UIManager.Instance.uiGameClear.GameClear();

    }

    public void NextWave()
    {
        GameManager.Instance.currentWave++;
        if (currentWave >= wave)
        {
            GameClear();
        }
        else
        {
            StartCoroutine(WaveSettingCoroutine());
        }
    }

    IEnumerator WaveSettingCoroutine()
    {
        //맵 타일 떨어지는거
        yield return mapManger.Wave();
        //맵 재설정

        //몬스터 스폰
        yield return spawnManager.NextWaveSpawn();
    }



}

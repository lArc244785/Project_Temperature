using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //Initializer();
        DOTween.defaultAutoPlay = AutoPlay.None;

        UIManager.Instance.uiInGame.nightIcon.fillAmount = .5f;
        UIManager.Instance.uiInGame.dayIcon.fillAmount = .0f;
    }

    private void Start()
    {
        Initializer();

        //SetMainMenu(true);
        SetInGameUI(true);
    }

    private void Initializer()
    {
        GetPlayerControl();
        GetCamerManger();
        GetInputManger();
        GetMapManger();
        GetEnemyManger();
    }

    public void StartStage(string stageName)
    {
        StartCoroutine(ProcessStage(stageName));
    }

    private IEnumerator ProcessStage(string stageName)
    {
        SetMainMenuUI(false);
        AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync(stageName, LoadSceneMode.Single);
        yield return sceneLoadAsync;
        SetInGameUI(true);

        yield break;
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

    public void SetMainMenuUI(bool value)
    {
        UIManager.Instance.uiMainMenu.Toggle(true);
        UIManager.Instance.uiInGame.Toggle(false);
        UIManager.Instance.uiOption.Toggle(false);
        UIManager.Instance.uiDynamic.Toggle(false);

        if (value)
            UIManager.Instance.isOverUI = value;
    }

    public void SetInGameUI(bool value)
    {
        UIManager.Instance.uiMainMenu.Toggle(false);
        UIManager.Instance.uiInGame.Toggle(true);
        UIManager.Instance.uiOption.Toggle(false);
        UIManager.Instance.uiDynamic.Toggle(true);

        if (value)
            UIManager.Instance.isOverUI = value;
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
}

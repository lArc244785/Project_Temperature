using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMagner : MonoBehaviour
{
    public static GameMagner Instance;

    private PlayerControl playerControl;
    private CamerManger camMagner;
    private InputManger inputManger;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(gameObject);
        }
        Intilize();
    }

    private void Intilize()
    {
        GetPlayerControl();
        GetCamerManger();
        GetInputManger();
    }

    private void Start()
    {
        UIManager.Instance.uiMainMenu.Toggle(false);
    }


    public PlayerControl GetPlayerControl()
    {
        if(playerControl == null)
        {
            playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
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
}

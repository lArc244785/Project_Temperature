using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : UIView
{
    private void Start()
    {
        AudioPool.Instance.PlayBGM("Main_BGM");
    }

    public void StartGame()
    {
        //GameManager.Instance.StartStage("Jun_map");
        AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync("Stage1", LoadSceneMode.Single);

        AudioPool.Instance.Play2D("Start");
    }

    public void ShowOption()
    {
        UIManager.Instance.uiOption.Toggle(true);
        AudioPool.Instance.Play2D("Click");
    }

    public void ExitGame()
    {
        //GameMagner.Instance.ExitGame();

        if (Application.isPlaying)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

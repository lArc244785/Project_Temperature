using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : UIView
{
    public void StartGame()
    {
        //GameManager.Instance.StartStage("Jun_map");
        AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync("Jun_map", LoadSceneMode.Single);
    }

    public void ShowOption()
    {
        UIManager.Instance.uiOption.Toggle(true);
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

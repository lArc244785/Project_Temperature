using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public void StartGame()
    {

    }

    public void ShowOption()
    {
        UIManager.Instance.uiOption.Toggle(true);
    }

    public void ExitGame()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(ProcessLoadGame());
    }

    IEnumerator ProcessLoadGame()
    {
        yield return null;

        yield return StartCoroutine(AddressableManager.Instance.PreloadAllAddressable());

        AudioPool.Instance.Init();

        var asyncOp = SceneManager.LoadSceneAsync("MainMenu");
        yield return asyncOp;

        yield break;
    }
}

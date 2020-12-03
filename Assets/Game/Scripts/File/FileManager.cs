using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private FileBase fileBase;

    private void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        fileBase = new FileBaseWindows();
#endif

    }

    public IEnumerator WriteText(string dataName, string data)
    {
        yield return StartCoroutine(fileBase.WriteText(dataName, data));

        yield break;
    }

    public string ReadText_Result;
    public IEnumerator ReadText(string dataName)
    {
        ReadText_Result = string.Empty;
        yield return StartCoroutine(fileBase.ReadText(dataName));
        ReadText_Result = fileBase.ReadText_Result;
    }

    public bool IsExist_Result;
    public IEnumerator IsExist(string dataName)
    {
        yield return StartCoroutine(fileBase.IsExist(dataName));
        IsExist_Result = fileBase.IsExist_Result;
    }
}

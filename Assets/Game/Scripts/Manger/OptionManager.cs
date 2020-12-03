using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance;

    private OptionData currentOptionData;

    public FileManager fileManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(ProcessStartGame());
    }

    IEnumerator ProcessStartGame()
    {
        yield return StartCoroutine(fileManager.IsExist("Option.dat"));
        if(fileManager.IsExist_Result)
        {
            yield return StartCoroutine(LoadOptionData());
        }
        else
        {
            GetDefaultOptionData();
            yield return StartCoroutine(SaveOptionData());
        }

        yield break;
    }

    public void ApplyOption(OptionData data)
    {
        currentOptionData = new OptionData(data);
        //apply here and save to disk
    }

    public IEnumerator SaveOptionData()
    {
        string dataString = JsonUtility.ToJson(currentOptionData, true);

        yield return StartCoroutine(fileManager.WriteText("Option.dat", dataString));
        yield break;   
    }

    public IEnumerator LoadOptionData()
    {
        yield return StartCoroutine(fileManager.ReadText("Option.dat"));
        if (!string.IsNullOrEmpty(fileManager.ReadText_Result))
        {
            var loadedOptionData = JsonUtility.FromJson<OptionData>(fileManager.ReadText_Result);
            ApplyOption(loadedOptionData);
        }
            
    }

    public OptionData GetCurrentOptionData()
    {
        return currentOptionData;
    }

    public OptionData GetDefaultOptionData()
    {
        currentOptionData = new OptionData();
        return currentOptionData;
    }
}

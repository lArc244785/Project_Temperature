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
        GetDefaultOptionData();
        
        yield return StartCoroutine(SaveOptionData());

        yield break;
    }

    public void ApplyOption(OptionData data)
    {
        //apply here and save to disk
        currentOptionData = new OptionData(data);
    }

    public IEnumerator SaveOptionData()
    {
        string dataString = JsonUtility.ToJson(currentOptionData, true);

        yield return StartCoroutine(fileManager.WriteText("Option.dat", dataString));
        yield break;   
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

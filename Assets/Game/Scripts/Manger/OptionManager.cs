using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance;

    private OptionData currentOptionData;

    public FileManager fileManager;

    public AudioMixer audioMixer;

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
        audioMixer.SetFloat("BGMVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.BGMVolume))) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.SFXVolume))) * 20);
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

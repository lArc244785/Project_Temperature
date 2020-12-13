using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    public static AddressableManager Instance;

    private Dictionary<string, AudioClip> audioClipFactory;

    bool isLoading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        audioClipFactory = new Dictionary<string, AudioClip>();
    }

    public IEnumerator PreloadAllAddressable()
    {
        isLoading = true;
        var loadAudioClipAsync = Addressables.LoadAssetsAsync<AudioClip>("AudioClip", null);
        loadAudioClipAsync.Completed += OnAudioClipLoadComplete;
        while (!loadAudioClipAsync.IsDone && isLoading)
            yield return null;

        yield break;
    }

    void OnAudioClipLoadComplete(AsyncOperationHandle<IList<AudioClip>> audioClipResult)
    {
        foreach (var audio in audioClipResult.Result)
        {
            audioClipFactory.Add(audio.name, audio);
        }

        isLoading = false;
    }

    public AudioClip GetAudioClip(string name)
    {
        if (audioClipFactory.ContainsKey(name))
        {
            return audioClipFactory[name];
        }

        return null;
    }
}

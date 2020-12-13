using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioPool : MonoBehaviour
{
    public static AudioPool Instance;

    public GameObject audioSourcePrefab;

    public List<AudioItem> poolList = new List<AudioItem>();

    private Dictionary<string, AudioItem> poolDict = new Dictionary<string, AudioItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void Init()
    {
        foreach (var pool in poolList)
        {
            if (!string.IsNullOrEmpty(pool.itemName))
            {
                pool.Init(this);
                poolDict.Add(pool.itemName, pool);
            }
        }
    }

    public AudioSource GetBGM(string itemName)
    {
        if (poolDict.ContainsKey(itemName))
        {
            var pool = poolDict[itemName];
            AudioSource result = pool.GetItem();
            return result;
        }

        return null;
    }

    public AudioSource Play(string itemName, Vector3 pos)
    {
        if (poolDict.ContainsKey(itemName))
        {
            var pool = poolDict[itemName];
            AudioSource result = pool.GetItem();
            result.transform.position = pos;
            result.spatialBlend = 1;
            result.Play();
            //result.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
            return result;
        }

        return null;
    }

    public AudioSource Play2D(string itemName)
    {
        if (poolDict.ContainsKey(itemName))
        {
            var pool = poolDict[itemName];
            AudioSource result = pool.GetItem();
            result.spatialBlend = 0;
            result.Play();
            //result.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
            return result;
        }

        return null;
    }

    public AudioSource PlayBGM(string itemName)
    {
        if (poolDict.ContainsKey(itemName))
        {
            var pool = poolDict[itemName];
            AudioSource result = pool.GetItem();
            result.spatialBlend = 0;
            result.loop = true;
            result.volume = 1;
            result.Play();
            //result.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
            return result;
        }

        return null;
    }

    private void Update()
    {
        foreach(var pool in poolList)
        {
            pool.Update();
        }
    }

    public void DespawnAll()
    {
        foreach (var pool in poolList)
        {
            pool.DespawnAll();
        }
    }
}

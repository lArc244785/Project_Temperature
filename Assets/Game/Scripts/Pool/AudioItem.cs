using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioItem 
{
    public string itemName;
    public int initialInstantiationCount;
    public bool additionalInstantiation;
    public AudioMixerGroup audioMixerGroup;

    private List<AudioSource> items = new List<AudioSource>();
    private List<AudioSource> spawnedItems = new List<AudioSource>();

    private AudioPool pool;
    private AudioClip clip;

    public void Init(AudioPool pool)
    {
        this.pool = pool;
        items = new List<AudioSource>();
        spawnedItems = new List<AudioSource>();

        if (initialInstantiationCount > 0)
        {
            for (int i = 0; i < initialInstantiationCount; i++)
            {
                AddItem();
            }
        }
    }
   
    public void Update()
    {
        for(int i =0; i<spawnedItems.Count; i++)
        {
            if (!spawnedItems[i].isPlaying)
            {
                PutItem(spawnedItems[i]);
                return;
            }
        }
    }

    private AudioSource AddItem()
    {
        clip = AddressableManager.Instance.GetAudioClip(itemName);
        if (clip != null)
        {
            Transform item = GameObject.Instantiate(pool.audioSourcePrefab).transform;
            item.gameObject.name = itemName;
            item.SetParent(pool.transform);
            item.localPosition = Vector3.zero;
            item.gameObject.SetActive(false);

            AudioSource audioSource = item.GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            items.Add(audioSource);
            return audioSource;
        }

        return null;
    }

    public AudioSource GetItem()
    {
        if (items.Count > 0)
        {
            AudioSource result = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            spawnedItems.Add(result);
            result.transform.SetParent(null);
            result.gameObject.SetActive(true);
            return result;
        }
        else
        {
            if (additionalInstantiation)
            {
                return AddItem();
            }
        }

        Debug.LogWarning("no more items in the pool..." + itemName);
        return null;
    }

    public void PutItem(AudioSource item)
    {
        items.Add(item);
        spawnedItems.Remove(item);
        item.transform.SetParent(pool.transform);
        item.transform.localPosition = Vector3.zero;
        item.gameObject.SetActive(false);
    }

    public void DespawnAll()
    {
        foreach (var item in spawnedItems)
        {
            PutItem(item);
            break;
        }
    }
}

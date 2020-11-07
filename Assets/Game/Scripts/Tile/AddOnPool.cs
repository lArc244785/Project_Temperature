using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnPool : MonoBehaviour
{
    public GameObject[] addOn;
    List<GameObject> addOnList = new List<GameObject>();

    public Transform mapTr;

    Vector3 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        for (int i =0; i<addOn.Length;i++)
        {
            spawnPos = new Vector3(addOn[i].GetComponent<TileAddOn>().upTargetPosX, -10, addOn[i].GetComponent<TileAddOn>().upTargetPosZ);
            GameObject ob = Instantiate(addOn[i], Vector3.zero, mapTr.rotation);
            ob.transform.SetParent(mapTr);
            ob.transform.localPosition = spawnPos;
            ob.SetActive(false);
            addOnList.Add(ob);
        }

        addOnList[0].SetActive(true);
    }
}

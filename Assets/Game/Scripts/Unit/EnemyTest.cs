using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public Transform hpBar;

    // Start is called before the first frame update
    void Start()
    {
        GameObject hpBarGO = Instantiate(Resources.Load("UIHpBar"), Vector3.zero, Quaternion.identity)as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

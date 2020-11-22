﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Status : MonoBehaviour
{
    [Header("Status")]
    public float hp = 0.0f;
    public float MAXHP = 5.0f;
    public bool isAlive = true;
    public float speed = 10.0f;

    public float temperature = 36.5f;
    public float HotTemperatuer = 50.0f;
    public float ColdTemperatuer = -50.0f;

    public string Name;

}

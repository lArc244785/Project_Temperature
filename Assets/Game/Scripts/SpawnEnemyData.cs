using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyData
{
    public GameObject PreFab;
    public StructInfo.Point SpawnPoint;

    public SpawnEnemyData(GameObject prefab, StructInfo.Point point)
    {
        PreFab = prefab;
        SpawnPoint = point;
    }

}

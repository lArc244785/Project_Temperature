using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveSpawnData 
{
    public List<SpawnEnemyData> WaveSpawnEnemyList = new List<SpawnEnemyData>();
    private bool isSpawnComplet = false;

    public void AddSpawnEnemyData(SpawnEnemyData data)
    {
        WaveSpawnEnemyList.Add(data);
    }

    public bool ChackWaveEnmeysDie()
    {
        foreach(var enemy in WaveSpawnEnemyList)
        {
            if (enemy.PreFab.GetComponent<UnitBase>().isAlive) return false;
        }
        return true;
    }

    public bool GetSpawnComplet()
    {
        return isSpawnComplet;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private List<Dictionary<string, object>> spawnDB;

    List<WaveSpawnData> waveList;

    public GameObject WaitEnemy;
    public GameObject RunEnmey;
    public GameObject DiedEnmey;


    public void Initializer(int StageNum)
    {
       // CreateWaveSpawnList(StageNum);
    }

    private void Update()
    {
        //if (GameManager.Instance.isWaveSetting || GameManager.Instance.isGameClear) return;

        //if (waveList[GameManager.Instance.currentWave].ChackWaveEnmeysDie())
        //{
        //    GameManager.Instance.NextWave();
        //}
    }

    public IEnumerator NextWaveSpawn()
    {
        WaveSpawnData wsd = waveList[GameManager.Instance.currentWave];
        foreach(var enemyData in wsd.WaveSpawnEnemyList)
        {
            GameObject enemy = enemyData.PreFab;
            enemy.transform.parent = RunEnmey.transform;
            TileBase spawnTile = GameManager.Instance.GetMapManger().GameMap[enemyData.SpawnPoint.y, enemyData.SpawnPoint.x];
            Vector3 spawnPos = spawnTile.transform.position;
            spawnPos.y += (spawnTile.GetComponent<BoxCollider>().size.y/2) +( enemy.GetComponent<UnitBase>().capsuleCollider.height / 2);
            enemy.transform.position = spawnPos;
        }
        GameManager.Instance.GetEnemyManger().HandleSetting();

        yield break;
    }


    private void CreateWaveSpawnList(int StageNum)
    {
        spawnDB = CSVReder.Read("EnemyListTest");
        waveList = new List<WaveSpawnData>();

        GameManager.Instance.wave = (int)spawnDB[0]["TOTALWAVE"];


        //웨이브에 수에 따라서 생성
        for (int i = 0; i < GameManager.Instance.wave; i++)
        {
            waveList.Add(new WaveSpawnData());
        }

        for (var i = 0; i < spawnDB.Count; i++)
        {
            if((int)spawnDB[i]["Stage"] == StageNum)
            {
                int waveIndex = (int)spawnDB[i]["Wave"];
                GameObject prefab = GetPrefabEnemy((int)spawnDB[i]["EnemyType"]);
                GameObject spawnEnemy = Instantiate(prefab, WaitEnemy.transform);
                StructInfo.Point spawnPoint = new StructInfo.Point((int)spawnDB[i]["SpawnX"], (int)spawnDB[i]["SpawnY"]);
                SpawnEnemyData data = new SpawnEnemyData(spawnEnemy, spawnPoint);
                spawnEnemy.GetComponent<UnitBase>().HandleSpawn();
                waveList[waveIndex].AddSpawnEnemyData(data);
            }
        }



        //DeBug용
        //foreach(var waveData in waveList)
        //{
        //    print("=====================");
        //    foreach(var enemyData in waveData.WaveSpawnEnemyList)
        //    {
        //        print(enemyData.PreFab.name + " , (" + enemyData.SpawnPoint.y + "," + enemyData.SpawnPoint.y + ")");
        //    }
        //    print("=====================");
        //}



    }

    private GameObject GetPrefabEnemy(int id)
    {
        EnumInfo.PrefabEnemy prefabEnemy = (EnumInfo.PrefabEnemy)id;
        GameObject returnGameObject = null;
        switch (prefabEnemy)
        {
            case EnumInfo.PrefabEnemy.MallangBasic:
                returnGameObject = Resources.Load("EnemyPrefab/MallangBasic") as GameObject;
                break;
            case EnumInfo.PrefabEnemy.MallangSpring:
                returnGameObject = Resources.Load("EnemyPrefab/MallangSpring") as GameObject;
                break;
            case EnumInfo.PrefabEnemy.EggDragon:
                returnGameObject = Resources.Load("EnemyPrefab/EggDragon") as GameObject;
                break;
        }

        return returnGameObject;
    }

}

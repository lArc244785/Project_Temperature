using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManger : MonoBehaviour
{
    public List<EnemyBasic> enemyList;


    public void Initializer()
    {
        enemyList = new List<EnemyBasic>();
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            EnemyBasic enemy = transform.GetChild(i).GetComponent<EnemyBasic>();
            enemy.HandleSpawn();
            enemyList.Add(enemy);
        }
    }

    public void SetPath(StructInfo.Point targetNode)
    {
        MapMagner mm = GameManger.Instance.GetMapManger();

        foreach (TileBase tile in mm.GameMap)
        {
            tile.mr.material = tile.nomalMaterial;
        }

        foreach (EnemyBasic enemy in enemyList)
        {
            enemy.FindPath(targetNode);
        }
    }


    public List<UnitBase> GetHitBoxInEnemy(bool isPlayerHitBox, Vector3 center, float Range)
    {
        List<UnitBase> HItBoxinEnemyList = new List<UnitBase>();
        if (isPlayerHitBox)
        {
            foreach(UnitBase unit in enemyList)
            {
                Debug.Log(Vector3.Distance(unit.GetUnitTransform().position, center));
                if(Mathf.Abs( Vector3.Distance(unit.GetUnitTransform().position, center)) < Range)
                {
                    HItBoxinEnemyList.Add(unit);
                }
            }
        }
        else
        {
            HItBoxinEnemyList.Add(GameManger.Instance.GetPlayerControl());
        }

        return HItBoxinEnemyList;
    }


}

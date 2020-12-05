using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManger : MonoBehaviour
{
    public List<EnemyBasic> enemyList= new List<EnemyBasic>();
    private List<StructInfo.Point> PassTileList = new List<StructInfo.Point>();
    private PlayerControl pc;

    private StructInfo.Point oldTargetNode;
    public bool DebugMode;

    public void Initializer()
    {
        pc = GameManager.Instance.GetPlayerControl();
    }

    public void HandleSetting()
    {
        enemyList.Clear();

        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            EnemyBasic enemy = transform.GetChild(i).GetComponent<EnemyBasic>();
            if (!DebugMode)
            {
                enemy.HandleSpawn();
                enemyList.Add(enemy);
            }
            else
            {
                enemy.gameObject.SetActive(false);
            }
        }

    }

    public void SetPath(StructInfo.Point targetNode)
    {
        oldTargetNode = targetNode;
        MapMagner mm = GameManager.Instance.GetMapManger();

        mm.ClearRoot();

        PassTileList.Clear();


        foreach (EnemyBasic enemy in enemyList)
        {
            if (mm.GetAroundTileCount()  <= PassTileList.Count)
            {
                PassTileList.Clear();
            }
                enemy.FindPath(targetNode);

        }
    }

    public void PathRootCorrection(StructInfo.Point visitNode)
    {
        PassTileList.Add(visitNode);
    }


    public void ReSetPath()
    {
        SetPath(oldTargetNode);
    }


    public List<UnitBase> GetHitBoxInEnemy(bool isPlayerHitBox, Vector3 center, float Range)
    {
        List<UnitBase> HItBoxinEnemyList = new List<UnitBase>();
        if (isPlayerHitBox)
        {
            foreach(UnitBase unit in enemyList)
            {
                //Debug.Log(Vector3.Distance(unit.GetUnitTransform().position, center));

                if(Mathf.Abs( Vector3.Distance(unit.GetSkinnedMeshPostionToPostion(), center)) < Range)
                {
                    HItBoxinEnemyList.Add(unit);
                }
            }
        }
        else
        {
           
            float targetDistance = Mathf.Abs(Vector3.Distance(pc.GetSkinnedMeshPostionToPostion(), center));
            if (targetDistance < Range)
            {
                HItBoxinEnemyList.Add(pc);
            }
        }

        return HItBoxinEnemyList;
    }

    public List<StructInfo.Point> GetPoints()
    {
        return PassTileList;
    }
}

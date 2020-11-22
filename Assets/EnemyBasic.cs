using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyBasic : UnitBase
{

    public TileBase tile;
    private TileBase moveToTile;
    public Stack<StructInfo.TileInfo> path;
    private Vector3 moveToPoint;
    private Vector3 moveDir;
    private PathFindTool pathFindTool;
    private bool isFind;
    public float Range;
    private UnitBase target;

    private float AttackTime;


    // Start is called before the first frame update

    //private void Start()
    //{
    //    HandleSpawn();
    //}
    public override void HandleSpawn()
    {
        base.HandleSpawn();
        path = new Stack<StructInfo.TileInfo>();
        StructInfo.TileInfo moveTile = GameManger.Instance.GetMapManger().GameMap[0, 0].tileInfo;
        pathFindTool = GetComponent<PathFindTool>();
        pathFindTool.Initialize();
        isFind = false;
        target = GameManger.Instance.GetPlayerControl();
        //FindPath(moveTile.point);
    }


    private void FixedUpdate()
    {
        if (!target.isAlive) return;
        AI();
    }

    private void SetNextMove()
    {
        if (path.Count == 0)
        {
            moveDir = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
            isFind = false;
        }
        else
        {
            StructInfo.Point nextPoint = path.Pop().point;
            moveToTile = GameManger.Instance.GetMapManger().GameMap[nextPoint.y, nextPoint.x];
            moveToPoint = moveToTile.tileInfo.position;
            moveToPoint.y = unitTransform.position.y;
            moveDir = (moveToPoint - unitTransform.position).normalized;
           if(!isAttackRate) unitTransform.LookAt(unitTransform.position + moveDir);
        }

    }

    public void FindPath(StructInfo.Point tNode)
    {
        isFind = pathFindTool.PathFind_AStar(tile.tileInfo.point, tNode, path);
        Debug.Log(isFind);
        if (isFind)
        {
            Vector3 nextMovePoint = path.Pop().position;

            SetNextMove();
        }
    }

    public override void Attack(int hitBox = 0)
    {
        UnitBase palyer = GameManger.Instance.GetPlayerControl();
        if (palyer.gameObject.layer == palyer.originLayer)
        {
            Debug.Log("몬스터 공격");
            base.Attack(hitBox);
        }
    }

    public override void HandleDeath()
    {
        GameManger.Instance.GetEnemyManger().enemyList.Remove(this);
        base.HandleDeath();
    }

    public void AI()
    {
        if (!isControl || isAttackRate)
        {
            return;
        }

        //공격범위까지
        if ((unitTransform.position - target.GetSkinnedMeshPostionToPostion()).sqrMagnitude < Range)
        {

            unitTransform.LookAt(target.GetUnitTransform().position);
            unitTransform.rotation = Quaternion.Euler(new Vector3(0, unitTransform.rotation.eulerAngles.y, 0));
            AttackTime += Time.deltaTime;
            if (AttackTime > weapon.tick)
            {
                Attack();
                StartCoroutine(AttackRate());
            }
            return;
        }
        else
        {
            AttackTime = 0;
        }


         if (isFind)
        {
            moveToPoint.y = unitTransform.position.y;
            //이동
            if ((unitTransform.position - moveToPoint).sqrMagnitude > 0.01f)
            {
                rigidbody.MovePosition(unitTransform.position + moveDir * speed * Time.deltaTime);
            }
            else
            {
                //Debug.Log("NextNode");
                SetNextMove();

                rigidbody.MovePosition(unitTransform.position + moveDir * speed * Time.deltaTime);
            }
        }


         

    }

    public override void HitEvent(List<Damage> damageList, WeaponBase weapon)
    {
        base.HitEvent(damageList, weapon);
        if(hp > 0)
        KnockBack(weapon.KnockBackTime, weapon.SternTime, weapon.GetParentUnit());

    }

    public override void KnockBack(float KnockBacktime, float SternTime, UnitBase TargetUnit)
    {
        base.KnockBack(KnockBacktime, SternTime, TargetUnit);
        StartCoroutine(HitSetFind());
    }

    IEnumerator HitSetFind()
    {
        while (!isControl)
        {
            yield return null;
        }
        FindPath(GameManger.Instance.GetMapManger().GetPlayerTile().GetTileInfo().point);
        transform.rotation = Quaternion.identity;
        SetNextMove();

    }

}

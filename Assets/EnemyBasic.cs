using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyBasic : UnitBase
{

    public TileBase tile;
    protected TileBase moveToTile;
    public Stack<StructInfo.TileInfo> path;
    protected Vector3 moveToPoint;
    protected Vector3 moveDir;
    protected PathFindTool pathFindTool;
    protected bool isFind;
    public float Range;
    protected UnitBase target;

    protected float AttackTime;

    protected int HitCount;

    protected Sequence rotionTween;


    public Transform hpBar;

    protected UIHpBar uiHpBar;


    

    public override void HandleSpawn()
    {
        base.HandleSpawn();
        path = new Stack<StructInfo.TileInfo>();
        StructInfo.TileInfo moveTile = GameManager.Instance.GetMapManger().GameMap[0, 0].tileInfo;
        pathFindTool = GetComponent<PathFindTool>();
        pathFindTool.Initialize();
        isFind = false;
        target = GameManager.Instance.GetPlayerControl();
        //FindPath(moveTile.point);

        GameObject hpBarGO = Instantiate(Resources.Load("UIHpBar"), Vector3.zero, Quaternion.identity) as GameObject;
        hpBarGO.transform.SetParent(UIManager.Instance.uiDynamic.GetAnchorTransform());
        hpBarGO.transform.localScale = Vector3.one;
        uiHpBar = hpBarGO.GetComponent<UIHpBar>();
        uiHpBar.UpdatePositionFromWorldPosition(hpBar.position);
    }



    protected void SetNextMove()
    {
        if (path.Count == 0)
        {
            moveDir = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
            isFind = false;
            SetLook(GameManager.Instance.GetPlayerControl().GetSkinnedMeshPostionToPostion(), 0);
        }
        else
        {
            StructInfo.Point nextPoint = path.Pop().point;
            moveToTile = GameManager.Instance.GetMapManger().GameMap[nextPoint.y, nextPoint.x];
            moveToPoint = moveToTile.tileInfo.position;
            moveToPoint.y = unitTransform.position.y;
            Debug.Log("TileMove: " + moveToPoint);
            moveDir = (moveToPoint - unitTransform.position).normalized;
           
            if(GetTargetDistance() > Range)
            SetLook(moveToPoint, 0);
        }
    }

    protected void SetLook(Vector3 targetPostion, int ID,float RotionSpeed = 0.5f)
    {
        targetPostion = new Vector3(targetPostion.x, unitTransform.position.y, targetPostion.z);
        Debug.Log("SetLook    " + ID + "targetPosition   " + targetPostion);
        Utility.KillTween(rotionTween);
        rotionTween.Insert(0,unitTransform.DOLookAt(targetPostion, RotionSpeed));
        rotionTween.Play();
    }

    public virtual void FindPath(StructInfo.Point tNode)
    {
        if (!isControl) return;

        isFind = pathFindTool.PathFind_AStar(tile.tileInfo.point, tNode, path);
        //Debug.Log(isFind);
        if (isFind)
        {
            Vector3 nextMovePoint = path.Pop().position;
            SetNextMove();
        }
        else
        {
            Debug.LogWarning(tile.tileInfo.point + "(" + tile.tileInfo.point.y + " , " + tile.tileInfo.point.x + ")"
                + "   (" + tNode.y + "," + tNode.x+ ")");
        }
    }


    public override void Attack(int hitBox = 0)
    {
        UnitBase player = GameManager.Instance.GetPlayerControl();
        if(player.gameObject.layer != player.GhostLayer)
            base.Attack(hitBox);
    }

    public override void HandleDeath()
    {
        GameManager.Instance.GetEnemyManger().enemyList.Remove(this);

        uiHpBar.HandleDestroy();

        base.HandleDeath();
    }
    protected float GetTargetDistance()
    {
        return Mathf.Abs(Vector3.Distance(unitTransform.position,
            target.GetSkinnedMeshPostionToPostion())); 
    }

    public virtual void AI()
    {
      

    }
    private bool isHit = false;


    public override void HitEvent(List<Damage> damageList, WeaponBase weapon)
    {
        //UnityEditor.EditorApplication.isPaused = true;
        base.HitEvent(damageList, weapon);


        
    }

    public override void KnockBack(float KnockBacktime, float SternTime, UnitBase TargetUnit, float Power = 0.8f)
    {
        base.KnockBack(KnockBacktime, SternTime, TargetUnit);
        StartCoroutine(HitSetFind());
    }

    protected IEnumerator HitSetFind()
    {
        yield return new WaitForFixedUpdate();
        while (!isControl)
        {
            yield return null;
        }
        HitCount--;
        if (HitCount > 0) yield break;
        ResetPath();
    }


    protected void ResetPath()
    {
        isAttackRate = false;
        ControlOn();
        GameManager.Instance.GetEnemyManger().ReSetPath();
        if (Range < GetTargetDistance())
        {
            SetNextMove();
            SetLook(moveToPoint, 0);
        }
    }


  


}

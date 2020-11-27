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

    private int HitCount;

    private Sequence rotionTween;
    private Sequence forwardRigMoveTween;

    private float AttackDashTime = 0.5f;

    private bool isForWardAttack;

    public Transform hpBar;

    private UIHpBar uiHpBar;

    private IEnumerator ForwardAttackCorutine;
    

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

    private void Update()
    {
        uiHpBar.UpdatePositionFromWorldPosition(hpBar.position);
    }

    private void FixedUpdate()
    {
        //if (!target.isAlive) return;
        AI();
    }



    private void SetNextMove()
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

    private void SetLook(Vector3 targetPostion, int ID,float RotionSpeed = 0.5f)
    {
        targetPostion = new Vector3(targetPostion.x, unitTransform.position.y, targetPostion.z);
        Debug.Log("SetLook    " + ID + "targetPosition   " + targetPostion);
        Utility.KillTween(rotionTween);
        rotionTween.Insert(0,unitTransform.DOLookAt(targetPostion, RotionSpeed));
        rotionTween.Play();
    }

    public void FindPath(StructInfo.Point tNode)
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


    
    IEnumerator ForWardAttack()
    {
        isControlOff();
        isAttackRate = true;
        Utility.KillTween(rotionTween);
        Utility.KillTween(forwardRigMoveTween);

        yield return new WaitForSeconds(weapon.tick);

        Vector3 forWardDir = weaponSensor.hitBoxs[0].gameObject.transform.position;
        forWardDir = new Vector3(forWardDir.x, unitTransform.position.y, forWardDir.z);
        forWardDir = forWardDir - unitTransform.position;
        forWardDir = forWardDir.normalized;

        Vector3 movePoint = unitTransform.position + (forWardDir * 2);
        float forwardDistance = Vector3.Distance(unitTransform.position, movePoint);
        float CalculationAttackTime = AttackDashTime;
        float CalculationDistance = .0f;
        if (isChackWall(forWardDir, forwardDistance))
        {
            CalculationDistance = raycastHit.distance - ColliderDistance;
            movePoint = unitTransform.position + (unitTransform.forward * CalculationDistance);
            CalculationAttackTime = forwardDistance / raycastHit.distance * CalculationAttackTime;
        }



        forwardRigMoveTween = DOTween.Sequence();
        rotionTween.Insert(0, rigidbody.DOMove(movePoint, CalculationAttackTime));

        rotionTween.Play();
        Attack();
        float time = 0;
        while(time < CalculationAttackTime)
        {
            yield return null;
            time += Time.deltaTime;
            Attack();
        }

        yield return new WaitForSeconds(weapon.tickRate);
        AttackTime = 0;
        ResetPath();

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
    private float GetTargetDistance()
    {
        return Mathf.Abs(Vector3.Distance(unitTransform.position,
            target.GetSkinnedMeshPostionToPostion())); 
    }
    public void AI()
    {
        string msg = string.Empty;
        msg += "AI: " + isControl + "   " + isAttackRate + "\n";
        if (!isControl || isAttackRate)
        {
            //Debug.Log(msg);
            return;
        }

        float targetDistance = GetTargetDistance();

        msg += "플레이어와의 거리: " + targetDistance + " 몬스터의 공격리치: " + Range + "\n";
        
        //공격범위에 들어왔을 떄
        if (targetDistance < Range)
        {
            if (ForwardAttackCorutine != null)
                StopForwardAttackCoroutine();


            SetLook(GameManager.Instance.GetPlayerControl().GetSkinnedMeshPostionToPostion(), 0, 0.1f);

            rigidbody.velocity = Vector3.zero;
            ForwardAttackCorutine = ForWardAttack();
            StartCoroutine(ForwardAttackCorutine);
            return;
        }
        //공격범위가 아닐때
        else if (isFind)
        {
            AttackTime = 0;
            msg += "플레이어까지의 루트 확인: " + isFind + "\n";
            msg += "플레어에게 이동하는 방향: " + moveDir + "\n";
          
            //이동
            if ((unitTransform.position - moveToPoint).sqrMagnitude < 0.01f)
            {
                SetNextMove();
            }
                Vector3 speedrig = moveDir * speed;
                rigidbody.velocity = speedrig;
        }


        //Debug.Log(msg);

    }
    private bool isHit = false;


    public override void HitEvent(List<Damage> damageList, WeaponBase weapon)
    {
        //UnityEditor.EditorApplication.isPaused = true;
        base.HitEvent(damageList, weapon);


        if (hp > 0)
        {
            SetLook(weapon.GetParentUnit().GetSkinnedMeshPostionToPostion(), 2, 0.35f);
            StopForwardAttackCoroutine();


            HitCount++;
            KnockBack(weapon.KnockBackTime, weapon.SternTime, weapon.GetParentUnit());




            uiHpBar.SetValue(hp, MAXHP);
        }
    }

    public override void KnockBack(float KnockBacktime, float SternTime, UnitBase TargetUnit, float Power = 0.8f)
    {
        base.KnockBack(KnockBacktime, SternTime, TargetUnit);
        StartCoroutine(HitSetFind());
    }

    IEnumerator HitSetFind()
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


    private void ResetPath()
    {
        isAttackRate = false;
        isControlOn();
        GameManager.Instance.GetEnemyManger().ReSetPath();
        if (Range < GetTargetDistance())
        {
            SetNextMove();
            SetLook(moveToPoint, 0);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            if(isForWardAttack) isForWardAttack = false;
        }
    }

    private void StopForwardAttackCoroutine()
    {
        if(ForwardAttackCorutine != null)
        {
            StopCoroutine(ForwardAttackCorutine);
        }
    }


}

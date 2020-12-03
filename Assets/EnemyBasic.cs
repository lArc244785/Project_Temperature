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

    private Sequence enemyTween;

    private float AttackDashTime = 0.5f;

    private bool isForWardAttack;

    public Transform hpBar;

    private UIHpBar uiHpBar;


    // Start is called before the first frame update

    //private void Start()
    //{
    //    HandleSpawn();
    //}
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

        GameObject hpBarGO = Instantiate(Resources.Load("UIHPBar"), Vector3.zero, Quaternion.identity) as GameObject;
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
        if (!isControl) return;


        if (path.Count == 0)
        {
            moveDir = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
            isFind = false;
        }
        else
        {
            StructInfo.Point nextPoint = path.Pop().point;
            moveToTile = GameManager.Instance.GetMapManger().GameMap[nextPoint.y, nextPoint.x];
            moveToPoint = moveToTile.tileInfo.position;
            moveToPoint.y = unitTransform.position.y;
            moveDir = (moveToPoint - unitTransform.position).normalized;
            if (!isAttackRate) {
                
                SetLoock(moveToPoint);
              //  unitTransform.DOLookAt(moveToPoint, 1.0f);
            }
        }
    }

    private void SetLoock(Vector3 targetPostion)
    {
        Utility.KillTween(enemyTween);
        enemyTween.Insert(0,unitTransform.DOLookAt(targetPostion, 0.7f));
        enemyTween.Play();
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
            Debug.LogError(tile.tileInfo.point + "   " + tNode);
        }
    }


    
    IEnumerator ForWardAttack()
    {
       // isControlOff();
        isAttackRate = true;
        isForWardAttack = true;
        Vector3 movePoint = unitTransform.position + (unitTransform.forward * 2);
        Utility.KillTween(enemyTween);
        enemyTween = DOTween.Sequence();
        enemyTween.Insert(0, rigidbody.DOMove(movePoint, AttackDashTime));
        enemyTween.Play();

        float time = 0;
        while(time < AttackDashTime)
        {
            time += Time.deltaTime;
            Attack();
            if (!isForWardAttack)
            {
                Utility.KillTween(enemyTween);
                rigidbody.velocity = Vector3.zero;
                yield break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(weapon.tickRate);
       // isControlOn();
        isAttackRate = false;
        AttackTime = 0;
    }


    public override void Attack(int hitBox = 0)
    {
        UnitBase palyer = GameManager.Instance.GetPlayerControl();
        if (palyer.gameObject.layer == palyer.originLayer)
        {
            //Debug.Log("몬스터 공격");
            base.Attack(hitBox);
        }
    }

    public override void HandleDeath()
    {
        GameManager.Instance.GetEnemyManger().enemyList.Remove(this);

        uiHpBar.HandleDestroy();

        base.HandleDeath();
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

        float targetDistance =
            Mathf.Abs(Vector3.Distance(unitTransform.position,
            target.GetSkinnedMeshPostionToPostion()));

        msg += "플레이어와의 거리: " + targetDistance + " 몬스터의 공격리치: " + Range + "\n";
        
        //공격범위까지
        if (targetDistance < Range)
        {
            Vector3 lookPos = target.GetUnitTransform().position;
            lookPos = new Vector3(lookPos.x, unitTransform.position.y, lookPos.z);

            SetLoock(lookPos);
           
            AttackTime += Time.deltaTime;
            msg += "공격 대기 타임: " + AttackTime + "공격 까지의 타임: " + weapon.tick + "\n"; 
            if (AttackTime > weapon.tick)
            {
                StartCoroutine(ForWardAttack());
            }
            return;
        }
        else if (isFind)
        {
            AttackTime = 0;
            msg += "플레이어까지의 루트 확인: " + isFind + "\n";
            msg += "플레어에게 이동하는 방향: " + moveDir + "\n";
            moveToPoint.y = unitTransform.position.y;
            //이동
            if ((unitTransform.position - moveToPoint).sqrMagnitude > 0.01f)
            {
                Vector3 speedrig = moveDir * speed ;
                rigidbody.velocity = speedrig;
            }
            else
            {
                //Debug.Log("NextNode");
                SetNextMove();

                Vector3 speedrig = moveDir * speed;
                rigidbody.velocity = speedrig;
            }
        }


        //Debug.Log(msg);

    }
    private bool isHit = false;


    public override void HitEvent(List<Damage> damageList, WeaponBase weapon)
    {
        base.HitEvent(damageList, weapon);
        if (hp > 0)
        {
            HitCount++;
            KnockBack(weapon.KnockBackTime, weapon.SternTime, weapon.GetParentUnit());

            uiHpBar.SetValue(hp, MAXHP);
        }
    }
    IEnumerator a;
    public override void KnockBack(float KnockBacktime, float SternTime, UnitBase TargetUnit, float Power = 0.8f)
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
        HitCount--;
        if (HitCount > 0) yield break;
        isAttackRate = false;
        FindPath(GameManager.Instance.GetMapManger().GetPlayerTile().GetTileInfo().point);
        transform.rotation = Quaternion.identity;
        SetNextMove();

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            if(isForWardAttack) isForWardAttack = false;
        }
    }



}

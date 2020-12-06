using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ForWardEnemy : EnemyBasic
{
    protected Sequence forwardRigMoveTween;
    protected float AttackDashTime = 0.5f;

    protected bool isForWardAttack;
    protected IEnumerator ForwardAttackCorutine;

    public override void HandleSpawn()
    {
        base.HandleSpawn();
    }

    public override void HandleDeath()
    {
        base.HandleDeath();
    }
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

    private void Update()
    {
        uiHpBar.UpdatePositionFromWorldPosition(hpBar.position);
    }

    private void FixedUpdate()
    {
        //if (!target.isAlive) return;
        AI();
    }


    public override void AI()
    {
        base.AI();
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
            //if (ForwardAttackCorutine != null)
            //    StopForwardAttackCoroutine();


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


    public override void Attack(int hitBox = 0)
    {
        base.Attack(hitBox);
    }


    IEnumerator ForWardAttack()
    {
        ControlOff();
        isAttackRate = true;
        Utility.KillTween(rotionTween);
        Utility.KillTween(forwardRigMoveTween);
        float t = 0.0f;
        while(t < weapon.tick)
        {
            SetLook(GameManager.Instance.GetPlayerControl().GetSkinnedMeshPostionToPostion(), 0, 0.1f);
            t += Time.deltaTime;
            yield return null;
        }

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
        while (time < CalculationAttackTime)
        {
            yield return null;
            time += Time.deltaTime;
            Attack();
        }

        yield return new WaitForSeconds(weapon.tickRate);
        AttackTime = 0;
        ResetPath();

    }

    private void StopForwardAttackCoroutine()
    {
        if (ForwardAttackCorutine != null)
        {
            StopCoroutine(ForwardAttackCorutine);
        }
    }
}

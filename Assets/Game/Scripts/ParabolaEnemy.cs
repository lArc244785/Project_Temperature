using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParabolaEnemy : EnemyBasic
{
    protected bool isParabolaAttack;
    protected IEnumerator ParabolaAttackCoroutine;


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
            if (ParabolaAttackCoroutine != null)
                StopParabolaAttackCoroutine();


            SetLook(GameManager.Instance.GetPlayerControl().GetSkinnedMeshPostionToPostion(), 0, 0.1f);

            rigidbody.velocity = Vector3.zero;
            ParabolaAttackCoroutine = ParabolaAttack();
            StartCoroutine(ParabolaAttackCoroutine);
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


        Debug.Log(msg);

    }

    public override void Attack(int hitBox = 0)
    {
        base.Attack(hitBox);
    }

    IEnumerator ParabolaAttack()
    {
        isControlOff();
        isAttackRate = true;
        Utility.KillTween(rotionTween);

        yield return new WaitForSeconds(weapon.tick);
        weapon.Attack(0);

        yield return new WaitForSeconds(weapon.tickRate);
        AttackTime = 0;
        ResetPath();

    }

    private void StopParabolaAttackCoroutine()
    {
        if (ParabolaAttackCoroutine != null)
        {
            StopCoroutine(ParabolaAttackCoroutine);
        }
    }

}

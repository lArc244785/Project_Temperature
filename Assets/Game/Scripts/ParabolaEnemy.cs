using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParabolaEnemy : EnemyBasic
{
    protected bool isParabolaAttack;
    protected IEnumerator ParabolaAttackCoroutine;

    //public void Start()
    //{
    //    HandleSpawn();
    //}

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
            StopParabolaAttackCoroutine();


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
        if (GameManager.Instance.isGameOver) return;
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
            //if (ParabolaAttackCoroutine != null)
            //    StopParabolaAttackCoroutine();

            rigidbody.velocity = Vector3.zero;
            ParabolaAttackCoroutine = ParabolaAttack();
            StartCoroutine(ParabolaAttackCoroutine);
            return;
        }
        else
        {
            AttackTime = 0;
        }

    }

    public override void Attack(int hitBox = 0)
    {
        base.Attack(hitBox);

    }

    IEnumerator ParabolaAttack()
    {
        ControlOff();
        isAttackRate = true;

        float t = 0.0f;
        while (t < weapon.tick)
        {
            SetLook(GameManager.Instance.GetPlayerControl().GetSkinnedMeshPostionToPostion(), 0, 0.1f);
            t += Time.deltaTime;
            yield return null;
        }


        //yield return new WaitForSeconds(weapon.tick);
        Utility.KillTween(rotionTween);
        weapon.SetShootAttackPath();
        weapon.Attack(0);

        AudioPool.Instance.Play2D("Monster_Dragon_Attack");

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

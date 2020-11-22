using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : WeaponBase
{
    public override void Initializer(Transform wt, UnitBase parent)
    {
        base.Initializer(wt, parent);
    }


    public override void Attack(int hitBox = 0)
    {
        Debug.Log("HitBox:" + hitBox);
        base.Attack(hitBox);
    }
}

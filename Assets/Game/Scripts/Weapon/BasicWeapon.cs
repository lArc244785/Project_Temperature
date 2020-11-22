using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : WeaponBase
{
    public override void Initializer(Transform wt, UnitBase parent)
    {
        base.Initializer(wt, parent);
    }


    public override void Attack()
    {
        base.Attack();
        Debug.Log("NomalAttack");
    }
}

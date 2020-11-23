using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponBase", menuName = "Data/WeaponBaseData", order = 2)]
public class WeaponKnockBack : WeaponBase
{

    public override void Initializer(Transform wt, UnitBase parent)
    {
        base.Initializer(wt, parent);
        SetTarget(GameManager.Instance.GetPlayerControl());
    }

    public override void Attack(int hitBox = 0)
    {
        base.Attack(hitBox);
        foreach(UnitBase unit in targetUnits)
        {
            unit.KnockBack(KnockBackTime, SternTime, parentUnit, KnockBackPower);
        }

    }

}

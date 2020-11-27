using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponBase", menuName ="Data/WeaponBaseData", order = 1)]
public class WeaponBase : ScriptableObject
{
    protected UnitBase parentUnit;
    protected List<UnitBase> targetUnits;
    protected Transform weaponTransfrom;
    public List<Damage> damageList;
    private WeaponSensor weaponSensor;
    public float tick;
    public float tickRate;
    public float KnockBackTime;
    public float SternTime;
    public float KnockBackPower;
    public virtual void Initializer(Transform wt, UnitBase parent)
    {
        weaponTransfrom = wt;
        parentUnit = parent;
        targetUnits = new List<UnitBase>();
        weaponSensor = parent.weaponSensor;

    }

    public virtual void SetTarget(UnitBase targetUnit)
    {
        targetUnits.Clear();
        targetUnits.Add(targetUnit);
    }

    public virtual void SetTarget(int hitBox)
    {
        targetUnits.Clear();
        targetUnits = weaponSensor.GetSensorHitUnits( hitBox);
    }

    //공격도 손봐야됨
    public virtual void Attack(int hitBox) {
        SetTarget(hitBox);

        if (targetUnits.Count > 0)
        {
            foreach (UnitBase unit in targetUnits)
            {

                unit.HitEvent(damageList, this);
            }

            LayerMask layer = 1 << LayerMask.NameToLayer("Enemy");

            if (parentUnit.targetLayer == layer)
            {
                PlayerControl pc = GameManager.Instance.GetPlayerControl();
                pc.WeaponTimeAction();
            }
        }
    }

    public UnitBase GetParentUnit()
    {
        return parentUnit;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponBase", menuName ="Data/WeaponBaseData", order = 1)]
public class WeaponBase : ScriptableObject
{
    private  UnitBase parentUnit;
    private UnitBase targetUnit;
    private Transform weaponTransfrom;
    public List<Damage> damageList;
    public float tickRate;

    public virtual void Initializer(Transform wt, UnitBase parent)
    {
        weaponTransfrom = wt;
        parentUnit = parent;
    }

    public virtual void SetTarget()
    {

    }

    public virtual void Attack() { 
        foreach(Damage var in damageList)
        {
            Debug.Log("Code 2: " + var.DamageType + "\t" + var.damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAmmo : AmmoBase
{

    public override void Initialize(UnitBase targetUnitBaic, WeaponBase weaponBase)
    {
        base.Initialize(targetUnitBaic, weaponBase);
    }


    public void Update()
    {
        if (curTick >= lifeTick) return;

            curTick += Time.deltaTime;
        if (curTick >= lifeTick) HandleDestory();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (((1 << other.gameObject.layer) & hitLayerMask) != 0)
        {
            var hitUnitBase = other.GetComponent<UnitBase>();
            if(hitUnitBase != null)
            {
                hitUnitBase.HitEvent(weponBase.damageList, weponBase);
                HandleHit();
            }
        }
    }



}

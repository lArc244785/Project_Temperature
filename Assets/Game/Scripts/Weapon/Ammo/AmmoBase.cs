using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AmmoBase : MonoBehaviour
{
    public Tween ammoMoveTween;

    protected LayerMask backGroundLayer;
    public LayerMask hitLayerMask;

    protected float lifeTick;
    protected float curTick;

    protected Transform transform;

    protected UnitBase targetUnitBase;
    protected WeaponBase weponBase;


    public virtual void Initialize(UnitBase targetUnitBaic, WeaponBase weaponBase)
    {
        this.targetUnitBase = targetUnitBase;
        this.weponBase = weaponBase;

        curTick = 0.0f;
        lifeTick = weaponBase.ShotBulletLifeTick;
    }

    public virtual void HandleHit()
    {
        Destroy(this.gameObject);
    }

    public virtual void HandleDestory()
    {
        Utility.KillTween(ammoMoveTween);
        Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & backGroundLayer) != 0)
        {
            HandleDestory();
        }
    }

}

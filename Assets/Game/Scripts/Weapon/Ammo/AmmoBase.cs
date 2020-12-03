using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[SerializeField]
public class AmmoBase : MonoBehaviour
{
    public Tween ammoMoveTween;

    public LayerMask backGroundLayer;
    public LayerMask hitLayerMask;

    protected float lifeTick;
    protected float curTick;

    protected Transform transform;

    protected UnitBase targetUnitBase;
    protected WeaponBase weponBase;


    public virtual void Initialize(UnitBase targetUnitBase, WeaponBase weaponBase)
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

    }

}

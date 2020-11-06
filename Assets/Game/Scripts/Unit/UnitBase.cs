using System.Collections;
using UnityEngine;

public class UnitBase : Status
{
    protected Transform unitTransform;
    protected Rigidbody rigidbody;
    public Transform weaponTransfrom;
    public WeaponBase weapon;
    public Transform modelTransfrom;
    public Animator modelAni;
    public WeaponSensor weaponSensor;

    //공격을 했나?
    public bool isAttackRate = false;
    public virtual void Initializer()
    {
        hp = MAXHP;


        unitTransform = transform;

        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            Debug.LogWarning("Code 100: rigidbody Null");
        }

        if (weaponTransfrom == null)
        {
            weaponTransfrom = transform.FindChild("WeaponTransform");
            if (weaponTransfrom == null)
            {
                Debug.LogWarning("Code 100: weaponTransfrom Null");
            }
        }
        if (weapon != null)
        {
            weapon.Initializer(weaponTransfrom, this);
        }
        else
        {
            Debug.LogWarning("Code 100: weapon Null");
        }

    }

    public virtual void Attack()
    {
        //AniTrigger
        weapon.Attack();
        if (weaponSensor != null)
            weaponSensor.HitEventOn();
    }

    public virtual void HandleDeath()
    {

    }

    public virtual void HandleSpawn()
    {

    }


    public IEnumerator AttackRate()
    {
        isAttackRate = true;
        yield return new WaitForSeconds(weapon.tickRate);
        isAttackRate = false;
    }


    public Rigidbody GetRigidbody()
    {
        return rigidbody;
    }



}

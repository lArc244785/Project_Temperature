using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : Status
{
    [Header("Move")]
    protected Transform unitTransform;
    protected Rigidbody rigidbody;

    [Header("Weapon")]
    public Transform weaponTransfrom;
    public WeaponBase weapon;
    public WeaponSensor weaponSensor;
    [Header("Model")]
    public Transform modelTransfrom;
    public Animator modelAni;

    [Header("Control")]
    public bool isControl;
    public bool isInputAction = true;

    [Header("=====")]
    public int originLayer;
    //Ghost Layer
    public int GhostLayer = 10;


    protected bool isKnockBackOn;

    [Header("=Target=")]
    public LayerMask targetLayer;
    public string targetName;
    [Header("=======")]
    //공격을 했나?
    public bool isAttackRate = false;


    public PlayerMotionHandler motionHandler;

    public SkinnedMeshRenderer SkinnedMesh;
    public MeshRenderer meshRender;

    public UnitHandler unitHandler;

    protected Sequence sequence;

    [Header("Material")]
    public Material IdleMaterial;
    public Material HitMaterial;
    public Material GhostMaterial;

    public virtual void Initializer()
    {
        originLayer = gameObject.layer;

        hp = MAXHP;
        isControl = true;

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
            if (weaponSensor != null)
            {
                weaponSensor.Initializer(weapon);
            }
        }
        else
        {
            Debug.LogWarning("Code 100: weapon Null");
        }




        if (unitHandler != null) unitHandler.Initializer(this);


    }


    public virtual void Attack(int hitBox = 0)
    {
        if (!isControl && !isInputAction) return;
        //AniTrigger
        if (weapon != null)
        {
            weapon.Attack(hitBox);
        }
        else
        {
            Debug.LogWarning("Weapon is NULL!!");
        }
    }



    public virtual void HandleDeath()
    {
        gameObject.active = false;
    }

    public virtual void HandleSpawn()
    {
        Initializer();
    }

    public virtual void HitEvent(List<Damage> damageList, WeaponBase weapon)
    {

        transform.position = GetSkinnedMeshPostionToPostion();
        if (modelAni != null)
            modelAni.SetTrigger("Hit");

        Debug.Log("Hit!!" + Name + "Attack Unit " + weapon.GetParentUnit().name);

        foreach (Damage damage in damageList)
        {
            switch (damage.DamageType)
            {
                case EnumInfo.DamageType.Nomal:
                    hp = Mathf.Lerp(hp - damage.damage, MAXHP, 0);
                    break;
                case EnumInfo.DamageType.Hot:
                    temperature += damage.damage;
                    break;
                case EnumInfo.DamageType.Cold:
                    temperature -= damage.damage;
                    break;
            }
        }

        if (hp <= 0)
        {
            isAlive = false;
            HandleDeath();
        }

        MaterialChange(EnumInfo.Materia.Hit);
    }

    public virtual void KnockBack(float KnockBacktime, float SternTime, UnitBase TargetUnit, float Power = 0.8f)
    {
        if (isKnockBackOn)
        {
            StopCoroutine("IE_KnockBack");
            isKnockBackOn = false;
        }

        StartCoroutine(IE_KnockBack(KnockBacktime, SternTime, TargetUnit, Power));
    }

    public IEnumerator IE_KnockBack(float KnockBacktime, float SternTime, UnitBase TargetUnit, float Power = 0.8f)
    {
        Utility.KillTween(sequence);

        isControl = false;
        isInputAction = false;
        isKnockBackOn = true;

        Vector3 knockBackDir = unitTransform.position - TargetUnit.unitTransform.position;
        knockBackDir.y = 0.0f;
        knockBackDir = knockBackDir.normalized * Power;

        sequence = DOTween.Sequence();
        Vector3 movePoint = transform.position + knockBackDir;
        sequence.Insert(0, rigidbody.DOMove(movePoint, KnockBacktime));
        sequence.Play();
        float t = 0.0f;
        while (t < KnockBacktime)
        {
            if (isMoveDirWall(knockBackDir))
            {
                StopTween();
                rigidbody.velocity = Vector3.zero;
            }
            t += Time.deltaTime;
        }

        rigidbody.velocity = Vector3.zero;
        yield return new WaitForSecondsRealtime(SternTime);
        isKnockBackOn = false;

        isControl = true;
        isInputAction = true;
    }

    public IEnumerator AttackRate()
    {
        isAttackRate = true;
        yield return new WaitForSeconds(weapon.tickRate);


        if (weapon != null)
        {
            Debug.Log(weapon.tickRate);
            yield return new WaitForSeconds(weapon.tickRate);
        }
        isAttackRate = false;
        
    }


    public Rigidbody GetRigidbody()
    {
        return rigidbody;
    }


    public Transform GetUnitTransform()
    {
        return unitTransform;
    }

    public void SetSkinnedMeshPostionToPostion()
    {
        unitTransform.position = weaponSensor.transform.position = new Vector3(
            SkinnedMesh.bounds.center.x,
            unitTransform.position.y,
            SkinnedMesh.bounds.center.z);
    }

    public Vector3 GetSkinnedMeshPostionToPostion()
    {
        if (SkinnedMesh == null) return unitTransform.position;
        return new Vector3(
            SkinnedMesh.bounds.center.x,
            unitTransform.position.y,
            SkinnedMesh.bounds.center.z);
    }



    public void isInputActionOn()
    {
        isInputAction = true;
    }

    public void isInputActionOff()
    {
        isInputAction = false;
    }

    public void isControlOn()
    {
        isControl = true;
    }

    public void isControlOff()
    {
        isControl = false;
    }

    public void StopTween()
    {
        Utility.KillTween(sequence);
    }

    private bool isMoveDirWall(Vector3 Dir)
    {
        return Physics.Raycast(transform.position, Dir, 0.8f, 1 << LayerMask.NameToLayer("Wall"));
    }

    private Sequence materialTween;
    public void MaterialChange(EnumInfo.Materia materia)
    {
        Utility.KillTween(materialTween);
        materialTween = DOTween.Sequence();
        isTweenEventing = false;
        switch (materia)
        {
            case EnumInfo.Materia.Idle:
                if (SkinnedMesh == null)
                {
                    meshRender.material = IdleMaterial;
                }
                else
                {
                    SkinnedMesh.material = IdleMaterial;
                }
                break;
            case EnumInfo.Materia.Hit:
                if (SkinnedMesh == null)
                {
                    meshRender.material = HitMaterial;
                    materialTween.Insert(
0, meshRender.material.DOColor(new Color(1, 0.55f, 0.55f), 0.15f).SetLoops(2, LoopType.Yoyo)
);

                }
                else
                {
                    SkinnedMesh.material = HitMaterial;
                    materialTween.Insert(
    0, SkinnedMesh.material.DOColor(new Color(1, 0.55f, 0.55f), 0.15f).SetLoops(2, LoopType.Yoyo)
);
                }
                StartCoroutine(ChangeTween(0.3f, EnumInfo.Materia.Idle));
                break;
            case EnumInfo.Materia.Ghost:
                if (SkinnedMesh == null)
                {
                    meshRender.material = GhostMaterial;
                    materialTween.Insert(
0, meshRender.material.DOColor(new Color(0.3f, 0.3f, 0.3f), 0.5f).SetLoops(-1, LoopType.Yoyo)
);
                }
                else
                {
                    SkinnedMesh.material = GhostMaterial;
                    materialTween.Insert(
0, SkinnedMesh.material.DOColor(new Color(0.3f, 0.3f, 0.3f), 0.5f).SetLoops(100, LoopType.Yoyo)
);
                }
                StartCoroutine(ChangeTween(3.0f, EnumInfo.Materia.Idle));
                break;
        }
        materialTween.Play();
    }

    private bool isTweenEventing = false;
    protected IEnumerator ChangeTween(float time, EnumInfo.Materia nextMateria)
    {
        isTweenEventing = true;
        float t  = .0f;
        while(t < time )
        {
            t += Time.unscaledDeltaTime;
            if (!isTweenEventing)
            {
                yield break;
            }
            yield return null;
        }
        MaterialChange(nextMateria);
        isTweenEventing = false;
    }


}

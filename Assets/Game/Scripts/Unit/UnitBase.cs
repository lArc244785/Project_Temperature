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

    public CapsuleCollider capsuleCollider;

    protected bool isKnockBackOn;
    protected IEnumerator KnockBackCorutine;
    protected Sequence KnockBackTween;
    protected float ColliderDistance;

    public LayerMask WallChackLayer;

    public TileSensor tileSensor;

    public virtual void Initializer()
    {
        originLayer = gameObject.layer;

        hp = MAXHP;
        isControlOn();

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
            weapon = Instantiate(weapon);
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

        
        ColliderDistance = (capsuleCollider.gameObject.transform.lossyScale.x * capsuleCollider.radius) + 0.1f;

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
        Utility.KillTween(sequence);
        isAlive = false;
        StopAllCoroutines();
        gameObject.active = false;
    }

    public virtual void HandleSpawn()
    {
        Initializer();
    }

    public virtual void HitEvent(List<Damage> damageList, WeaponBase weapon)
    {
        
        SetSkinnedMeshPostionToPostion();

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
            isKnockBackOn = false;
        }

        if (KnockBackCorutine != null)
        {
            StopCoroutine(KnockBackCorutine);

        }

        KnockBackCorutine = IE_KnockBack(KnockBacktime, SternTime, TargetUnit, Power);

        StartCoroutine(KnockBackCorutine);
    }

    public IEnumerator IE_KnockBack(float KnockBacktime, float SternTime, UnitBase TargetUnit, float Power = 0.8f)
    {
        StopKnockBackTween();
        isControlOff();
        rigidbody.velocity = Vector3.zero;
        isInputAction = false;
        isKnockBackOn = true;

        Vector3 knockBackDir = unitTransform.position - TargetUnit.unitTransform.position;
        knockBackDir.y = 0.0f;
        knockBackDir = knockBackDir.normalized ;


        Vector3 movePoint = GetSkinnedMeshPostionToPostion() + (knockBackDir * Power);
        float knockBackDistance = Vector3.Distance(transform.position, movePoint);
        //원래 이동 방향에 벽이 있일 경우 재 수정
        if(isChackWall(knockBackDir, knockBackDistance))
        {
            float fixMoveScalar = raycastHit.distance - capsuleCollider.radius;
            if (fixMoveScalar < 0) fixMoveScalar = 0.0f;
             movePoint = GetSkinnedMeshPostionToPostion() + (knockBackDir * fixMoveScalar);
        }

        KnockBackTween = DOTween.Sequence();
        KnockBackTween.Insert(0, rigidbody.DOMove(movePoint, KnockBacktime));
        KnockBackTween.Play();
        
        
        float t = 0.0f;

        yield return new WaitForSeconds(KnockBacktime);

        rigidbody.velocity = Vector3.zero;

        yield return new WaitForSecondsRealtime(SternTime);


        isKnockBackOn = false;
        isControlOn();
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

    //SkineedMesh로 움직이는 애니메이션이 끝났을 때 Player.Postion을 SkineedMesh위치로  Update
    public void SetSkinnedMeshPostionToPostion()
    {
        if (SkinnedMesh == null) return;

        capsuleCollider.enabled = false;

        unitTransform.position = weaponSensor.transform.position = new Vector3(
            SkinnedMesh.bounds.center.x,
            unitTransform.position.y,
            SkinnedMesh.bounds.center.z);

        capsuleCollider.enabled = true;
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
        if (modelAni != null)
            modelAni.SetBool("isControl", true);
    }

    public void isControlOff()
    {
        isControl = false;
        if(modelAni != null)
        modelAni.SetBool("isControl", false);
    }

    public void StopTween()
    {
        Utility.KillTween(sequence);
    }
    
    public void StopKnockBackTween()
    {
        Utility.KillTween(KnockBackTween);
    }


    //private bool isMoveDirWall(Vector3 Dir)
    //{
    //    return Physics.Raycast(transform.position, Dir, 0.8f,  LayerMask.NameToLayer("Wall"));
    //}

    private Sequence materialTween;
    public void MaterialChange(EnumInfo.Materia materia)
    {
        if (!isAlive)
            return;
        Utility.KillTween(materialTween);
        materialTween = DOTween.Sequence();
        isTweenEventing = false;
        switch (materia)
        {
            case EnumInfo.Materia.Idle:
                if (SkinnedMesh == null)
                {
                    meshRender.material.color = Color.white;
                }
                else
                {
                    SkinnedMesh.material.color = Color.white;
                }
                break;
            case EnumInfo.Materia.Hit:
                if (SkinnedMesh == null)
                {
                    meshRender.material.color = Color.red;
                    materialTween.Insert(
0, meshRender.material.DOColor(new Color(1, 0.55f, 0.55f), 0.15f).SetLoops(2, LoopType.Yoyo)
);

                }
                else
                {
                    SkinnedMesh.material.color = Color.red;
                    materialTween.Insert(
    0, SkinnedMesh.material.DOColor(new Color(1, 0.55f, 0.55f), 0.15f).SetLoops(2, LoopType.Yoyo)
);
                }
                StartCoroutine(ChangeTween(0.3f, EnumInfo.Materia.Idle));
                break;
            case EnumInfo.Materia.Ghost:
                if (SkinnedMesh == null)
                {
                    meshRender.material.color = Color.black;
                    materialTween.Insert(
0, meshRender.material.DOColor(new Color(0.3f, 0.3f, 0.3f), 0.5f).SetLoops(-1, LoopType.Yoyo)
);
                }
                else
                {
                    SkinnedMesh.material.color = Color.black;
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


    protected RaycastHit raycastHit;
    protected bool isChackWall(Vector3 dir, float distance)
    {
        return Physics.Raycast(transform.position,  dir, out raycastHit, distance, WallChackLayer);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAmmo : AmmoBase
{
    Vector3 targetPos;
    Vector3 currentPos;
    Vector3 currentPosVellocity;

    private Transform Model;
    private Transform tr;

    public float speed = 1.0f;
    public override void Initialize(UnitBase targetUnitBase, WeaponBase weaponBase)
    {
        base.Initialize(targetUnitBase, weaponBase);
    }

    private void Start()
    {
        tr = gameObject.transform;
        SetTarget();

        Model = gameObject.transform.FindChild("Model");
        currentPos = Model.position;
        currentPosVellocity = currentPos;
    }

    public void Update()
    {
        if (curTick >= lifeTick) return;

            curTick += Time.deltaTime;
        if (curTick >= lifeTick) HandleDestory();

        SetTarget();
        currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref currentPosVellocity, speed);
        Model.position = currentPos;
        Model.localPosition = new Vector3(Model.localPosition.x, 0, Model.localPosition.z);

    }


    private void SetTarget()
    {
        targetPos = targetUnitBase.GetSkinnedMeshPostionToPostion();
        targetPos.y = tr.position.y;

    }





    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (((1 << other.gameObject.layer) & hitLayerMask) != 0)
        {
            var hitUnitHandler = other.GetComponent<UnitHandler>();

            if(hitUnitHandler != null)
            {
                var hitUnit = hitUnitHandler.GetUnit();
                hitUnit.HitEvent(weponBase.damageList, weponBase);
                HandleHit();
            }
            else
            {
                //몬스터
            }

        }
    }



}

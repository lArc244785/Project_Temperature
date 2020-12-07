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
    private float DownSpeed = 1.0f;

    private bool isTrackingMode = false;
    public float range;
    public override void Initialize(UnitBase targetUnitBase, WeaponBase weaponBase)
    {
        base.Initialize(targetUnitBase, weaponBase);
    }

    private void Start()
    {
        tr = gameObject.transform;
        Model = gameObject.transform.FindChild("Model");

    }

    public void Update()
    {
        if (curTick >= lifeTick) return;

            curTick += Time.deltaTime;
        if (curTick >= lifeTick) HandleDestory();

        //if (isTrackingMode)
        //{
        //    SetTarget();
        //    currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref currentPosVellocity, speed);
        //    tr.position = currentPos;
        //}

    }


    private void SetTarget()
    {
        targetPos = targetUnitBase.GetSkinnedMeshPostionToPostion();
        targetPos = new Vector3(targetPos.x, 0.0f, targetPos.z);
    }





    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        
        if(other.gameObject.tag == "Wall")
        {
            HandleDestory();
        }

            if (((1 << other.gameObject.layer) & backGroundLayer) != 0)
            {
            if(((1<< other.gameObject.layer) & LayerMask.GetMask("Tile")) != 0)
            {
                SetTarget();
                if (Vector3.Distance(tr.position, targetPos) <= range)
                {
                    targetUnitBase.HitEvent(weponBase.damageList, weponBase);
                    targetUnitBase.KnockBack(weponBase.KnockBackTime, weponBase.SternTime, weponBase.GetParentUnit(), weponBase.KnockBackPower);
                }
            }
                HandleDestory();
            }
        

        if (((1 << other.gameObject.layer) & hitLayerMask) != 0)
        {
            var hitUnitHandler = other.GetComponent<UnitHandler>();

            if(hitUnitHandler != null)
            {
                var hitUnit = hitUnitHandler.GetUnit();
                hitUnit.HitEvent(weponBase.damageList, weponBase);
                hitUnit.KnockBack(weponBase.KnockBackTime, weponBase.SternTime, weponBase.GetParentUnit(), weponBase.KnockBackPower);
                HandleHit();
            }
            else
            {
                //몬스터
            }

        }
    }


    public void TrackingModeOn()
    {
        SetTarget();
        currentPos = tr.position;
       // currentPosVellocity = currentPos;

        isTrackingMode = true;
    }

}

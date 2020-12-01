using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSensor : MonoBehaviour
{
    private bool isHitEvent;
    private bool isHitAction;
    PlayerControl pc;
    WeaponBase weapon;
    public HitBox[] hitBoxs;


    public void Initializer(WeaponBase weapon)
    {
        this.weapon = weapon;

        pc = GameManager.Instance.GetPlayerControl();
        if(hitBoxs == null)
        {
            Debug.LogError("No Weapon HitBox!!!!");
        }

        foreach(HitBox box in hitBoxs)
        {
            box.Intializer(weapon.GetParentUnit());
        }

    }



    //타겟을 보냄
    public List<UnitBase> GetSensorHitUnits(int Index = 0)
    {
        List<UnitBase> targetUnits;
        Debug.Log(weapon.GetParentUnit().name);
        targetUnits = hitBoxs[Index].GetHitBoxInEnmey();

        return targetUnits;
    }


}

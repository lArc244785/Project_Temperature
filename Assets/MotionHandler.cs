using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionHandler : MonoBehaviour
{
    UnitBase unit;

    public void Initializer(UnitBase unit)
    {
        this.unit = unit;
    }
    public void HandleAttack(int hitBox)
    {
        unit.Attack(hitBox);
    }

    public void HandleSetSkinnedMeshPostionToPostion()
    {
        unit.SetSkinnedMeshPostionToPostion();
    }

    public void isInputActionOn()
    {
        unit.isInputActionOn();
    }

    public void isInputActionOff()
    {
        unit.isInputActionOff();
    }

}

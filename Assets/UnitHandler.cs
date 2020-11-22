using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    private UnitBase unit;
    public void Initializer(UnitBase unit)
    {
        this.unit = unit;
    }

    public UnitBase GetUnit()
    {
        return unit;
    }



}

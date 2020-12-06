using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSystem : MonoBehaviour
{
   private UnitBase unit;
    private TimeManager time;

    public float nightDown;
    public float dayUp;

    public void Initializer(UnitBase unit)
    {
        this.unit = unit;
        time = GameManager.Instance.GetTimeManager();
    }

    private void Update()
    {
        NatureTemperature();
    }

    private void NatureTemperature()
    {
        //낮
        if (!time.isNight)
        {
            unit.currentTemperature += dayUp * Time.deltaTime;

        }
        //밤
        else
        {
            unit.currentTemperature -= nightDown * Time.deltaTime;
        }
        ChakTemperature();
    }

    private void ChakTemperature()
    {
        if (unit.currentTemperature > unit.HotTemperatuer)
        {
            Debug.Log("감당 가능한 온도를 넘었습니다.");
        }
        else if(unit.currentTemperature < unit.ColdTemperatuer)
        {
            Debug.Log("온도가 너무 낮습니다.");

        }
        else
        {

        }
    }


    public void addTemperature(float temper)
    {
        unit.currentTemperature += temper;
    }


}

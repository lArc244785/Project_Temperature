using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPMSystem : MonoBehaviour
{
    private UnitBase unit;
    private float currentBPM;
    private const float MIN_BPM = 60.0f;
    private const float MAX_BPM = 150.0f;

    private IEnumerator RecoveryBPM;
    private IEnumerator WalkBPM;

    public void Initializer(UnitBase unit)
    {
        currentBPM = 70.0f;
        this.unit = unit;
    }

    public void AddBPM(float bpm)
    {
        currentBPM += bpm;
        Clamp();
    }

    float nerf = 0.2f;

    public float GetBPMTemperature()
    {


        if (unit.GetTemperature() > unit.temperature)
        {

            if (currentBPM < 70)
            {
                return -0.5f;
            }
            else if (currentBPM < 80)
            {
                return -0.3f;
            }
            else if (currentBPM < 90)
            {
                return -0.1f;
            }
        }

         if(currentBPM > 90.0f && currentBPM < 100)
        {
            return 0.1f* nerf;
        }else if(currentBPM > 100 && currentBPM < 130)
        {
            return 0.3f * nerf;
        }
        else if(currentBPM > 130)
        {
            return 0.5f * nerf;
        }

        return 0.0f;
    }

    public void RecoveryBPMCoroutineOn()
    {
        if(RecoveryBPM == null)
        {
            RecoveryBPM = RecoveryBPMCoroutine();
            StartCoroutine(RecoveryBPM);
        }
    }

    public void RecoveryBPMCoroutineOff()
    {
        if (RecoveryBPM != null)
        {
            StopCoroutine(RecoveryBPM);
            RecoveryBPM = null;
        }
    }


    IEnumerator RecoveryBPMCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        while(currentBPM > MIN_BPM )
        {
            currentBPM -= 1.0f * Time.deltaTime;
            Clamp();
            yield return null;
        }
    }

    public void WalkBPMCoroutineOn()
    {
        if (WalkBPM == null)
        {
            WalkBPM = WalkBPMCoroutine();
            StartCoroutine(WalkBPM);
        }
    }

    public void WalkBPMCoroutineOff()
    {
        if (WalkBPM != null)
        {
            StopCoroutine(WalkBPM);
            WalkBPM = null;
        }
    }



    IEnumerator WalkBPMCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (currentBPM < MAX_BPM)
        {
            currentBPM += 1.0f * Time.deltaTime;
            Clamp();
            yield return null;
        }
    }

    public string GetBPMString()
    {
        return currentBPM.ToString("F");
    }

    private void Clamp()
    {
        currentBPM = Mathf.Clamp(currentBPM , MIN_BPM, MAX_BPM );
    }

}

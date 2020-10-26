using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSensor : MonoBehaviour
{
    private bool isHitEvent;
    private bool isHitAction;
    PlayerControl pc;

    private void Start()
    {
        pc = GameMagner.Instance.GetPlayerControl();
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Enemy")
        {
            if (isHitEvent)
            {
                print("Enem Hit");
                if (isHitAction)
                    pc.WeaponTimeAction();
            }

        }
    }

    public void HitEventOn()
    {
        isHitEvent = true;
    }
    public void HitEvnetOff()
    {
        isHitEvent = false;
    }

    public void HitActionOn()
    {
        isHitAction = true;
    }

    public void HitActionOff()
    {
        isHitAction = false;
    }

}

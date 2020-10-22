using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSensor : MonoBehaviour
{
    private bool isAction;
    PlayerControl pc;

    private void Start()
    {
        pc = GameMagner.Instance.GetPlayerControl();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAction) return;
        if(other.tag == "Enemy")
        {
            pc.WeaponTimeAction();
            isAction = false;
        }
    }

    public void ActionStart()
    {
        isAction = true;
    }
}

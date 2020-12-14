using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public bool isPlayerHitBox;
    public float Range;
    public Color GizmosColor;
    private GameObject fx_attack;


    public void Intializer(UnitBase parentUnit)
    {
        isPlayerHitBox = parentUnit.Name == "Player" ? true : false;
        if (isPlayerHitBox)
        {
            fx_attack = transform.GetChild(0).gameObject;
            
        }
    }



    public List<UnitBase> GetHitBoxInEnmey()
    {

        return GameManager.Instance.GetEnemyManger().GetHitBoxInEnemy(isPlayerHitBox, transform.position, Range);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    public void Fx_attackOn()
    {
        if (fx_attack == null) return;
        fx_attack.SetActive(false);
        fx_attack.SetActive(true);
    }

    public void Fx_attackOff()
    {
        if (fx_attack == null) return;
        fx_attack.SetActive(false);
    }
}


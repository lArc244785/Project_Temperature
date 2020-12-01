using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public bool isPlayerHitBox;
    public float Range;
    public Color GizmosColor;


    public void Intializer(UnitBase parentUnit)
    {
        isPlayerHitBox = parentUnit.Name == "Player" ? true : false; 
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


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    private PlayerControl playerControl;
    public Animator UnitAni;
    private int MaxCombo = 3;
    private int currentCombo;

    private bool IsComboPossible = true;


    private void Start()
    {
        playerControl = GameMagner.Instance.GetPlayerControl();
    }

    public void Attack()
    {
        if (!IsComboPossible) return;
        playerControl.isMove = false;

        if (currentCombo == 0)
        {
            UnitAni.SetTrigger("Attack");
            currentCombo++;
        }

        else if ( currentCombo <= MaxCombo)
        {
            currentCombo++;
        }

        UnitAni.SetInteger("AttackCombo", currentCombo);
        playerControl.weaponSensor.HitEventOn();
        playerControl.weaponSensor.HitActionOn();
        IsComboPossible = false;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        UnitAni.SetInteger("AttackCombo", currentCombo);
        ComboPossible();
        WeaponHItEventOff();
    }

    public void isMovePossible()
    {
        playerControl.isMove = true;
    }

    public void ComboPossible()
    {
        IsComboPossible = true;
    }

    public void WeaponHItEventOff()
    {
        playerControl.weaponSensor.HitEvnetOff();
    }
    
}

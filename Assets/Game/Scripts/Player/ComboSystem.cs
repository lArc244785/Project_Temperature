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

    public bool Attack()
    {
        if (!IsComboPossible) return false;
        bool isAttack = false;
        playerControl.isMove = false;
        if (currentCombo == 0)
        {
            playerControl.GetRigidbody().velocity = Vector3.zero;
            UnitAni.SetTrigger("Attack");
            currentCombo++;
            isAttack = true;
        }

        else if ( currentCombo <= MaxCombo)
        {
            currentCombo++;
            isAttack = true;
        }

        UnitAni.SetInteger("AttackCombo", currentCombo);
        //playerControl.weaponSensor.HitEventOn();
        //playerControl.weaponSensor.HitActionOn();
        IsComboPossible = false;

        return isAttack;
    }

    public void MoveAction()
    {
        playerControl.ActionMove();
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

    public void WeaponHitOn() {
        playerControl.weaponSensor.HitEventOn();
        playerControl.weaponSensor.HitActionOn();
    }


}

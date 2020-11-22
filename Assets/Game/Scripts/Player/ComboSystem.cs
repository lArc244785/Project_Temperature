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
    private int hitBoxSize;

    private void Start()
    {
        playerControl = GameManger.Instance.GetPlayerControl();
        hitBoxSize = playerControl.weaponSensor.hitBoxs.Length;
    }

    public bool isPushCombo()
    {
        if (!IsComboPossible) return false;
        bool isComboOn = false;
        playerControl.isMove = false;
        if (currentCombo == 0)
        {
            playerControl.GetRigidbody().velocity = Vector3.zero;
            UnitAni.SetTrigger("Attack");
            currentCombo++;
            isComboOn = true;
        }

        else if ( currentCombo <= MaxCombo)
        {
            currentCombo++;
            isComboOn = true;
        }

        UnitAni.SetInteger("AttackCombo", currentCombo);
        //playerControl.weaponSensor.HitEventOn();
        //playerControl.weaponSensor.HitActionOn();
        IsComboPossible = false;

        return isComboOn;
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
    }

    public void isMovePossible()
    {
        playerControl.isMove = true;
    }

    public void ComboPossible()
    {
        IsComboPossible = true;
    }

  public void Attack(int hitBox)
    {

       if(hitBox < 0 || hitBox > hitBoxSize)
        {
            Debug.LogWarning("HitBox Index Over :" + hitBox);
            return;
        }

        playerControl.Attack(hitBox);
    }


}

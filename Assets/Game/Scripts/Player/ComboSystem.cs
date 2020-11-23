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
        playerControl = GameManager.Instance.GetPlayerControl();
        hitBoxSize = playerControl.weaponSensor.hitBoxs.Length;
    }

   

    public void ResetCombo()
    {
        currentComboReset();
        playerControl.modelAni.SetInteger("ComboCount", currentCombo);
    }

    public void currentComboReset()
    {
        currentCombo = 0;
    }

    public void isMovePossible()
    {
        playerControl.isMove = true;
    }

    public void ComboPossible()
    {
        IsComboPossible = true;
    }

    public void AddCombo()
    {
            currentCombo++;
            IsComboPossible = false;
        playerControl.modelAni.SetInteger("ComboCount", currentCombo);
    }

    public void Attack()
    {
        if(currentCombo == 0)
        {
            playerControl.GetRigidbody().velocity = Vector3.zero;
            playerControl.modelAni.SetTrigger("Attack");
            AddCombo();
        }
        else
        {
            //콤보 입력이 가능한 경우에만 콤보를 이어나갈 수 있게 해주자
            if (IsComboPossible) AddCombo();
        }
    }


    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public bool GetIsConnetCombo(int combo)
    {
        return currentCombo == combo;
    }

    public void ComboChack(int combo) 
    {
        if (!GetIsConnetCombo(combo))
        {
           // playerControl.SetSkinnedMeshPostionToPostion();
            playerControl.modelAni.SetTrigger("ComboCancle");
            //ResetCombo();
        }
    }


}

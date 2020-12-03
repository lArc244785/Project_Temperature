using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionHandler : MonoBehaviour
{
    PlayerControl pc;

    public void Initializer(PlayerControl unit)
    {
        pc = unit;
    }
    public void HandleAttack(int hitBox)
    {
        pc.Attack(hitBox);
    }

    public void HandleSetSkinnedMeshPostionToPostion()
    {
        pc.SetSkinnedMeshPostionToPostion();
    }

    public void HandleIsInputActionOn()
    {
        pc.isInputActionOn();
        pc.comboSystem.ResetCombo();
    }

    public void HandleIsInputActionOff()
    {
        pc.isInputActionOff();
    }

    public void HandleIsControlOn()
    {
        pc.ControlOn();
        pc.comboSystem.ResetCombo();
    }

    public void HandleIsControlOff()
    {
        pc.GetRigidbody().velocity = Vector3.zero;
        pc.ControlOff();
    }


    public void HandleComboPossible()
    {
        pc.comboSystem.ComboPossible();
    }

    public void HandleComboChack(int combo)
    {
        pc.comboSystem.ComboChack(combo);
    }

    public void MoveActionFinsh()
    {
        pc.SetSkinnedMeshPostionToPostion();
        HandleIsControlOn();
    }


}

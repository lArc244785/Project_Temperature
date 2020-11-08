﻿using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
    PlayerControl pc;
    public void Initializer()
    {
        pc = GameMagner.Instance.GetPlayerControl();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();


        Debug.Log("Code 1 : OnMove" + input );
        if(input == Vector2.zero)
        {
            pc.oldMoveDir = pc.moveDir;
        }
        pc.moveDir = new Vector3(input.x, 0, input.y).normalized;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //  Debug.Log("Code 1 : OnAttack!!" );
            pc.Attack();
        }
    }

    public void OnRolling(InputAction.CallbackContext context)
    {
        pc.Rolling();
    }

}
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();


        Debug.Log("Code 1 : OnMove" + input );
        GameMagner.Instance.GetPlayerControl().moveDir = new Vector3(input.x, 0, input.y).normalized;


    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
          //  Debug.Log("Code 1 : OnAttack!!" );
            GameMagner.Instance.GetPlayerControl().Attack();
        }
    }

    public void OnRolling(InputAction.CallbackContext context)
    {
        GameMagner.Instance.GetPlayerControl().Rolling();
    }

}

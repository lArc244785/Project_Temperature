using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
    PlayerControl pc;
    Camera mainCam;



    public void Initializer()
    {
        pc = GameManger.Instance.GetPlayerControl();
        mainCam = GameManger.Instance.GetCamerManger().GetMainCamera();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();


        //Debug.Log("Code 1 : OnMove" + input );
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
            pc.modelAni.SetBool("MousePush", true);
            pc.AttackMotion();
        }else if (context.canceled)
        {
            pc.modelAni.SetBool("MousePush", false);
        }
    }

    public void OnRolling(InputAction.CallbackContext context)
    {
        pc.Desh();
    }



    public Vector2 GetMousePostionToScreen()
    {
        return (Vector2)mainCam.ScreenToViewportPoint(Mouse.current.position.ReadValue());
    }

}

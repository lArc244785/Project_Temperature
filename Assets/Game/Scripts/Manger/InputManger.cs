using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
    PlayerControl pc;
    Camera mainCam;

    private bool isMouseButtonPush;

    public void Initializer()
    {
        pc = GameManager.Instance.GetPlayerControl();
        mainCam = GameManager.Instance.GetCamerManger().GetMainCamera();
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
            Debug.Log("isInput: " + pc.isInputAction );
            pc.modelAni.SetBool("IsAttackLoop", true);
            pc.comboAttack();
            isMouseButtonPush = true;
        }
        else if (context.canceled)
        {
            pc.modelAni.SetBool("IsAttackLoop", false);
            isMouseButtonPush = false;

        }

        //Debug.Log(context.performed);
    }

    private void Update()
    {
        if (isMouseButtonPush)
        {
            pc.comboAttack();
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

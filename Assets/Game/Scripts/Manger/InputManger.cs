using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
    PlayerControl pc;
    private Vector2 mouseScreenPoint;
    public Vector2 MousePointToScreen
    {
        get
        {
            return mouseScreenPoint;
        }
    }


    public void Initializer()
    {
<<<<<<< HEAD
        pc = GameManger.Instance.GetPlayerControl();
        mainCam = GameManger.Instance.GetCamerManger().GetMainCamera();
=======
        pc = GameMagner.Instance.GetPlayerControl();
>>>>>>> Jun
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

    private void Update()
    {
        mouseScreenPoint = (Vector2)Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
    }

}

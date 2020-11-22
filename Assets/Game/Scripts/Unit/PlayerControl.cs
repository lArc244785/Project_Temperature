using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : UnitBase
{

    public Vector3 moveDir;
    public Vector3 oldMoveDir;

    public bool isInputAction = true;
    public bool isMove = true;
    private bool isChaking;


    public float RollingCoolTime;
    public float RollingTime = 0.5f;
    private bool isRollingCollTime;
    public float rollingSpeed;

    private bool isTimeAction = false;
    private float currentTime;
    private float oldTime;
    private float waitTime = 0.1f;
    private float tick = 0.01f;

    public ComboSystem comboSystem;

    private bool isTimeStopCorutine = false;

    public float stopTimeScale;
    public float stopTime;
    public float recoveryTime;

   

    public void Start()
    {
        Initializer();
    }

    public override void Initializer()
    {
        base.Initializer();
    }

    public void Update()
    {
        TS = Time.timeScale;
        //print("AA");
        if (isInputAction)
        {
            if(isMove)
            Rotation();
            Move();
        }


    }

    public void Move()
    {
        if (!isInputAction || !isMove) return;


        if (!isMove)
        {
            //rigidbody.velocity = Vector3.zero;
            return;
        }

        if (moveDir.sqrMagnitude > 0.1f)
        {
            rigidbody.velocity = moveDir * speed;
            modelAni.SetBool("IsWalk", true);
        }
        else 
        {
            modelAni.SetBool("IsWalk", false);
            rigidbody.velocity = Vector3.zero;
        }
    }

    IEnumerator SkipFram()
    {
        isChaking = false; 
        yield return new WaitForFixedUpdate();
        if(moveDir.sqrMagnitude < 0.1f && isInputAction)
        {
            modelAni.SetBool("IsWalk", false);
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void Rotation()
    {
        Camera mainCam = GameMagner.Instance.GetCamerManger().GetMainCamera();

        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = GameMagner.Instance.GetInputManger().MousePointToScreen;

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(b.x - a.x, b.y - a.y) * Mathf.Rad2Deg;
    }


    public override void Attack()
    {
        if (!isInputAction) return;
        comboSystem.Attack(); 

    }

    public void ActionMove()
    {
        rigidbody.velocity = transform.forward.normalized * 3;
    }



    public void WeaponTimeAction()
    {
        if (isTimeStopCorutine) StopCoroutine(TimeAction());

        Time.timeScale = 0.00f;
        isTimeStopCorutine = true;
        weaponSensor.HitActionOff();
        StartCoroutine(TimeAction());

        GameMagner.Instance.GetCamerManger().TimeAction();
    }
    public float TS;
    protected float recoveryTimeScale;
    float t;
    IEnumerator TimeAction()
    {

        Debug.Log("TimeActionStart");
        float scale = stopTimeScale;
        Time.timeScale = scale;
        recoveryTimeScale = 1 / recoveryTime;
 
        yield return new WaitForSecondsRealtime(stopTime);
        Time.timeScale = 1.0f;

        //while (true)
        //{
        //    t += Time.unscaledDeltaTime ;
        //    Time.timeScale += Time.unscaledDeltaTime * recoveryTimeScale;
        //    if(Time.timeScale >= 1)
        //    {
        //        Time.timeScale = 1.0f;
        //        break;
        //    }
        //    yield return null;
        //}
        Debug.Log("TimeAction Time : " + t);
        t = 0;
        isTimeStopCorutine = false;

    }



    public void Rolling()
    {
        if (isRollingCollTime) return;
        if (isTimeStopCorutine)
        {
            StopCoroutine(TimeAction());
            Time.timeScale = 1.0f;
        }


        comboSystem.ResetCombo();

        isRollingCollTime = true;
        isInputAction = false;
        isMove = false;


        modelAni.SetTrigger("Rolling");

        StartCoroutine(RollingEvent());
    }


    IEnumerator RollingEvent()
    {
        float time = Time.deltaTime;
        Rotation();
        //  Vector3 rollinPower = transform.forward.normalized * rollingSpeed;
        Vector3 dir = moveDir == Vector3.zero ? oldMoveDir : moveDir;
        Vector3 rollinPower = dir * rollingSpeed;
        transform.LookAt(transform.position + dir);

        while (time < RollingTime)
        {
            rigidbody.velocity = rollinPower;
            time += Time.deltaTime;
            yield return null;
        }

        rigidbody.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.1f);


        isMove = true;
        isInputAction = true;

        yield return new WaitForSeconds(RollingCoolTime);
        isRollingCollTime = false;
    }

    public void SetTimeStopCorutine(bool value)
    {
        isTimeStopCorutine = value;
    }

    public bool GetTimeStopCorutine()
    {
        return isTimeStopCorutine;
    }
}

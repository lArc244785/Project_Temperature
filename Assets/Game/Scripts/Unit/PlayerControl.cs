using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerControl : UnitBase
{

    public Vector3 moveDir;

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
        //print("AA");
        if (isMove)
        {
            Rotation();
        }
        Move();
    }

    public void Move()
    {
        if (!isInputAction) return;


        if (!isMove)
        {
                rigidbody.velocity = Vector3.zero;
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
        if (moveDir == Vector3.zero) return;
        transform.LookAt(transform.position + moveDir);
    }


    public override void Attack()
    {
        comboSystem.Attack();
    }


    public void WeaponTimeAction()
    {
        if (isTimeStopCorutine) StopCoroutine(TimeAction());
        isTimeAction = true;
        Time.timeScale = 0.00f;
        isTimeStopCorutine = false;
        StartCoroutine(TimeAction());
    }
    public float addtime;
    IEnumerator TimeAction()
    {
        float scale = stopTimeScale;
        Time.timeScale = scale;
        addtime = 0;
        yield return new WaitForSecondsRealtime(stopTime);

        while (true)
        {
            Time.timeScale += scale;
            if(Time.timeScale >= 1)
            {
                Time.timeScale = 1.0f;
                break;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
            yield return new WaitForSecondsRealtime(0.01f);
        isTimeStopCorutine = false;

    }



    public void Rolling()
    {
        if (isRollingCollTime) return;

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
        Vector3 rollinPower = transform.forward.normalized * rollingSpeed;

        while (time < RollingTime)
        {
            rigidbody.velocity = rollinPower;
            time += Time.deltaTime;
            yield return null;
        }

        rigidbody.velocity = Vector3.zero;

        isMove = true;
        isInputAction = true;

        yield return new WaitForSeconds(RollingCoolTime);
        isRollingCollTime = false;
    }
}

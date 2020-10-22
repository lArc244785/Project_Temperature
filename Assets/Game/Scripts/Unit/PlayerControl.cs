using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerControl : UnitBase
{

    public Vector3 moveDir;

    private bool isInputCommand = true;
    public bool isMove = true;
    public EnumInfo.Command currentCommand, nextCommand;
    private int AttackCount;
    private bool isChaking;


    public float RollingCoolTime;
    private bool isRollingCollTime;

    private bool isTimeAction = false;
    private float currentTime;
    private float oldTime;
    private float waitTime = 0.1f;
    private float tick = 0.01f;

    public void Start()
    {
        Initializer();
    }

    public override void Initializer()
    {
        base.Initializer();
        currentCommand = EnumInfo.Command.NoCommand;
        nextCommand = currentCommand;
    }

    public void Update()
    {
        //print("AA");
        Rotation();
        Move();
        Attack();


    }

    public void Move()
    {
        if (!isMove)
        {
            if(!isRollingCollTime)
            rigidbody.velocity = Vector3.zero;
            return;
        }

        if (moveDir.sqrMagnitude > 0.1f)
        {
            rigidbody.velocity = moveDir * speed;
            modelAni.SetBool("IsWalk", true);
            isChaking = true;
        }
        else
        {
            if (isChaking)
            {
                StartCoroutine(SkipFram());
            }
        }
    }

    IEnumerator SkipFram()
    {
        isChaking = false; 
        yield return new WaitForFixedUpdate();
        if(moveDir.sqrMagnitude < 0.1f)
        {
            modelAni.SetBool("IsWalk", false);
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void Rotation()
    {
        if (!isMove) return;
        if (moveDir == Vector3.zero) return;
        transform.LookAt(transform.position + moveDir);
    }

    public void CommandAddAttack()
    {
        SetNextCommand(EnumInfo.Command.Attack);
    }


    public override void Attack()
    {
        if (isAttackRate || !isInputCommand) return;

        NextCommand();

        if (currentCommand == EnumInfo.Command.Attack && AttackCount < 1)
            {
            isMove = false;
                base.Attack();
                Debug.Log("Player Attack");
                AttackCount++;
                modelAni.SetInteger("AttackCount", AttackCount);
               modelAni.SetTrigger("Attack");

            StartCoroutine(AttackRate());
        }
        else
        {
            AttackCount = 0;
        }

    }


    public void WeaponTimeAction()
    {
        isTimeAction = true;
        Time.timeScale = 0.00f;
        StartCoroutine(TimeAction());
    }

    IEnumerator TimeAction()
    {
        float scale = 0.01f;
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(0.35f);
        while (scale < 1.0f)
        {
            scale += 0.05f;
            Mathf.Clamp(scale, 0.0f, 1.0f);
            Time.timeScale = scale;
            print(scale);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }


    public void SetNextCommand(EnumInfo.Command command)
    {
        if (!isInputCommand) return;
        nextCommand = command;
    }

    public void NextCommand()
    {
        currentCommand = nextCommand;
        nextCommand = EnumInfo.Command.NoCommand;

    }

    public void ResetAttackCount()
    {
        AttackCount = 0;
        modelAni.SetInteger("AttackCount", AttackCount);
    }

    public int GetAttackCount()
    {
        return AttackCount;
    }

    public void Rolling()
    {
        if (isRollingCollTime) return;

        isRollingCollTime = true;
        isInputCommand = false;
        isMove = false;


        modelAni.SetTrigger("Rolling");
        rigidbody.velocity = transform.forward.normalized * 7.0f;
        StartCoroutine(RollingCoolEvent());
    }


    IEnumerator RollingCoolEvent()
    {
        yield return new WaitForSeconds(RollingCoolTime);
        isMove = true;
        isInputCommand = true;
        isRollingCollTime = false;
    }
}

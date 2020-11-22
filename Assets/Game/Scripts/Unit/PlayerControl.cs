using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : UnitBase
{

    public Vector3 moveDir;
    public Vector3 oldMoveDir;


    public bool isMove = true;
    private bool isChaking;


    public float RollingCoolTime;
    public float DeshTime = 1.0f;
    private bool isDeshCollTime;
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
        if (isInputAction && isControl)
        {

            Rotation();
           Move();

        }

        weaponSensor.transform.position = new Vector3(
            SkinnedMesh.bounds.center.x,
            weaponSensor.transform.position.y, 
            SkinnedMesh.bounds.center.z);

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);


 
    }

    public void Move()
    {

        if (moveDir.sqrMagnitude > 0.1f)
        {
            rigidbody.velocity = moveDir * speed;
            modelAni.SetFloat("Walk" ,1.0f);
            isMove = true;
        }
        else 
        {
            modelAni.SetFloat("Walk", 0.0f);
            isMove = false;
        }

        if (!isMove)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }
    }

    IEnumerator SkipFram()
    {
        isChaking = false; 
        yield return new WaitForFixedUpdate();
        if(moveDir.sqrMagnitude < 0.1f && isInputAction)
        {
            modelAni.SetFloat("Walk", 0.0f);
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void Rotation()
    {
        Camera mainCam = GameManger.Instance.GetCamerManger().GetMainCamera();

        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
<<<<<<< HEAD
        Vector2 mouseOnScreen = GameManger.Instance.GetInputManger().GetMousePostionToScreen();
=======
        Vector2 mouseOnScreen = GameMagner.Instance.GetInputManger().MousePointToScreen;
>>>>>>> Jun

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(b.x - a.x, b.y - a.y) * Mathf.Rad2Deg;
    }


    public  void AttackMotion()
    {

        if (!isInputAction || !isControl) return;
        rigidbody.velocity = Vector3.zero;
        modelAni.SetTrigger("Attack");
        isInputAction = false;
       // comboSystem.isPushCombo(); 
    }




    public override void Attack(int hitBox = 0)
    {
        base.Attack(hitBox);
    }



    public override void HitEvent(List<Damage> damageList, WeaponBase weapon)
    {
        if (gameObject.layer == GhostLayer) return;
        base.HitEvent(damageList, weapon);
        StartCoroutine(GhosetState(3.0f));
    }

    IEnumerator GhosetState(float time)
    {
        gameObject.layer = GhostLayer;
        yield return new WaitForSecondsRealtime(time);
        gameObject.layer = originLayer;
    }

    public void ActionMove()
    {
      //  rigidbody.velocity = transform.forward.normalized * 3;
    }



    public void WeaponTimeAction()
    {
        if (isTimeStopCorutine) StopCoroutine(TimeAction());

        Time.timeScale = 0.00f;
        isTimeStopCorutine = true;
 
        StartCoroutine(TimeAction());

        GameManger.Instance.GetCamerManger().TimeAction();
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



    public void Desh()
    {
        if (isDeshCollTime) return;
        if (isTimeStopCorutine)
        {
            StopCoroutine(TimeAction());
            Time.timeScale = 1.0f;
        }


        comboSystem.ResetCombo();

        isDeshCollTime = true;
        isInputAction = false;
        isMove = false;

        modelAni.SetBool("isDeshing", true);
        modelAni.SetTrigger("Dash");

        SetSkinnedMeshPostionToPostion();
        StartCoroutine(DeshCoroutine());
    }


    IEnumerator DeshCoroutine()
    {
        float time = Time.deltaTime;
        Rotation();
        //  Vector3 rollinPower = transform.forward.normalized * rollingSpeed;
        Vector3 dir = moveDir == Vector3.zero ? oldMoveDir : moveDir;
        Vector3 rollinPower = dir * rollingSpeed;
        transform.LookAt(transform.position + dir);

        while (time < DeshTime)
        {
            rigidbody.velocity = rollinPower;
            time += Time.deltaTime;
            yield return null;
        }
        modelAni.SetBool("isDeshing", false);
        rigidbody.velocity = Vector3.zero;

        //yield return new WaitForSeconds(0.1f);

        isInputAction = true;

        yield return new WaitForSeconds(RollingCoolTime);
        isDeshCollTime = false;
    }

    public void SetTimeStopCorutine(bool value)
    {
        isTimeStopCorutine = value;
    }

    public bool GetTimeStopCorutine()
    {
        return isTimeStopCorutine;
    }

    public Transform GetUnitTransfrom()
    {
        return unitTransform;
    }



}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Animation attackAnimation;

    private float GhostTime = 1.5f;

    public bool isRotion;

    private IEnumerator RotaionFreamOnOff;

    public Transform AttackPivot;

    private BPMSystem bpmSystem;

    public void Start()
    {
        Initializer();
        isRotion = true;
    }

    public override void Initializer()
    {
        base.Initializer();
        motionHandler.Initializer(this);
        RotaionOn();
        bpmSystem = GetComponent<BPMSystem>();
        bpmSystem.Initializer(this);
        temperatureSystem = GameManager.Instance.GetTemperatureSystem();
    }

    public void OnDrawGizmos()
    {
        if (capsuleCollider == null) return;
        ColliderDistance = (capsuleCollider.gameObject.transform.lossyScale.x * capsuleCollider.radius) + 0.1f;
        Vector3 testDrawDistance = new Vector3(transform.position.x, transform.position.y, transform.position.z - ColliderDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, testDrawDistance);

    }



    public void Update()
    {
        TS = Time.timeScale;
        //print("AA");
        if ( isControl)
        {
            Move();
        }


        UpdateSensorPos();
        Rotation();

        //bpmSystem.BPMTemperature();

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

    }

    public void Move()
    {

        if (moveDir.sqrMagnitude > 0.1f)
        {
            rigidbody.velocity = moveDir * speed;
            AddTemperature(0.01f);
            modelAni.SetFloat("Walk", 1.0f);
            bpmSystem.WalkBPMCoroutineOn();
            bpmSystem.RecoveryBPMCoroutineOff();
            isMove = true;
        }
        else
        {
            modelAni.SetFloat("Walk", 0.0f);
            bpmSystem.WalkBPMCoroutineOff();
            bpmSystem.RecoveryBPMCoroutineOn();
            isMove = false;
        }

        if (!isMove)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }
    }



    private void Rotation()
    {
        if (!isRotion) return;

        Camera mainCam = GameManager.Instance.GetCamerManger().GetMainCamera();

        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = GameManager.Instance.GetInputManger().GetMousePostionToScreen();

        //Get the angle between the points
        float angle = Utility.AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        if (isControl)
        {
            unitTransform.parent = null;
            AttackPivot.parent = unitTransform;
            unitTransform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
        }
        else
        {
            unitTransform.parent = null;
            AttackPivot.parent = null;
            AttackPivot.position = GetSkinnedMeshPostionToPostion();
            unitTransform.parent = AttackPivot;
            AttackPivot.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
            unitTransform.localRotation = Quaternion.identity;
        }
    }



    public void comboAttack()
    {

        if (!isInputAction) return;
      
        comboSystem.Attack();
    }




    public override void Attack(int hitBox = 0)
    {
        
        base.Attack(hitBox);
        bpmSystem.AddBPM(3.0f);

        RotaionOnOffCoroutine(1);
    }

    private void UpdateSensorPos()
    {
        weaponSensor.transform.position = new Vector3(
        SkinnedMesh.bounds.center.x,
        weaponSensor.transform.position.y,
        SkinnedMesh.bounds.center.z);
    }

    public override void HitEvent(List<Damage> damageList, WeaponBase weapon)
    {

        Debug.Log("Player HIt" + gameObject.layer + "   " + GhostLayer);
        if (gameObject.layer == GhostLayer) return;
        base.HitEvent(damageList, weapon);
        modelAni.SetTrigger("Hit");
        StartCoroutine(GhosetState(GhostTime));
        //MaterialChange(EnumInfo.Materia.Ghost);
        comboSystem.currentComboReset();
    }

    IEnumerator GhosetState(float time)
    {
        gameObject.layer = GhostLayer;
        yield return new WaitForSecondsRealtime(time);
        gameObject.layer = originLayer;
    }


    public override void ControlOff()
    {
        base.ControlOff();
        isRotion = false;
        bpmSystem.WalkBPMCoroutineOff();
        bpmSystem.RecoveryBPMCoroutineOff();
    }

    public override void ControlOn()
    {
        base.ControlOn();
        isRotion = true;
    }

    public void RotaionOn()
    {
        isRotion = true;
    }

    public void RotaionOff()
    {
        isRotion = false;
    }

    public void RotaionOnOffCoroutine(int fream)
    {
        if(RotaionFreamOnOff != null)
        {
            StopCoroutine(RotaionFreamOnOff);
        }
        RotaionFreamOnOff = RotionFreameOn(fream);
        StartCoroutine(RotaionFreamOnOff);
    }

    IEnumerator RotionFreameOn(int fream)
    {
        RotaionOn();
        for (int i = 0; i < fream; i++)
        {
            yield return null;
        }
        RotaionOff();
    }


    public void WeaponTimeAction()
    {
        if (isTimeStopCorutine) StopCoroutine(TimeAction());

        Time.timeScale = 0.00f;
        isTimeStopCorutine = true;

        StartCoroutine(TimeAction());

        GameManager.Instance.GetCamerManger().TimeAction();
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
        if (isDeshCollTime || !isInputAction) return;
        if (isTimeStopCorutine)
        {
            StopCoroutine(TimeAction());
            Time.timeScale = 1.0f;
        }

        AddTemperature(0.5f);

        comboSystem.ResetCombo();

        isDeshCollTime = true;
        isInputAction = false;
        isMove = false;

        modelAni.SetBool("isDeshing", true);
        modelAni.SetTrigger("Dash");

        SetSkinnedMeshPostionToPostion();
        StartCoroutine(DeshCoroutine());

        bpmSystem.AddBPM(5.0f);
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

        ControlOn();
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


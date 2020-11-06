using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerManger : MonoBehaviour
{
    public Transform Target;

    private Vector3 currentPos;
    private Vector3 currentVelocity;
    private Vector3 targetPos;

    public Vector3 offset;

    private Camera mainCam;
    private Transform mainCamPos;

    private Vector3 camPos_orignal;

    public float shake;

    private void Start()
    {
        mainCam =GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamPos = transform;

        SetTarget();

        currentPos = targetPos;
        currentVelocity = currentPos;
        camPos_orignal = Vector3.zero;
    }

    private void SetTarget()
    {
        targetPos = Target.position + offset;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(camPos_orignal == Vector3.zero)
        TargetCam();
    }

    private void TargetCam()
    {
        SetTarget();
        currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref currentVelocity, 0.2f);
        mainCamPos.position = currentPos;
    }

    public Camera GetMainCamera()
    {
        return mainCam;
    }

    public void TimeAction()
    {
        if(camPos_orignal != Vector3.zero)
        {
            transform.position = camPos_orignal;
            StopCoroutine(IE_TimeAction());
        }
        StartCoroutine(IE_TimeAction());
    }

    private float RandomShake()
    {
        return Random.Range(-shake, shake);
    }

    IEnumerator IE_TimeAction()
    {
        camPos_orignal = transform.position;
        transform.position = new Vector3(RandomShake() + transform.position.x, 
            transform.position.y, 
            RandomShake() + transform.position.z);
        yield return new WaitForSecondsRealtime(0.05f);
        transform.position = new Vector3(RandomShake() + transform.position.x,
    transform.position.y,
    RandomShake() + transform.position.z);
        yield return new WaitForSecondsRealtime(0.05f);
        transform.position = camPos_orignal;
        camPos_orignal = Vector3.zero;
    }

}

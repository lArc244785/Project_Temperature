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

    private void Start()
    {
        mainCam =GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamPos = mainCam.transform;

        SetTarget();

        currentPos = targetPos;
        currentVelocity = currentPos;
    }

    private void SetTarget()
    {
        targetPos = Target.position + offset;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        TargetCam();
    }

    private void TargetCam()
    {
        SetTarget();
        currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref currentVelocity, 0.2f);
        mainCamPos.position = currentPos;
    }

}

using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerManger : MonoBehaviour
{
    public Transform Target;



    private Camera mainCam;
    private Transform mainCamPos;
    public CinemachineVirtualCamera FollowCam;


    private Vector3 camPos_orignal;

    public float shake;
    public float FollowOffsetSpeed;
    public Vector3 FollowCamOffsetSize;
    private Vector3 OrigineFollowCamOffset;
    private Vector3 currentFollowCamOffset;
    private Vector3 currentFollowCamOffsetVelocity;
    private Vector3 targetFollowCamOffset;
    public CinemachineOrbitalTransposer orbital;
    public void Initializer()
    {
        mainCam =GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamPos = transform;
        orbital = FollowCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        OrigineFollowCamOffset = orbital.m_FollowOffset;
        currentFollowCamOffset = OrigineFollowCamOffset;
        targetFollowCamOffset = currentFollowCamOffset;
    }




    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 ScreenMousePos = GameMagner.Instance.GetInPutManger().ScreenMousePos;
        //마우스의 스크린 퍼센트를 확인
        //X와 Y둘중 0.95을 넘었는지 확인
        if (Mathf.Abs(ScreenMousePos.x) > 0.95f)
        {
       
            targetFollowCamOffset.x = ScreenMousePos.x > 0 ? FollowCamOffsetSize.x : -FollowCamOffsetSize.x;
            targetFollowCamOffset.x += OrigineFollowCamOffset.x;
        }
        else
        {
            targetFollowCamOffset.x = OrigineFollowCamOffset.x; 
        }

        if (Mathf.Abs(ScreenMousePos.y) > 0.95f)
        {
            targetFollowCamOffset.z = ScreenMousePos.y > 0 ? FollowCamOffsetSize.z : -FollowCamOffsetSize.z;
            targetFollowCamOffset.z += OrigineFollowCamOffset.z;
        }
        else
        {
            targetFollowCamOffset.y = OrigineFollowCamOffset.y;
        }

        currentFollowCamOffset = Vector3.SmoothDamp(
            currentFollowCamOffset,
            targetFollowCamOffset,
            ref currentFollowCamOffsetVelocity,
            0.2f, FollowOffsetSpeed);

        orbital.m_FollowOffset = currentFollowCamOffset;
        //넘었으면 지정한 간격까지 일정속도로 카메라의 Offset변경


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

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
    private Vector3 currentOffset;
    private Vector3 currentOffsetVelocity;
    private Vector3 targetOffset;
    private Vector3 origineOffset;
    public Vector2 moveOffsetRadius;
    public float offsetTime = 0.5f;

    private Camera mainCam;
    private Transform mainCamPos;

    private Vector3 camPos_orignal;

    public float shake;

    public bool isMouseOffset;


    public void Intilize()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamPos = transform;

        SetTarget();

        currentPos = targetPos;
        currentVelocity = currentPos;
        camPos_orignal = Vector3.zero;

        origineOffset = offset;
        currentOffset = origineOffset;
        currentOffsetVelocity = currentOffset;
        targetOffset = currentOffset;

        isMouseOffset = false;
      
    }

    private void Start()
    {
        Invoke("MouseOffsetTrue", 1.0f);
    }

    private void MouseOffsetTrue()
    {
        isMouseOffset = true;
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

        SetOffset();
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

    public void SetOffset()
    {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 3fa5113e526e5a67d0cd631bb812b482601fc58a
        if (!isMouseOffset) return;

        Vector2 screenPoint = GameManger.Instance.GetInputManger().GetMousePostionToScreen();
        //Debug.Log(screenPoint);
        if (screenPoint.x > 0.9f  || screenPoint.x  < 0.1f )
<<<<<<< HEAD
=======
        Vector2 screenPoint = GameMagner.Instance.GetInputManger().MousePointToScreen;
        //Debug.Log(screenPoint);
        if (screenPoint.x > 0.95f  || screenPoint.x  < 0.1f)
>>>>>>> Jun
=======
>>>>>>> 3fa5113e526e5a67d0cd631bb812b482601fc58a
        {
            if(screenPoint.x > 0.1f)
            {
                targetOffset.x = origineOffset.x + moveOffsetRadius.x;
            }
            else
            {
                targetOffset.x = origineOffset.x - moveOffsetRadius.x;
            }
        }
        else
        {
            targetOffset.x = origineOffset.x;
        }

        if(screenPoint.y > 0.9f || screenPoint.y < 0.1f)
        {
            if(screenPoint.y > 0.1f)
            {
                targetOffset.z = origineOffset.z + moveOffsetRadius.y;
                targetOffset.z -= 0.5f;
            }
            else
            {
                targetOffset.z = origineOffset.z - moveOffsetRadius.y;

            }
        }
        else
        {
            targetOffset.z = origineOffset.z;
        }

        TargetOffCam();
    }

    private void TargetOffCam()
    {
        if (targetOffset == origineOffset && origineOffset == currentOffset) return;

        currentOffset = Vector3.SmoothDamp(currentOffset, targetOffset, ref currentOffsetVelocity, offsetTime);
       
        currentOffset.y = origineOffset.y;

        offset = currentOffset;
    }

}

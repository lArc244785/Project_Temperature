using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileAddOn : MonoBehaviour
{
    //public float speed;
    public float upTargetPosX;
    public float upTargetPosY;
    public float upTargetPosZ;

    public float addOnTargetPosX;
    public float addOnTargetPosZ;

    Vector3 upTargetPos;
    Vector3 addOnTargetPos;

    private float randomPosY;

    private float randomRotX;
    private float randomRotY;
    private float randomRotZ;

    //public float tileMoveSpeed;
    private void Awake()
    {
        //tileMoveSpeed = 0.02f;
        upTargetPos = new Vector3(upTargetPosX, upTargetPosY, upTargetPosZ);
        addOnTargetPos = new Vector3(addOnTargetPosX, upTargetPosY, addOnTargetPosZ);
    }

    private void Start()
    {
        for(int i=0;i < transform.childCount; i++)
        {
            randomPosY = Random.Range(-7f, -1f);

            randomRotX = Random.Range(0f, 180f);
            randomRotY = Random.Range(0f, 180f);
            randomRotZ = Random.Range(0f, 180f);

            transform.GetChild(i).localPosition = new Vector3(transform.GetChild(i).localPosition.x, randomPosY, transform.GetChild(i).localPosition.z);
            transform.GetChild(i).localRotation = Quaternion.Euler(randomRotX, randomRotY, randomRotZ);
        }

        StartCoroutine(addOn());
    }

    // Update is called once per frame
    void Update()
    {
        //TileCombine();
        //AddOnUp();
        //AddOnCombine();
    }

    IEnumerator addOn()
    {
        float checkUpPos = upTargetPosY - .01f;
        float checkAddPosX = addOnTargetPosX - .01f;
        float checkAddPosZ = addOnTargetPosZ - .01f;

        AddOnUp();
        TileCombine();

        yield return new WaitForSeconds(5f);

        AddOnCombine();
    }

    public void AddOnUp()
    {
        //Vector3 velocity = Vector3.zero;
        //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, 0.1f);
        transform.DOLocalMove(upTargetPos, 4f);

        //if(transform.localPosition.y < 4.99f)
        //{
        //    transform.localPosition = upTargetPos;
        //}
       
        //if (transform.localPosition.y == upTargetPosY)
        //{
        //    //Invoke("AddOnCombine",1f);
        //    //AddOnCombine();
        //}
    }

    public void TileCombine()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 targetPos = new Vector3(transform.GetChild(i).localPosition.x, 0, transform.GetChild(i).localPosition.z);

            //if (transform.GetChild(i).localPosition.y < -4)
            //{
            //    tileMoveSpeed = 0.02f;
            //}
            //else if (transform.GetChild(i).localPosition.y >= -4 && transform.GetChild(i).localPosition.y < -3)
            //{
            //    tileMoveSpeed = 0.015f;
            //}
            //else
            //{
            //    tileMoveSpeed = 0.01f;
            //}

            //transform.GetChild(i).localPosition = Vector3.MoveTowards(transform.GetChild(i).localPosition, targetPos, tileMoveSpeed);
            transform.GetChild(i).DOLocalMove(targetPos, 3f).SetEase(Ease.OutQuad);
            //transform.GetChild(i).localRotation = Quaternion.Lerp(transform.GetChild(i).localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 1.5f);
            transform.GetChild(i).DOLocalRotate(Vector3.zero, 2f).SetEase(Ease.OutQuad);
        }
    }

    public void AddOnCombine()
    {
        transform.DOLocalMove(addOnTargetPos, 1f).SetEase(Ease.InQuad);
    }
}

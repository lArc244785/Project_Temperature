﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[Serializable]
public class TileBase : MonoBehaviour
{
    public StructInfo.TileInfo tileInfo;

    public Transform modelTr;
    public Rigidbody rigidbody;
    public ConstantForce constantForce;
    public BoxCollider collider;

    private Vector3 gravity;
    private float gravityValue;
    public bool test = false;

    public bool isWall;

    public MeshRenderer mr;

    public Material pathMaterial;
    public Material nomalMaterial;

    public void SetTileIndex(int x, int y)
    {
        tileInfo.SetTile(x, y, transform.position, isWall);
    }



    public StructInfo.TileInfo GetTileInfo()
    {
        return tileInfo;
    }


    private void Start()
    {
        
        gravityValue = UnityEngine.Random.Range(-10f,-3f);
        gravity = new Vector3(0, gravityValue, 0);
        constantForce.force = gravity;

        if (test ==true)
        {
            //HandleFall();
            StartCoroutine(Fall());
        }
    }

    private void Update()
    { 

    }

    IEnumerator Fall()
    {
        rigidbody.isKinematic = false;
        collider.isTrigger = true;

        Invoke("Death", 2f);

        yield return new WaitForSeconds(.1f);

        collider.isTrigger = false;

    }

    //public void HandleFall()
    //{
    //    rigidbody.isKinematic = false;
    //    collider.isTrigger = true;
    //    Invoke("Death", 2f);
    //}

    public void Death()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit")
        {
            modelTr.Translate(0, -0.07f, 0);
            GameManger.Instance.GetMapManger().SetPlayerTile(this);
            GameManger.Instance.GetEnemyManger().SetPath(this.tileInfo.point);
        }

        if (other.tag == "Enemy")
        {
            other.transform.GetComponent<EnemyBasic>().tile = this;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Unit")
        {
            modelTr.Translate(0 , - 0.07f, 0);
            GameManger.Instance.GetMapManger().SetPlayerTile(this);
            GameManger.Instance.GetEnemyManger().SetPath(this.tileInfo.point);
        }

        if(collision.collider.tag == "Enemy")
        {
            collision.transform.GetComponent<EnemyBasic>().tile = this;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Unit")
        {
            if (modelTr.localPosition.y < 0)
            {
                modelTr.DOLocalMove(Vector3.zero, 1f);
                //modelTr.Translate(transform.localPosition);
            }
        }
    }
}

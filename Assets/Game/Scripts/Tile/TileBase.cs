using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class TileBase : MonoBehaviour
{
    public Transform modelTr;
    public Rigidbody rigidbody;
    public ConstantForce constantForce;
    public BoxCollider collider;

    private Vector3 gravity;
    private float gravityValue;

    public bool test = false;

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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Unit")
        {
            modelTr.Translate(0 , - 0.07f, 0);
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

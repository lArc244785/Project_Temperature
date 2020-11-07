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

    public Vector3 gravity;
    public int gravityValue;

    public bool test = false;

    private void Start()
    {
        gravityValue = UnityEngine.Random.Range(-10,-3);
        gravity = new Vector3(0, gravityValue, 0);
        constantForce.force = gravity;

        if (test ==true)
        {
            HandleFall();
        }
    }

    private void Update()
    { 
    }

    public void HandleFall()
    {
        rigidbody.isKinematic = false;
        Invoke("Death", 2f);
    }

    public void Death()
    {
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Unit")
        {
            modelTr.Translate(0, transform.position.y - 0.05f, 0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Unit")
        {
            if (modelTr.localPosition.y < transform.localPosition.y)
            {
                modelTr.DOMove(transform.position, 1f);
                //modelTr.Translate(transform.localPosition);
            }
        }
    }
}

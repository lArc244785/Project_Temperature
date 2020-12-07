using System;
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


    public Material originMaterial;

    private Tween tween;

    private Vector3 targetPos = new Vector3(0, -0.1f, 0);

    public bool isStage5;
    public bool isStage6;
    public bool isStage10;


    public void SetTileIndex(int x, int y)
    {
        tileInfo.SetTile(x, y, transform.position, isWall);
    }

    public void SetTileMaterial(Material material)
    {
        mr.material = material;
    }

    public void ClearTileMaterial()
    {
        mr.material = originMaterial;
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

        originMaterial = mr.material;

        if (test ==true)
        {
            //HandleFall();
            StartCoroutine(Fall());
        }
    }

    private void Update()
    {

    }

    //public IEnumerator FallForWave()
    //{
    //    if (GameManager.Instance.currentWave + 1 == 5 && isStage5)
    //    {
    //        StartCoroutine(Fall());
    //    }

    //    if (GameManager.Instance.currentWave + 1 == 6 && isStage6)
    //    {
    //        StartCoroutine(Fall());
    //    }

    //    if (GameManager.Instance.currentWave + 1 == 10 && isStage10)
    //    {
    //        StartCoroutine(Fall());
    //    }

    //    yield break;
    //}

    public IEnumerator Fall()
    {
        rigidbody.isKinematic = false;
        collider.isTrigger = true;

        isWall = true;

        yield return new WaitForSeconds(.1f);

        collider.isTrigger = false;

        yield return new WaitForSeconds(2f);

        Death();

        yield break;

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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Unit")
    //    {
    //        modelTr.Translate(0, -0.07f, 0);

    //    }


    //}


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Unit")
        {
            
            Utility.KillTween(tween);
            //if(modelTr.parent.y)
            //modelTr.Translate(0 , - 0.07f, 0);
            modelTr.localPosition = Vector3.MoveTowards(modelTr.localPosition, targetPos, 0.1f);

        }


    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Unit")
        {
            if (modelTr.localPosition.y < 0)
            {
                tween = modelTr.DOLocalMove(Vector3.zero, 1f);
                //modelTr.Translate(transform.localPosition);

            }
        }
    }
}

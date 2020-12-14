using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LampFx : MonoBehaviour
{

    private Vector3 targetTr;

    public Transform lampTr;


    public Material lampMaterial;
    public Light lampLight;

    private Sequence lampPadeInOut;

    private float padeInOutSpeed = 1.0f;
    private float lampPadeInOutSpeed = 10.0f;
    private float rotationSpeed = 50.0f;

    private bool isPadeInEventOn;
    private bool isPadeOutEventOn;

    private float playerRecoveryRange = 3.0f;

    public void Start()
    {

        lampMaterial.color = new Color(1, 1, 1, 0);


        PadeInOutEvnetReset();

    }

    private void Update()
    {

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        if (isPadeInEventOn)
        {
            Vector3 playerPos = GameManager.Instance.GetPlayerControl().GetSkinnedMeshPostionToPostion();
            Vector3 lampPos = lampTr.position;
            playerPos = Utility.VectorProduct(playerPos, new Vector3(1, 0, 1));
            lampPos = Utility.VectorProduct(lampPos, new Vector3(1, 0, 1));
            if (Vector3.Distance(playerPos, lampPos) < playerRecoveryRange)
            {
                GameManager.Instance.GetPlayerControl().AddTemperature(0.01f);
            }
        }
    }



   
    public void LampPadeIn()
    {
        if (isPadeInEventOn) return;
        isPadeInEventOn = true;
        Utility.KillTween(lampPadeInOut);
        lampPadeInOut = DOTween.Sequence();
        lampPadeInOut.Insert(0, lampMaterial.DOFade(0.8f, padeInOutSpeed));
        lampPadeInOut.Insert(0, 
            lampLight.DOIntensity(10.0f, lampPadeInOutSpeed ).SetLoops(100,LoopType.Yoyo));
        
        lampPadeInOut.Play();
        
    }

    public void LampPadeOut()
    {
        if (isPadeOutEventOn) return;
        isPadeOutEventOn = true;
        Utility.KillTween(lampPadeInOut);
        lampPadeInOut = DOTween.Sequence();
        lampPadeInOut.Insert(0, lampMaterial.DOFade(0.0f, padeInOutSpeed));
        lampPadeInOut.Insert(0,
           lampLight.DOIntensity(0.0f, lampPadeInOutSpeed));
        lampPadeInOut.Play();

    }

    public void PadeInOutEvnetReset()
    {
        Utility.KillTween(lampPadeInOut);
        isPadeInEventOn = false;
        isPadeOutEventOn = false;
        lampLight.intensity = 0.0f;
    }


}

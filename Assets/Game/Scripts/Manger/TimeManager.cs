﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    private float blend = 0f;

    public Material[] mat;

    public int index;

    public Light mainLight;
    public Light nightLIght;

    private float currentIntensity;
    private float targetIntensity;
    private float currentIntensityVelocity;

    private float currentMainIntensity;
    private float targetMainIntensity;
    private float currentMainIntensityVelocity;

    private float degreePerSecond;
    private float daySecond;

    private float timer = 0f;

    public bool isNight;

    private void Start()
    {
        daySecond = 320;
        degreePerSecond = 360 / daySecond;
        mainLight.transform.rotation = Quaternion.Euler(90, 0, 0);
        nightLIght.intensity = 0;
        currentMainIntensity = .8f;

        isNight = false;
    }

    private void Update()
    {
        MainLIghtControl();
        NightLightControl();

        SkyboxBlend();

        UIManager.Instance.uiInGame.DayNIghtIcon(isNight);
    }

    public void Timer()
    {
        timer += Time.deltaTime;
    }

    public void MainLIghtControl()
    {
        //time for game
        //360 = 1day 300sec = 6degree
        mainLight.transform.Rotate(Vector3.right, degreePerSecond*Time.deltaTime);

        if (mainLight.transform.eulerAngles.x > 270)
        {
            isNight = true;

            targetMainIntensity = 0f;
            currentMainIntensity = Mathf.SmoothDamp(currentMainIntensity, targetMainIntensity, ref currentMainIntensityVelocity, 1f);
            mainLight.intensity = currentMainIntensity;
        }
        else
        {
            isNight = false;

            targetMainIntensity = .8f;
            currentMainIntensity = Mathf.SmoothDamp(currentMainIntensity, targetMainIntensity, ref currentMainIntensityVelocity, 1f);
            mainLight.intensity = currentMainIntensity;
        }
    }

    public void NightLightControl()
    {
        if (mainLight.transform.eulerAngles.x > 270 || mainLight.transform.eulerAngles.x < 20)
        {
            targetIntensity = 0.3f;
            currentIntensity = Mathf.SmoothDamp(currentIntensity, targetIntensity, ref currentIntensityVelocity, 10f);
            nightLIght.intensity = currentIntensity;
        }
        else
        {
            targetIntensity = 0f;
            currentIntensity = Mathf.SmoothDamp(currentIntensity, targetIntensity, ref currentIntensityVelocity, 10f);
            nightLIght.intensity = currentIntensity;
        }
    }

    public void SkyboxBlend()
    {
        if(blend < 1)
        {
            blend += Time.deltaTime * 0.0125f;
            //test ;
            //blend += Time.deltaTime * 0.1f;
            mat[index].SetFloat("_Blend", blend);
        }
        else
        {
            if (index < 3)
            {
                index++;
                RenderSettings.skybox = mat[index];
                blend = 0;

                if (mat[index - 1].GetFloat("_Blend")>0)
                    mat[index - 1].SetFloat("_Blend", 0);
            }
            else
            {
                index = 0;
                RenderSettings.skybox = mat[index];
                blend = 0;

                if (mat[3].GetFloat("_Blend") > 0)
                    mat[3].SetFloat("_Blend", 0);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSystem : MonoBehaviour
{
   private UnitBase player;
    private TimeManager time;


    private float[] timeTemperature;



    public void Initializer()
    {
        this.player = GameManager.Instance.GetPlayerControl();
        time = GameManager.Instance.GetTimeManager();
        timeTemperature = new float[24];
        
        SetTimeTemperature(12, 15, 30, 50);
        SetTimeTemperature(15, 18, 50, 20);
        SetTimeTemperature(18, 23, 20, 5);
        SetTimeTemperature(0, 6, 5, -10);
        SetTimeTemperature(6, 9, -10, 10);
        SetTimeTemperature(9, 12, 10, 30);

    }

    private void SetTimeTemperature(int startIndex, int endIndex, float startTemperature, float endTemperature)
    {
        if(timeTemperature.Length < startIndex || timeTemperature.Length < endIndex)
        {
            Debug.LogError("TimeTemperSet Error: " + startIndex + " , " + endIndex);
        }

        int range = endIndex - startIndex;

        float nextAddTemperature = (endTemperature - startTemperature) / range;

        int count = 0;
        for ( int i =  startIndex ;    i <= endIndex;   i ++)
        {
            timeTemperature[i] = startTemperature + (nextAddTemperature * count++);
        }

    }


    private void Update()
    {
        Temperature();
    }

    private void Temperature()
    {

        float windChill = timeTemperature[(int)time.hour] - player.GetTemperature();
        windChill = Mathf.Abs(windChill);
        
        if(windChill <= 10)
        {
            player.AddSecondeTemperature(0.1f);
        }
        else if(windChill <= 29)
        {
            player.AddSecondeTemperature(0.3f);
        }
        else
        {
            player.AddSecondeTemperature(0.5f);
        }
    }

    private void ChakTemperature()
    {
        if (player.currentTemperature > player.HotTemperatuer)
        {
            Debug.Log("감당 가능한 온도를 넘었습니다.");
        }
        else if(player.currentTemperature < player.ColdTemperatuer)
        {
            Debug.Log("온도가 너무 낮습니다.");

        }
        else
        {

        }
    }


    public void addTemperature(float temper)
    {
        player.currentTemperature += temper;
    }


}

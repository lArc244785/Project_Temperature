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

        int i = 0;
        string log = string.Empty;
        foreach(float temp in timeTemperature)
        {
            log += i + "시 외부 온도 : " + temp + "\n";
            i++;
        }
        print(log);
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




    public void DrawLog()
    {
        float windChill = timeTemperature[time.GetHour()]  - player.GetTemperature()  ;
        windChill = Mathf.Abs(windChill);

        string msg =
            "TestView : 시간 : " + time.GetHour() + " , " + "외부 온도 : " + timeTemperature[time.GetHour()] + "\n" +
            "플레이어 온도 : " + player.GetTemperature() + "체감온도 : " + windChill;
        print(msg);
    }


    public float GetTemperature()
    {
        DrawLog();
        float windChill = timeTemperature[time.GetHour()]  - player.GetTemperature()  ;
        windChill = Mathf.Abs(windChill);

        int isAboveZero = timeTemperature[time.GetHour()] > 0 ? 1 : -1;
        float resultTemperature = 0.0f;

        if (windChill <= 10)
        {
            resultTemperature = 0.1f * isAboveZero;
        }
        else if(windChill <= 29)
        {
            resultTemperature = 0.3f * isAboveZero;
        }
        else
        {
            resultTemperature = 0.5f * isAboveZero;
        }

        return resultTemperature;
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

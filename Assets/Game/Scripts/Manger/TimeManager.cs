using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    private float blend = 0f;
    private float timer = 0.0f;

    public Material[] mat;

    public int index;

    public Light mainLight;
    public Light colorLight;
    public Light nightLIght;

    public Color colerRed;
    public Color colorBlue;

    private Color originalNightLightColor;
    private Color originalColorLightColor;

    private void Start()
    {
        transform.GetComponent<MeshRenderer>().sharedMaterial.shader = Shader.Find("Skybox/Blend");

        originalNightLightColor = nightLIght.color;
    }

    private void Update()
    {
        //time for game
        //360 = 1day 80sec = 6degree
        mainLight.transform.Rotate(new Vector3(4.5f, 0f, 0f) * Time.deltaTime);

        SkyboxBlend();

    }

    public void SkyboxBlend()
    {
        if(blend < 1)
        {
            blend += Time.deltaTime * 0.05f;
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

    public void lightColor()
    {
        //if(mainLight.transform.eulerAngles.x >)
        nightLIght.color = Color.Lerp(originalNightLightColor, colorBlue, 0.01f);
    }
}

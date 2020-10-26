using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMagner : MonoBehaviour
{
    public static GameMagner Instance;

    private PlayerControl playerControl;
    private CamerManger camMagner;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else {
            Destroy(gameObject);
        }
        Intilize();
    }

    private void Intilize()
    {
        GetPlayerControl();
        GetCamerManger();
    }


    public PlayerControl GetPlayerControl()
    {
        if(playerControl == null)
        {
            playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        }

        return playerControl;
    }

    public CamerManger GetCamerManger()
    {
        if(camMagner == null)
        {
            camMagner = GameObject.Find("CamerManger").GetComponent<CamerManger>();
        }
        return camMagner;
    }

    public Transform GetCamMangerPos()
    {
        return camMagner.transform;
    }


}

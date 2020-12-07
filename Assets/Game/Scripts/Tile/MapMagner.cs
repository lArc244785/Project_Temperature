﻿using UnityEngine;
using System;
using System.Collections;

public class MapMagner : MonoBehaviour
{
    public TileBase[,] GameMap;

    public int width = 10;
    public int height = 10;

    public TileBase playerTile;

    public TileBase[] wave5;
    public TileBase[] wave6;
    public TileBase[] wave10;

    public bool isDebugMode;
    public Material rootMaterial;

    private int aroundTileCount;


    public GameObject Wall0,Wall1;

    // Start is called before the first frame update
    public void Initializer()
    {
        GameMap = new TileBase[height, width];

        int findChildCount;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                findChildCount = y * height + x;
                TileBase tile = transform.GetChild(findChildCount).GetComponent<TileBase>();
                if (tile == null)
                {
                    Debug.LogError("[" + y + " , " + x + "]" + "Not import TileBase");
                }
                else
                {
                    tile.SetTileIndex(x, y);
                    GameMap[y, x] = tile;
                }
            }
        }
        aroundTileCount = 0;
    }

    public void SetPlayerTile(TileBase tile)
    {
        if (playerTile != tile)
        {
            aroundTileCount = 0;
            for (int tx = -1; tx <= 1; tx++)
            {
                for (int ty = -1; ty <= 1; ty++)
                {
                    StructInfo.Point currentPoint = tile.GetTileInfo().point;
                    currentPoint.x += tx;
                    currentPoint.y += ty;
                    if (currentPoint.x < 0 || currentPoint.x > width - 1 || currentPoint.y < 0 || currentPoint.y > height - 1)
                        continue;
                    if (tx == 0 && ty == 0) continue;

                    if (GameMap[currentPoint.y, currentPoint.x].tileInfo.cost != StructInfo.TILEWALL)
                    {
                        aroundTileCount++;
                    }
                }
            }
           // Debug.Log("플레이어한테 접근 가능한 타일 수: " + aroundTileCount);


            GameManager.Instance.GetEnemyManger().SetPath(tile.tileInfo.point);
            playerTile = tile;
        }
    }



    public TileBase GetPlayerTile()
    {
        return playerTile;
    }


    public void ClearRoot()
    {
        if (!isDebugMode) return;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameMap[y, x].ClearTileMaterial();
            }
        }
    }

    public void SetPathTile(StructInfo.Point p)
    {
        if (!isDebugMode) return;
        GameMap[p.y, p.x].SetTileMaterial(rootMaterial);
    }

    public int GetAroundTileCount()
    {
        return aroundTileCount;
    }

    public IEnumerator Wave()
    {
        if (GameManager.Instance.currentWave + 1 == 5) 
        {
            for(int i=0; i<wave5.Length; i++)
            {
                StartCoroutine(wave5[i].Fall());
            }
            yield return new WaitForSeconds(2f);
        }
        
        if (GameManager.Instance.currentWave + 1 == 6)
        {
            for (int i = 0; i < wave6.Length; i++)
            {
                StartCoroutine(wave6[i].Fall());
            }
            yield return new WaitForSeconds(2f);
        }

        if (GameManager.Instance.currentWave + 1 == 10)
        {
            for (int i = 0; i < wave10.Length; i++)
            {
                StartCoroutine(wave10[i].Fall());
            }
            yield return new WaitForSeconds(2f);
        }
        Wall0.SetActive(false);
        Wall1.SetActive(true);
        yield break;
    }
}

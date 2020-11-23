using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMagner : MonoBehaviour
{
    public TileBase[,] GameMap;

    public int width = 10;
    public int height = 10;

    public TileBase playerTile;


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
                TileBase tile  = transform.GetChild(findChildCount).GetComponent<TileBase>();
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
    }

    public void SetPlayerTile(TileBase tile)
    {
        if (playerTile != tile)
        {
            GameManager.Instance.GetEnemyManger().SetPath(tile.tileInfo.point);
            playerTile = tile;
        }
    }

    public TileBase GetPlayerTile()
    {
        return playerTile;
    }

}

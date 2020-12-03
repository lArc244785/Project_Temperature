using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSensor : MonoBehaviour
{
    public bool isPlayer;
    public EnemyBasic enemy;

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 5.0f, LayerMask.GetMask("Tile")) ){
            TileBase tileBase = hit.transform.GetComponent<TileBase>();
            if (isPlayer)
            {
                GameManager.Instance.GetMapManger().SetPlayerTile(tileBase);
            }
            else
            {
                enemy.tile = tileBase;
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down* 3));
    }
}

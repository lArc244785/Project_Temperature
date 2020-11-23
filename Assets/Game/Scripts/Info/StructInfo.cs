using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructInfo
{
    public const int TILEMAX = 9999;
    public const int TILEWALL =99999;
    public struct TileInfo
    {
        //타일 배열 위치값
        public Point point;
        //타일의 게임뷰 위치
        public Vector3 position;
        //AStar에서의 비용
        public int cost;
        //Astar에서 방문에 대한 
        public bool isVisit;

        public bool isWall;

        public void SetTile(int x, int y, Vector3 position, bool isWall, int cost = TILEMAX)
        {
            point.x = x;
            point.y = y;
            this.cost = cost;
            this.position = position;
            isVisit = false;
            this.isWall = isWall;
            cost = isWall ? TILEWALL : cost;
        }

        public void Reset()
        {
            cost = TILEMAX;
            isVisit = false;
        }

        public void SetIndex(int x, int y)
        {
            point.x = x;
            point.y = y;
        }

        public void SetWallCost()
        {
            cost = TILEWALL;
        }

    }
    public struct Point
    {
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x, y;
    }


}

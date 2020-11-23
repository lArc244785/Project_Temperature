using System.Collections.Generic;
using UnityEngine;

public class PathFindTool : MonoBehaviour
{
    private StructInfo.TileInfo[,] copyGameMap;
    private int width;
    private int height;
    private StructInfo.Point[,] parentNode;
    private List<StructInfo.Point> visitNodeList;


    public void Initialize()
    {
        MapMagner mapManger = GameManager.Instance.GetMapManger();
        //copyGameMap = (StructInfo.TileInfo[,])mapManger.GameMap.Clone();
        this.width = mapManger.width;
        this.height = mapManger.height;

        copyGameMap = new StructInfo.TileInfo[height, width];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                copyGameMap[h, w] = mapManger.GameMap[h, w].tileInfo;
            }
        }

        parentNode = new StructInfo.Point[height, width];
        visitNodeList = new List<StructInfo.Point>();
    }


    private void Reset()
    {
        MapMagner mm = GameManager.Instance.GetMapManger();
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (!copyGameMap[h, w].isWall)
                    copyGameMap[h, w].cost = StructInfo.TILEMAX;
                else
                    copyGameMap[h, w].cost = StructInfo.TILEWALL;
               // mm.GameMap[h, w].mr.material = mm.GameMap[h, w].nomalMaterial;
                copyGameMap[h, w].isVisit = false;

            }
        }
        visitNodeList.Clear();
    }

    //path에는 경로가 담긴다.
    public bool PathFind_AStar
        (StructInfo.Point startNode, StructInfo.Point targetNode, Stack<StructInfo.TileInfo> path)
    {
        path.Clear();
        Reset();

        bool isFind = false;
        bool isOnOrDownWall = false;

        parentNode[startNode.y, startNode.x] = startNode;
        copyGameMap[startNode.y, startNode.x].isVisit = true;
        copyGameMap[startNode.y, startNode.x].cost = 0;

        StructInfo.Point curNode = startNode;

        int nx, ny;
        int cost;
        for (int i = 0; i < width * height; i++)
        {

            copyGameMap[curNode.y, curNode.x].isVisit = true;
            visitNodeList.Add(curNode);

            if (curNode.x == targetNode.x && curNode.y == targetNode.y)
            {
                isFind = true;
                break;
            }

            //현재 선택된 노드의 cost설정
            for (int ty = -1; ty <= 1; ty++)
            {
                for (int tx = -1; tx <= 1; tx++)
                {
                    isOnOrDownWall = false;
                    nx = curNode.x + tx;
                    ny = curNode.y + ty;
                    if (isMapOver(new StructInfo.Point(nx,ny)) || (tx == 0 && ty == 0)) continue;
                    if (copyGameMap[ny, nx].isVisit == false)
                    {
                        cost = copyGameMap[curNode.y, curNode.x].cost;
                        cost += (tx == 0 || ty == 0) ? 10 : 14;
                        if (copyGameMap[ny, nx].cost > cost && copyGameMap[ny, nx].cost != StructInfo.TILEWALL)
                        {

                            if ((tx != 0 && ty != 0))
                            {
                                StructInfo.Point nextNodeLeftOrRight = new StructInfo.Point(curNode.x, ny);
                                StructInfo.Point nextNodeDown = new StructInfo.Point(nx, curNode.y);
                                if(copyGameMap[nextNodeLeftOrRight.y, nextNodeLeftOrRight.x].cost == StructInfo.TILEWALL ||
                                    copyGameMap[nextNodeDown.y, nextNodeDown.x].cost == StructInfo.TILEWALL)
                                isOnOrDownWall = true;
                            }

                   
                                copyGameMap[ny, nx].cost = !isOnOrDownWall ?cost : copyGameMap[curNode.y, curNode.x].cost + 24;
                                parentNode[ny, nx] = curNode;
                           
                            }

                        }
                    }
                
            }
            curNode = Chosse(targetNode);
        }

        if (isFind)
        {
            StructInfo.TileInfo node;
            StructInfo.Point point;
            node = copyGameMap[targetNode.y, targetNode.x];
            path.Push(node);
           // print("PATH====");
            //print("sNode: " + startNode.y + "   " + startNode.x + "  tNode: " + targetNode.y + "  " + targetNode.x);
           // print(node.point.y + " " + node.point.x);

           //MapMagner mm = GameManager.Instance.GetMapManger();
           //mm.GameMap[targetNode.y, targetNode.x].mr.material = mm.GameMap[targetNode.y, targetNode.x].pathMaterial;

            do
            {
                point = parentNode[node.point.y, node.point.x];
              //  print(point.y + " " + point.x);
                node = copyGameMap[point.y, point.x];
              // mm.GameMap[point.y, point.x].mr.material = mm.GameMap[point.y, point.x].pathMaterial;
                path.Push(node);
            } while (point.x != startNode.x || point.y != startNode.y);
           //print("====");
        }

        //타일 디버그용
        //Debug.Log("====AAA=====");
        //for (int y = 0; y < height; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        Debug.Log("[" + y + "," + x + "]" +copyGameMap[y, x].cost);
        //    }
        //}
        //Debug.Log("====AAA=====");

        return isFind;
    }

    private StructInfo.Point Chosse(StructInfo.Point targetNode)
    {
        StructInfo.Point chosseNode = new StructInfo.Point();
        StructInfo.Point chackPoint = new StructInfo.Point();
        StructInfo.Point relaxPoint = new StructInfo.Point();
        int nx, ny;
        int hcost;
        int min = StructInfo.TILEMAX + 1; // 초기셋 보다 커야될 것 같아서
        //현재 방문한 노드에서 갈 수 있는 노드 중에 가장 COST가 적은 노드를 찾아라
        foreach (StructInfo.Point curNode in visitNodeList)
        {
            for (int ty = -1; ty <= 1; ty++)
            {
                for (int tx = -1; tx <= 1; tx++)
                {
                    nx = curNode.x + tx;
                    ny = curNode.y + ty;
                    if (isMapOver(new StructInfo.Point(nx, ny)) || (tx == 0 && ty == 0)) continue;
                    hcost =
                        Mathf.Abs(nx - targetNode.x) * 10 +
                        Mathf.Abs(ny - targetNode.y) * 10;
                    int curCost = copyGameMap[ny, nx].cost;
                    if (min > curCost + hcost && copyGameMap[ny, nx].isVisit == false)
                    {
                        bool isOnOrDownWall = false;
                        //최소 루트가 대각인지 확인
                        //if((tx != 0 && ty != 0) && copyGameMap[curNode.y, nx].cost == StructInfo.TILEWALL)
                        //     {
                        //         isOnOrDownWall = true;
                        //         StructInfo.TileInfo tile = copyGameMap[ny, curNode.x];
                        //         if (tile.cost != StructInfo.TILEWALL && !tile.isVisit)
                        //         {
                        //             min = curCost + hcost;
                        //             chosseNode.x = curNode.x;
                        //             chosseNode.y = ny;
                        //         }
                        //     }

                        //if (!isOnOrDownWall)
                        //{
                        //    min = curCost + hcost;
                        //    chosseNode.x = nx;
                        //    chosseNode.y = ny;
                        //}
                        //else
                        //{
                        //    copyGameMap[ny, nx].cost = copyGameMap[curNode.y, curNode.x].cost + 24;
                        //}

                        min = curCost + hcost;
                        chosseNode.x = nx;
                        chosseNode.y = ny;
                    }
                }
            }
        }


        //Debug.Log(chosseNode.x + "  " + chosseNode.y);
        return chosseNode;
    }


    private bool isMapOver(StructInfo.Point point)
    {
        return point.x < 0 || point.x > width - 1 || point.y < 0 || point.y > height - 1;
    }
}

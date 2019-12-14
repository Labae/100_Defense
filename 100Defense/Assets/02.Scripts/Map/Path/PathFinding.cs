using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinding : MonoBehaviour
{
    /// <summary>
    /// 맵 클래스.
    /// </summary>
    private MapManager mMap;
    /// <summary>
    /// Path를 찾아주는 클래스.
    /// </summary>
    private PathRequestManager mPathRequsetManager;
    /// <summary>
    /// 적이 걸어갈 길.
    /// </summary>
    private CellClass[] mPath;
    /// <summary>
    /// 시작 Cell.
    /// </summary>
    private CellClass mStartCell;
    /// <summary>
    /// Goal Cell.
    /// </summary>
    private CellClass mGoalCell;

    /// <summary>
    /// 길찾기를 성공하였는지.
    /// </summary>
    private bool mPathSuccess;

    #region Method
    /// <summary>
    /// 길찾기 초기화 함수.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public bool Initialize(MapManager map)
    {
        mMap = map;

        mStartCell = mMap.GetCell(0, 0);
        if (!mStartCell)
        {
            Debug.Log("Failed Get startCell.");
            return false;
        }
        mGoalCell = mMap.GetCell(mMap.GetMapSizeX() - 1, mMap.GetMapSizeY() - 1);
        if (!mGoalCell)
        {
            Debug.Log("Failed Get goalCell.");
            return false;
        }

        mPathRequsetManager = gameObject.AddComponent<PathRequestManager>();
        mPathRequsetManager.Initialize();

        return true;
    }

    /// <summary>
    /// 길찾기 함수.
    /// </summary>
    public void PathFind()
    {
        PathRequestManager.RequestPath(mStartCell, mGoalCell, OnPathFound);
    }

    /// <summary>
    /// 길찾기 Callback함수.
    /// </summary>
    /// <param name="newPath"></param>
    /// <param name="pathSuccess"></param>
    public void OnPathFound(CellClass[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            mPath = newPath;
            StopCoroutine(ShowPath());
            StartCoroutine(ShowPath());
        }
    }

    /// <summary>
    /// 길찾기 시작 함수.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    public void StartFindPath(CellClass start, CellClass goal)
    {
        StartCoroutine(FindPath(start, goal));
    }

    /// <summary>
    /// 찾은 길을 거꾸로 올라가는 함수.
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    /// <returns></returns>
    private CellClass[] RetracePath(CellClass startNode, CellClass endNode)
    {
        List<CellClass> path = new List<CellClass>();
        CellClass currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.GetParent();
        }

        path.Reverse();
        path.Insert(0, startNode);

        return path.ToArray();
    }

    #endregion

    #region Coroutine
    /// <summary>
    /// 길찾기 코루틴
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    private IEnumerator FindPath(CellClass start, CellClass goal)
    {
        CellClass[] wayPoints = new CellClass[0];
        mPathSuccess = false;
        if (start.GetWalkable() && goal.GetWalkable())
        {
            Heap<CellClass> openSet = new Heap<CellClass>(mMap.GetMaxSize());
            HashSet<CellClass> closeSet = new HashSet<CellClass>();

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                CellClass currentNode = openSet.RemoveFirst();
                closeSet.Add(currentNode);

                if (currentNode == goal)
                {
                    mPathSuccess = true;
                    break;
                }

                foreach (CellClass neighbour in mMap.GetNeighbours(currentNode))
                {
                    if (!neighbour.GetWalkable() || closeSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.GetGCost() + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.GetGCost() || !openSet.Contains(neighbour))
                    {
                        neighbour.SetGCost(newMovementCostToNeighbour);
                        neighbour.SetHCost(GetDistance(neighbour, goal));
                        neighbour.SetParent(currentNode);

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        yield return null;
        if (mPathSuccess)
        {
            wayPoints = RetracePath(start, goal);
        }
        mPathRequsetManager.FinishedProcessingPath(wayPoints, mPathSuccess);
    }

    /// <summary>
    /// 찾은 길을 표시해주는 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowPath()
    {
        for (int x = 0; x < mMap.GetMapSizeX(); x++)
        {
            for (int y = 0; y < mMap.GetMapSizeY(); y++)
            {
                if (mMap.GetCell(x, y).GetState() == CellClass.CellState.EStart ||
                    mMap.GetCell(x, y).GetState() == CellClass.CellState.EGoal)
                {
                    continue;
                }

                mMap.SetSelectedCell(null);
                mMap.GetCell(x, y).SetState(CellClass.CellState.EDefault);
            }
        }

        for (int i = 0; i < mPath.Length; i++)
        {
            if (mPath[i].GetState() == CellClass.CellState.EStart)
            {
                continue;
            }
            else if (mPath[i].GetState() == CellClass.CellState.EGoal)
            {
                continue;
            }

            if (mPath[i].GetState() != CellClass.CellState.ERoad)
            {
                mPath[i].SetState(CellClass.CellState.ERoad);
            }

            mPath[i].Anim();

            yield return null;
        }

        yield return null;
    }
    #endregion


    #region Get
    /// <summary>
    /// 거리 가져오기.
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns></returns>
    private int GetDistance(CellClass nodeA, CellClass nodeB)
    {
        int distX = Mathf.Abs(nodeA.GetCellX() - nodeB.GetCellX());
        int distY = Mathf.Abs(nodeA.GetCellY() - nodeB.GetCellY());

        return 10 * distX + 10 * distY;
    }

    /// <summary>
    /// 길찾기 성공 여부를 가져옴.
    /// </summary>
    /// <returns></returns>
    public bool GetPathSuccess()
    {
        return mPathSuccess;
    }

    /// <summary>
    /// 길찾은 길을 가져옴.
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetPath()
    {
        if (mPath == null)
        {
            Debug.Log("Path is null");
            return null;
        }

        List<Vector3> path = new List<Vector3>();
        for (int i = 0; i < mPath.Length; i++)
        {
            path.Add(mPath[i].transform.position);
        }

        return path;
    }
    #endregion
}

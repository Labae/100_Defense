using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinding : MonoBehaviour
{
    private MapManager mMap;
    private PathRequestManager mPathRequsetManager;
    private CellClass[] mPath;
    private CellClass mStartCell;
    private CellClass mGoalCell;

    private bool mPathSuccess;

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

    public void PathFind()
    {
        PathRequestManager.RequestPath(mStartCell, mGoalCell, OnPathFound);
    }

    public void OnPathFound(CellClass[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            mPath = newPath;
            StopCoroutine(ShowPath());
            StartCoroutine(ShowPath());
        }
    }

    public void StartFindPath(CellClass start, CellClass goal)
    {
        StartCoroutine(FindPath(start, goal));
    }

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

    private int GetDistance(CellClass nodeA, CellClass nodeB)
    {
        int distX = Mathf.Abs(nodeA.GetCellX() - nodeB.GetCellX());
        int distY = Mathf.Abs(nodeA.GetCellY() - nodeB.GetCellY());

        return 10 * distX + 10 * distY;
    }

    public bool GetPathSuccess()
    {
        return mPathSuccess;
    }

    public List<Vector3> GetPath()
    {
        if(mPath == null)
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
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private CellClass[,] mMap;
    private CellClass mSelectedCell;
    private CellClass mStartCell;
    private CellClass mGoalCell;

    private const int mMapSizeX = 10;
    private const int mMapSizeY = 10;

    private WaitForSeconds mWFSCellApperanceSquareAnimTime;
    private WaitForSeconds mWFSCellStartGoalAnimTime;
    private int mLayerMask;
    private bool mIsFinishedCellAnim;

    private Map mMapData;
    private PathFinding mPathFind;
    public bool Initialize()
    {
        mMapData = Resources.Load("03.Datas/MapData") as Map;

        mMap = new CellClass[mMapSizeX, mMapSizeY];
        if (mMap.Length <= 0)
        {
            Debug.Log("Failed Initialize mCell.");
            return false;
        }

        for (int x = 0; x < mMapSizeX; x++)
        {
            for (int y = 0; y < mMapSizeY; y++)
            {
                mMap[x, y] = transform.GetChild(y).GetChild(x).GetComponent<CellClass>();
                if (mMap[x, y] == null)
                {
                    Debug.Log("Failed GetComponent Cell.");
                    return false;
                }

                if (!mMap[x, y].Initialize(x, y, mMapData.dataArray[y]))
                {
                    Debug.Log("Failed Initialize Cell Component.");
                    return false;
                }
            }
        }

        mStartCell = mMap[0, 0];
        mGoalCell = mMap[mMapSizeX - 1, mMapSizeY - 1];

        mWFSCellApperanceSquareAnimTime = new WaitForSeconds(0.05f);
        mWFSCellStartGoalAnimTime = new WaitForSeconds(0.5f);

        mPathFind = gameObject.AddComponent<PathFinding>();
        if(!mPathFind)
        {
            Debug.Log("Failed Add PathFinding Component.");
            return false;
        }

        if (!mPathFind.Initialize(this))
        {
            Debug.Log("Failed Initialize PathFind.");
        }

        return true;
    }

    public IEnumerator MapAnimCoroutine()
    {
        yield return StartCoroutine(CellAnimationCoroutine());
        for (int x = 0; x < mMapSizeX; x++)
        {
            for (int y = 0; y < mMapSizeY; y++)
            {
               if(mMap[x,y].GetTower() != null)
                {
                    StartCoroutine(mMap[x, y].GetTower().ApperanceAnim());
                }
            }
        }
        yield return null;
    }
    private IEnumerator CellAnimationCoroutine()
    {
        mStartCell.Anim();
        mGoalCell.Anim();
        yield return mWFSCellStartGoalAnimTime;

        for (int x = 4; x < 6; x++)
        {
            for (int y = 4; y < 6; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 3; x < 7; x++)
        {
            for (int y = 3; y < 7; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 2; x < 8; x++)
        {
            for (int y = 2; y < 8; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 1; x < 9; x++)
        {
            for (int y = 1; y < 9; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        mIsFinishedCellAnim = true;
    }

    public void SetSelectedCell(CellClass cell)
    {
        if (mSelectedCell != null)
        {
            mSelectedCell.ReleaseSelected();
        }

        mSelectedCell = cell;
    }

    public CellClass GetSelectedCell()
    {
        return mSelectedCell;
    }

    public int GetMapSizeX()
    {
        return mMapSizeX;
    }
    public int GetMapSizeY()
    {
        return mMapSizeY;
    }

    public int GetMapMaxSize()
    {
        return mMapSizeX * mMapSizeY;
    }

    public int GetLayerMask()
    {
        return 1 << LayerMask.NameToLayer("Cell");
    }

    public bool GetIsFinishedCellsAnim()
    {
        return mIsFinishedCellAnim;
    }

    public PathFinding GetPathFinding()
    {
        return mPathFind;
    }

    public List<CellClass> GetNeighbours(CellClass node)
    {
        List<CellClass> neighbours = new List<CellClass>();

        // Left
        if(node.GetCellX() - 1 >= 0)
        {
            neighbours.Add(mMap[node.GetCellX() - 1, node.GetCellY()]);
        }
        // Right
        if (node.GetCellX() + 1 < mMapSizeX)
        {
            neighbours.Add(mMap[node.GetCellX() + 1, node.GetCellY()]);
        }
        // Up
        if (node.GetCellY() - 1 >= 0)
        {
            neighbours.Add(mMap[node.GetCellX(), node.GetCellY() - 1]);
        }
        // Down
        if (node.GetCellY() + 1 < mMapSizeY)
        {
            neighbours.Add(mMap[node.GetCellX(), node.GetCellY() + 1]);
        }

        return neighbours;
    }

    public CellClass[,] GetCells()
    {
        return mMap;
    }

    public CellClass GetCell(int x, int y)
    {
        return mMap[x, y];
    }
}

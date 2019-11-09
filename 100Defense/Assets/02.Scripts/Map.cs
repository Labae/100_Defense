using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Cell[,] mCell;

    private const int mMapSizeX = 10;
    private const int mMapSizeY = 10;

    private WaitForSeconds mWFSCellApperanceSquareAnimTime;

    public bool Initialize()
    {
        mCell = new Cell[mMapSizeX, mMapSizeY];
        if (mCell.Length <= 0)
        {
            Debug.Log("Failed Initialize mCell.");
            return false;
        }

        for (int x = 0; x < mMapSizeX; x++)
        {
            for (int y = 0; y < mMapSizeY; y++)
            {
                mCell[x, y] = transform.GetChild(y).GetChild(x).GetComponent<Cell>();
                if (mCell[x, y] == null)
                {
                    Debug.Log("Failed GetComponent Cell.");
                    return false;
                }

                if (!mCell[x, y].Initialize(x, y))
                {
                    Debug.Log("Failed Initialize Cell Component.");
                    return false;
                }
            }
        }

        mWFSCellApperanceSquareAnimTime = new WaitForSeconds(0.1f);
        StopCoroutine(CellAnimationCoroutine());
        StartCoroutine(CellAnimationCoroutine());

        return true;
    }

    private IEnumerator CellAnimationCoroutine()
    {
        for (int x = 4; x < 6; x++)
        {
            for (int y = 4; y < 6; y++)
            {
                mCell[y, x].ApperanceAnimation();
                mCell[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 3; x < 7; x++)
        {
            for (int y = 3; y < 7; y++)
            {
                mCell[y, x].ApperanceAnimation();
                mCell[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 2; x < 8; x++)
        {
            for (int y = 2; y < 8; y++)
            {
                mCell[y, x].ApperanceAnimation();
                mCell[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 1; x < 9; x++)
        {
            for (int y = 1; y < 9; y++)
            {
                mCell[y, x].ApperanceAnimation();
                mCell[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                mCell[y, x].ApperanceAnimation();
                mCell[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;
    }

    public int GetMapSizeX()
    {
        return mMapSizeX;
    }
    public int GetMapSizeY()
    {
        return mMapSizeY;
    }
}

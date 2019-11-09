using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Cell[,] mCell;

    private const int mMapSizeX = 10;
    private const int mMapSizeY = 10;

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

        return true;
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

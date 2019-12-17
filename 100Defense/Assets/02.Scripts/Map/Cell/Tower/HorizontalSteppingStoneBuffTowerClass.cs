using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSteppingStoneBuffTowerClass : BuffTowerClass
{
    protected override List<Tuple<int, int>> GetTargetPositions()
    {
        List<Tuple<int, int>> positions = new List<Tuple<int, int>>();
        for (int x = mCurrentCellX; x < mMapSizeX; x += 2)
        {
            if (x == mCurrentCellX)
            {
                continue;
            }
            Tuple<int, int> targetPos = new Tuple<int, int>(x, mCurrentCellY);
            positions.Add(targetPos);
        }
        for (int x = mCurrentCellX; x >= 0; x -= 2)
        {
            if (x == mCurrentCellX)
            {
                continue;
            }
            Tuple<int, int> targetPos = new Tuple<int, int>(x, mCurrentCellY);
            positions.Add(targetPos);
        }
        return positions;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSteppingStoneBuffTowerClass : BuffTowerClass
{
    protected override List<Tuple<int, int>> GetTargetPositions()
    {
        List<Tuple<int, int>> positions = new List<Tuple<int, int>>();
        for (int y = mCurrentCellY; y < mMapSizeY; y += 2)
        {
            if(y == mCurrentCellY)
            {
                continue;
            }
            Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX, y);
            positions.Add(targetPos);
        }
        for (int y = mCurrentCellY; y >= 0; y -= 2)
        {
            if (y == mCurrentCellY)
            {
                continue;
            }
            Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX, y);
            positions.Add(targetPos);
        }
        return positions;
    }
}

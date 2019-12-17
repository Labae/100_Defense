using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// + 모양으로 버프를 줄 타워 클래스.
/// </summary>
public class PlusBuffTowerClass : BuffTowerClass
{
    #region Method
    /// <summary>
    /// 버프를 받을 수 있는 좌표를 받아오는 함수.
    /// </summary>
    /// <returns></returns>
    protected override List<Tuple<int, int>> GetTargetPositions()
    {
        List<Tuple<int, int>> positions = new List<Tuple<int, int>>();
        for (int x = -1; x < 2; x++)
        {
            if (x == 0)
            {
                continue;
            }

            int posX = x + mCurrentCellX;

            if (posX < 0 || posX >= mMapSizeX)
            {
                continue;
            }
            Tuple<int, int> targetPos = new Tuple<int, int>(posX, mCurrentCellY);
            positions.Add(targetPos);
        }
        for (int y = -1; y < 2; y++)
        {
            if (y == 0)
            {
                continue;
            }

            int posY = y + mCurrentCellY;

            if (posY < 0 || posY >= mMapSizeY)
            {
                continue;
            }
            Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX, posY);
            positions.Add(targetPos);
        }

        return positions;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ㅁ모양으로 버프를 줄 타워 클래스.
/// </summary>
public class CubeBuffTowerClass : BuffTowerClass
{
    #region Method
    /// <summary>
    /// 버프를 받을 수 있는 좌표를 받아오는 함수.
    /// </summary>
    /// <returns></returns>
    protected override List<Tuple<int, int>> GetTargetPositions()
    {
        List<Tuple<int, int>> positions = new List<Tuple<int, int>>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int posX = i + mCurrentCellX;
                int posY = j + mCurrentCellY;
                if (posX < 0 || posX >= mMapSizeX)
                {
                    continue;
                }
                if (posY < 0 || posY >= mMapSizeY)
                {
                    continue;
                }

                Tuple<int, int> targetPos = new Tuple<int, int>(posX, posY);
                positions.Add(targetPos);
            }
        }

        return positions;
    }
    #endregion
}

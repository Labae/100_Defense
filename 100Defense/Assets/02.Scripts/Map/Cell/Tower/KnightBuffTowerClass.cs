using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBuffTowerClass : BuffTowerClass
{
    #region Method
    /// <summary>
    /// 버프를 받을 수 있는 좌표를 받아오는 함수.
    /// </summary>
    /// <returns></returns>
    protected override List<Tuple<int, int>> GetTargetPositions()
    {
        List<Tuple<int, int>> positions = new List<Tuple<int, int>>(); 
        // ＠ㅁㅁㅁㅁ
        // ㅁㅁ★ㅁㅁ
        // ＠ㅁㅁㅁㅁ
        if(mCurrentCellX - 2 >= 0)
        {
            if(mCurrentCellY -1 >= 0)
            {
                Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX - 2, mCurrentCellY - 1);
                positions.Add(targetPos);
            }
            if (mCurrentCellY + 1 < mMapSizeY)
            {
                Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX - 2, mCurrentCellY + 1);
                positions.Add(targetPos);
            }
        }

        // ㅁㅁㅁㅁ＠
        // ㅁㅁ★ㅁㅁ
        // ㅁㅁㅁㅁ＠
        if (mCurrentCellX + 2 < mMapSizeX)
        {
            if (mCurrentCellY - 1 >= 0)
            {
                Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX + 2, mCurrentCellY - 1);
                positions.Add(targetPos);
            }
            if (mCurrentCellY + 1 < mMapSizeY)
            {
                Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX + 2, mCurrentCellY + 1);
                positions.Add(targetPos);
            }
        }

        // ㅁㅁㅁㅁㅁ
        // ㅁㅁㅁㅁㅁ
        // ㅁㅁ★ㅁㅁ
        // ㅁㅁㅁㅁㅁ
        // ㅁ＠ㅁ＠ㅁ
        if (mCurrentCellY - 2 >= 0)
        {
            if (mCurrentCellX - 1 >= 0)
            {
                Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX - 1, mCurrentCellY - 2);
                positions.Add(targetPos);
            }
            if (mCurrentCellX + 1 < mMapSizeX)
            {
                Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX + 1, mCurrentCellY - 2);
                positions.Add(targetPos);
            }
        }

        // ㅁ＠ㅁ＠ㅁ
        // ㅁㅁㅁㅁㅁ
        // ㅁㅁ★ㅁㅁ
        // ㅁㅁㅁㅁㅁ
        // ㅁㅁㅁㅁㅁ
        if (mCurrentCellY + 2 < mMapSizeY)
        {
            if (mCurrentCellX - 1 >= 0)
            {
                Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX - 1, mCurrentCellY + 2);
                positions.Add(targetPos);
            }
            if (mCurrentCellX + 1 < mMapSizeX)
            {
                Tuple<int, int> targetPos = new Tuple<int, int>(mCurrentCellX + 1, mCurrentCellY + 2);
                positions.Add(targetPos);
            }
        }
        return positions;
    }
    #endregion
}

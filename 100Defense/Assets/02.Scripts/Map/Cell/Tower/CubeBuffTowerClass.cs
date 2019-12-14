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
    /// 타워 초기화 함수.
    /// </summary>
    /// <param name="cellData"></param>
    /// <returns></returns>
    public override bool Initialize(string cellData)
    {
        if (!base.Initialize(cellData))
        {
            return false;
        }
        mTargetPositions = SetTargetTowers();

        return true;
    }

    /// <summary>
    /// 타워 짓는 함수.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="cellData"></param>
    /// <returns></returns>
    public override bool Build(CellClass cell, string cellData)
    {
        if (!base.Build(cell, cellData))
        {
            return false;
        }
        mTargetPositions = SetTargetTowers();

        return true;
    }

    /// <summary>
    /// 버프를 받을 대상을 설정 하는 함수.
    /// </summary>
    /// <returns></returns>
    private List<Tuple<int, int>> SetTargetTowers()
    {
        mTargetPositions = GetTargetPositions();

        switch (mTowerData.BUFFTYPE)
        {
            case BuffType.Damage:
                mIBuffAmount = (int)mTowerData.Buffamount;
                break;
            case BuffType.AttackSpeed:
                mBuffAmount = mTowerData.Buffamount;
                break;
            case BuffType.AttackRange:
                mBuffAmount = mTowerData.Buffamount;
                break;
        }

        mTargetTowers = GetTargetTowers(mTargetPositions);
        mPrevTargetTowerCount = mTargetTowers.Length;
        for (int i = 0; i < mTargetTowers.Length; i++)
        {
            switch (mTowerData.BUFFTYPE)
            {
                case BuffType.Damage:
                    mTargetTowers[i].UpgradeAttackDamage(mIBuffAmount);
                    break;
                case BuffType.AttackSpeed:
                    mTargetTowers[i].UpgradeAttackSpeed(mBuffAmount);
                    break;
                case BuffType.AttackRange:
                    mTargetTowers[i].UpgradeAttackRange(mBuffAmount);
                    break;
                default:
                    continue;
            }
            mEffectObjects.Add(mObjectPool.SpawnTowerEffect(mTowerData.BUFFTYPE, mTargetTowers[i].transform));
        }

        return mTargetPositions;
    }

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

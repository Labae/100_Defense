using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlusBuffTowerClass : BuffTowerClass
{
    public override bool Initialize(string cellData)
    {
        if (!base.Initialize(cellData))
        {
            return false;
        }

        mTargetPositions = InitializeTargets();

        return true;
    }

    public override bool Build(CellClass cell, string cellData)
    {
        if (!base.Build(cell, cellData))
        {
            return false;
        }

        mTargetPositions = InitializeTargets();

        return true;
    }

    private List<Tuple<int, int>> InitializeTargets()
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
                    mTargetTowers[i].UpdateAttackDamage(mIBuffAmount);
                    break;
                case BuffType.AttackSpeed:
                    mTargetTowers[i].UpdateAttackSpeed(mBuffAmount);
                    break;
                case BuffType.AttackRange:
                    mTargetTowers[i].UpdateAttackRange(mBuffAmount);
                    break;
                default:
                    continue;
            }
            mEffectObjects.Add(mObjectPool.SpawnTowerEffect(mTowerData.BUFFTYPE, mTargetTowers[i].transform));
        }

        return mTargetPositions;
    }

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
}

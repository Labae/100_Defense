using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeBuffTowerClass : BuffTowerClass
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
}

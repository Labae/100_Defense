using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffTowerClass : TowerClass
{
    protected TowerClass[] mTargetTowers;
    protected List<Tuple<int, int>> mTargetPositions;
    protected ObjectPool mObjectPool;
    protected List<GameObject> mEffectObjects;

    protected int mCurrentCellX;
    protected int mCurrentCellY;
    protected int mMapSizeX;
    protected int mMapSizeY;
    /// <summary>
    /// int buffamount value.
    /// </summary>
    protected int mIBuffAmount;
    protected float mBuffAmount;
    protected int mPrevTargetTowerCount;

    public override bool Initialize(string cellData)
    {
        if (!base.Initialize(cellData))
        {
            return false;
        }

        CellClass currentCell = GetComponentInParent<CellClass>();
        if (currentCell == null)
        {
            Debug.Log("Failed to Get current Cell");
            return false;
        }
        mCurrentCellX = currentCell.GetCellX();
        mCurrentCellY = currentCell.GetCellY();
        mMapSizeX = currentCell.GetMap().GetMapSizeX();
        mMapSizeY = currentCell.GetMap().GetMapSizeY();

        mObjectPool = GameManager.Instance.GetObjectPool();
        mEffectObjects = new List<GameObject>();

        return true;
    }

    public override bool Build(CellClass cell, string cellData)
    {
        if (!base.Build(cell, cellData))
        {
            return false;
        }
        CellClass currentCell = GetComponentInParent<CellClass>();
        if (currentCell == null)
        {
            Debug.Log("Failed to Get current Cell");
            return false;
        }
        mCurrentCellX = currentCell.GetCellX();
        mCurrentCellY = currentCell.GetCellY();
        mMapSizeX = currentCell.GetMap().GetMapSizeX();
        mMapSizeY = currentCell.GetMap().GetMapSizeY();

        mObjectPool = GameManager.Instance.GetObjectPool();
        mEffectObjects = new List<GameObject>();

        return true;
    }

    public override void Loop(List<EnemyClass> enemies)
    {
        base.Loop(enemies);

        mTargetPositions = GetTargetPositions();
        mTargetTowers = GetTargetTowers(mTargetPositions);
        if (mPrevTargetTowerCount != mTargetTowers.Length)
        {
            mPrevTargetTowerCount = mTargetTowers.Length;
            for (int objectCount = 0; objectCount < mEffectObjects.Count; objectCount++)
            {
                mObjectPool.HideTowerEffect(mTowerData.BUFFTYPE, mEffectObjects[objectCount]);
            }
            mEffectObjects.Clear();

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
        }
    }

    public override void DestroyTower(CellClass cell)
    {
        mTargetTowers = GetTargetTowers(mTargetPositions);
        for (int i = 0; i < mTargetTowers.Length; i++)
        {
            switch (mTowerData.BUFFTYPE)
            {
                case BuffType.Damage:
                    mTargetTowers[i].DowngradeAttackDamage(mIBuffAmount);
                    break;
                case BuffType.AttackSpeed:
                    mTargetTowers[i].DowngradeAttackSpeed(mBuffAmount);
                    break;
                case BuffType.AttackRange:
                    mTargetTowers[i].DowngradeAttackRange(mBuffAmount);
                    break;
                default:
                    continue;
            }
        }

        for (int i = 0; i < mEffectObjects.Count; i++)
        {
            mObjectPool.HideTowerEffect(mTowerData.BUFFTYPE, mEffectObjects[i]);
        }

        base.DestroyTower(cell);
    }

    protected TowerClass[] GetTargetTowers(List<Tuple<int, int>> coordi)
    {
        List<TowerClass> targetTowers = new List<TowerClass>();
        CellClass[,] map = GameManager.Instance.GetMapManager().GetMap();
        for (int i = 0; i < coordi.Count; i++)
        {
            int x = coordi[i].Item1;
            int y = coordi[i].Item2;
            TowerClass targetTower = map[x, y].GetTower();
            if (targetTower != null)
            {
                targetTowers.Add(targetTower);
            }
        }

        return targetTowers.ToArray();
    }

    protected virtual List<Tuple<int, int>> GetTargetPositions()
    {
        return null;
    }
}

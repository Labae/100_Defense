﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffTowerClass : TowerClass
{
    /// <summary>
    /// 버프를 줄 타워들
    /// </summary>
    protected TowerClass[] mTargetTowers;
    /// <summary>
    /// 버프를 줄 수 있는 좌표.
    /// </summary>
    protected List<Tuple<int, int>> mTargetPositions;
    /// <summary>
    /// 오브젝트 풀 클래스.
    /// </summary>
    protected ObjectPool mObjectPool;
    /// <summary>
    /// 버프 이펙트 list.
    /// </summary>
    protected List<GameObject> mEffectObjects;

    /// <summary>
    /// 현재 칸의 X좌표.
    /// </summary>
    protected int mCurrentCellX;
    /// <summary>
    /// 현재 칸의 y좌표.
    /// </summary>
    protected int mCurrentCellY;
    /// <summary>
    /// 맵의 X크기
    /// </summary>
    protected int mMapSizeX;
    /// <summary>
    /// 맵의 Y크기.
    /// </summary>
    protected int mMapSizeY;
    /// <summary>
    /// int buffamount value.
    /// </summary>
    protected int mIBuffAmount;
    /// <summary>
    /// FLOAT 버프 증가량.
    /// </summary>
    protected float mBuffAmount;
    /// <summary>
    /// 업데이트 되기 전의 타겟 타워의 개수.
    /// </summary>
    protected int mPrevTargetTowerCount;

    #region Method
    /// <summary>
    /// 버프 타워 초기화 함수.
    /// </summary>
    /// <param name="cellData"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 버프 타워 짓느 함수.
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

    /// <summary>
    /// 버프 타워 루프 함수.
    /// </summary>
    /// <param name="enemies"></param>
    public override void Loop(List<EnemyClass> enemies)
    {
        base.Loop(enemies);

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
        }
    }

    /// <summary>
    /// 버프 타워 파괴 함수.
    /// </summary>
    /// <param name="cell"></param>
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

    /// <summary>
    /// 버프를 줄 수 있는 좌표에 있는 타워들을 가져오는 함수.
    /// </summary>
    /// <param name="coordi"></param>
    /// <returns></returns>
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
    #endregion
}

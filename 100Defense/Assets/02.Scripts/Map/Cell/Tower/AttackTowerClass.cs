using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTowerClass : TowerClass
{
    /// <summary>
    /// 캐논(발사할 곳)
    /// </summary>
    private Canon mCanon;
    /// <summary>
    /// 공격 속도.
    /// </summary>
    private float mAttackRange;
    /// <summary>
    /// 타워 회전 속도.(공격 대상을 향하도록)
    /// </summary>
    private readonly float mRotateSpeed = 5.0f;

    #region Method
    /// <summary>
    /// 공격 타워 초기화 함수.
    /// </summary>
    /// <param name="cellData"></param>
    /// <returns></returns>
    public override bool Initialize(string cellData)
    {
        if(!base.Initialize(cellData))
        {
            return false;
        }

        mCanon = GetComponentInChildren<Canon>();
        if (!mCanon)
        {
            Debug.Log("Failed Get Canon Component");
            return false;
        }

        if (!mCanon.GetIsInitialize())
        {
            if (!mCanon.Initialize(mTowerData))
            {
                Debug.Log("Failed Initialize Canon");
                return false;
            }
        }

        mAttackRange = mTowerData.Range;

        return true;
    }

    /// <summary>
    /// 공격 타워 짓는 함수.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="cellData"></param>
    /// <returns></returns>
    public override bool Build(CellClass cell, string cellData)
    {
        if(!base.Build(cell, cellData))
        {
            return false;
        }

        mCanon = GetComponentInChildren<Canon>();
        if (!mCanon)
        {
            Debug.Log("Failed Get Canon Component");
            return false;
        }

        if (!mCanon.Initialize(mTowerData))
        {
            Debug.Log("Failed Initialize Canon");
            return false;
        }

        mAttackRange = mTowerData.Range;

        return true;
    }

    /// <summary>
    /// 공격 타워 루프 함수.
    /// </summary>
    /// <param name="enemies"></param>
    public override void Loop(List<EnemyClass> enemies)
    {
        base.Loop(enemies);
        Transform target = null;
        target = GetWithinRange(enemies);
        Rotate(target); 
        mCanon.Loop(target, transform.eulerAngles.y);
    }

    /// <summary>
    /// 공격 범위에 있는 적 가져오기.
    /// </summary>
    /// <param name="enemies"></param>
    /// <returns></returns>
    private Transform GetWithinRange(List<EnemyClass> enemies)
    {
        Transform nearestEnemy = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, enemies[i].transform.position);
            if (dist <= mAttackRange)
            {
                nearestEnemy = enemies[i].transform;
                break;
            }
        }

        return nearestEnemy;
    }

    /// <summary>
    /// 타워 회전 함수.
    /// </summary>
    /// <param name="target"></param>
    private void Rotate(Transform target)
    {
        if (target == null)
        {
            if (!GameManager.Instance.GetWaveManager().GetIsWaving())
            {
                Vector3 dir = GameManager.Instance.GetMapManager().GetStartCell().transform.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * mRotateSpeed).eulerAngles;
                transform.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);
            }
        }
        else
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * mRotateSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);
        }
    }

    /// <summary>
    /// 타워 파괴 함수.
    /// </summary>
    /// <param name="cell"></param>
    public override void DestroyTower(CellClass cell)
    {
        base.DestroyTower(cell);
    }

    /// <summary>
    /// 공격 범위 업그레이드.
    /// </summary>
    /// <param name="newAttackRange"></param>
    public override void UpgradeAttackRange(float newAttackRange)
    {
        mAttackRange = mAttackRange + newAttackRange;
    }

    /// <summary>
    /// 공격력 업그레이드.
    /// </summary>
    /// <param name="newAttackDamage"></param>
    public override void UpgradeAttackDamage(int newAttackDamage)
    {
        mCanon.UpgradeAttackDamage(newAttackDamage);
    }

    /// <summary>
    /// 공격 속도 업그레이드.
    /// </summary>
    /// <param name="newAttackSpeed"></param>
    public override void UpgradeAttackSpeed(float newAttackSpeed)
    {
        mCanon.UpgradeAttackSpeed(newAttackSpeed);
    }

    /// <summary>
    /// 공격력 다운그레이드.
    /// </summary>
    /// <param name="newAttackDamage"></param>
    public override void DowngradeAttackDamage(int newAttackDamage)
    {
        mCanon.DowngradeAttackDamage(newAttackDamage);
    }

    /// <summary>
    /// 공격력 업그레이드.
    /// </summary>
    /// <param name="newAttackRange"></param>
    public override void DowngradeAttackRange(float newAttackRange)
    {
        mAttackRange = mAttackRange - newAttackRange;
    }

    /// <summary>
    /// 공격속도 다운그레이드
    /// </summary>
    /// <param name="newAttackSpeed"></param>
    public override void DowngradeAttackSpeed(float newAttackSpeed)
    {
        mCanon.DowngradeAttackSpeed(newAttackSpeed);
    }
    #endregion
}

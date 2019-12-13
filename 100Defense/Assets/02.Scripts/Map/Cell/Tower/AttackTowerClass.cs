using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTowerClass : TowerClass
{
    private Canon mCanon;
    private float mAttackRange;
    private readonly float mRotateSpeed = 5.0f;

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

        if (!mCanon.IsInitialize())
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

    public override void Loop(List<EnemyClass> enemies)
    {
        base.Loop(enemies);
        Transform target = null;
        target = GetWithinRange(enemies);
        Rotate(target); 
        mCanon.Loop(target, transform.eulerAngles.y);
    }

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

    public override void DestroyTower(CellClass cell)
    {
        base.DestroyTower(cell);
    }

    public override void UpdateAttackRange(float newAttackRange)
    {
        mAttackRange = mAttackRange + newAttackRange;
    }

    public override void UpdateAttackDamage(int newAttackDamage)
    {
        mCanon.UpdateAttackDamage(newAttackDamage);
    }

    public override void UpdateAttackSpeed(float newAttackSpeed)
    {
        mCanon.UpdateAttackSpeed(newAttackSpeed);
    }

    public override void DowngradeAttackDamage(int newAttackDamage)
    {
        mCanon.DowngradeAttackDamage(newAttackDamage);
    }

    public override void DowngradeAttackRange(float newAttackRange)
    {
        mAttackRange = mAttackRange - newAttackRange;
    }

    public override void DowngradeAttackSpeed(float newAttackSpeed)
    {
        mCanon.DowngradeAttackSpeed(newAttackSpeed);
    }
}
